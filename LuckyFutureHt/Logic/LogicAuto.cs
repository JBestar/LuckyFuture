using ChartCtrl;
using LuckyFutureLib.Include;
using LuckyFuture.Models.ValueObjects;
using LuckyFuture.Properties;
using LuckyFuture.Site;
using LuckyFuture.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace LuckyFuture.Logic
{
	
    internal class LogicAuto: StageThreadEx
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

		private int m_tickOrder = 0;
        public int m_tickCancel = 0;
		private bool m_boLiquid = false;
		private bool m_ForceLiquid = false;
        private double m_maxProfit = 0;
        private int m_tickStateLog = 0;
        private int m_tickValueLog = 0;

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
					
					if (!bAutoStop && _signalSite != null) {
						Thread.Sleep(3000);
						if(!_signalSite.Start())
							bAutoStop = true;
					} 

					if(!bAutoStop){
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
						m_ForceLiquid = false;

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

			if (NoticeEvent!= null)
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
                case SITETYPE.DREAM:
                    _currentSite = new SiteReantek(SITETYPE.DREAM);
                    break;
                case SITETYPE.TOPASSET:
                    _currentSite = new SiteReantek(SITETYPE.TOPASSET);
                    break;
                case SITETYPE.KIWOOM:
                    _currentSite = new KFOpen(axKFOpenAPI);
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
				if(!Settings.Default.IsAutoMode)
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
//             string logTrade = "";
//             bool bTradeChanged = CheckTradeChange(ref logTrade);
// 			if(logTrade.Length > 0)
// 				this.frmMain.AddLog(logTrade);

			// no auto mode
			if (!Settings.Default.IsAutoMode)
				return false;

			// has order list
			if (_currentSite == null || (_currentSite.OrderList != null && _currentSite.OrderList.Count > 0))
				return false;

			lock (_objLock)
			{

                if (Math.Abs(Environment.TickCount - m_tickCancel) <= _delayOrder) //3000
                    return false;
                if (Math.Abs(Environment.TickCount - m_tickOrder) <= _delayOrder) //10000
					return false;
			}

            _tradeTypeToOrder = SelectTradeType(); 

			return _tradeTypeToOrder != TRADETYPE.NONE;
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
            if (Settings.Default.BettingType == (int)BETTYPE.UPDOWN)
			{
				lastCandlelist = frmMain.GetCandleList(Settings.Default.CandlePayoffCount);

				if (lastCandlelist.Count < Settings.Default.CandlePayoffCount)
					return false;

				avgType = (CH_AVGTYPE)Settings.Default.AvgType;
				fTickDiff = Math.Abs(lastCandlelist.First().GetAvgVal(avgType) - lastCandlelist.Last().GetAvgVal(avgType));
				fTickConf = Settings.Default.ItemOverTick * Settings.Default.TickPayoffCount;
				iFirstIdx = lastCandlelist.First().Index;
			} else if(Settings.Default.BettingType == (int)BETTYPE.CROSS && Settings.Default.Reorder)
            {
                lastCandlelist = frmMain.GetCandleList(2, Settings.Default.BettingCandleComplete == 0);/*false*/
				if (lastCandlelist.Count < 2)
                    return false;
            }
			else if (Settings.Default.BettingType == (int)BETTYPE.BOLINE || Settings.Default.BettingType == (int)BETTYPE.CCI)
            {
                lastCandlelist = frmMain.GetCandleList(Settings.Default.BettingCandleCount, true);
				if (lastCandlelist.Count < Settings.Default.BettingCandleCount) 
					return false;
                dCurCci = lastCandlelist.Last().Cci;
            }
            else if (Settings.Default.BettingType == (int)BETTYPE.HYBRID)
            {
                lastCandlelist = frmMain.GetCandleList(1, Settings.Default.BettingCandleComplete == 0);
                if (lastCandlelist.Count < 1)
                    return false;
            }

            int nNowTick = Environment.TickCount;

            double dDeltaTick = 0, dAvgPrice = 0, dCurPrice = 0;
			lock (_currentSite.OrderList)
			{
				_orderToCancel = _currentSite.OrderList.FirstOrDefault<OrderInfo>(
					delegate (OrderInfo o)
					{
                        if (o.Valuation == null)
                            return false;


						if (o.OrderType == "미체결")
						{
							if (Settings.Default.OrderStop && Settings.Default.OrderStopDelay >= 0 && o.OrderTime > 0)
							{
								if (nNowTick - o.OrderTime >= Settings.Default.OrderStopDelay * 1000)
									return true;
							}
							return false;
						} else { //체결

                            if (o.MaxCciPrice < dCurCci)
                                o.MaxCciPrice = dCurCci;

                            long valuation = _currentSite.ValuationList[0].CurrentProfit;
                            if (valuation > 0)
                            {
                                if (Settings.Default.LiquidStop && Settings.Default.EarnStop && Settings.Default.EarnStopMoney >= 0 && valuation >= Settings.Default.EarnStopMoney * 10000)
                                {
                                    m_ForceLiquid = true;
                                    this.frmMain.AddLog(string.Format("정지 실시간수익: {0:N0}원 익절:{1}만원",
										valuation,
										Settings.Default.EarnStopMoney.ToString()));
                                    return true;
                                }
                            }
                            else if (valuation < 0)
                            {
                                if (Settings.Default.LossStop && Settings.Default.LossStopMoney >= 0 && valuation <= -Settings.Default.LossStopMoney * 10000)
                                {
                                    m_ForceLiquid = true;
                                    this.frmMain.AddLog(string.Format("정지 실시간수익: {0:N0}원 손절:{1}만원",
                                        valuation,
                                        Settings.Default.LossStopMoney.ToString()));
                                    return true;
                                }
                            }

                            if (Settings.Default.ProfitStop && m_maxProfit > 0 && valuation > 0)
                            {

                                if (valuation < m_maxProfit * (100 - Settings.Default.ProfitStopRate) / 100)
                                {
                                    m_ForceLiquid = true;
                                    this.frmMain.AddLog(string.Format("정지 현재 실시간수익: {0:N0}원 실현손익:{1:N0}원의 {2}%하락",
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

							if (Settings.Default.BettingType == (int)BETTYPE.EQUIVALENT ||  Settings.Default.BettingType == (int)BETTYPE.CROSS)
							{

								if (Settings.Default.BettingType == (int)BETTYPE.CROSS && Settings.Default.Reorder)
								{
									if (o.TradeType == TRADETYPE.BUY)   //매수
									{
										if (lastCandlelist.Last().GetCrossTrend(CH_AVGTYPE.LINE_1, CH_AVGTYPE.LINE_2) == CH_TRENDTYPE.DOWN)
										{
											if (!Settings.Default.OrderSelectOn || (Settings.Default.OrderSelectOn && Settings.Default.OrderSelectType == 0))
												_reorderToCancel = true;
											return true;
										}
									}
									else if (o.TradeType == TRADETYPE.SELL) //매도
									{
										if (lastCandlelist.Last().GetCrossTrend(CH_AVGTYPE.LINE_1, CH_AVGTYPE.LINE_2) == CH_TRENDTYPE.UP)
										{
											if (!Settings.Default.OrderSelectOn || (Settings.Default.OrderSelectOn && Settings.Default.OrderSelectType == 0))
												_reorderToCancel = true;
											return true;
										}
									}
								}

								if (o.TradeType == TRADETYPE.BUY)
								{
									if (Settings.Default.EarnPayoff && Settings.Default.EarnPayoffMoney >= 0
										&& dDeltaTick >= Settings.Default.EarnPayoffMoney * Settings.Default.ItemOverTick)
										return true;
									if (Settings.Default.LossPayoff)
                                    {
                                        if (CheckLossPayoff(o.TradeType, dCurPrice, dAvgPrice, o.MaxAveragePrice, o.OrderQty))
                                            return true;
                                    }

									if (Settings.Default.SmartLossPayoff)
									{
										if (CheckSmartLossPayoff(o.TradeType, dCurPrice, dAvgPrice, o.MaxAveragePrice, o.OrderQty))
											return true;
									}
								}
								else if (o.TradeType == TRADETYPE.SELL) //매도
								{
									if (Settings.Default.EarnPayoff && Settings.Default.EarnPayoffMoney >= 0
										&& dDeltaTick <= -Settings.Default.EarnPayoffMoney * Settings.Default.ItemOverTick)
										return true;
                                    if (Settings.Default.LossPayoff)
                                    {
                                        if (CheckLossPayoff(o.TradeType, dCurPrice, dAvgPrice, o.MaxAveragePrice, o.OrderQty))
                                            return true;
                                    }

                                    if (Settings.Default.SmartLossPayoff)
                                    {
                                        if (CheckSmartLossPayoff(o.TradeType, dCurPrice, dAvgPrice, o.MaxAveragePrice, o.OrderQty))
                                            return true;
                                    }
                                }
							}
							else if (Settings.Default.BettingType == (int)BETTYPE.UPDOWN)
							{
								if (o.TradeType == TRADETYPE.BUY)
								{
									if (fTickDiff >= fTickConf && lastCandlelist.Count<DItem>(d => d.GetTrendDown(avgType, iFirstIdx) == CH_TRENDTYPE.DOWN) >= Settings.Default.CandlePayoffCount)
										return true;
								}
								else if (o.TradeType == TRADETYPE.SELL) //매도
								{
									if (fTickDiff >= fTickConf && lastCandlelist.Count<DItem>(d => d.GetTrendUp(avgType, iFirstIdx) == CH_TRENDTYPE.UP) >= Settings.Default.CandlePayoffCount)
										return true;
								}
							}
							else if (Settings.Default.BettingType == (int)BETTYPE.BOLINE)           //Check Equivalent Candle 
							{
								string logTrade = "" ; 
								bool bTradeChanged = CheckTradeChange(ref logTrade);
								if (o.TradeType == TRADETYPE.BUY)   //매수
								{
									if ( lastCandlelist.Count<DItem>(d => d.Est_Type == RESULTSTATE.SELL) >= 1 //lastCandlelist.Count 
										&& lastCandlelist.Last().Orders.Count < 1 && (o.CrossAveragePrice == 0 || o.CrossAveragePrice < dCurPrice)/* && bTradeChanged*/)
									{
										o.CrossAveragePrice = dCurPrice; //교차점에서 현재가
										Trace.TraceInformation("<LogicAuto> BUY AveragePrice = {0}, CrossAveragePrice = {1} ", dAvgPrice, o.CrossAveragePrice);
									}

									if ((Settings.Default.EarnPayoff && Settings.Default.EarnPayoffMoney >= 0
										&& dDeltaTick >= Settings.Default.EarnPayoffMoney * Settings.Default.ItemOverTick)
											|| (!Settings.Default.EarnPayoff && dDeltaTick >= 0))
									{
										if (Settings.Default.EarnPayoff && Settings.Default.ForceEarnPayoff)        //강제수익청산
										{
                                            log = "강제청산 수익:" + Math.Abs(dDeltaTick) / Settings.Default.ItemOverTick + "틱";
											if(Settings.Default.EarnPayoff)
												log += "(설정:" + Settings.Default.EarnPayoffMoney.ToString() + "틱)";
											this.frmMain.AddLog(log);
											m_boLiquid = true;
											return true;
										}
										else if (lastCandlelist.Count<DItem>(d => d.Est_Type == RESULTSTATE.SELL) >= lastCandlelist.Count && bTradeChanged)
										{
											if(logTrade.Length > 0)
                                                this.frmMain.AddLog(logTrade);

                                            if (!Settings.Default.OrderSelectOn || (Settings.Default.OrderSelectOn && Settings.Default.OrderSelectType==0))
											{
                                                log = "수익:" + Math.Abs(dDeltaTick) / Settings.Default.ItemOverTick + "틱";
                                                if (Settings.Default.EarnPayoff)
                                                    log += "(설정:" + Settings.Default.EarnPayoffMoney.ToString() + "틱)";
                                                this.frmMain.AddLog(log);
                                                lock (_objLock)
												{
													_reorderToCancel = true;
												}
											}
											
											return true;

										}
									}

                                    if (Settings.Default.LossPayoff)
                                    {
                                        if (CheckLossPayoff(o.TradeType, dCurPrice, dAvgPrice, o.MaxAveragePrice, o.OrderQty))
                                            return true;
                                    }

                                    if (Settings.Default.SmartLossPayoff)
                                    {
                                        if (CheckSmartLossPayoff(o.TradeType, dCurPrice, dAvgPrice, o.MaxAveragePrice, o.OrderQty))
                                            return true;
                                    }

									if ( Settings.Default.CrossLossPayoff && o.CrossAveragePrice > 0)
									{
                                        if (CheckCrossLossPayoff(o.TradeType, dCurPrice, dAvgPrice, o.CrossAveragePrice, o.OrderQty))
                                            return true;
									}

								}
								else if (o.TradeType == TRADETYPE.SELL) //매도
								{
									if ( lastCandlelist.Count<DItem>(d => d.Est_Type == RESULTSTATE.BUY) >= 1 //lastCandlelist.Count 
										&& lastCandlelist.Last().Orders.Count < 1 && (o.CrossAveragePrice == 0 || o.CrossAveragePrice > dCurPrice)/* && bTradeChanged*/)
									{
										o.CrossAveragePrice = dCurPrice; //교차점에서 현재가
										Trace.TraceInformation("<LogicAuto> SELL AveragePrice = {0}, CrossAveragePrice = {1} ", dAvgPrice, o.CrossAveragePrice);
									}

									if ((Settings.Default.EarnPayoff && Settings.Default.EarnPayoffMoney >= 0
										&& dDeltaTick <= -Settings.Default.EarnPayoffMoney * Settings.Default.ItemOverTick)
											|| (!Settings.Default.EarnPayoff && dDeltaTick <= 0))
									{
										if (Settings.Default.EarnPayoff && Settings.Default.ForceEarnPayoff)        //강제수익청산
										{
                                            log = "강제청산 수익:" + Math.Abs(dDeltaTick) / Settings.Default.ItemOverTick + "틱";
                                            if (Settings.Default.EarnPayoff)
                                                log += "(설정:" + Settings.Default.EarnPayoffMoney.ToString() + "틱)";
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
                                                log = "수익:" + Math.Abs(dDeltaTick) / Settings.Default.ItemOverTick + "틱";
                                                if (Settings.Default.EarnPayoff)
                                                    log += "(설정:" + Settings.Default.EarnPayoffMoney.ToString() + "틱)";
                                                this.frmMain.AddLog(log);
                                                lock (_objLock)
												{
													_reorderToCancel = true;
												}
											}
											return true;
										}
									}
                                    if (Settings.Default.LossPayoff)
                                    {
                                        if (CheckLossPayoff(o.TradeType, dCurPrice, dAvgPrice, o.MaxAveragePrice, o.OrderQty))
                                            return true;
                                    }

                                    if (Settings.Default.SmartLossPayoff)
									{
										if (CheckSmartLossPayoff(o.TradeType, dCurPrice, dAvgPrice, o.MaxAveragePrice, o.OrderQty))
											return true;
									}

									if (Settings.Default.CrossLossPayoff && o.CrossAveragePrice > 0)
									{
                                        if (CheckCrossLossPayoff(o.TradeType, dCurPrice, dAvgPrice, o.CrossAveragePrice, o.OrderQty))
                                            return true;
									}

								}
                                if (Settings.Default.CciPayoff)
                                {
                                    if (CheckCciPayoff(dCurCci, o.MaxCciPrice))
                                        return true;
                                }
							}
                            else if (Settings.Default.BettingType == (int)BETTYPE.CCI)           //Check Equivalent Candle 
                            {
                                string logTrade = "";
                                if (o.TradeType == TRADETYPE.BUY)   //매수
                                {
                                    if (lastCandlelist.Count<DItem>(d => d.Est_Type == RESULTSTATE.SELL) >= 1 //lastCandlelist.Count 
                                        && lastCandlelist.Last().Orders.Count < 1 && (o.CrossAveragePrice == 0 || o.CrossAveragePrice < dCurPrice)/* && bTradeChanged*/)
                                    {
                                        o.CrossAveragePrice = dCurPrice; //교차점에서 현재가
                                        Trace.TraceInformation("<LogicAuto> BUY AveragePrice = {0}, CrossAveragePrice = {1} ", dAvgPrice, o.CrossAveragePrice);
                                    }

                                    if ((Settings.Default.EarnPayoff && Settings.Default.EarnPayoffMoney >= 0
                                        && dDeltaTick >= Settings.Default.EarnPayoffMoney * Settings.Default.ItemOverTick)
                                            || (!Settings.Default.EarnPayoff && dDeltaTick >= 0))
                                    {
                                        if (Settings.Default.EarnPayoff && Settings.Default.ForceEarnPayoff)        //강제수익청산
                                        {
                                            log = "강제청산 수익:" + Math.Abs(dDeltaTick) / Settings.Default.ItemOverTick + "틱";
                                            if (Settings.Default.EarnPayoff)
                                                log += "(설정:" + Settings.Default.EarnPayoffMoney.ToString() + "틱)";
                                            this.frmMain.AddLog(log);
                                            m_boLiquid = true;
                                            return true;
                                        }
                                        else if (lastCandlelist.Count<DItem>(d => d.Est_Type == RESULTSTATE.SELL) >= lastCandlelist.Count)
                                        {
                                            if (logTrade.Length > 0)
                                                this.frmMain.AddLog(logTrade);

                                            if (!Settings.Default.OrderSelectOn || (Settings.Default.OrderSelectOn && Settings.Default.OrderSelectType == 0))
                                            {
                                                log = "수익:" + Math.Abs(dDeltaTick) / Settings.Default.ItemOverTick + "틱";
                                                if (Settings.Default.EarnPayoff)
                                                    log += "(설정:" + Settings.Default.EarnPayoffMoney.ToString() + "틱)";
                                                this.frmMain.AddLog(log);
                                                m_boLiquid = true;
                                            }

                                            return true;

                                        }
                                    }

                                    if (Settings.Default.LossPayoff)
                                    {
                                        if (CheckLossPayoff(o.TradeType, dCurPrice, dAvgPrice, o.MaxAveragePrice, o.OrderQty))
                                            return true;
                                    }

                                    if (Settings.Default.SmartLossPayoff)
                                    {
                                        if (CheckSmartLossPayoff(o.TradeType, dCurPrice, dAvgPrice, o.MaxAveragePrice, o.OrderQty))
                                            return true;
                                    }

                                    if (Settings.Default.CrossLossPayoff && o.CrossAveragePrice > 0)
                                    {
                                        if (CheckCrossLossPayoff(o.TradeType, dCurPrice, dAvgPrice, o.CrossAveragePrice, o.OrderQty))
                                            return true;
                                    }

                                }
                                else if (o.TradeType == TRADETYPE.SELL) //매도
                                {
                                    if (lastCandlelist.Count<DItem>(d => d.Est_Type == RESULTSTATE.BUY) >= 1 //lastCandlelist.Count 
                                        && lastCandlelist.Last().Orders.Count < 1 && (o.CrossAveragePrice == 0 || o.CrossAveragePrice > dCurPrice)/* && bTradeChanged*/)
                                    {
                                        o.CrossAveragePrice = dCurPrice; //교차점에서 현재가
                                        Trace.TraceInformation("<LogicAuto> SELL AveragePrice = {0}, CrossAveragePrice = {1} ", dAvgPrice, o.CrossAveragePrice);
                                    }

                                    if ((Settings.Default.EarnPayoff && Settings.Default.EarnPayoffMoney >= 0
                                        && dDeltaTick <= -Settings.Default.EarnPayoffMoney * Settings.Default.ItemOverTick)
                                            || (!Settings.Default.EarnPayoff && dDeltaTick <= 0))
                                    {
                                        if (Settings.Default.EarnPayoff && Settings.Default.ForceEarnPayoff)        //강제수익청산
                                        {
                                            log = "강제청산 수익:" + Math.Abs(dDeltaTick) / Settings.Default.ItemOverTick + "틱";
                                            if (Settings.Default.EarnPayoff)
                                                log += "(설정:" + Settings.Default.EarnPayoffMoney.ToString() + "틱)";
                                            this.frmMain.AddLog(log);
                                            m_boLiquid = true;
                                            return true;
                                        }
                                        else if (lastCandlelist.Count<DItem>(d => d.Est_Type == RESULTSTATE.BUY) >= lastCandlelist.Count)
                                        {
                                            if (logTrade.Length > 0)
                                                this.frmMain.AddLog(logTrade);
                                            if (!Settings.Default.OrderSelectOn || (Settings.Default.OrderSelectOn && Settings.Default.OrderSelectType == 0))
                                            {
                                                log = "수익:" + Math.Abs(dDeltaTick) / Settings.Default.ItemOverTick + "틱";
                                                if (Settings.Default.EarnPayoff)
                                                    log += "(설정:" + Settings.Default.EarnPayoffMoney.ToString() + "틱)";
                                                this.frmMain.AddLog(log);
                                                m_boLiquid = true;
                                            }
                                            return true;
                                        }
                                    }
                                    if (Settings.Default.LossPayoff)
                                    {
                                        if (CheckLossPayoff(o.TradeType, dCurPrice, dAvgPrice, o.MaxAveragePrice, o.OrderQty))
                                            return true;
                                    }

                                    if (Settings.Default.SmartLossPayoff)
                                    {
                                        if (CheckSmartLossPayoff(o.TradeType, dCurPrice, dAvgPrice, o.MaxAveragePrice, o.OrderQty))
                                            return true;
                                    }

                                    if (Settings.Default.CrossLossPayoff && o.CrossAveragePrice > 0)
                                    {
                                        if (CheckCrossLossPayoff(o.TradeType, dCurPrice, dAvgPrice, o.CrossAveragePrice, o.OrderQty))
                                            return true;
                                    }

                                }
                                if (Settings.Default.CciPayoff)
                                {
                                    if (CheckCciPayoff(dCurCci, o.MaxCciPrice))
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

									} else if (Settings.Default.EarnPayoff && Settings.Default.EarnPayoffMoney >= 0
										&& dDeltaTick >= Settings.Default.EarnPayoffMoney * Settings.Default.ItemOverTick)
									{
										return true;
									}

                                    if (Settings.Default.LossPayoff)
                                    {
                                        if (CheckLossPayoff(o.TradeType, dCurPrice, dAvgPrice, o.MaxAveragePrice, o.OrderQty))
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
                                        if (CheckLossPayoff(o.TradeType, dCurPrice, dAvgPrice, o.MaxAveragePrice, o.OrderQty))
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

		private int StageRangePercent(RANGETYPE rangeType, long lValuation)
        {
			int nRate = -1;
            int amountUnit = 10000;
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
            else return -1;

            if(lossConfs.Count > 0)
            {
                if (lValuation < lossConfs[0].Amount * amountUnit)
                    return -1;
            }
			for(int i = lossConfs.Count-1; i >= 0; i-- )
            {
				if (lossConfs[i].Enabled == 1 && lValuation >= lossConfs[i].Amount * amountUnit )
                {
                    nRate = lossConfs[i].Rate;
					break;
				}
            }
			return nRate;

        }
        private bool CheckLossPayoff(TRADETYPE tradeType, double dCurPrice, double dAvgPrice, double dMaxAveragePrice, int orderQty)
        {
            double dDeltaTick = dCurPrice - dAvgPrice;
            string log = "";
            if (tradeType == TRADETYPE.BUY)
            {
                if (Settings.Default.LossRangePayoff)
                {
                    long lValuation = (long)((dMaxAveragePrice - dAvgPrice) / CurrentSite.CurItemSymbol.OverTick * CurrentSite.CurItemSymbol.ValueTick * CurrentSite.CurItemSymbol.Exchange * orderQty);
                    int nTick = StageRangePercent(RANGETYPE.PayoffLoss, lValuation);

                    double dLossTick = (dMaxAveragePrice - dCurPrice)/ Settings.Default.ItemOverTick;
                    if (nTick >= 0 && dLossTick >= 0 && dLossTick > nTick)
                    {
                        log = "손실청산:" + dLossTick + "틱";
                        log += string.Format("(영역: {0:N0}원 {1}틱이상)", lValuation, nTick);
                        this.frmMain.AddLog(log);
                        m_boLiquid = true;
                        return true;
                    }
                }
                else if (Settings.Default.LossPayoffMoney >= 0 && dDeltaTick <= -Settings.Default.LossPayoffMoney * Settings.Default.ItemOverTick)
                {
                    log = "손실:" + Math.Abs(dDeltaTick) / Settings.Default.ItemOverTick + "틱";
                    log += "(설정:" + Settings.Default.LossPayoffMoney.ToString() + "틱)";
                    this.frmMain.AddLog(log);
                    m_boLiquid = true;
                    return true;
                }
            }
            else if (tradeType == TRADETYPE.SELL)
            {
                if (Settings.Default.LossRangePayoff)
                {
                    long lValuation = (long)((dAvgPrice - dMaxAveragePrice) / CurrentSite.CurItemSymbol.OverTick * CurrentSite.CurItemSymbol.ValueTick * CurrentSite.CurItemSymbol.Exchange * orderQty);
                    int nTick = StageRangePercent(RANGETYPE.PayoffLoss, lValuation);

                    double dLossTick = (dCurPrice - dMaxAveragePrice) / Settings.Default.ItemOverTick;
                    if (nTick >= 0 && dLossTick >= 0 && dLossTick > nTick)
                    {
                        log = "손실청산:" + dLossTick + "틱";
                        log += string.Format("(영역: {0:N0}원 {1}틱이상)", lValuation, nTick);
                        this.frmMain.AddLog(log);
                        m_boLiquid = true;
                        return true;
                    }
                }
                else if (Settings.Default.LossPayoffMoney >= 0 && dDeltaTick >= Settings.Default.LossPayoffMoney * Settings.Default.ItemOverTick)
                {
					log = "손실:" + Math.Abs(dDeltaTick) / Settings.Default.ItemOverTick + "틱";
                    log += "(설정:" + Settings.Default.LossPayoffMoney.ToString() + "틱)";
                    this.frmMain.AddLog(log);
                    m_boLiquid = true;
                    return true;
                }
            }

            return false;
        }
        private bool CheckSmartLossPayoff(TRADETYPE tradeType, double dCurPrice, double dAvgPrice, double dMaxAveragePrice, int orderQty)
        {
			double dSmartEarn = 0;
			if (tradeType == TRADETYPE.BUY)
            {
				if (Settings.Default.SmartRangePayoff)
				{
					long lValuation = (long)((dMaxAveragePrice - dAvgPrice) / CurrentSite.CurItemSymbol.OverTick * CurrentSite.CurItemSymbol.ValueTick * CurrentSite.CurItemSymbol.Exchange * orderQty);
					int nPercent = StageRangePercent(RANGETYPE.SmartLoss, lValuation);

					long lLossVal = (long)((dMaxAveragePrice - dCurPrice) / CurrentSite.CurItemSymbol.OverTick * CurrentSite.CurItemSymbol.ValueTick * CurrentSite.CurItemSymbol.Exchange * orderQty);
					if (nPercent > 0 && lLossVal > 0 && lLossVal > lValuation * nPercent / 100)
					{
						m_boLiquid = true;
						this.frmMain.AddLog(string.Format("스마트청산 최대수익가:{0}({1:N0}원) 설정:{2}%", string.Format(Settings.Default.PriceFormat, dMaxAveragePrice), lValuation, nPercent));
						return true;
					}
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
                        this.frmMain.AddLog("스마트청산 최대수익가:" + string.Format(Settings.Default.PriceFormat, dMaxAveragePrice) + " 설정:" + Settings.Default.SmartLossTick.ToString() + (Settings.Default.SmartLossUnit == 0 ? "%" : "틱"));
                        return true;
                    }
                }
            } else if(tradeType == TRADETYPE.SELL)
            {
                if (Settings.Default.SmartRangePayoff)
                {
                    long lValuation = (long)((dAvgPrice - dMaxAveragePrice) / CurrentSite.CurItemSymbol.OverTick * CurrentSite.CurItemSymbol.ValueTick * CurrentSite.CurItemSymbol.Exchange * orderQty);
                    int nPercent = StageRangePercent(RANGETYPE.SmartLoss, lValuation);

                    long lLossVal = (long)((dCurPrice - dMaxAveragePrice) / CurrentSite.CurItemSymbol.OverTick * CurrentSite.CurItemSymbol.ValueTick * CurrentSite.CurItemSymbol.Exchange * orderQty);
                    if (nPercent > 0 && lLossVal > 0 && lLossVal > lValuation * nPercent / 100)
                    {
                        m_boLiquid = true;
                        this.frmMain.AddLog(string.Format("스마트청산 최대수익가:{0}({1:N0}원) 설정:{2}%", string.Format(Settings.Default.PriceFormat, dMaxAveragePrice), lValuation, nPercent));
                        return true;
                    }
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
                        this.frmMain.AddLog("스마트청산 최대수익가:" + string.Format(Settings.Default.PriceFormat, dMaxAveragePrice) + " 설정:" + Settings.Default.SmartLossTick.ToString() + (Settings.Default.SmartLossUnit == 0 ? "%" : "틱"));
                        return true;
                    }
                }
            }

			return false;
        }
        private bool CheckCciPayoff(double dCurCci, double dMaxCci)
        {
            string log = "";
            if (dCurCci == 0 || dMaxCci == 0)
                return false;

            if (Settings.Default.CciRangePayoff)
            {
                int nRate = StageRangePercent(RANGETYPE.CciLoss, (long)dMaxCci);

                double dLossCci = dMaxCci - dCurCci;
                if (nRate >= 0 && dLossCci >= 0 && dLossCci > dMaxCci * nRate / 100.0)
                {
                    log = string.Format(" CCI청산:{0:N2} ", dCurCci); 
                    log += string.Format(" (영역:{0:N2} ", dMaxCci) + ", " + nRate.ToString() + "%하락)";
                    this.frmMain.AddLog(log);
                    m_boLiquid = true;
                    return true;
                }
            }
            else if (dCurCci >= Settings.Default.CciPayoffValue1 && dCurCci <= Settings.Default.CciPayoffValue2)
            {
                log = string.Format(" CCI청산:{0:N2} ", dCurCci);
                log += "(설정:" + Settings.Default.CciPayoffValue1.ToString() + "~" + Settings.Default.CciPayoffValue2.ToString() + ")";
                this.frmMain.AddLog(log);
                m_boLiquid = true;
                return true;
            }
            

            return false;
        }
        private bool CheckCrossLossPayoff(TRADETYPE tradeType, double dCurPrice, double dAvgPrice, double dCrossAveragePrice, int orderQty)
        {
            // Trace.TraceInformation("<LogicAuto> CheckCrossLossPayoff AveragePrice = {0}, CrossAveragePrice = {1} ", dAvgPrice, dCrossAveragePrice);

            double dSmartEarn = 0;
			if(tradeType == TRADETYPE.BUY)
            {
                if (Settings.Default.CrossRangePayoff)
                {
                    long lValuation = (long)((dCrossAveragePrice - dAvgPrice) / CurrentSite.CurItemSymbol.OverTick * CurrentSite.CurItemSymbol.ValueTick * CurrentSite.CurItemSymbol.Exchange * orderQty);
                    int nPercent = StageRangePercent(RANGETYPE.CrossLoss, lValuation);

                    long lLossVal = (long)((dCrossAveragePrice - dCurPrice) / CurrentSite.CurItemSymbol.OverTick * CurrentSite.CurItemSymbol.ValueTick * CurrentSite.CurItemSymbol.Exchange * orderQty);
                    if (nPercent > 0 && lLossVal > 0 && lLossVal > lValuation * nPercent / 100)
                    {
                        m_boLiquid = true;
                        //Trace.TraceInformation(string.Format("교차점에서 하락청산 교차점:{0}({1:N0}) 설정:{2}%", string.Format(Settings.Default.PriceFormat, dCrossAveragePrice), lValuation, nPercent));

						this.frmMain.AddLog(string.Format("교차점에서 하락청산 교차점:{0}({1:N0}원) 설정:{2}%", string.Format(Settings.Default.PriceFormat, dCrossAveragePrice), lValuation, nPercent));
                        return true;
                    }
                }
                else if (Settings.Default.CrossLossTick >= 0)
                {
                    if (Settings.Default.CrossLossUnit == 0)
                        dSmartEarn = (dCrossAveragePrice - dAvgPrice) * Settings.Default.CrossLossTick / 100;
                    else dSmartEarn = Settings.Default.CrossLossTick * Settings.Default.ItemOverTick;

                    if (dCrossAveragePrice - dCurPrice >= dSmartEarn)
                    {
                        m_boLiquid = true;
                        //Trace.TraceInformation("교차점에서 하락청산 교차점:" + string.Format(Settings.Default.PriceFormat, dCrossAveragePrice) + " 설정:" + Settings.Default.CrossLossTick.ToString() + (Settings.Default.CrossLossUnit == 0 ? "%" : "틱"));

                        this.frmMain.AddLog("교차점에서 하락청산 교차점:" + string.Format(Settings.Default.PriceFormat, dCrossAveragePrice) + " 설정:" + Settings.Default.CrossLossTick.ToString() + (Settings.Default.CrossLossUnit == 0 ? "%" : "틱"));
                        return true;
                    }
                }

            } else if(tradeType == TRADETYPE.SELL)
            {
                if (Settings.Default.CrossRangePayoff)
                {
                    long lValuation = (long)((dAvgPrice - dCrossAveragePrice) / CurrentSite.CurItemSymbol.OverTick * CurrentSite.CurItemSymbol.ValueTick * CurrentSite.CurItemSymbol.Exchange * orderQty);
                    int nPercent = StageRangePercent(RANGETYPE.CrossLoss, lValuation);

                    long lLossVal = (long)((dCurPrice - dCrossAveragePrice) / CurrentSite.CurItemSymbol.OverTick * CurrentSite.CurItemSymbol.ValueTick * CurrentSite.CurItemSymbol.Exchange * orderQty);
                    if (nPercent > 0 && lLossVal > 0 && lLossVal > lValuation * nPercent / 100)
                    {
                        //Trace.TraceInformation("교차점에서 하락청산 교차점:{0}({1:N0}) 설정:{2}%", string.Format(Settings.Default.PriceFormat, dCrossAveragePrice), lValuation, nPercent);

                        m_boLiquid = true;
                        this.frmMain.AddLog(string.Format("교차점에서 하락청산 교차점:{0}({1:N0}원) 설정:{2}%", string.Format(Settings.Default.PriceFormat, dCrossAveragePrice), lValuation, nPercent));
                        return true;
                    }
                } else if (Settings.Default.CrossLossPayoff && Settings.Default.CrossLossTick >= 0 && dCrossAveragePrice > 0)
                {
                    if (Settings.Default.CrossLossUnit == 0)
                        dSmartEarn = (dAvgPrice - dCrossAveragePrice) * Settings.Default.CrossLossTick / 100;
                    else dSmartEarn = Settings.Default.CrossLossTick * Settings.Default.ItemOverTick;

                    if (dCurPrice - dCrossAveragePrice >= dSmartEarn)
                    {
                        //Trace.TraceInformation("교차점에서 하락청산 교차점:" + string.Format(Settings.Default.PriceFormat, dCrossAveragePrice) + " 설정:" + Settings.Default.CrossLossTick.ToString() + (Settings.Default.CrossLossUnit == 0 ? "%" : "틱"));

                        m_boLiquid = true;
                        this.frmMain.AddLog("교차점에서 하락청산 교차점:" + string.Format(Settings.Default.PriceFormat, dCrossAveragePrice) + " 설정:" + Settings.Default.CrossLossTick.ToString() + (Settings.Default.CrossLossUnit == 0 ? "%" : "틱"));
                        return true;
                    }

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
					// Trace.TraceInformation("<LogicAuto> TradeChanged() 1: {0} > {1} per {2}MIN ", nConc, Settings.Default.Conc1Cnt, Settings.Default.Conc1Min);
                    if (nConc > Settings.Default.Conc1Cnt)
                    {
						log = string.Format("{0}분당거래{1}({2}) &", Settings.Default.Conc1Min, Settings.Default.Conc1Cnt, nConc);
                        // Trace.TraceInformation(log);
						bChanged = true;
                    }
                }
			}
            if (Settings.Default.Conc2On && bChanged)
            {
                if (Settings.Default.Conc2Candle > 0 && Settings.Default.Conc2Cnt > 0)
                {
                    bChanged = false;

                    List<CItem> lastCandlelist = frmMain.GetCandleList2(Settings.Default.Conc2Candle+1, true);

					if(lastCandlelist.Count >= Settings.Default.Conc2Candle + 1)
                    {
						int nConcSum = 0;
						lastCandlelist.ForEach(c => nConcSum += c.Conc);
						nConcSum -= lastCandlelist[lastCandlelist.Count - 1].Conc;
                        // Trace.TraceInformation("<LogicAuto> TradeChanged() 2: {0} >= {1} * {2}% / {3} ", lastCandlelist[lastCandlelist.Count - 1].Conc, nConcSum, Settings.Default.Conc2Cnt, Settings.Default.Conc2Candle);
                        if ( lastCandlelist[lastCandlelist.Count-1].Conc >= nConcSum * Settings.Default.Conc2Cnt / 100 / Settings.Default.Conc2Candle)
                        {
							log += string.Format("{0}차트{1}봉평균{2}의 {3}%({4}) &", 
								Common.GetChartTypeStr((CHARTTYPE)Settings.Default.Conc2Chart), 
									Settings.Default.Conc2Candle,
									nConcSum / Settings.Default.Conc2Candle,
									Settings.Default.Conc2Cnt, 
									lastCandlelist[lastCandlelist.Count - 1].Conc
								);
                            // Trace.TraceInformation(log);
							bChanged = true;
                        }

					}

                }
			}
            if (Settings.Default.AdxOn && bChanged)
            {
				bChanged = false;
				List<DItem> lastCandlelist = frmMain.GetCandleList(1, true);
                if (lastCandlelist.Count > 0 && lastCandlelist[0].Adx > Settings.Default.AdxCnt)
                {
					log += string.Format("ADX:{0}({1:N2})", Settings.Default.AdxCnt, lastCandlelist[0].Adx);
                    // Trace.TraceInformation(log);
					bChanged = true;
                }
			}
            
            return bChanged;
        }
		private TRADETYPE GetTradeCondition(ref string log)
		{
			TRADETYPE tradeType = TRADETYPE.NONE;
			if (!Settings.Default.CciOn && !Settings.Default.RsiOn && !Settings.Default.AvgsOn)
				return tradeType;

            List<DItem> lastCandlelist = frmMain.GetCandleList(2, true);
			if (lastCandlelist.Count < 2)
				return tradeType;
            if (Settings.Default.CciOn)
            {
				if (lastCandlelist[0].Cci != 0 && lastCandlelist[0].Cci < Settings.Default.CciRange1 && lastCandlelist[1].Cci > Settings.Default.CciRange1)
				{
					log += string.Format("CCI:{0}상승({1:N2})", Settings.Default.CciRange1, lastCandlelist[1].Cci);
					// Trace.TraceInformation(log);
					tradeType = Settings.Default.CciSide1 == 0 ? TRADETYPE.BUY : TRADETYPE.SELL;

				}
				else if (lastCandlelist[0].Cci != 0 && lastCandlelist[0].Cci > Settings.Default.CciRange2 && lastCandlelist[1].Cci < Settings.Default.CciRange2)
				{
					log += string.Format("CCI:{0}하락({1:N2})", Settings.Default.CciRange2, lastCandlelist[1].Cci);
					// Trace.TraceInformation(log);
					tradeType = Settings.Default.CciSide2 == 0 ? TRADETYPE.BUY : TRADETYPE.SELL;
				}
				else return TRADETYPE.NONE;
            }
            if (Settings.Default.RsiOn)
            {
                if (lastCandlelist[0].Rsi > 0 && lastCandlelist[0].Rsi < Settings.Default.RsiRange1 && lastCandlelist[1].Rsi > Settings.Default.RsiRange1)
                {
                    log += string.Format("RSI:{0}상승({1:N2})", Settings.Default.RsiRange1, lastCandlelist[1].Rsi);
					// Trace.TraceInformation(log);
					TRADETYPE tradeType2 = Settings.Default.RsiSide1 == 0 ? TRADETYPE.BUY : TRADETYPE.SELL;
					if (tradeType == TRADETYPE.NONE)
						tradeType = tradeType2;
					else if (tradeType != tradeType2) 
                    {
                        return TRADETYPE.NONE;
                    }
					//if CCI Tradetype == RSI Tradetype, Continue;
				}
				else if (lastCandlelist[0].Rsi > 0 && lastCandlelist[0].Rsi > Settings.Default.RsiRange2 && lastCandlelist[1].Rsi < Settings.Default.RsiRange2)
                {
                    log += string.Format("RSI:{0}하락({1:N2})", Settings.Default.RsiRange2, lastCandlelist[1].Rsi);
					// Trace.TraceInformation(log);
					TRADETYPE tradeType2 = Settings.Default.RsiSide2 == 0 ? TRADETYPE.BUY : TRADETYPE.SELL;
                    if (tradeType == TRADETYPE.NONE)
                        tradeType = tradeType2;
                    else if (tradeType != tradeType2)
                    {
                        return TRADETYPE.NONE;
                    }
					//if CCI Tradetype == RSI Tradetype, Continue;
				}
				else
                {
					return TRADETYPE.NONE;
				}
            }
			if (Settings.Default.AvgsOn)
            {
				if (Settings.Default.AvgsCandle < 1)
					return tradeType;

				List<DItem> lastCandlelist2 = frmMain.GetCandleList(Settings.Default.AvgsCandle, false);
				if(lastCandlelist2.Count < Settings.Default.AvgsCandle)
                {
					return TRADETYPE.NONE; 
				}

				int upDown = lastCandlelist2[0].AvgPos;
				if (upDown == 0)
                    return TRADETYPE.NONE;
				else if(upDown == 1)
                {
					if (lastCandlelist2.Count<DItem>(d => d.AvgPos == upDown) >= lastCandlelist2.Count)
                    {
                        log += string.Format("이평선S1:{0}봉 U ", Settings.Default.AvgsCandle); //200일선 위상태
                        // Trace.TraceInformation(log);
                        TRADETYPE tradeType3 = Settings.Default.AvgsSide1 == 0 ? TRADETYPE.BUY : TRADETYPE.SELL;
                        if (tradeType == TRADETYPE.NONE)
                            tradeType = tradeType3;
                        else if (tradeType != tradeType3)
                        {
                            return TRADETYPE.NONE;
                        }
                    } else return TRADETYPE.NONE;
                }
                else if (upDown == -1)
                {
                    if (lastCandlelist2.Count<DItem>(d => d.AvgPos == upDown) >= lastCandlelist2.Count)
                    {

                        log += string.Format("이평선S1:{0}봉 D ", Settings.Default.AvgsCandle); //200일선 아래
                        // Trace.TraceInformation(log);
                        TRADETYPE tradeType3 = Settings.Default.AvgsSide2 == 0 ? TRADETYPE.BUY : TRADETYPE.SELL;
                        if (tradeType == TRADETYPE.NONE)
                            tradeType = tradeType3;
                        else if (tradeType != tradeType3)
                        {
                            return TRADETYPE.NONE;
                        }
                    }
                    else return TRADETYPE.NONE;
                } 

            }

            return tradeType;
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
                            Common.GetChartTypeStr((CHARTTYPE)Settings.Default.Conc2Chart),
                            Settings.Default.Conc2Candle,
                                nConcSum / Settings.Default.Conc2Candle);
                    }
                }
            }
            
            if (log.Length > 0)
            {
                DateTime dtCurrent = DateTime.Now;
                log = string.Format("[{0:D2}:{1:D2}:{2:D2}]", dtCurrent.Hour, dtCurrent.Minute, dtCurrent.Second) + log;
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
            List<DItem> lasDItemlist = null;

            lasDItemlist = frmMain.GetCandleList(1, true);
            if(lasDItemlist.Count > 0)
            {
                if (lasDItemlist[0].Adx > 0)
                {
                    log += string.Format(" ADX:{0:N2} |", lasDItemlist[0].Adx);
                }

                if (lasDItemlist[0].Cci != 0)
                {
                    log += string.Format(" CCI:{0:N2} |", lasDItemlist[0].Cci);
                }

                if (lasDItemlist[0].Rsi > 0)
                {
                    log += string.Format(" RSI:{0:N2} |", lasDItemlist[0].Rsi);
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
                            log += string.Format(" 이평선S1:{0}봉 U ", Settings.Default.AvgsCandle); //200일선 위상태
                        }
                    }
                    else if (upDown == -1)
                    {
                        if (lastCandlelist2.Count<DItem>(d => d.AvgPos == upDown) >= lastCandlelist2.Count)
                        {

                            log += string.Format(" 이평선S1:{0}봉 D ", Settings.Default.AvgsCandle);  //200일선 아래상태
                            // Trace.TraceInformation(log);
                        }
                    }
                }
            }


            if (log.Length > 0)
            {
                DateTime dtCurrent = DateTime.Now;
                log = string.Format("[{0:D2}:{1:D2}:{2:D2}]", dtCurrent.Hour, dtCurrent.Minute, dtCurrent.Second) + log;
                this.frmMain.AddValueLog(log);
            }
			return true;
        }

		private bool NeedToStop(bool bLog=false)
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

            if (m_ForceLiquid)
            {
				if(bLog)
					this.frmMain.AddLog("강제청산되었습니다."); //OnLogEvent
				return true;
			}

            long valuation = 0;
            lock (_currentSite.ValuationList)
            {
                valuation = _currentSite.ValuationList[0].CurrentProfit;
            }

            if (Settings.Default.EarnStop && Settings.Default.EarnStopMoney >= 0)
			{
				if (valuation >= Settings.Default.EarnStopMoney * 10000)
					return true;
			}

			if (Settings.Default.LossStop && Settings.Default.LossStopMoney >= 0)
			{
				if (valuation <= -Settings.Default.LossStopMoney * 10000)
					return true;
			}

            if (Settings.Default.ProfitStop && Settings.Default.ProfitStopRate > 0 )
            {
                if (valuation > 0 && m_maxProfit > 0 && valuation < m_maxProfit * (100 - Settings.Default.ProfitStopRate) / 100)
                    return true;
            }

            return false;
		}
		private bool NeedToAuto()
		{
            if (!Settings.Default.IsAutoMode && Settings.Default.AutoReserveOn )
            {
				DateTime dtNow = DateTime.Now;
				DateTime dtToday = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day);
				if (dtNow >= dtToday + Settings.Default.AutoReserveTime.TimeOfDay && dtNow <= dtToday.AddSeconds(3) + Settings.Default.AutoReserveTime.TimeOfDay)
					return true;
			}
			return false;
		}
        private bool DoOrder(int nQuantity)
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
            lock (_currentSite.QuoteList)
            {
                quoteInfo = _currentSite.QuoteList.FirstOrDefault(q => Math.Abs(q.Price - currentPrice) < 1E-06);
            }
            if (quoteInfo == null)
                return false;
            switch (_tradeTypeToOrder)
			{
				case TRADETYPE.BUY:
					return _currentSite.DoBuyOrder(quoteInfo, nQuantity, Settings.Default.OrderType==0);
				case TRADETYPE.SELL:
					return _currentSite.DoSellOrder(quoteInfo, nQuantity, Settings.Default.OrderType == 0);
			}

			return false;
		}

		private bool DoCancel()
		{
			if (_orderToCancel == null)
				return false;
			if (_orderToCancel.OrderType == "미체결")
				return _currentSite.CancelOrder(_orderToCancel);
			else if(_reorderToCancel && ( (Settings.Default.BettingType == (int)BETTYPE.CROSS && Settings.Default.Reorder)
										 || Settings.Default.BettingType == (int)BETTYPE.BOLINE
										 || Settings.Default.BettingType == (int)BETTYPE.BOT1) ) 
			{ 
                string quantity;
                int nQuantity = 0;
                lock (_objLock)
                {
                    _reorderToCancel = false;
                }
				if (Common.ExtractString(out quantity, _orderToCancel.Qty, "[", "]") < 0)
					return false;

				if (!int.TryParse(quantity, out nQuantity))
				{
					return false;
				}
				else if (nQuantity < 1 || nQuantity > 5)
				{
					//return false;
				}
				else if (_orderToCancel.TradeType == TRADETYPE.BUY)		//매수
				{
					_tradeTypeToOrder = TRADETYPE.SELL;
					lock (_objLock)
					{
						m_tickOrder = Environment.TickCount;
					}
					if (DoOrder(nQuantity * 2))
						return true;

				}
				else if (_orderToCancel.TradeType == TRADETYPE.SELL)	//매도
				{
					_tradeTypeToOrder = TRADETYPE.BUY;
					lock (_objLock)
					{
						m_tickOrder = Environment.TickCount;
					}
					if (DoOrder(nQuantity * 2))
						return true;
				}
				else return false;
                
            }
			
			return _currentSite.LiquidateOrder(_orderToCancel);
		}

		private TRADETYPE SelectTradeType()
		{
			bool bNeedLast = false;
			int nCandleCnt = Settings.Default.BettingCandleCount;
			if (Settings.Default.BettingType == (int)BETTYPE.CROSS || Settings.Default.BettingType == (int)BETTYPE.HYBRID)
            {
				bNeedLast = Settings.Default.BettingCandleComplete == 0;
				nCandleCnt = 2;
			} else if (Settings.Default.BettingType == (int)BETTYPE.BOLINE || Settings.Default.BettingType == (int)BETTYPE.BOT1)
            {
				bNeedLast = true;
				nCandleCnt = Settings.Default.BettingCandleCount;
			}	

			if (nCandleCnt < 1)
                return TRADETYPE.NONE;

            List<DItem> lastCandlelist = frmMain.GetCandleList(nCandleCnt, bNeedLast);
			
			if (lastCandlelist.Count < nCandleCnt)
				return TRADETYPE.NONE;

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
			else if(Settings.Default.BettingType == (int)BETTYPE.UPDOWN)               //Check Moving Average Line
			{
				CH_AVGTYPE avgType = (CH_AVGTYPE)Settings.Default.AvgType;
				float fTickDiff = Math.Abs(lastCandlelist.First().GetAvgVal(avgType) - lastCandlelist.Last().GetAvgVal(avgType));
				float fTickConf = Settings.Default.ItemOverTick * Settings.Default.BettingTickCount;

				int iFirstIdx = lastCandlelist.First().Index;

				if (fTickDiff >= fTickConf && lastCandlelist.Count<DItem>(d => d.GetTrendUp(avgType, iFirstIdx) == CH_TRENDTYPE.UP) >= Settings.Default.BettingCandleCount)
					trade_type = TRADETYPE.BUY;
                else if(fTickDiff >= fTickConf && lastCandlelist.Count<DItem>(d => d.GetTrendDown(avgType, iFirstIdx) == CH_TRENDTYPE.DOWN) >= Settings.Default.BettingCandleCount)
                    trade_type = TRADETYPE.SELL;
				else trade_type = TRADETYPE.NONE;
            } 
			else if(Settings.Default.BettingType == (int)BETTYPE.CROSS || Settings.Default.BettingType == (int)BETTYPE.HYBRID)
            {
				CH_TRENDTYPE trend_type = lastCandlelist.Last().GetCrossTrend(CH_AVGTYPE.LINE_1, CH_AVGTYPE.LINE_2);
				if (trend_type == CH_TRENDTYPE.UP)
					trade_type = TRADETYPE.BUY;
				else if (trend_type == CH_TRENDTYPE.DOWN)
					trade_type = TRADETYPE.SELL;
				else trade_type = TRADETYPE.NONE;
			} 
			else if (Settings.Default.BettingType == (int)BETTYPE.BOLINE || Settings.Default.BettingType == (int)BETTYPE.BOT1)           //Check Equivalent Candle 
            {
				DItem firstCandle = lastCandlelist.First<DItem>();
				if (firstCandle.Bos[0] == firstCandle.Bos[1] || (Settings.Default.BettingEnter && m_boLiquid ))
				{
					string logTrade = "";
					bool bTradeChanged = CheckTradeChange(ref logTrade);
					if (lastCandlelist.Count<DItem>(d => d.Est_Type == RESULTSTATE.BUY) >= lastCandlelist.Count && bTradeChanged)
					{
						if(!Settings.Default.CciOn || (Settings.Default.CciOn && Settings.Default.CciSide1 <= 1) )
                        {
                            if (logTrade.Length > 0)
                                this.frmMain.AddLog(logTrade);
                            m_boLiquid = false;
                            trade_type = TRADETYPE.BUY;
                        }
					}
					else if (lastCandlelist.Count<DItem>(d => d.Est_Type == RESULTSTATE.SELL) >= lastCandlelist.Count && bTradeChanged)
					{
						if (!Settings.Default.CciOn || (Settings.Default.CciOn && (Settings.Default.CciSide1 == 0 || Settings.Default.CciSide1 == 2)) )
                        {
                            if (logTrade.Length > 0)
                                this.frmMain.AddLog(logTrade);
                            m_boLiquid = false;
                            trade_type = TRADETYPE.SELL;
                        }
							
					}
				}
            }
            else if (Settings.Default.BettingType == (int)BETTYPE.CCI)
            {
                string logTrade = "";
                trade_type = GetTradeCondition(ref logTrade);
				if(trade_type != TRADETYPE.NONE)
                {
                    if (logTrade.Length > 0)
                        this.frmMain.AddLog(logTrade);
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
            return trade_type;
		}
	}
}
