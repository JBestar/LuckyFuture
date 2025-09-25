// #define DEBUG_LOG

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChartCtrl;
using LuckyFuture.Models.ValueObjects;
using LuckyFuture.Properties;
using LuckyFuture.Site;
using LuckyFuture.UI;
using LuckyFutureLib.Include;

namespace LuckyFuture.Logic
{

    internal class LogicAuto : StageThreadEx
    {
        public static LogicAuto Default = new LogicAuto();

        private FutureSite _currentSite = null;
        private FutureSite _signalSite = null;
        private EventHandler<FutureSiteLogArgs> LogEvent;
        private EventHandler<FutureSiteEventArgs> NoticeEvent;
        private FrmMain frmMain = null;
        private TRADETYPE _tradeTypeToOrder = TRADETYPE.NONE;
        private OrderInfo _orderToCancel = null;
        private bool _reorderToCancel = false;
        private readonly object _objLock = new object();

        private const int _delayOrder = 5000;
        private int _iOrderStage = 0;

        private int m_tickOrder = 0;
        public int m_tickCancel = 0;
        private bool m_boLiquid = false;
        private bool m_forceLiquid = false;
        private double m_maxProfit = 0;
        private int m_tickStateLog = 0;
        private int m_tickValueLog = 0;
        private int m_tickValueWLog = 0;

        public FutureSite CurrentSite { get => _currentSite; }
        public FutureSite SignalSite { get => _signalSite; }
        public bool Start(
            SITETYPE siteType, string id, string password, string acc,
            string item,
            EventHandler<FutureSiteLogArgs> logEvent,
            EventHandler<FutureSiteEventArgs> noticeEvent,
            FrmMain frmMain, AxKFOpenAPILib.AxKFOpenAPI axKFOpenAPI
        )
        {
            if (IsRunning)
                return false;

            if (!CreateSiteObject(siteType, id, password, acc, item, logEvent, noticeEvent, axKFOpenAPI))
                return false;

            _iOrderStage = 0;
            LogEvent += logEvent;
            NoticeEvent += noticeEvent;
            this.frmMain = frmMain;

            return Start();
        }
        enum LASTAGE
        {
            LOGIN = STAGE.LAST,
            CHECK = LOGIN + 1,
            ORDER = CHECK + 1,
            CANCEL = ORDER + 1,
        }

        protected override bool Run()
        {
            bool bAutoStop = false;
            switch (_stage)
            {
                case (int)STAGE.NONE:
                    SetStage((int)LASTAGE.LOGIN);
                    break;
                // login
                case (int)LASTAGE.LOGIN:
                    if (!_currentSite.Start())
                        bAutoStop = true;

                    if (!bAutoStop && _signalSite != null)
                    {
                        Thread.Sleep(3000);
                        if (!_signalSite.Start())
                            bAutoStop = true;
                    }

                    if (!bAutoStop)
                    {
                        m_tickStateLog = Environment.TickCount;
                        m_boLiquid = false;
                        SetStageWait((int)LASTAGE.CHECK, 10000);
                    }

                    break;
                // check
                case (int)LASTAGE.CHECK:
                    if (!_currentSite.IsRunning)
                        bAutoStop = true;
                    else if (_signalSite != null && !_signalSite.IsRunning)
                        bAutoStop = true;
                    else if (NeedToStop(true))
                    {
                        m_forceLiquid = false;

                        if (Settings.Default.IsAutoMode)
                        {
                            Settings.Default.IsAutoMode = false;

                            OnNoticeEvent(SITE_NOTICEEVENTTYPE.MANUAL);
                        }
                        SetStageWait((int)LASTAGE.CHECK, 1000);
                    }
                    else if (NeedToAuto())
                    {
                        OnNoticeEvent(SITE_NOTICEEVENTTYPE.AUTO);
                        SetStageWait((int)LASTAGE.CHECK, 1000);
                    }
                    else
                    {
                        TraceIntervalLog();
                        SetStageWait((int)LASTAGE.CHECK, 1000);
                    }
                    break;
                default:
                    bAutoStop = !base.Run();
                    break;
            }

            return !bAutoStop;
        }

        protected override void OnStarted()
        {
            base.OnStarted();
        }

        protected override void OnStopped(bool bAutoStop)
        {
            ReleaseSiteObject();

            OnNoticeEvent(SITE_NOTICEEVENTTYPE.STOP);

            if (LogEvent != null)
            {
                foreach (Delegate d in LogEvent.GetInvocationList())
                    LogEvent -= (EventHandler<FutureSiteLogArgs>)d;
            }

            if (NoticeEvent != null)
            {
                foreach (Delegate d in NoticeEvent.GetInvocationList())
                    NoticeEvent -= (EventHandler<FutureSiteEventArgs>)d;
            }

            base.OnStopped(bAutoStop);
        }

        private bool CreateSiteObject(
            SITETYPE siteType, string id, string password, string acc, string item,
            EventHandler<FutureSiteLogArgs> logEvent,
            EventHandler<FutureSiteEventArgs> noticeEvent,
            AxKFOpenAPILib.AxKFOpenAPI axKFOpenAPI
        )
        {
            if (_currentSite != null)
                return false;

            switch (siteType)
            {
                case SITETYPE.KIWOOM:
                    _currentSite = new KFOpen(axKFOpenAPI);
                    break;
                case SITETYPE.CMG:
                    _currentSite = new MetaTrader();
                    break;
                case SITETYPE.DREAM:
                    _currentSite = new SiteReantek(SITETYPE.DREAM);
                    break;
                case SITETYPE.TOPASSET:
                    _currentSite = new SiteReantek(SITETYPE.TOPASSET);
                    break;
                case SITETYPE.MIRAE2:
                    //_currentSite = new SiteMirae(SITETYPE.MIRAE);
                    _currentSite = new SiteReantek(SITETYPE.MIRAE2);
                    break;
                default:
                    _currentSite = null;
                    break;
            }


            if (_currentSite != null)
            {

                _currentSite.UserId = id;
                _currentSite.UserPassword = password;
                _currentSite.UserAcc = acc;
                _currentSite.ItemSymbol = item;
                _currentSite.LogEvent += logEvent;
                _currentSite.NoticeEvent += noticeEvent;

                if (siteType != SITETYPE.KIWOOM && Settings.Default.SignalSiteOn)
                {
                    _signalSite = new KFOpen(axKFOpenAPI);

                    _signalSite.UserId = id;
                    _signalSite.UserPassword = password;
                    _signalSite.UserAcc = acc;
                    _signalSite.ItemSymbol = item;
                    _signalSite.LogEvent += logEvent;
                    _signalSite.NoticeEvent += noticeEvent;
                }
            }

            return _currentSite != null;
        }

        private void ReleaseSiteObject()
        {
            if (_currentSite != null)
            {
                try
                {
                    _currentSite.Close();
                }
                catch (Exception)
                {
                    // OnLogEvent(e.Message);
                }
                _currentSite = null;
            }
            if (_signalSite != null)
            {
                try
                {
                    _signalSite.Close();
                }
                catch (Exception)
                {
                    // OnLogEvent(e.Message);
                }
                _signalSite = null;
            }

        }

        private void OnLogEvent(string log)
        {
            if (LogEvent != null)
                LogEvent(this, new FutureSiteLogArgs(log));
        }

        private void OnNoticeEvent(object obj)
        {
            if (NoticeEvent != null)
                NoticeEvent(this, new FutureSiteEventArgs(obj));
        }

        public void OnLogicNoticeReceive()
        {
            try
            {
                if (CurrentSite != null && CurrentSite.ValuationList[0].TotalProfit >= 10000 && m_maxProfit < CurrentSite.ValuationList[0].TotalProfit)
                    m_maxProfit = CurrentSite.ValuationList[0].TotalProfit;
                if (!Settings.Default.IsAutoMode)
                {
                    if (CurrentSite != null && CurrentSite.ValuationList[0].TotalProfit >= 10000)
                        m_maxProfit = CurrentSite.ValuationList[0].TotalProfit;
                    else m_maxProfit = 0;
                }
                TraceRealtimeLog();

                if (NeedToStop())
                    return;
                else if (IsAutoTradeEnable())
                {
                    lock (_objLock)
                    {
                        if (Math.Abs(Environment.TickCount - m_tickOrder) >= _delayOrder) //5000
                        {
                            m_tickOrder = Environment.TickCount;
                        }
                        else return;
                    }
                    DoOrder(Settings.Default.OrderCount <= Settings.Default.OrderMax ? Settings.Default.OrderCount : Settings.Default.OrderMax);
                }
                else if (NeedToCancel())
                {
                    lock (_objLock)
                    {
                        if (Math.Abs(Environment.TickCount - m_tickCancel) >= _delayOrder) //5000
                        {
                            m_tickCancel = Environment.TickCount;

                        }
                        else return;
                    }
                    DoCancel();

                }
            }
            catch (Exception) { }
        }

        private bool IsAutoTradeEnable()
        {
            // no auto mode
            if (!Settings.Default.IsAutoMode)
                return false;

            // has order list
            if (_currentSite == null)
                return false;

            if (_currentSite.OrderList.Count > 1)
                return false;

            if (!Settings.Default.BothOrder && _currentSite.OrderList.Count > 0)
                return false;

            //if (Settings.Default.BothOrder && Settings.Default.OrderType == 0)
            //    return false;

            lock (_objLock)
            {

                if (Math.Abs(Environment.TickCount - m_tickCancel) <= _delayOrder) //3000
                    return false;
                if (Math.Abs(Environment.TickCount - m_tickOrder) <= _delayOrder) //10000
                    return false;
            }

            string log = "";
            _tradeTypeToOrder = SelectTradeType(ref log);

            if (Settings.Default.BothOrder && _currentSite.OrderList.Count > 0 && _tradeTypeToOrder != TRADETYPE.NONE)
            {
                if (_currentSite.OrderList.FirstOrDefault(o => o.TradeType == _tradeTypeToOrder) != null)
                    return false;
            } else if(_tradeTypeToOrder != TRADETYPE.NONE)
            {
                
                if (Settings.Default.ReverseOrder && Settings.Default.ReverseOrdCnt1 + Settings.Default.ReverseOrdCnt2 > 0)
                {
                    _iOrderStage++;
                    if (_iOrderStage > Settings.Default.ReverseOrdCnt1 + Settings.Default.ReverseOrdCnt2)
                    {
                        _iOrderStage = 1;
                    }

                    if (_iOrderStage <= Settings.Default.ReverseOrdCnt1)
                    {
                        if(Settings.Default.ReverseOrdSel1 == 1)
                        {
                            _tradeTypeToOrder = _tradeTypeToOrder == TRADETYPE.BUY ? TRADETYPE.SELL : TRADETYPE.BUY;
                            log += string.Format(" <{0}단계:역배>", _iOrderStage);
                        }
                        else
                        {
                            log += string.Format(" <{0}단계:정배>", _iOrderStage);
                        }
                    } else
                    {
                        if (Settings.Default.ReverseOrdSel2 == 1)
                        {
                            _tradeTypeToOrder = _tradeTypeToOrder == TRADETYPE.BUY ? TRADETYPE.SELL : TRADETYPE.BUY;
                            log += string.Format(" <{0}단계:역배>", _iOrderStage);
                        }
                        else
                        {
                            log += string.Format(" <{0}단계:정배>", _iOrderStage);
                        }
                    }
                }
            }

            if (_tradeTypeToOrder != TRADETYPE.NONE)
            {
                if (log.Length > 0)
                    this.frmMain.AddLog(log);
                return true;
            }
            else
                return false;
        }

        private bool NeedToCancel()
        {
            // no auto mode
            if (!Settings.Default.IsAutoMode)
                return false;

            // has order list
            if (_currentSite == null || _currentSite.OrderList == null || _currentSite.OrderList.Count <= 0)
                return false;
            lock (_objLock)
            {
                if (Math.Abs(Environment.TickCount - m_tickOrder) <= _delayOrder) //3000
                    return false;

                if (Math.Abs(Environment.TickCount - m_tickCancel) <= _delayOrder) //5000
                    return false;
            }

            List<DItem> lastCandlelist = null;
            CH_AVGTYPE avgType = CH_AVGTYPE.LINE_1;
            float fTickDiff = 0.0f, fTickConf = 0.0f;
            int iFirstIdx = 0;
            double dCurCci = 0;
            double dCurRsi = 0;
            if (Settings.Default.BettingType == (int)BETTYPE.UPDOWN)
            {
                lastCandlelist = frmMain.GetCandleList(Settings.Default.CandlePayoffCount+1);

                if (lastCandlelist.Count < Settings.Default.CandlePayoffCount+1)
                    return false;

                avgType = (CH_AVGTYPE)Settings.Default.AvgType;
                fTickDiff = Math.Abs(lastCandlelist.First().GetAvgVal(avgType) - lastCandlelist.Last().GetAvgVal(avgType));
                fTickConf = Settings.Default.ItemOverTick * Settings.Default.TickPayoffCount;
                iFirstIdx = lastCandlelist.First().Index;

#if DEBUG_LOG
                Trace.TraceInformation("NeedToCancel() BETTYPE.UPDOWN 틱={0:N2}(설정:이평선={1}, {2}개 틱={3})", fTickDiff, (int)avgType, Settings.Default.CandlePayoffCount, fTickConf);
#endif
            }
            else if (Settings.Default.BettingType == (int)BETTYPE.CROSS)
            {
                lastCandlelist = frmMain.GetCandleList(2, Settings.Default.BettingCandleComplete == 0);/*false*/
                if (lastCandlelist.Count < 2)
                    return false;
                dCurCci = lastCandlelist.Last().Cci;
                dCurRsi = lastCandlelist.Last().Rsi;
            }
            else if (Settings.Default.BettingType == (int)BETTYPE.BOLINE)
            {
                lastCandlelist = frmMain.GetCandleList(Settings.Default.BettingCandleCount, true);
                if (lastCandlelist.Count < Settings.Default.BettingCandleCount)
                    return false;
                dCurCci = lastCandlelist.Last().Cci;
                dCurRsi = lastCandlelist.Last().Rsi;
            }
            else if (Settings.Default.BettingType == (int)BETTYPE.HYBRID)
            {
                lastCandlelist = frmMain.GetCandleList(1, Settings.Default.BettingCandleComplete == 0);
                if (lastCandlelist.Count < 1)
                    return false;
            }

            int nNowTick = Environment.TickCount;
            SITETYPE currentSiteType = _currentSite.Type;
            double dDeltaTick = 0, dAvgPrice = 0, dCurPrice = 0;
            lock (_currentSite.OrderList)
            {
                _orderToCancel = _currentSite.OrderList.FirstOrDefault<OrderInfo>(
                    delegate (OrderInfo o)
                    {
                        
                        if (o.OrderType == "미체결")
                        {
                            if (Settings.Default.OrderStop && Settings.Default.OrderStopDelay >= 0 && o.OrderTime > 0)
                            {
                                if (Math.Abs(nNowTick - o.OrderTime) >= Settings.Default.OrderStopDelay * 1000)
                                    return true;
                            }
                            return false;
                        }
                        else
                        { //체결

                            if (o.StartCciPrice == -10000)   //Setting start CCI value
                            {
                                o.StartCciPrice = dCurCci;
                            }

                            if (o.TradeType == TRADETYPE.BUY) //Change max CCI value
                            {
                                if (_currentSite.OrderList.FirstOrDefault(u => u.TradeType == TRADETYPE.SELL && u.OrderType == "미체결") != null)
                                    return false;

                                if (dCurCci > o.MaxCciPrice)
                                    o.MaxCciPrice = dCurCci;
                            }
                            else if (o.TradeType == TRADETYPE.SELL)
                            {
                                if (_currentSite.OrderList.FirstOrDefault(u => u.TradeType == TRADETYPE.BUY && u.OrderType == "미체결") != null)
                                    return false;

                                if (dCurCci < o.MaxCciPrice)
                                    o.MaxCciPrice = dCurCci;
                            }

                            double valuation = _currentSite.ValuationList[0].CurrentProfit;
                            if (valuation > 0)
                            {
                                if (Settings.Default.LiquidStop && Settings.Default.EarnStop && Settings.Default.EarnStopMoney >= 0 && valuation >= Settings.Default.EarnStopMoney)
                                {
                                    m_forceLiquid = true;
                                    this.frmMain.AddLog(string.Format("[정지] 실시간수익: {0:N0}USD 익절:{1}USD",
                                        valuation,
                                        Settings.Default.EarnStopMoney.ToString()));
                                    return true;
                                }
                            }
                            else if (valuation < 0)
                            {
                                if (Settings.Default.LossStop && Settings.Default.LossStopMoney >= 0 && valuation <= -Settings.Default.LossStopMoney)
                                {
                                    m_forceLiquid = true;
                                    this.frmMain.AddLog(string.Format("[정지] 실시간수익: {0:N0}USD 손절:{1}USD",
                                        valuation,
                                        Settings.Default.LossStopMoney.ToString()));
                                    return true;
                                }
                            }

                            if (Settings.Default.ProfitStop && m_maxProfit > 0 && valuation > 0)
                            {

                                if (valuation < m_maxProfit * (100 - Settings.Default.ProfitStopRate) / 100)
                                {
                                    m_forceLiquid = true;
                                    this.frmMain.AddLog(string.Format("[정지] 현재 실시간수익: {0:N0}USD 실현손익:{1:N0}USD의 {2}%하락",
                                        valuation,
                                        m_maxProfit,
                                        Settings.Default.ProfitStopRate.ToString()));
                                    return true;
                                }
                            }

                            try
                            {
                                dCurPrice = double.Parse(o.CurrentPrice);
                                dAvgPrice = double.Parse(o.AveragePrice);
                            }
                            catch (Exception)
                            {
                                return false;
                            }
                            if (dCurPrice > 0 && dAvgPrice > 0)
                                dDeltaTick = dCurPrice - dAvgPrice;
                            else return false;

                            string log = "";
                            //주문수량에 상관없이 검사
                            // 							if (o.TradeType == TRADETYPE.BUY && o.OrderQty > 0)
                            //                             {
                            //                                 if (Settings.Default.EarnPayoffN && Settings.Default.EarnPayoffMoneyN >= 0
                            //                                         && dDeltaTick* o.OrderQty >= Settings.Default.EarnPayoffMoneyN * Settings.Default.ItemOverTick)
                            //                                 {
                            //                                     this.frmMain.AddLog("주문수량 관계없이 청산:매수 현재가:" + string.Format(Settings.Default.PriceFormat, dCurPrice) + " 체결가:" + string.Format(Settings.Default.PriceFormat, dCurPrice) + " 수량:" + o.OrderQty.ToString() + " 익절설정:" + Settings.Default.EarnPayoffMoneyN.ToString()+"틱");
                            //                                     return true;
                            // 								}
                            //                                 else if (Settings.Default.LossPayoffN && Settings.Default.LossPayoffMoneyN >= 0
                            //                                     && dDeltaTick * o.OrderQty <= -Settings.Default.LossPayoffMoneyN * Settings.Default.ItemOverTick)
                            //                                 {
                            // 									this.frmMain.AddLog("주문수량 관계없이 청산:매수 현재가:" + string.Format(Settings.Default.PriceFormat, dCurPrice) + " 체결가:" + string.Format(Settings.Default.PriceFormat, dCurPrice) + " 수량:" + o.OrderQty.ToString() + " 손절설정:" + Settings.Default.LossPayoffMoneyN.ToString() + "틱");
                            // 									return true;
                            // 
                            //                                 }
                            //                             }
                            //                             else if (o.TradeType == TRADETYPE.SELL && o.OrderQty > 0)
                            //                             {
                            //                                 if (Settings.Default.EarnPayoffN && Settings.Default.EarnPayoffMoneyN >= 0
                            //                                     && dDeltaTick * o.OrderQty <= -Settings.Default.EarnPayoffMoneyN * Settings.Default.ItemOverTick)
                            //                                 {
                            // 									this.frmMain.AddLog("주문수량 관계없이 청산:매도 현재가:" + string.Format(Settings.Default.PriceFormat, dCurPrice) + " 체결가:" + string.Format(Settings.Default.PriceFormat, dCurPrice) + " 수량:" + o.OrderQty.ToString() + " 익절설정:" + Settings.Default.EarnPayoffMoneyN.ToString() + "틱");
                            //                                     return true;
                            //                                 }
                            //                                 else if (Settings.Default.LossPayoffN && Settings.Default.LossPayoffMoneyN >= 0
                            //                                     && dDeltaTick * o.OrderQty >= Settings.Default.LossPayoffMoneyN * Settings.Default.ItemOverTick)
                            //                                 {
                            //                                     this.frmMain.AddLog("주문수량 관계없이 청산:매도 현재가:" + string.Format(Settings.Default.PriceFormat, dCurPrice) + " 체결가:" + string.Format(Settings.Default.PriceFormat, dCurPrice) + " 수량:" + o.OrderQty.ToString() + " 손절설정:" + Settings.Default.LossPayoffMoneyN.ToString() + "틱");
                            //                                     return true;
                            //                                 }
                            //                             }

                            if (Settings.Default.BettingType == (int)BETTYPE.EQUIVALENT)
                            {

                                if (o.TradeType == TRADETYPE.BUY)
                                {
                                    if (Settings.Default.EarnPayoff && Settings.Default.EarnPayoffMoney >= 0
                                        && dDeltaTick >= Settings.Default.EarnPayoffMoney * Settings.Default.ItemOverTick)
                                        return true;
                                }
                                else if (o.TradeType == TRADETYPE.SELL) //매도
                                {
                                    if (Settings.Default.EarnPayoff && Settings.Default.EarnPayoffMoney >= 0
                                        && dDeltaTick <= -Settings.Default.EarnPayoffMoney * Settings.Default.ItemOverTick)
                                        return true;
                                }

                                if (Settings.Default.LossPayoff)
                                {
                                    if (CheckLossPayoff(o, dCurPrice, dAvgPrice))
                                        return true;
                                }

                                if (Settings.Default.SmartLossPayoff)
                                {
                                    if (CheckSmartLossPayoff(o.TradeType, dCurPrice, dAvgPrice, o.MaxAveragePrice, dCurRsi, o.OrderQty))
                                        return true;
                                }
                            }
                            else if (Settings.Default.BettingType == (int)BETTYPE.UPDOWN)
                            {
                                string logPayoff = "";
                                bool bBollPayoff = CheckBollPayoff(o.TradeType, lastCandlelist.Last(), ref logPayoff);
                                
                                if (bBollPayoff && !Settings.Default.PayoffWithEarn && Settings.Default.BollPayoff)
                                {                   //볼린저밴드 개별청산조건
                                    log = "[청산]";
                                    if (logPayoff.Length > 0)
                                        log += logPayoff;
                                    this.frmMain.AddLog(log);
                                    return true;
                                }
                                if (!Settings.Default.PayoffWithEarn)
                                {
                                    bBollPayoff = true;
                                }

                                if (o.TradeType == TRADETYPE.BUY)
                                {
                                    if (bBollPayoff && fTickDiff >= fTickConf && lastCandlelist.Count<DItem>(d => d.GetTrendDown(avgType, iFirstIdx) == CH_TRENDTYPE.DOWN) >= Settings.Default.CandlePayoffCount+1)
                                    {

                                        log = String.Format("[청산] 하락:{0:N2}틱", fTickDiff / Settings.Default.ItemOverTick);
                                        if (logPayoff.Length > 0)
                                            log += logPayoff;
                                        this.frmMain.AddLog(log);
                                        return true;
                                    }
                                }
                                else if (o.TradeType == TRADETYPE.SELL) //매도
                                {
                                    if (bBollPayoff && fTickDiff >= fTickConf && lastCandlelist.Count<DItem>(d => d.GetTrendUp(avgType, iFirstIdx) == CH_TRENDTYPE.UP) >= Settings.Default.CandlePayoffCount+1)
                                    {
                                        log = String.Format("[청산] 상승:{0:N2}틱", fTickDiff / Settings.Default.ItemOverTick);
                                        if (logPayoff.Length > 0)
                                            log += logPayoff;
                                        this.frmMain.AddLog(log);
                                        return true;
                                    }
                                }
                            }
                            if (Settings.Default.BettingType == (int)BETTYPE.CROSS)
                            {
                                
                                if(Settings.Default.ReturnOption == 2) //되돌림 청산
                                {
                                    if (o.TradeType == TRADETYPE.BUY)   //매수
                                    {
                                        if (lastCandlelist.Last().GetCrossTrend((CH_AVGTYPE)Settings.Default.CrossAvgLine1, (CH_AVGTYPE)Settings.Default.CrossAvgLine2) == CH_TRENDTYPE.DOWN)
                                        {
                                            log = "[청산] 되돌림청산 ";
                                            this.frmMain.AddLog(log);
                                            return true;
                                        }
                                    }
                                    else if (o.TradeType == TRADETYPE.SELL) //매도
                                    {
                                        if (lastCandlelist.Last().GetCrossTrend((CH_AVGTYPE)Settings.Default.CrossAvgLine1, (CH_AVGTYPE)Settings.Default.CrossAvgLine2) == CH_TRENDTYPE.UP)
                                        {
                                            
                                            log = "[청산] 되돌림청산 ";
                                            this.frmMain.AddLog(log);
                                            return true;
                                        }
                                    }
                                }
                                else if (Settings.Default.ReturnOption == 1) //되돌림 주문
                                {
                                    string logTrade = "";

                                    bool bTradeChanged = CheckTradeChange(ref logTrade);
                                    
                                    if (o.TradeType == TRADETYPE.BUY)   //매수
                                    {
                                        if (lastCandlelist.Last().GetCrossTrend((CH_AVGTYPE)Settings.Default.CrossAvgLine1, (CH_AVGTYPE)Settings.Default.CrossAvgLine2) == CH_TRENDTYPE.DOWN && bTradeChanged)
                                        {
                                            if (!Settings.Default.OrderSelectOn || (Settings.Default.OrderSelectOn && Settings.Default.OrderSelectType == 0))
                                            {
                                                if(currentSiteType == SITETYPE.CMG)
                                                {
                                                    log = "[청산] ";
                                                }
                                                else
                                                {
                                                    log = "[청산] 되돌림주문 ";
                                                    _reorderToCancel = true;
                                                }
                                                if (logTrade.Length > 0)
                                                    log += logTrade;

                                                this.frmMain.AddLog(log);

                                                return true;
                                            }
                                        }
                                    }
                                    else if (o.TradeType == TRADETYPE.SELL) //매도
                                    {
                                        if (lastCandlelist.Last().GetCrossTrend((CH_AVGTYPE)Settings.Default.CrossAvgLine1, (CH_AVGTYPE)Settings.Default.CrossAvgLine2) == CH_TRENDTYPE.UP && bTradeChanged)
                                        {
                                            if (!Settings.Default.OrderSelectOn || (Settings.Default.OrderSelectOn && Settings.Default.OrderSelectType == 0))
                                            {
                                                if (currentSiteType == SITETYPE.CMG)
                                                {
                                                    log = "[청산] ";
                                                }
                                                else
                                                {
                                                    log = "[청산] 되돌림주문 ";
                                                    _reorderToCancel = true;
                                                }
                                                if (logTrade.Length > 0)
                                                    log += logTrade;

                                                this.frmMain.AddLog(log);

                                                return true;
                                            }
                                        }
                                    }
                                }

                                string logPayoff = "";
                                bool bBollPayoff = CheckBollPayoff(o.TradeType, lastCandlelist.Last(), ref logPayoff);

                                if (bBollPayoff && !Settings.Default.PayoffWithEarn && Settings.Default.BollPayoff)
                                {                           //볼린저밴드 개별청산조건
                                    log = "[청산]";
                                    if (logPayoff.Length > 0)
                                        log += logPayoff;
                                    this.frmMain.AddLog(log);
                                    return true;
                                }
                                if (!Settings.Default.PayoffWithEarn)
                                {
                                    bBollPayoff = true;
                                }

                                if (o.TradeType == TRADETYPE.BUY)
                                {
                                    if (lastCandlelist.Last().GetCrossTrend((CH_AVGTYPE)Settings.Default.CrossAvgLine1, (CH_AVGTYPE)Settings.Default.CrossAvgLine2) == CH_TRENDTYPE.DOWN //lastCandlelist.Count 
                                        && lastCandlelist.Last().Orders.Count < 1 && (o.CrossAveragePrice == 0 || o.CrossAveragePrice < dCurPrice))
                                    {
                                        o.CrossAveragePrice = dCurPrice; //교차점에서 현재가
#if DEBUG_LOG
                                        Trace.TraceInformation("<LogicAuto> BETTYPE.CROSS TRADETYPE.BUY AveragePrice = {0}, CrossAveragePrice = {1} ", dAvgPrice, o.CrossAveragePrice);
#endif
                                    }

                                    if (Settings.Default.EarnPayoff && Settings.Default.EarnPayoffMoney >= 0
                                        && dDeltaTick >= Settings.Default.EarnPayoffMoney * Settings.Default.ItemOverTick && bBollPayoff)
                                    {
                                        log = string.Format("[청산] 수익:{0:N2}틱", Math.Abs(dDeltaTick) / Settings.Default.ItemOverTick);
                                        if (Settings.Default.EarnPayoff)
                                            log += "(설정:" + Settings.Default.EarnPayoffMoney.ToString() + "틱)";
                                        if (logPayoff.Length > 0)
                                            log += logPayoff;
                                        this.frmMain.AddLog(log);
                                        return true;
                                    }

                                }
                                else if (o.TradeType == TRADETYPE.SELL) //매도
                                {
                                    if (lastCandlelist.Last().GetCrossTrend((CH_AVGTYPE)Settings.Default.CrossAvgLine1, (CH_AVGTYPE)Settings.Default.CrossAvgLine2) == CH_TRENDTYPE.UP //lastCandlelist.Count 
                                        && lastCandlelist.Last().Orders.Count < 1 && (o.CrossAveragePrice == 0 || o.CrossAveragePrice > dCurPrice))
                                    {
                                        o.CrossAveragePrice = dCurPrice; //교차점에서 현재가
#if DEBUG_LOG
                                        Trace.TraceInformation("<LogicAuto> BETTYPE.CROSS, TRADETYPE.SELL AveragePrice = {0}, CrossAveragePrice = {1} ", dAvgPrice, o.CrossAveragePrice);
#endif
                                    }

                                    if (Settings.Default.EarnPayoff && Settings.Default.EarnPayoffMoney >= 0
                                        && dDeltaTick <= -Settings.Default.EarnPayoffMoney * Settings.Default.ItemOverTick && bBollPayoff)
                                    {
                                        log = string.Format("[청산] 수익:{0:N2}틱", Math.Abs(dDeltaTick) / Settings.Default.ItemOverTick);
                                        if (Settings.Default.EarnPayoff)
                                            log += "(설정:" + Settings.Default.EarnPayoffMoney.ToString() + "틱)";
                                        if (logPayoff.Length > 0)
                                            log += logPayoff;
                                        this.frmMain.AddLog(log);
                                        return true;
                                    }

                                }

                                if (Settings.Default.LossPayoff)
                                {
                                    if (CheckLossPayoff(o, dCurPrice, dAvgPrice))
                                        return true;
                                }

                                if (Settings.Default.SmartLossPayoff)
                                {
                                    if (CheckSmartLossPayoff(o.TradeType, dCurPrice, dAvgPrice, o.MaxAveragePrice, dCurRsi, o.OrderQty))
                                        return true;
                                }

                                if (Settings.Default.CrossLossPayoff && o.CrossAveragePrice > 0)
                                {
                                    if (CheckCrossLossPayoff(o.TradeType, dCurPrice, dAvgPrice, o.CrossAveragePrice, dCurRsi, o.OrderQty))
                                        return true;
                                }

                                if (Settings.Default.CciPayoff)
                                {
                                    if (CheckCciPayoff(o.TradeType, dCurCci, o.StartCciPrice, o.MaxCciPrice, dCurRsi))
                                        return true;
                                }

                            }
                            else if (Settings.Default.BettingType == (int)BETTYPE.BOLINE)           //Check Equivalent Candle 
                            {
                                string logPayoff = "";
                                bool bBollPayoff = CheckBollPayoff(o.TradeType, lastCandlelist.Last(), ref logPayoff);
                                if (bBollPayoff && !Settings.Default.PayoffWithEarn && Settings.Default.BollPayoff)
                                {                           //볼린저밴드 개별청산조건
                                    log = "[청산]";
                                    if (logPayoff.Length > 0)
                                        log += logPayoff;
                                    this.frmMain.AddLog(log);
                                    return true;
                                }
                                if (!Settings.Default.PayoffWithEarn)
                                {
                                    bBollPayoff = true;
                                }

                                string logTrade = "";
                                if (o.TradeType == TRADETYPE.BUY)   //매수
                                {
                                    if (lastCandlelist.Count<DItem>(d => d.Est_Type == RESULTSTATE.SELL) >= 1 //lastCandlelist.Count 
                                        && lastCandlelist.Last().Orders.Count < 1 && (o.CrossAveragePrice == 0 || o.CrossAveragePrice < dCurPrice)/* && bTradeChanged*/)
                                    {
                                        o.CrossAveragePrice = dCurPrice; //교차점에서 현재가
#if DEBUG_LOG
                                        Trace.TraceInformation("<LogicAuto> BETTYPE.BOLINE, TRADETYPE.BUY AveragePrice = {0}, CrossAveragePrice = {1} ", dAvgPrice, o.CrossAveragePrice);
#endif
                                    }

                                    if ((Settings.Default.EarnPayoff && Settings.Default.EarnPayoffMoney >= 0
                                        && dDeltaTick >= Settings.Default.EarnPayoffMoney * Settings.Default.ItemOverTick && bBollPayoff)
                                            || (!Settings.Default.EarnPayoff && dDeltaTick >= 0))
                                    {
                                        bool bTradeChanged = CheckTradeChange(ref logTrade);

                                        if (Settings.Default.EarnPayoff && Settings.Default.ForceEarnPayoff)        //강제수익청산
                                        {
                                            log = string.Format("[강제청산] 수익:{0:N2}틱", Math.Abs(dDeltaTick) / Settings.Default.ItemOverTick);
                                            if (Settings.Default.EarnPayoff)
                                                log += "(설정:" + Settings.Default.EarnPayoffMoney.ToString() + "틱)";
                                            if (logPayoff.Length > 0)
                                                log += logPayoff;
                                            this.frmMain.AddLog(log);
                                            m_boLiquid = true;
                                            return true;
                                        }
                                        else if (lastCandlelist.Count<DItem>(d => d.Est_Type == RESULTSTATE.SELL) >= lastCandlelist.Count && bTradeChanged)
                                        {
                                            if (logTrade.Length > 0)
                                                this.frmMain.AddLog(logTrade);

                                            if (!Settings.Default.OrderSelectOn || (Settings.Default.OrderSelectOn && Settings.Default.OrderSelectType == 0))
                                            {
                                                log = string.Format("[청산] 수익:{0:N2}틱", Math.Abs(dDeltaTick) / Settings.Default.ItemOverTick);
                                                if (Settings.Default.EarnPayoff)
                                                    log += "(설정:" + Settings.Default.EarnPayoffMoney.ToString() + "틱)";
                                                if (logPayoff.Length > 0)
                                                    log += logPayoff;
                                                this.frmMain.AddLog(log);
                                                if (Settings.Default.BoOrdType == 0)
                                                {
                                                    lock (_objLock)
                                                    {
                                                        _reorderToCancel = true;
                                                    }
                                                }
                                            }
                                            return true;
                                        }
                                    }

                                }
                                else if (o.TradeType == TRADETYPE.SELL) //매도
                                {
                                    if (lastCandlelist.Count<DItem>(d => d.Est_Type == RESULTSTATE.BUY) >= 1 //lastCandlelist.Count 
                                        && lastCandlelist.Last().Orders.Count < 1 && (o.CrossAveragePrice == 0 || o.CrossAveragePrice > dCurPrice)/* && bTradeChanged*/)
                                    {
                                        o.CrossAveragePrice = dCurPrice; //교차점에서 현재가
#if DEBUG_LOG
                                        Trace.TraceInformation("<LogicAuto> BETTYPE.BOLINE, TRADETYPE.SELL AveragePrice = {0}, CrossAveragePrice = {1} ", dAvgPrice, o.CrossAveragePrice);
#endif
                                    }

                                    if ((Settings.Default.EarnPayoff && Settings.Default.EarnPayoffMoney >= 0
                                        && dDeltaTick <= -Settings.Default.EarnPayoffMoney * Settings.Default.ItemOverTick && bBollPayoff)
                                            || (!Settings.Default.EarnPayoff && dDeltaTick <= 0))
                                    {
                                        bool bTradeChanged = CheckTradeChange(ref logTrade);
                                        if (Settings.Default.EarnPayoff && Settings.Default.ForceEarnPayoff)        //강제수익청산
                                        {
                                            log = string.Format("[강제청산] 수익:{0:N2}틱", Math.Abs(dDeltaTick) / Settings.Default.ItemOverTick);
                                            if (Settings.Default.EarnPayoff)
                                                log += "(설정:" + Settings.Default.EarnPayoffMoney.ToString() + "틱)";
                                            if (logPayoff.Length > 0)
                                                log += logPayoff;
                                            this.frmMain.AddLog(log);
                                            m_boLiquid = true;
                                            return true;
                                        }
                                        else if (lastCandlelist.Count<DItem>(d => d.Est_Type == RESULTSTATE.BUY) >= lastCandlelist.Count && bTradeChanged)
                                        {
                                            if (logTrade.Length > 0)
                                                this.frmMain.AddLog(logTrade);
                                            if (!Settings.Default.OrderSelectOn || (Settings.Default.OrderSelectOn && Settings.Default.OrderSelectType == 0))
                                            {
                                                log = string.Format("[청산] 수익:{0:N2}틱", Math.Abs(dDeltaTick) / Settings.Default.ItemOverTick);
                                                if (Settings.Default.EarnPayoff)
                                                    log += "(설정:" + Settings.Default.EarnPayoffMoney.ToString() + "틱)";
                                                if (logPayoff.Length > 0)
                                                    log += logPayoff;
                                                this.frmMain.AddLog(log);
                                                if (Settings.Default.BoOrdType == 0) //진입체결:S-B
                                                {
                                                    lock (_objLock)
                                                    {
                                                        _reorderToCancel = true;
                                                    }
                                                }
                                            }
                                            return true;
                                        }
                                    }
                                }

                                if (Settings.Default.LossPayoff)
                                {
                                    if (CheckLossPayoff(o, dCurPrice, dAvgPrice))
                                        return true;
                                }

                                if (Settings.Default.SmartLossPayoff)
                                {
                                    if (CheckSmartLossPayoff(o.TradeType, dCurPrice, dAvgPrice, o.MaxAveragePrice, dCurRsi, o.OrderQty))
                                        return true;
                                }

                                if (Settings.Default.CrossLossPayoff && o.CrossAveragePrice > 0)
                                {
                                    if (CheckCrossLossPayoff(o.TradeType, dCurPrice, dAvgPrice, o.CrossAveragePrice, dCurRsi, o.OrderQty))
                                        return true;
                                }

                                if (Settings.Default.CciPayoff)
                                {
                                    if (CheckCciPayoff(o.TradeType, dCurCci, o.StartCciPrice, o.MaxCciPrice, dCurRsi))
                                        return true;
                                }
                            }
                            else if (Settings.Default.BettingType == (int)BETTYPE.HYBRID)           //Check Equivalent Candle 
                            {
                                if (o.TradeType == TRADETYPE.BUY)
                                {
                                    if (lastCandlelist.Count<DItem>(d => d.Est_Type == RESULTSTATE.SELL) >= lastCandlelist.Count)
                                    {
                                        return true;

                                    }
                                    else if (Settings.Default.EarnPayoff && Settings.Default.EarnPayoffMoney >= 0
                                      && dDeltaTick >= Settings.Default.EarnPayoffMoney * Settings.Default.ItemOverTick)
                                    {
                                        return true;
                                    }

                                    if (Settings.Default.LossPayoff)
                                    {
                                        if (CheckLossPayoff(o, dCurPrice, dAvgPrice))
                                            return true;
                                    }

                                }
                                else if (o.TradeType == TRADETYPE.SELL) //매도
                                {
                                    if (lastCandlelist.Count<DItem>(d => d.Est_Type == RESULTSTATE.BUY) >= lastCandlelist.Count)
                                    {
                                        return true;

                                    }
                                    else if (Settings.Default.EarnPayoff && Settings.Default.EarnPayoffMoney >= 0
                                        && dDeltaTick <= -Settings.Default.EarnPayoffMoney * Settings.Default.ItemOverTick)
                                    {
                                        return true;
                                    }

                                    if (Settings.Default.LossPayoff)
                                    {
                                        if (CheckLossPayoff(o, dCurPrice, dAvgPrice))
                                            return true;
                                    }
                                }
                            }
                        }
                        return false;
                    }
                );
            }
            return _orderToCancel != null;
        }

        private int[] StageRangePercent(RANGETYPE rangeType, long lValuation)
        {
            int[] arrInfo = null;
            int amountUnit = 1;
            List<PayoffLossInfo> lossConfs = null;
            if (rangeType == RANGETYPE.PayoffLoss)
            {
                lossConfs = AppConfig.PayoffLossConfs;
            }
            else if (rangeType == RANGETYPE.SmartLoss)
            {
                lossConfs = AppConfig.SmartLossConfs;

            }
            else if (rangeType == RANGETYPE.CrossLoss)
            {
                lossConfs = AppConfig.CrossLossConfs;

            }
            else if (rangeType == RANGETYPE.CciLoss)
            {
                lossConfs = AppConfig.CciLossConfs;
                amountUnit = 1;
            }
            else return null;

            if (lossConfs.Count > 0)
            {
                if (lValuation < lossConfs[0].Amount * amountUnit)
                    return null;
            }
            int nParam = -1;

            for (int i = lossConfs.Count - 1; i >= 0; i--)
            {
                if (lossConfs[i].Enabled == 1 && lValuation >= lossConfs[i].Amount * amountUnit)
                {
                    arrInfo = new int[3] { -1, -1, -1 };
                    arrInfo[0] = lossConfs[i].Rate;
                    if (lossConfs[i].Param.Length > 0)
                    {
                        if (!int.TryParse(lossConfs[i].Param, out nParam))
                        {
                            nParam = -1;
                        }
                        arrInfo[1] = nParam;
                    }
                    if (lossConfs[i].Param2.Length > 0)
                    {
                        if (!int.TryParse(lossConfs[i].Param2, out nParam))
                        {
                            nParam = -1;
                        }
                        arrInfo[2] = nParam;
                    }


                    break;
                }
            }
            return arrInfo;
        }
        private bool CheckLossPayoff(OrderInfo o, double dCurPrice, double dAvgPrice)
        {
            double dDeltaValue = dCurPrice - dAvgPrice;
            string log = "";
            long lValuation = (long)o.Valuation;
            // if (lValuation > 0)
            //     return false;
            int lossTick = Settings.Default.LossPayoffMoney;
            int earnTick = -1;
            if (o.TradeType == TRADETYPE.BUY)
            {
                if (Settings.Default.LossRangePayoff)
                {
                    lossTick = -1;
                    earnTick = -1;
                    int nTick = -1;
                    int[] info = null;

                    if (lValuation >= 0)
                    {
                        info = StageRangePercent(RANGETYPE.PayoffLoss, lValuation);
                        if (info != null)
                        {
                            nTick = info[1];
                        }
                        if (nTick > 0 && nTick != o.EarnPayoffTick)
                        {
                            o.EarnPayoffTick = nTick;

                            log += string.Format("손실영역변경: {0:N0}USD 이익={1}틱", lValuation, nTick);
                            this.frmMain.AddLog(log);
                            m_boLiquid = true;
                        }
                        if (o.EarnPayoffTick > 0)
                            earnTick = o.EarnPayoffTick;
                    }
                    else
                    {
                        info = StageRangePercent(RANGETYPE.PayoffLoss, 0 - lValuation);

                        if (info != null)
                        {
                            nTick = info[0];
                        }
                        if (nTick > 0 && nTick != o.LossPayoffTick)
                        {
                            o.LossPayoffTick = nTick;

                            log += string.Format("손실영역변경: {0:N0}USD 손실={1}틱", lValuation, nTick);
                            this.frmMain.AddLog(log);
                            m_boLiquid = true;
                        }
                        if (o.LossPayoffTick > 0)
                            lossTick = o.LossPayoffTick;
                    }

                }
                if (lValuation >= 0)
                {
                    if (earnTick >= 0 && dDeltaValue >= earnTick * Settings.Default.ItemOverTick)
                    {
                        log = string.Format("[청산] 이익:{0:N2}틱", Math.Abs(dDeltaValue) / Settings.Default.ItemOverTick);
                        log += "(설정:" + earnTick.ToString() + "틱)";
                        this.frmMain.AddLog(log);
                        m_boLiquid = true;
                        return true;
                    }
                }
                else
                {
                    if (lossTick >= 0 && dDeltaValue <= -lossTick * Settings.Default.ItemOverTick)
                    {
                        log = string.Format("[청산] 손실:{0:N2}틱", Math.Abs(dDeltaValue) / Settings.Default.ItemOverTick);
                        log += "(설정:" + lossTick.ToString() + "틱)";
                        this.frmMain.AddLog(log);
                        m_boLiquid = true;
                        return true;
                    }
                }

            }
            else if (o.TradeType == TRADETYPE.SELL)
            {
                if (Settings.Default.LossRangePayoff)
                {
                    lossTick = -1;
                    earnTick = -1;
                    int nTick = -1;
                    int[] info = null;

                    if (lValuation >= 0)
                    {
                        info = StageRangePercent(RANGETYPE.PayoffLoss, lValuation);
                        if (info != null)
                        {
                            nTick = info[1];
                        }
                        if (nTick > 0 && nTick != o.EarnPayoffTick)
                        {
                            o.EarnPayoffTick = nTick;

                            log += string.Format("손실영역변경: {0:N0}USD 이익={1}틱", lValuation, nTick);
                            this.frmMain.AddLog(log);
                            m_boLiquid = true;
                        }
                        if (o.EarnPayoffTick > 0)
                            earnTick = o.EarnPayoffTick;
                    }
                    else
                    {
                        info = StageRangePercent(RANGETYPE.PayoffLoss, 0 - lValuation);

                        if (info != null)
                        {
                            nTick = info[0];
                        }

                        if (nTick >= 0 && nTick != o.LossPayoffTick)
                        {
                            o.LossPayoffTick = nTick;

                            log += string.Format("손실영역변경: {0:N0}USD 손실={1}틱)", lValuation, nTick);
                            this.frmMain.AddLog(log);
                            m_boLiquid = true;
                        }
                        if (o.LossPayoffTick > 0)
                            lossTick = o.LossPayoffTick;
                    }
                }
                if (lValuation >= 0)
                {
                    if (earnTick >= 0 && dDeltaValue <= -earnTick * Settings.Default.ItemOverTick)
                    {
                        log = string.Format("[청산] 이익:{0:N2}틱", Math.Abs(dDeltaValue) / Settings.Default.ItemOverTick);
                        log += "(설정:" + earnTick.ToString() + "틱)";
                        this.frmMain.AddLog(log);
                        m_boLiquid = true;
                        return true;
                    }
                }
                else
                {
                    if (lossTick >= 0 && dDeltaValue >= lossTick * Settings.Default.ItemOverTick)
                    {
                        log = string.Format("[청산] 손실:{0:N2}틱", Math.Abs(dDeltaValue) / Settings.Default.ItemOverTick);
                        log += "(설정:" + lossTick.ToString() + "틱)";
                        this.frmMain.AddLog(log);
                        m_boLiquid = true;
                        return true;
                    }
                }

            }

            return false;
        }
        private bool CheckSmartLossPayoff(TRADETYPE tradeType, double dCurPrice, double dAvgPrice, double dMaxAveragePrice, double dCurRsi, double orderQty)
        {
            double dSmartEarn = 0;
            if (tradeType == TRADETYPE.BUY)
            {
                if (Settings.Default.SmartRangePayoff)
                {
                    long lValuation = (long)((dMaxAveragePrice - dAvgPrice) / CurrentSite.CurItemSymbol.OverTick * CurrentSite.CurItemSymbol.ValueTick * CurrentSite.CurItemSymbol.Exchange * orderQty);

                    int nPercent = -1;
                    int nRsi = -1;
                    int[] info = StageRangePercent(RANGETYPE.SmartLoss, lValuation);
                    if (info != null)
                    {
                        nPercent = info[0];
                        nRsi = info[1];
                    }

                    long lLossVal = (long)((dMaxAveragePrice - dCurPrice) / CurrentSite.CurItemSymbol.OverTick * CurrentSite.CurItemSymbol.ValueTick * CurrentSite.CurItemSymbol.Exchange * orderQty);
                    if (nRsi > 0 && dCurRsi > nRsi && nPercent > 0 && lLossVal > 0 && lLossVal > lValuation * nPercent / 100)
                    {
                        m_boLiquid = true;
                        this.frmMain.AddLog(string.Format("[청산] 스마트청산 최대수익가:{0}({1:N0}USD)(설정:{2}%이상), Rsi={3:N2}(설정:{4}이상)",
                            string.Format(Settings.Default.PriceFormat, dMaxAveragePrice), lValuation, nPercent, dCurRsi, nRsi));
                        return true;
                    }
#if DEBUG_LOG
                    Trace.TraceInformation("[청산] 스마트영역청산 최대수익가:{0}({1:N0}USD)(설정:{2}%이상), Rsi={3:N2}(설정:{4}이상)",
                            string.Format(Settings.Default.PriceFormat, dMaxAveragePrice), lValuation, nPercent, dCurRsi, nRsi);
#endif
                }
                else if (Settings.Default.SmartEarnTick >= 0 && Settings.Default.SmartLossTick >= 0
                                        && dMaxAveragePrice - dAvgPrice >= Settings.Default.SmartEarnTick * Settings.Default.ItemOverTick)
                {
                    if (Settings.Default.SmartLossUnit == 0)
                        dSmartEarn = (dMaxAveragePrice - dAvgPrice) * Settings.Default.SmartLossTick / 100;
                    else dSmartEarn = Settings.Default.SmartLossTick * Settings.Default.ItemOverTick;

                    if (dMaxAveragePrice - dCurPrice >= dSmartEarn)
                    {
                        m_boLiquid = true;
                        this.frmMain.AddLog("[청산]스마트청산 최대수익가:" + string.Format(Settings.Default.PriceFormat, dMaxAveragePrice) + " (설정:" + Settings.Default.SmartLossTick.ToString() + (Settings.Default.SmartLossUnit == 0 ? "%)" : "틱)"));
                        return true;
                    }
#if DEBUG_LOG
                    Trace.TraceInformation("[청산] 스마트청산 최대수익가:{0} (설정:{1}{2})",
                            string.Format(Settings.Default.PriceFormat, dMaxAveragePrice), Settings.Default.SmartLossTick, Settings.Default.SmartLossUnit == 0 ? "%)" : "틱)");
#endif
                }
            }
            else if (tradeType == TRADETYPE.SELL)
            {
                if (Settings.Default.SmartRangePayoff)
                {
                    long lValuation = (long)((dAvgPrice - dMaxAveragePrice) / CurrentSite.CurItemSymbol.OverTick * CurrentSite.CurItemSymbol.ValueTick * CurrentSite.CurItemSymbol.Exchange * orderQty);

                    int nPercent = -1;
                    int nRsi = -1;
                    int[] info = StageRangePercent(RANGETYPE.SmartLoss, lValuation);
                    if (info != null)
                    {
                        nPercent = info[0];
                        nRsi = info[2];
                    }

                    long lLossVal = (long)((dCurPrice - dMaxAveragePrice) / CurrentSite.CurItemSymbol.OverTick * CurrentSite.CurItemSymbol.ValueTick * CurrentSite.CurItemSymbol.Exchange * orderQty);
                    if (nRsi > 0 && dCurRsi < nRsi && nPercent > 0 && lLossVal > 0 && lLossVal > lValuation * nPercent / 100)
                    {
                        m_boLiquid = true;
                        this.frmMain.AddLog(string.Format("[청산] 스마트청산 최대수익가:{0}({1:N0}USD)(설정:{2}%이상), Rsi={3:N2}(설정:{4}이하)",
                            string.Format(Settings.Default.PriceFormat, dMaxAveragePrice), lValuation, nPercent, dCurRsi, nRsi));
                        return true;
                    }
#if DEBUG_LOG
                    Trace.TraceInformation("[청산] 스마트영역청산 최대수익가:{0}({1:N0}USD)(설정:{2}%이상), Rsi={3:N2}(설정:{4}이하)",
                            string.Format(Settings.Default.PriceFormat, dMaxAveragePrice), lValuation, nPercent, dCurRsi, nRsi);
#endif
                }
                else if (Settings.Default.SmartEarnTick >= 0 && Settings.Default.SmartLossTick >= 0
                        && dAvgPrice - dMaxAveragePrice >= Settings.Default.SmartEarnTick * Settings.Default.ItemOverTick)
                {
                    if (Settings.Default.SmartLossUnit == 0)
                        dSmartEarn = (dAvgPrice - dMaxAveragePrice) * Settings.Default.SmartLossTick / 100;
                    else dSmartEarn = Settings.Default.SmartLossTick * Settings.Default.ItemOverTick;

                    if (dCurPrice - dMaxAveragePrice >= dSmartEarn)
                    {
                        m_boLiquid = true;
                        this.frmMain.AddLog("[청산] 스마트청산 최대수익가:" + string.Format(Settings.Default.PriceFormat, dMaxAveragePrice) + " (설정:" + Settings.Default.SmartLossTick.ToString() + (Settings.Default.SmartLossUnit == 0 ? "%)" : "틱)"));
                        return true;
                    }
#if DEBUG_LOG
                    Trace.TraceInformation("[청산] 스마트청산 최대수익가:{0} (설정:{1}{2})",
                            string.Format(Settings.Default.PriceFormat, dMaxAveragePrice), Settings.Default.SmartLossTick, Settings.Default.SmartLossUnit == 0 ? "%)" : "틱)");
#endif
                }
            }

            return false;
        }
        private bool CheckCciPayoff(TRADETYPE tradeType, double dCurCci, double dStartCci, double dMaxCci, double dCurRsi)
        {
            string log = "";
            if (dCurCci == 0 || dMaxCci == 0)
                return false;

            if (tradeType == TRADETYPE.BUY)
            {
                double dRangeCci = dMaxCci - dStartCci;
                double dLossCci = dMaxCci - dCurCci;
                if (Settings.Default.CciRangePayoff)
                {
                    int nRate = -1;
                    int nRsi = -1;
                    int[] info = StageRangePercent(RANGETYPE.CciLoss, (long)dRangeCci);
                    if (info != null)
                    {
                        nRate = info[0];
                        nRsi = info[1];
                    }

                    if (nRsi > 0 && dCurRsi > nRsi && nRate >= 0 && dLossCci >= 0 && dLossCci > dRangeCci * nRate / 100.0)
                    {
                        log = string.Format("[청산] CCI영역청산: CCI={0:N2} ", dCurCci);
                        log += string.Format(", StartCci ={0:N2}, MaxCci={1:N2}, ", dStartCci, dMaxCci) + "(설정:" + nRate.ToString() + "%하락)";
                        log += string.Format(" Rsi={0:N2}(설정:{1}이상)", dCurRsi, nRsi);
                        this.frmMain.AddLog(log);
                        m_boLiquid = true;
                        return true;
                    }
                }
                else if (Settings.Default.CciPayoffValue1 > 0 && dRangeCci >= Settings.Default.CciPayoffValue1
                  && Settings.Default.CciPayoffValue2 > 0 && dLossCci > dRangeCci * Settings.Default.CciPayoffValue2 / 100.0
                  && Settings.Default.CciPayoffValue3 > 0 && dCurRsi > Settings.Default.CciPayoffValue3
                  )
                {
                    log = string.Format("[청산] CCI청산: CCI={0:N2} ", dCurCci);
                    log += string.Format(", StartCci ={0:N2}, MaxCci={1:N2}, (설정: {2}이상 {3}%하락) ", dStartCci, dMaxCci, Settings.Default.CciPayoffValue1, Settings.Default.CciPayoffValue2);
                    log += string.Format(" Rsi={0:N2}(설정:{1}이상)", dCurRsi, Settings.Default.CciPayoffValue3);
                    this.frmMain.AddLog(log);
                    m_boLiquid = true;
                    return true;
                }
            }
            else if (tradeType == TRADETYPE.SELL)
            {
                double dRangeCci = dStartCci - dMaxCci;
                double dLossCci = dCurCci - dMaxCci;
                if (Settings.Default.CciRangePayoff)
                {
                    int nRate = -1;
                    int nRsi = -1;
                    int[] info = StageRangePercent(RANGETYPE.CciLoss, (long)dRangeCci);
                    if (info != null)
                    {
                        nRate = info[0];
                        nRsi = info[2];
                    }

                    if (nRsi > 0 && dCurRsi < nRsi && nRate >= 0 && dLossCci >= 0 && dLossCci > dRangeCci * nRate / 100.0)
                    {
                        log = string.Format("[청산] CCI영역청산: CCI={0:N2} ", dCurCci);
                        log += string.Format(", StartCci ={0:N2}, MaxCci={1:N2}, ", dStartCci, dMaxCci) + "(설정:" + nRate.ToString() + "%하락)";
                        log += string.Format(" Rsi={0:N2}(설정:{1}이하)", dCurRsi, nRsi);
                        this.frmMain.AddLog(log);
                        m_boLiquid = true;
                        return true;
                    }
                }
                else if (Settings.Default.CciPayoffValue1 > 0 && dRangeCci >= Settings.Default.CciPayoffValue1
                  && Settings.Default.CciPayoffValue2 > 0 && dLossCci > dRangeCci * Settings.Default.CciPayoffValue2 / 100.0
                  && Settings.Default.CciPayoffValue3 > 0 && dCurRsi < Settings.Default.CciPayoffValue3
                  )
                {
                    log = string.Format("[청산] CCI청산: CCI={0:N2} ", dCurCci);
                    log += string.Format(", StartCci ={0:N2}, MaxCci={1:N2}, (설정: {2}이상 {3}%하락) ", dStartCci, dMaxCci, Settings.Default.CciPayoffValue1, Settings.Default.CciPayoffValue2);
                    log += string.Format(" Rsi={0:N2}(설정:{1}이하)", dCurRsi, Settings.Default.CciPayoffValue3);
                    this.frmMain.AddLog(log);
                    m_boLiquid = true;
                    return true;
                }

            }

            return false;
        }
        private bool CheckCrossLossPayoff(TRADETYPE tradeType, double dCurPrice, double dAvgPrice, double dCrossAveragePrice, double dCurRsi, double orderQty)
        {
#if DEBUG_LOG
            Trace.TraceInformation("<LogicAuto> CheckCrossLossPayoff AveragePrice = {0}, CrossAveragePrice = {1} ", dAvgPrice, dCrossAveragePrice);
#endif
            double dSmartEarn = 0;
            if (tradeType == TRADETYPE.BUY)
            {
                if (Settings.Default.CrossRangePayoff)
                {
                    long lValuation = (long)((dCrossAveragePrice - dAvgPrice) / CurrentSite.CurItemSymbol.OverTick * CurrentSite.CurItemSymbol.ValueTick * CurrentSite.CurItemSymbol.Exchange * orderQty);

                    int nPercent = -1;
                    int nRsi = -1;
                    int[] info = StageRangePercent(RANGETYPE.CrossLoss, lValuation);
                    if (info != null)
                    {
                        nPercent = info[0];
                        nRsi = info[1];
                    }

                    long lLossVal = (long)((dCrossAveragePrice - dCurPrice) / CurrentSite.CurItemSymbol.OverTick * CurrentSite.CurItemSymbol.ValueTick * CurrentSite.CurItemSymbol.Exchange * orderQty);
                    if (nRsi > 0 && dCurRsi > nRsi && nPercent > 0 && lLossVal > 0 && lLossVal > lValuation * nPercent / 100)
                    {
                        m_boLiquid = true;
                        this.frmMain.AddLog(string.Format("교차점에서 하락청산 교차점:{0}({1:N0}USD)(설정:{2}%이상하락), Rsi={3:N2}(설정:{4}이상) ",
                            string.Format(Settings.Default.PriceFormat, dCrossAveragePrice), lValuation, nPercent, dCurRsi, nRsi));
                        return true;
                    }
#if DEBUG_LOG
                    Trace.TraceInformation(string.Format("교차점에서 하락청산 교차점:{0}({1:N0}USD)(설정:{2}%이상하락), Rsi={3:N2}(설정:{4}이상) ",
                            string.Format(Settings.Default.PriceFormat, dCrossAveragePrice), lValuation, nPercent, dCurRsi, nRsi));
#endif
                }
                else if (Settings.Default.CrossLossTick >= 0)
                {
                    if (Settings.Default.CrossLossUnit == 0)
                        dSmartEarn = (dCrossAveragePrice - dAvgPrice) * Settings.Default.CrossLossTick / 100;
                    else dSmartEarn = Settings.Default.CrossLossTick * Settings.Default.ItemOverTick;

                    if (dCrossAveragePrice - dCurPrice >= dSmartEarn)
                    {
                        m_boLiquid = true;
                        this.frmMain.AddLog("교차점에서 하락청산 교차점:" + string.Format(Settings.Default.PriceFormat, dCrossAveragePrice) + " 설정:" + Settings.Default.CrossLossTick.ToString() + (Settings.Default.CrossLossUnit == 0 ? "%" : "틱"));
                        return true;
                    }
#if DEBUG_LOG
                    Trace.TraceInformation("교차점에서 하락청산 교차점:" + string.Format(Settings.Default.PriceFormat, dCrossAveragePrice) + " 설정:" + Settings.Default.CrossLossTick.ToString() + (Settings.Default.CrossLossUnit == 0 ? "%" : "틱"));
#endif
                }

            }
            else if (tradeType == TRADETYPE.SELL)
            {
                if (Settings.Default.CrossRangePayoff)
                {
                    long lValuation = (long)((dAvgPrice - dCrossAveragePrice) / CurrentSite.CurItemSymbol.OverTick * CurrentSite.CurItemSymbol.ValueTick * CurrentSite.CurItemSymbol.Exchange * orderQty);

                    int nPercent = -1;
                    int nRsi = -1;
                    int[] info = StageRangePercent(RANGETYPE.CrossLoss, lValuation);
                    if (info != null)
                    {
                        nPercent = info[0];
                        nRsi = info[2];
                    }

                    long lLossVal = (long)((dCurPrice - dCrossAveragePrice) / CurrentSite.CurItemSymbol.OverTick * CurrentSite.CurItemSymbol.ValueTick * CurrentSite.CurItemSymbol.Exchange * orderQty);
                    if (nRsi > 0 && dCurRsi < nRsi && nPercent > 0 && lLossVal > 0 && lLossVal > lValuation * nPercent / 100)
                    {
                        m_boLiquid = true;
                        this.frmMain.AddLog(string.Format("교차점에서 하락청산 교차점:{0}({1:N0}USD) (설정:{2}%이상상승), Rsi={3:N2}(설정:{4}이하)",
                            string.Format(Settings.Default.PriceFormat, dCrossAveragePrice), lValuation, nPercent, dCurRsi, nRsi));
                        return true;
                    }
#if DEBUG_LOG
                    Trace.TraceInformation("교차점에서 하락청산 교차점:{0}({1:N0}USD) (설정:{2}%이상상승), Rsi={3:N2}(설정:{4}이하)",
                            string.Format(Settings.Default.PriceFormat, dCrossAveragePrice), lValuation, nPercent, dCurRsi, nRsi);
#endif
                }
                else if (Settings.Default.CrossLossPayoff && Settings.Default.CrossLossTick >= 0 && dCrossAveragePrice > 0)
                {
                    if (Settings.Default.CrossLossUnit == 0)
                        dSmartEarn = (dAvgPrice - dCrossAveragePrice) * Settings.Default.CrossLossTick / 100;
                    else dSmartEarn = Settings.Default.CrossLossTick * Settings.Default.ItemOverTick;

                    if (dCurPrice - dCrossAveragePrice >= dSmartEarn)
                    {
                        m_boLiquid = true;
                        this.frmMain.AddLog("교차점에서 하락청산 교차점:" + string.Format(Settings.Default.PriceFormat, dCrossAveragePrice) + " 설정:" + Settings.Default.CrossLossTick.ToString() + (Settings.Default.CrossLossUnit == 0 ? "%" : "틱"));
                        return true;
                    }
#if DEBUG_LOG
                    Trace.TraceInformation("교차점에서 하락청산 교차점:" + string.Format(Settings.Default.PriceFormat, dCrossAveragePrice) + " 설정:" + Settings.Default.CrossLossTick.ToString() + (Settings.Default.CrossLossUnit == 0 ? "%" : "틱"));
#endif
                }
            }
            return false;
        }

        private bool CheckTradeChange(ref string log)
        {
            log = "";
            if (!Settings.Default.Conc1On && !Settings.Default.Conc2On && !Settings.Default.AdxOn)
                return true;

            bool bChanged = true;
            if (Settings.Default.Conc1On)
            {
                if (Settings.Default.Conc1Min > 0 && Settings.Default.Conc1Cnt > 0)
                {
                    bChanged = false;
                    int nConc = frmMain.GetConcPerMin(Settings.Default.Conc1Min);
#if DEBUG_LOG
                    // Trace.TraceInformation("CheckTradeChange() {0}분당거래{1}(설정:{2}) ", Settings.Default.Conc1Min, nConc, Settings.Default.Conc1Cnt);
#endif
                    if (nConc > Settings.Default.Conc1Cnt)
                    {
                        log = string.Format("{0}분당거래{1}(설정:{2}) ", Settings.Default.Conc1Min, nConc, Settings.Default.Conc1Cnt);
                        bChanged = true;
                    }
                }
            }
            if (Settings.Default.Conc2On && bChanged)
            {
                if (Settings.Default.Conc2Candle > 0 && Settings.Default.Conc2Cnt > 0)
                {
                    bChanged = false;

                    List<CItem> lastCandlelist = frmMain.GetCandleList2(Settings.Default.Conc2Candle + 1, true);

                    if (lastCandlelist.Count >= Settings.Default.Conc2Candle + 1)
                    {
                        int nConcSum = 0;
                        lastCandlelist.ForEach(c => nConcSum += c.Conc);
                        nConcSum -= lastCandlelist[lastCandlelist.Count - 1].Conc;
#if DEBUG_LOG
                        //Trace.TraceInformation("CheckTradeChange() {0}차트 거래량 {1}봉평균{2} 현재:{3}(설정:{4}%) ", Common.GetChartTypeStr(CurrentSite.DChartType),
                        //            Settings.Default.Conc2Candle,
                        //            nConcSum / Settings.Default.Conc2Candle,
                        //            lastCandlelist[lastCandlelist.Count - 1].Conc,
                        //            Settings.Default.Conc2Cnt);
#endif
                        if (lastCandlelist[lastCandlelist.Count - 1].Conc >= nConcSum * Settings.Default.Conc2Cnt / 100 / Settings.Default.Conc2Candle)
                        {
                            log += string.Format("{0}차트 거래량 {1}봉평균{2} 현재:{3}(설정:{4}%) ",
                                Common.GetChartTypeStr(CurrentSite.DChartType),
                                    Settings.Default.Conc2Candle,
                                    nConcSum / Settings.Default.Conc2Candle,
                                    lastCandlelist[lastCandlelist.Count - 1].Conc,
                                    Settings.Default.Conc2Cnt
                                );
                            bChanged = true;
                        }

                    }

                }
            }
            if (Settings.Default.AdxOn && bChanged)
            {
                bChanged = false;
                List<DItem> lastCandlelist = frmMain.GetCandleList(1, true);
#if DEBUG_LOG
                //Trace.TraceInformation("CheckTradeChange() ADX:{0:N2}(설정:{1}이상) ", lastCandlelist[0].Adx, Settings.Default.AdxCnt);
#endif
                if (lastCandlelist.Count > 0 && lastCandlelist[0].Adx > Settings.Default.AdxCnt)
                {
                    log += string.Format("ADX:{0:N2}(설정:{1}이상) ", lastCandlelist[0].Adx, Settings.Default.AdxCnt);
                    bChanged = true;
                }
            }

            return bChanged;
        }
        private TRADETYPE GetTradeCondition(ref string log, TRADETYPE selType)
        {
            if (!Settings.Default.CciOn && !Settings.Default.RsiOn && !Settings.Default.AvgsOn)
                return selType;

            List<DItem> lastCandlelist = frmMain.GetCandleList(2, true);
            if (lastCandlelist.Count < 2)
                return TRADETYPE.NONE;
            if (Settings.Default.CciOn)
            {
#if DEBUG_LOG
                Trace.TraceInformation("GetTradeCondition() CCI:{0:N2}, {1:N2}", lastCandlelist[0].Cci, lastCandlelist[1].Cci);
#endif
                if(selType == TRADETYPE.BUY)
                {
                    if (Settings.Default.BettingType == (int)BETTYPE.BOLINE && Settings.Default.BoOrdType == 1)     //CCI
                    {
                        if (Settings.Default.CciSide1 == 0 && lastCandlelist[0].Cci != 0 && lastCandlelist[1].Cci > Settings.Default.CciRange1 + Settings.Default.CciRange11)
                        {
                            log += string.Format("CCI:{0:N2}(설정:{1}상승 {2}이상)", lastCandlelist[1].Cci, Settings.Default.CciRange1, Settings.Default.CciRange11);
                            return TRADETYPE.BUY;
                        }
                        if (Settings.Default.CciSide2 == 0 && lastCandlelist[0].Cci != 0 && lastCandlelist[1].Cci < Settings.Default.CciRange2 - Settings.Default.CciRange21)
                        {
                            log += string.Format("CCI:{0:N2}(설정:{1}하락 {2}이하)", lastCandlelist[1].Cci, Settings.Default.CciRange2, Settings.Default.CciRange21);
                            return TRADETYPE.BUY;
                        }
                    } else
                    {
                        if (Settings.Default.CciSide1 == 0 && lastCandlelist[0].Cci != 0 && lastCandlelist[1].Cci > Settings.Default.CciRange1)
                        {
                            log += string.Format("CCI:{0:N2}(설정:{1}이상)", lastCandlelist[1].Cci, Settings.Default.CciRange1);
                            return TRADETYPE.BUY;
                        }
                        else if (Settings.Default.CciSide2 == 0 && lastCandlelist[0].Cci != 0 && lastCandlelist[1].Cci < Settings.Default.CciRange2)
                        {
                            log += string.Format("CCI:{0:N2}(설정:{1}이하)", lastCandlelist[1].Cci, Settings.Default.CciRange2);
                            return TRADETYPE.BUY;
                        }
                    }
                    
                } else if (selType == TRADETYPE.SELL)
                {
                    if (Settings.Default.BettingType == (int)BETTYPE.BOLINE && Settings.Default.BoOrdType == 1)     //CCI
                    {
                        if (Settings.Default.CciSide1 == 1 && lastCandlelist[0].Cci != 0 && lastCandlelist[1].Cci > Settings.Default.CciRange1 + Settings.Default.CciRange11)
                        {
                            log += string.Format("CCI:{0:N2}(설정:{1}상승 {2}이상)", lastCandlelist[1].Cci, Settings.Default.CciRange1, Settings.Default.CciRange11);
                            return TRADETYPE.SELL;
                        }
                        if (Settings.Default.CciSide2 == 1 && lastCandlelist[0].Cci != 0 && lastCandlelist[1].Cci < Settings.Default.CciRange2 - Settings.Default.CciRange21)
                        {
                            log += string.Format("CCI:{0:N2}(설정:{1}하락 {2}이하)", lastCandlelist[1].Cci, Settings.Default.CciRange2, Settings.Default.CciRange21);
                            return TRADETYPE.SELL;
                        }
                    } else
                    {
                        if (Settings.Default.CciSide1 == 1 && lastCandlelist[0].Cci != 0 && lastCandlelist[1].Cci > Settings.Default.CciRange1)
                        {
                            log += string.Format("CCI:{0:N2}(설정:{1}이상)", lastCandlelist[1].Cci, Settings.Default.CciRange1);
                            return TRADETYPE.SELL;
                        }
                        else if (Settings.Default.CciSide2 == 1 && lastCandlelist[0].Cci != 0 && lastCandlelist[1].Cci < Settings.Default.CciRange2)
                        {
                            log += string.Format("CCI:{0:N2}(설정:{1}이하)", lastCandlelist[1].Cci, Settings.Default.CciRange2);
                            return TRADETYPE.SELL;
                        }
                    }
                    
                } else
                {
                    if (Settings.Default.BettingType == (int)BETTYPE.BOLINE && Settings.Default.BoOrdType == 1)     //CCI
                    {
                        if (lastCandlelist[0].Cci != 0 && lastCandlelist[1].Cci > Settings.Default.CciRange1 + Settings.Default.CciRange11)
                        {
                            log += string.Format("CCI:{0:N2}(설정:{1}상승 {2}이상)", lastCandlelist[1].Cci, Settings.Default.CciRange1, Settings.Default.CciRange11);
                            return Settings.Default.CciSide1 == 0 ? TRADETYPE.BUY : TRADETYPE.SELL;

                        }
                        else if (lastCandlelist[0].Cci != 0 && lastCandlelist[1].Cci < Settings.Default.CciRange2 - Settings.Default.CciRange21)
                        {
                            log += string.Format("CCI:{0:N2}(설정:{1}하락 {2}이하)", lastCandlelist[1].Cci, Settings.Default.CciRange2, Settings.Default.CciRange21);
                            return Settings.Default.CciSide2 == 0 ? TRADETYPE.BUY : TRADETYPE.SELL;
                        }
                    }
                    else
                    {
                        if (lastCandlelist[0].Cci != 0 && lastCandlelist[1].Cci > Settings.Default.CciRange1)
                        {
                            log += string.Format("CCI:{0:N2}(설정:{1}이상)", lastCandlelist[1].Cci, Settings.Default.CciRange1);
                            return Settings.Default.CciSide1 == 0 ? TRADETYPE.BUY : TRADETYPE.SELL;

                        }
                        else if (lastCandlelist[0].Cci != 0 && lastCandlelist[1].Cci < Settings.Default.CciRange2)
                        {
                            log += string.Format("CCI:{0:N2}(설정:{1}이하)", lastCandlelist[1].Cci, Settings.Default.CciRange2);
                            return Settings.Default.CciSide2 == 0 ? TRADETYPE.BUY : TRADETYPE.SELL;
                        }
                    }
                }
            }
            if (Settings.Default.RsiOn)
            {
#if DEBUG_LOG
                Trace.TraceInformation("GetTradeCondition() RSI:{0:N2}, {1:N2}", lastCandlelist[0].Rsi, lastCandlelist[1].Rsi);
#endif
                if (selType == TRADETYPE.BUY)
                {
                    if(Settings.Default.RsiSide1 == 0 && lastCandlelist[0].Rsi > 0 && lastCandlelist[1].Rsi > Settings.Default.RsiRange1)
                    {
                        return TRADETYPE.BUY;
                    } else if(Settings.Default.RsiSide2 == 0 && lastCandlelist[0].Rsi > 0 && lastCandlelist[1].Rsi < Settings.Default.RsiRange2)
                    {
                        return TRADETYPE.BUY;
                    }
                }
                else if (selType == TRADETYPE.SELL)
                {
                    if (Settings.Default.RsiSide1 == 1 && lastCandlelist[0].Rsi > 0 && lastCandlelist[1].Rsi > Settings.Default.RsiRange1)
                    {
                        return TRADETYPE.SELL;
                    }
                    else if (Settings.Default.RsiSide2 == 1 && lastCandlelist[0].Rsi > 0 && lastCandlelist[1].Rsi < Settings.Default.RsiRange2)
                    {
                        return TRADETYPE.SELL;
                    }
                } else
                {
                    if (lastCandlelist[0].Rsi > 0 && lastCandlelist[1].Rsi > Settings.Default.RsiRange1)
                    {
                        log += string.Format("RSI:{0:N2}(설정:{1}이상)", lastCandlelist[1].Rsi, Settings.Default.RsiRange1);
                        return Settings.Default.RsiSide1 == 0 ? TRADETYPE.BUY : TRADETYPE.SELL;
                    }
                    else if (lastCandlelist[0].Rsi > 0 && lastCandlelist[1].Rsi < Settings.Default.RsiRange2)
                    {
                        log += string.Format("RSI:{0:N2}(설정:{1}이하)", lastCandlelist[1].Rsi, Settings.Default.RsiRange2);
                        return Settings.Default.RsiSide2 == 0 ? TRADETYPE.BUY : TRADETYPE.SELL;
                    }
                }
            }
            if (Settings.Default.AvgsOn)
            {
                if (Settings.Default.AvgsCandle < 1)
                    return TRADETYPE.NONE;

                List<DItem> lastCandlelist2 = frmMain.GetCandleList(Settings.Default.AvgsCandle, false);
                if (lastCandlelist2.Count < Settings.Default.AvgsCandle)
                {
                    return TRADETYPE.NONE;
                }

                int upDown = lastCandlelist2[0].AvgPos;
                if (upDown == 0)
                    return TRADETYPE.NONE;

                if (selType == TRADETYPE.BUY)
                {
                    if (upDown == 1 && Settings.Default.AvgsSide1 == 0)
                    {
                        if (lastCandlelist2.Count<DItem>(d => d.AvgPos == upDown) >= lastCandlelist2.Count)
                        {
                            log += string.Format("이평S1:{0}봉 U ", Settings.Default.AvgsCandle); //200일선 위상태
                            return TRADETYPE.BUY;
                        }
                    }
                    else if (upDown == -1 && Settings.Default.AvgsSide2 == 0)
                    {
                        if (lastCandlelist2.Count<DItem>(d => d.AvgPos == upDown) >= lastCandlelist2.Count)
                        {

                            log += string.Format("이평S1:{0}봉 D ", Settings.Default.AvgsCandle); //200일선 아래
                            return TRADETYPE.BUY;
                        }
                    }
                } else if (selType == TRADETYPE.SELL)
                {
                    if (upDown == 1 && Settings.Default.AvgsSide1 == 1)
                    {
                        if (lastCandlelist2.Count<DItem>(d => d.AvgPos == upDown) >= lastCandlelist2.Count)
                        {
                            log += string.Format("이평S1:{0}봉 U ", Settings.Default.AvgsCandle); //200일선 위상태
                            return TRADETYPE.SELL;
                        }
                    }
                    else if (upDown == -1 && Settings.Default.AvgsSide2 == 1)
                    {
                        if (lastCandlelist2.Count<DItem>(d => d.AvgPos == upDown) >= lastCandlelist2.Count)
                        {

                            log += string.Format("이평S1:{0}봉 D ", Settings.Default.AvgsCandle); //200일선 아래
                            return TRADETYPE.SELL;
                        }
                    }
                } else
                {
                    if (upDown == 1)
                    {
                        if (lastCandlelist2.Count<DItem>(d => d.AvgPos == upDown) >= lastCandlelist2.Count)
                        {
                            log += string.Format("이평S1:{0}봉 U ", Settings.Default.AvgsCandle); //200일선 위상태
                            return Settings.Default.AvgsSide1 == 0 ? TRADETYPE.BUY : TRADETYPE.SELL;
                        }
                    }
                    else if (upDown == -1)
                    {
                        if (lastCandlelist2.Count<DItem>(d => d.AvgPos == upDown) >= lastCandlelist2.Count)
                        {

                            log += string.Format("이평S1:{0}봉 D ", Settings.Default.AvgsCandle); //200일선 아래
                            return Settings.Default.AvgsSide2 == 0 ? TRADETYPE.BUY : TRADETYPE.SELL;
                        }
                    }
                }
            }
            return TRADETYPE.NONE;
        }
        private bool CheckBollPayoff(TRADETYPE tradeType, DItem lastCandle, ref string log)
        {
            if (!Settings.Default.BollPayoff)
                return true;

            bool bPayoff = true;
            if (Settings.Default.BollPayoff)
            {
                if(tradeType == TRADETYPE.BUY)
                {
                    if (lastCandle.BollPerb >= Settings.Default.BollPayoffUp)
                    {
                        log += string.Format("볼린저밴드 %B:{0:N2}(설정:{1}이상)", lastCandle.BollPerb, Settings.Default.BollPayoffUp);
                        bPayoff = true;
                    }
                    else bPayoff = false;

                }
                else if (tradeType == TRADETYPE.SELL)
                {
                    if (lastCandle.BollPerb <= Settings.Default.BollPayoffDown)
                    {
                        log += string.Format("볼린저밴드 %B:{0:N2}(설정:{1}이하)", lastCandle.BollPerb, Settings.Default.BollPayoffDown);
                        bPayoff = true;
                    }
                    else bPayoff = false;

                }
            }

            if (Settings.Default.MacdPayoff && bPayoff)
            {
                float fMacd = lastCandle.MacdVal - lastCandle.MacdSig;
                if (tradeType == TRADETYPE.BUY)
                {
                    if (fMacd >= 0)
                    {
                        log += string.Format(", MACD 양수({0:N2})", fMacd);
                        bPayoff = true;
                    }
                    else bPayoff = false;

                }
                else if (tradeType == TRADETYPE.SELL)
                {
                    if (fMacd < 0)
                    {
                        log += string.Format(", MACD 음수({0:N2})", fMacd);
                        bPayoff = true;
                    }
                    else bPayoff = false;
                }
            }
            return bPayoff;
        }
        private bool TraceIntervalLog()
        {
            if (Math.Abs(Environment.TickCount - m_tickStateLog) < 60000)
                return false;
            m_tickStateLog = Environment.TickCount;

            string log = "";

            if (Settings.Default.Conc1On)
            {
                if (Settings.Default.Conc1Min > 0)
                {
                    int nConc = frmMain.GetConcPerMin(Settings.Default.Conc1Min);
                    log += string.Format(" {0}분당 거래량:{1} |", Settings.Default.Conc1Min, nConc);
                }

            }
            if (Settings.Default.Conc2On)
            {
                if (Settings.Default.Conc2Candle > 0)
                {
                    List<CItem> lastCItemlist = frmMain.GetCandleList2(Settings.Default.Conc2Candle + 1, true);
                    if (lastCItemlist.Count >= Settings.Default.Conc2Candle + 1)
                    {
                        int nConcSum = 0;
                        lastCItemlist.ForEach(c => nConcSum += c.Conc);
                        nConcSum -= lastCItemlist[lastCItemlist.Count - 1].Conc;

                        log += string.Format(" {0}차트 {1}봉 평균거래량:{2} ",
                            Common.GetChartTypeStr(CurrentSite.DChartType),
                            Settings.Default.Conc2Candle,
                                nConcSum / Settings.Default.Conc2Candle);
                    }
                }
            }

            if (log.Length > 0)
            {
                this.frmMain.AddStateLog(log);
            }

            return true;
        }
        private bool TraceRealtimeLog()
        {
            if (Math.Abs(Environment.TickCount - m_tickValueLog) < 300)
                return false;
            m_tickValueLog = Environment.TickCount;

            string log = "";
            string sValue = "";
            List<DItem> lasDItemlist = null;
            this.frmMain.AppendValue(log);

            lasDItemlist = frmMain.GetCandleList(1, true);
            if (lasDItemlist.Count > 0)
            {
                if (lasDItemlist[0].Adx > 0)
                {
                    sValue = string.Format("ADX:{0:N2} |", lasDItemlist[0].Adx);
                    this.frmMain.AppendValue(sValue);
                    log += sValue;
                    sValue = "";
                }

                if (lasDItemlist[0].Cci != 0)
                {
                    sValue = string.Format(" CCI:{0:N2} |", lasDItemlist[0].Cci);
                    if (lasDItemlist[0].Cci < 0)
                        this.frmMain.AppendValue(sValue, new object[1]);
                    else
                        this.frmMain.AppendValue(sValue, new object[2]);
                    log += sValue;
                    sValue = "";
                }

                if (lasDItemlist[0].Rsi > 0)
                {
                    sValue = string.Format(" RSI:{0:N2} |", lasDItemlist[0].Rsi);
                    if (lasDItemlist[0].Rsi < 50)
                        this.frmMain.AppendValue(sValue, new object[1]);
                    else
                        this.frmMain.AppendValue(sValue, new object[2]);
                    log += sValue;
                    sValue = "";
                }

                if (lasDItemlist[0].BollAvg != 0)
                {
                    sValue = string.Format(" 볼%B:{0:N2} |", lasDItemlist[0].BollPerb);
                    if (lasDItemlist[0].BollPerb < 0.5)
                        this.frmMain.AppendValue(sValue, new object[1]);
                    else
                        this.frmMain.AppendValue(sValue, new object[2]);
                    log += sValue;
                    sValue = "";
                }

                if (lasDItemlist[0].MacdSig != 0)
                {
                    sValue = string.Format(" MACD:{0:N2} |", lasDItemlist[0].MacdVal-lasDItemlist[0].MacdSig);
                    if (lasDItemlist[0].MacdVal - lasDItemlist[0].MacdSig < 0)
                        this.frmMain.AppendValue(sValue, new object[1]);
                    else
                        this.frmMain.AppendValue(sValue, new object[2]);
                    log += sValue;
                    sValue = "";
                }
            }

            if (Settings.Default.AvgsCandle > 0)
            {
                List<DItem> lastCandlelist2 = frmMain.GetCandleList(Settings.Default.AvgsCandle, false);
                if (lastCandlelist2.Count >= Settings.Default.AvgsCandle)
                {
                    int upDown = lastCandlelist2[0].AvgPos;

                    if (upDown == 1)
                    {
                        if (lastCandlelist2.Count<DItem>(d => d.AvgPos == upDown) >= lastCandlelist2.Count)
                        {
                            sValue += string.Format(" 이평S1:{0}봉 U ", Settings.Default.AvgsCandle); //200일선 위상태
                        }
                    }
                    else if (upDown == -1)
                    {
                        if (lastCandlelist2.Count<DItem>(d => d.AvgPos == upDown) >= lastCandlelist2.Count)
                        {

                            sValue += string.Format(" 이평S1:{0}봉 D ", Settings.Default.AvgsCandle);  //200일선 아래상태
                        }
                    }
                }
            }

            if (sValue.Length > 0)
            {
                this.frmMain.AppendValue(sValue);
                log += sValue;
            }
            if (log.Length > 0 && Math.Abs(Environment.TickCount - m_tickValueWLog) > 3000)
            {
                m_tickValueWLog = Environment.TickCount;
                this.frmMain.AddValueLog(log);
            }
            return true;
        }

        private bool NeedToStop(bool bLog = false)
        {
            // no auto mode
            if (!Settings.Default.IsAutoMode)
                return false;

            if (_currentSite == null || _currentSite.CurrentUserAccount == null)
                return false;

            if (!_currentSite.ValuationList.Any<ValuationInfo>())
                return false;

            if (_currentSite.OrderList != null && _currentSite.OrderList.Count > 0)
                return false;

            if (m_forceLiquid)
            {
                if (bLog)
                    this.frmMain.AddLog("[청산] 강제청산되었습니다."); //OnLogEvent
                return true;
            }

            double valuation = 0;
            lock (_currentSite.ValuationList)
            {
                valuation = _currentSite.ValuationList[0].CurrentProfit;
            }

            if (Settings.Default.EarnStop && Settings.Default.EarnStopMoney >= 0)
            {
                if (valuation >= Settings.Default.EarnStopMoney)
                    return true;
            }

            if (Settings.Default.LossStop && Settings.Default.LossStopMoney >= 0)
            {
                if (valuation <= -Settings.Default.LossStopMoney)
                    return true;
            }

            if (Settings.Default.ProfitStop && Settings.Default.ProfitStopRate > 0)
            {
                if (valuation > 0 && m_maxProfit > 0 && valuation < m_maxProfit * (100 - Settings.Default.ProfitStopRate) / 100)
                    return true;
            }

            return false;
        }
        private bool NeedToAuto()
        {
            if (!Settings.Default.IsAutoMode && Settings.Default.AutoReserveOn)
            {
                DateTime dtNow = DateTime.Now;
                if (AppConfig._DtDelay != 0)
                {
                    dtNow = dtNow.AddSeconds(AppConfig._DtDelay);
                }
                DateTime dtToday = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day);
                if (dtNow >= dtToday + Settings.Default.AutoReserveTime.TimeOfDay && dtNow <= dtToday.AddSeconds(3) + Settings.Default.AutoReserveTime.TimeOfDay)
                    return true;
            }
            return false;
        }
        private bool DoOrder(double dQuantity)
        {
            if (_tradeTypeToOrder == TRADETYPE.NONE)
                return false;

            if (_currentSite == null || _currentSite.QuoteList == null || _currentSite.Current == null)
                return false;
            double currentPrice = 0.0;
            lock (_currentSite.Current)
            {
                currentPrice = _currentSite.Current.CurrentPrice;
            }
            QuoteInfo quoteInfo = null;
            if (_currentSite.Type == SITETYPE.CMG)
            {
                if(_tradeTypeToOrder == TRADETYPE.SELL)
                    currentPrice = _currentSite.Current.CurrentPrice2;

                quoteInfo = new QuoteInfo
                {
                    Price = currentPrice
                };
            }
            else {
                lock (_currentSite.QuoteList)
                {
                    quoteInfo = _currentSite.QuoteList.FirstOrDefault(q => Math.Abs(q.Price - currentPrice) < 1E-06);
                }
                if (quoteInfo == null)
                    return false;
            }

            switch (_tradeTypeToOrder)
            {
                case TRADETYPE.BUY:
                    return _currentSite.DoBuyOrder(quoteInfo, dQuantity, Settings.Default.OrderType == 0);
                case TRADETYPE.SELL:
                    return _currentSite.DoSellOrder(quoteInfo, dQuantity, Settings.Default.OrderType == 0);
            }

            return false;
        }

        private bool DoCancel()
        {
            if (_orderToCancel == null)
                return false;
            if (_orderToCancel.OrderType == "미체결")
                return _currentSite.CancelOrder(_orderToCancel);
            if(_currentSite.Type == SITETYPE.CMG)
            {
                _reorderToCancel = false;
            }
            else if (_reorderToCancel && ((Settings.Default.BettingType == (int)BETTYPE.CROSS && Settings.Default.Reorder)
                                         || Settings.Default.BettingType == (int)BETTYPE.BOLINE
                                         || Settings.Default.BettingType == (int)BETTYPE.BOT1))
            {
                string quantity;
                double dQuantity = 0;
                lock (_objLock)
                {
                    _reorderToCancel = false;
                }
                if (Common.ExtractString(out quantity, _orderToCancel.Qty, "[", "]") < 0)
                    return false;

                if (!double.TryParse(quantity, out dQuantity))
                {
                    return false;
                }
                else if (_orderToCancel.TradeType == TRADETYPE.BUY)     //매수
                {
                    _tradeTypeToOrder = TRADETYPE.SELL;
                    lock (_objLock)
                    {
                        m_tickOrder = Environment.TickCount;
                    }
                    if (DoOrder(dQuantity * 2))
                    {
                        return true;
                    }
                }
                else if (_orderToCancel.TradeType == TRADETYPE.SELL)    //매도
                {
                    _tradeTypeToOrder = TRADETYPE.BUY;
                    lock (_objLock)
                    {
                        m_tickOrder = Environment.TickCount;
                    }
                    if (DoOrder(dQuantity * 2))
                    {
                        return true;
                    }
                }
                else return false;

            }

            return _currentSite.LiquidateOrder(_orderToCancel);
        }

        private TRADETYPE SelectTradeType(ref string tLog)
        {
            bool bNeedLast = false;
            int nCandleCnt = Settings.Default.BettingCandleCount;
            if (Settings.Default.BettingType == (int)BETTYPE.UPDOWN)
            {
                nCandleCnt = Settings.Default.BettingCandleCount + 1;
            }
            if (Settings.Default.BettingType == (int)BETTYPE.CROSS || Settings.Default.BettingType == (int)BETTYPE.HYBRID)
            {
                bNeedLast = Settings.Default.BettingCandleComplete == 0;
                nCandleCnt = 2;
            }
            else if (Settings.Default.BettingType == (int)BETTYPE.BOLINE || Settings.Default.BettingType == (int)BETTYPE.BOT1)
            {
                bNeedLast = true;
            }

            if (nCandleCnt < 1)
                return TRADETYPE.NONE;

            List<DItem> lastCandlelist = frmMain.GetCandleList(nCandleCnt, bNeedLast);

            if (lastCandlelist.Count < nCandleCnt)
                return TRADETYPE.NONE;
            string log = "[주문] ";

            TRADETYPE trade_type = TRADETYPE.NONE;

            if (Settings.Default.BettingType == (int)BETTYPE.EQUIVALENT)           //Check Equivalent Candle 
            {
                DItem lastCandle = lastCandlelist.Last<DItem>();
                if (lastCandle.Result == RESULTSTATE.IGNORE)
                    return TRADETYPE.NONE;

                if (lastCandlelist.Count<DItem>(d => d.Result == lastCandle.Result) < Settings.Default.BettingCandleCount)
                    return TRADETYPE.NONE;

                trade_type = lastCandle.Result == RESULTSTATE.BUY ? TRADETYPE.BUY : TRADETYPE.SELL;

            }
            else if (Settings.Default.BettingType == (int)BETTYPE.UPDOWN)               //Check Moving Average Line
            {
                CH_AVGTYPE avgType = (CH_AVGTYPE)Settings.Default.AvgType;
                float fTickDiff = Math.Abs(lastCandlelist.First().GetAvgVal(avgType) - lastCandlelist.Last().GetAvgVal(avgType));
                float fTickConf = Settings.Default.ItemOverTick * Settings.Default.BettingTickCount;

                int iFirstIdx = lastCandlelist.First().Index;
#if DEBUG_LOG
                Trace.TraceInformation("SelectTradeType() BETTYPE.UPDOWN 틱={0:N2}(설정:이평선={1}, {2}개 틱={3})", fTickDiff, (int)avgType, Settings.Default.BettingCandleCount, fTickConf);
#endif
                if (fTickDiff >= fTickConf && lastCandlelist.Count<DItem>(d => d.GetTrendUp(avgType, iFirstIdx) == CH_TRENDTYPE.UP) >= Settings.Default.BettingCandleCount + 1)
                {
                    log += String.Format("틱={0:N2}(설정:{1}개 틱={2})", fTickDiff, Settings.Default.BettingCandleCount, fTickConf);
                    trade_type = TRADETYPE.BUY;
                }
                else if (fTickDiff >= fTickConf && lastCandlelist.Count<DItem>(d => d.GetTrendDown(avgType, iFirstIdx) == CH_TRENDTYPE.DOWN) >= Settings.Default.BettingCandleCount + 1)
                {
                    log += String.Format("틱={0:N2}(설정:{1}개 틱={2})", fTickDiff, Settings.Default.BettingCandleCount, fTickConf);
                    trade_type = TRADETYPE.SELL;
                }
                else trade_type = TRADETYPE.NONE;
            }
            else if (Settings.Default.BettingType == (int)BETTYPE.CROSS || Settings.Default.BettingType == (int)BETTYPE.HYBRID)
            {
                string logTrade = "";
                bool bTradeChanged = CheckTradeChange(ref logTrade);
                if (bTradeChanged)
                {

                    if (logTrade.Length > 0)
                    {
                        log += logTrade;
#if DEBUG_LOG
                        Trace.TraceInformation(logTrade);
#endif
                    }
                    logTrade = "";

                    CH_TRENDTYPE trend_type = lastCandlelist.Last().GetCrossTrend((CH_AVGTYPE)Settings.Default.CrossAvgLine1, (CH_AVGTYPE)Settings.Default.CrossAvgLine2);
                    if (trend_type == CH_TRENDTYPE.UP)
                    {
                        trade_type = GetTradeCondition(ref logTrade, TRADETYPE.BUY);
#if DEBUG_LOG
                        if (logTrade.Length > 0) 
                            Trace.TraceInformation(logTrade);
#endif
                        if (trade_type != TRADETYPE.NONE)
                        {
                            if (logTrade.Length > 0)
                                log += logTrade;
                        }
                    }
                    else if (trend_type == CH_TRENDTYPE.DOWN)
                    {
                        trade_type = GetTradeCondition(ref logTrade, TRADETYPE.SELL);
#if DEBUG_LOG
                        if (logTrade.Length > 0)
                            Trace.TraceInformation(logTrade);
#endif
                        if (trade_type != TRADETYPE.NONE)
                        {
                            if (logTrade.Length > 0)
                                log += logTrade;
                        }
                    }
                    else trade_type = TRADETYPE.NONE;
                }
                else trade_type = TRADETYPE.NONE;

            }
            else if (Settings.Default.BettingType == (int)BETTYPE.BOLINE || Settings.Default.BettingType == (int)BETTYPE.BOT1)           //Check Equivalent Candle 
            {
                if (Settings.Default.BoOrdType == 1) //CCI Mode
                {
                    string logTrade = "";
                    bool bTradeChanged = CheckTradeChange(ref logTrade);
                    if (bTradeChanged)
                    {
                        if (logTrade.Length > 0)
                        {
                            log += logTrade;
#if DEBUG_LOG
                            Trace.TraceInformation(logTrade);
#endif
                        }
                        logTrade = "";
                        trade_type = GetTradeCondition(ref logTrade, TRADETYPE.NONE);
#if DEBUG_LOG
                        if (logTrade.Length > 0)
                            Trace.TraceInformation(logTrade);
#endif
                        
                        if (trade_type != TRADETYPE.NONE)
                        {
                            if (logTrade.Length > 0)
                                log += logTrade;
                        }
                    }

                }
                else
                {
                    DItem firstCandle = lastCandlelist.First<DItem>();
                    if (firstCandle.Bos[0] == firstCandle.Bos[1] || (Settings.Default.BettingEnter && m_boLiquid))
                    {
                        string logTrade = "";
                        bool bTradeChanged = CheckTradeChange(ref logTrade);
                        if (logTrade.Length > 0)
                        {
                            log += logTrade;
#if DEBUG_LOG
                            Trace.TraceInformation(logTrade);
#endif
                        }
                        logTrade = "";
                        if (lastCandlelist.Count<DItem>(d => d.Est_Type == RESULTSTATE.BUY) >= lastCandlelist.Count && bTradeChanged)
                        {
                            trade_type = GetTradeCondition(ref logTrade, TRADETYPE.BUY);
#if DEBUG_LOG
                            if (logTrade.Length > 0)
                                Trace.TraceInformation(logTrade);
#endif
                            if (trade_type != TRADETYPE.NONE)
                            {
                                if (logTrade.Length > 0)
                                    log += logTrade;
                            }

                        }
                        else if (lastCandlelist.Count<DItem>(d => d.Est_Type == RESULTSTATE.SELL) >= lastCandlelist.Count && bTradeChanged)
                        {
                            trade_type = GetTradeCondition(ref logTrade, TRADETYPE.SELL);
#if DEBUG_LOG
                            if (logTrade.Length > 0)
                                Trace.TraceInformation(logTrade);
#endif
                            if (trade_type != TRADETYPE.NONE)
                            {
                                if (logTrade.Length > 0)
                                    log += logTrade;
                            }

                        }
                    }
                }

            }

            if (trade_type != TRADETYPE.NONE && Settings.Default.OrderSelectOn)
            {
                if (Settings.Default.OrderSelectType == 1) //매수만
                {
                    if (trade_type == TRADETYPE.SELL)
                        trade_type = TRADETYPE.NONE;

                }
                else if (Settings.Default.OrderSelectType == 2) //매도만
                {
                    if (trade_type == TRADETYPE.BUY)
                        trade_type = TRADETYPE.NONE;
                }
            }

            if (trade_type != TRADETYPE.NONE)
            {
                if (log.Length > 0)
                    tLog = log;
                    //this.frmMain.AddLog(log);
                m_boLiquid = false;

            }
            return trade_type;
        }
    }
}
