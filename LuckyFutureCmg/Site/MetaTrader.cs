// #define WRITE_LOG

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Json;
using System.Linq;
using System.Text;
using System.Threading;
using ChartCtrl;
using Goodbyte.TradingSystem.Domain.Entities;
using LuckyFuture.Models.ValueObjects;
using LuckyFuture.Properties;
using LuckyFutureLib.Include;
using MtApi;

namespace LuckyFuture.Site
{
    /// <summary>
    /// CMG 연동용 MetaTrader 사이트. MtApi로 MT4와 통신하며, 기존 자동매매 로직은 FutureSite 흐름을 그대로 사용.
    /// </summary>
    /// <remarks>
    /// [수정 시 원칙] 통신부만 손대고 나머지는 그대로 둬서 기존 자동매매 로직이 작동하도록 유지.
    /// - 통신부: Login/DisconnectSocket, MtApiClient 연결/해제, QuoteUpdated → Current 생성 후 OnReceiveCurrent 호출,
    ///   OrderSend/OrderDelete/OrderClose, AccountBalance/AccountEquity/HistoryDeals/GetOrders 등 조회, CopyRates, SymbolSelect/reqQuote.
    /// - 로직부(유지): Prepare/Check/OnPrepare/OnLogin, DoBuyOrder/DoSellOrder의 검증·흐름, CreateQuoteInfo, OnReceiveCurrent 이후 처리,
    ///   OrderList/CurrentList/QuoteList/CurItemSymbol 기반 로직, OnFutureSiteNoticeEvent 등.
    /// </remarks>
    class MetaTrader : FutureSite
    {
        public override SITETYPE Type { get; set; }
        private MtApiClient _mtApiClient = null;
        private readonly object _mtLock = new object();

        /// <summary>MtApi EA 연결 호스트 (기본 localhost)</summary>
        public const string MTAPI_DEFAULT_HOST = "localhost";
        /// <summary>MtApi EA 포트 (MT4 기본 8222)</summary>
        public const int MTAPI_DEFAULT_PORT = 8222;

        private string mt_oldSymbol = "";
        private int m_tickCurrent = 0;
        private int m_tickAccount = 0;
        private int m_tickContrastDiag = 0;
        private int m_tickOrderList = 0;
        private const int ORDERLIST_REFRESH_MS = 3000;
        private bool m_bNeedAcc = false;
        private bool _mtApiConnected = false;

        public MetaTrader()
        {
            Type = SITETYPE.CMG;
#if WRITE_LOG
            CreateLogFile();
#endif
        }
        string _logPath = "";

        private void CreateLogFile()
        {
            _logPath = "D://BinHts/Meta_" + DateTime.Now.ToString("yyyyMMdd");
            _logPath += ".txt";
            WriteLog("<!=============시작중입니다.===============>");

        }
        public void WriteLog(string strLog)
        {
#if WRITE_LOG
            try
            {
                DateTime dtServer = DateTime.Now;
                string log = string.Format("[{0:D2}:{1:D2}:{2:D2}] ", dtServer.Hour, dtServer.Minute, dtServer.Second);
                log += strLog;
                using (StreamWriter outputFile = new StreamWriter(_logPath, true))
                {
                    outputFile.WriteLine(log);
                }
            }
            catch (Exception ex)
            { string error = ex.Message; }
#endif
        }

        protected override ERRORCODE Login(string id, string password)
        {
            if (_mtApiClient != null)
            {
                DisconnectSocket();
            }

            _mtApiClient = new MtApiClient();
            _mtApiClient.ConnectionStateChanged += MtApiClient_ConnectionStateChanged;
            _mtApiClient.QuoteUpdated += MtApiClient_QuoteUpdated;

            try
            {
                _mtApiClient.BeginConnect(MTAPI_DEFAULT_HOST, MTAPI_DEFAULT_PORT);
                int waitMs = 0;
                while (!_mtApiConnected && waitMs < 10000)
                {
                    Thread.Sleep(100);
                    waitMs += 100;
                }
                if (!_mtApiConnected)
                {
                    WriteLog("MtApi 연결 시간 초과");
                    return ERRORCODE.CANT_CONNECT;
                }
            }
            catch (Exception ex)
            {
                WriteLog(String.Format("MtApi Connect error: {0}", ex.Message));
                return ERRORCODE.CANT_CONNECT;
            }

            lock (_mtLock)
            {
                if (_mtApiClient == null || _mtApiClient.ConnectionState != MtConnectionState.Connected)
                    return ERRORCODE.CANT_CONNECT;
            }

            double balance = 0, equity = 0;
            string accNum = "";
            string accountName = "";
            try
            {
                balance = _mtApiClient.AccountBalance();
                equity = _mtApiClient.AccountEquity();
                accNum = _mtApiClient.AccountNumber().ToString();
                try { accountName = _mtApiClient.AccountName() ?? ""; } catch { }
                UserAcc = accNum;
                WriteLog(String.Format("Login Balance={0}, Equity={1}, Account={2}", balance, equity, accNum));
            }
            catch (Exception ex)
            {
                WriteLog(String.Format("AccountInfo error: {0}", ex.Message));
                return ERRORCODE.UNKNOWN_FAILED;
            }

            double valuation = equity - balance;
            string accountDisplay = string.IsNullOrEmpty(accountName) ? accNum : string.Format("{0} ({1})", accountName, accNum);
            this.CurrentUserAccount = new UserAccountInfo
            {
                UserAccountId = UserAcc,
                UserAccountStr = accountDisplay,
                Balance = balance
            };
            this.UserAccounts = new List<UserAccountInfo>();
            this.UserAccounts.Add(this.CurrentUserAccount);
            DayProfitLoss = new DayProfitLossInfo();
            OnLogin();
            this.ValuationList[0].TotalValuation = 0;

            RequestHistoryDeals();

            return ERRORCODE.SUCCESS;
        }


        protected override void OnLogin()
        {
            base.OnLogin();
            bQutoteCreated = false;
            reqItemlist();
            ConnectSocket();
            Thread.Sleep(1500);
        }

        protected override ERRORCODE LogOut()
        {
            return ERRORCODE.SUCCESS;
        }

        private void MtApiClient_ConnectionStateChanged(object sender, MtApi.MtConnectionEventArgs e)
        {
            _mtApiConnected = (e != null && e.Status == MtConnectionState.Connected);
            WriteLog("[MtApi] ConnectionStateChanged: " + (e?.Status.ToString() ?? "null"));
        }

        /// <summary>통신부: MtApi 호가 수신 → 기존 로직용 Current 생성 후 OnReceiveCurrent 호출 (자동매매 로직은 변경 없음)</summary>
        private void MtApiClient_QuoteUpdated(object sender, string symbol, DateTime time, double bid, double ask)
        {
            if (string.IsNullOrEmpty(symbol) || symbol != ItemSymbol) return;

            Current current = new Current();
            current.ReceivedDate = time;
            current.CurrentPrice = bid;
            current.CurrentPrice2 = ask;
            current.ConclusionVolume = 1;
            FillCurrentOhlcFromLastBar(current);
            FillContrastFromApi(current);

            if (CurItemSymbol != null && CurItemSymbol.MidPrice == 0)
                CurItemSymbol.MidPrice = ask;

            if (bQutoteCreated)
                OnReceiveCurrent(current);
        }

        /// <summary>현재 종목의 최근 봉(시가/고가/저가) 정보로 current를 채움. MT4 CopyRates는 startPos=1(마지막 확정봉) 또는 0(현재봉)으로 요청.</summary>
        private void FillCurrentOhlcFromLastBar(Current current)
        {
            try
            {
                lock (_mtLock)
                {
                    if (_mtApiClient == null || _mtApiClient.ConnectionState != MtConnectionState.Connected) return;
                    MqlRates[] rates = _mtApiClient.CopyRates(ItemSymbol, MtApi.ENUM_TIMEFRAMES.PERIOD_M1, 1, 1)?.ToArray() ?? new MqlRates[0];
                    if (rates == null || rates.Length == 0)
                        rates = _mtApiClient.CopyRates(ItemSymbol, MtApi.ENUM_TIMEFRAMES.PERIOD_M1, 0, 1)?.ToArray() ?? new MqlRates[0];
                    if (rates == null || rates.Length == 0)
                        rates = _mtApiClient.CopyRates(ItemSymbol, MtApi.ENUM_TIMEFRAMES.PERIOD_M1, 0, 10)?.ToArray() ?? new MqlRates[0];
                    if (rates != null && rates.Length > 0)
                    {
                        var r = rates[0];
                        for (int i = 1; i < rates.Length; i++)
                            if (rates[i].Time > r.Time) r = rates[i];
                        current.StartPrice = r.Open;
                        current.HighPrice = r.High;
                        current.LowPrice = r.Low;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog("[FillCurrentOhlcFromLastBar] " + ex.Message);
            }
        }

        /// <summary>MT4 API 제공 시세 기준으로 전일대비(Contrast)/등락률(ContrastPer) 설정. 세션시가 → M1시가 → D1 전일종가 순으로 기준가 확보.</summary>
        private void FillContrastFromApi(Current current)
        {
            try
            {
                double refPrice = 0;
                double sessionOpen = 0;
                double d1Close = 0, d1Open = 0;
                int d1Count = 0;
                lock (_mtLock)
                {
                    if (_mtApiClient != null && _mtApiClient.ConnectionState == MtConnectionState.Connected)
                    {
                        try
                        {
                            sessionOpen = _mtApiClient.SymbolInfoDouble(ItemSymbol, EnumSymbolInfoDouble.SYMBOL_SESSION_OPEN);
                            refPrice = sessionOpen;
                        }
                        catch
                        {
                            refPrice = 0;
                        }
                    }
                }
                if (refPrice <= 0)
                    refPrice = current.StartPrice;
                if (refPrice <= 0)
                {
                    lock (_mtLock)
                    {
                        if (_mtApiClient != null && _mtApiClient.ConnectionState == MtConnectionState.Connected)
                        {
                            try
                            {
                                var d1 = _mtApiClient.CopyRates(ItemSymbol, MtApi.ENUM_TIMEFRAMES.PERIOD_D1, 1, 1)?.ToArray() ?? new MqlRates[0];
                                d1Count = d1 != null ? d1.Length : 0;
                                if (d1 != null && d1.Length > 0)
                                {
                                    d1Close = d1[0].Close;
                                    d1Open = d1[0].Open;
                                    refPrice = d1Close > 0 ? d1Close : d1Open;
                                }
                            }
                            catch { }
                        }
                    }
                }
                if (refPrice > 0)
                {
                    current.Contrast = current.CurrentPrice - refPrice;
                    current.ContrastPer = (current.CurrentPrice - refPrice) / refPrice * 100.0;
                }
                else
                {
                    // [등락률 디버그] 기준가가 0일 때만 10초마다 로그
                    // if (Math.Abs(Environment.TickCount - m_tickContrastDiag) >= 10000)
                    // {
                    //     m_tickContrastDiag = Environment.TickCount;
                    //     string msg = string.Format("[등락률진단] 기준가=0 → SESSION_OPEN={0}, M1시가={1}, D1봉수={2}, D1Close={3}, D1Open={4}",
                    //         sessionOpen, current.StartPrice, d1Count, d1Close, d1Open);
                    //     OnFutureSiteLogEvent(msg);
                    //     WriteLog("[등락률진단] symbol=" + (ItemSymbol ?? "") + " " + msg);
                    // }
                }
            }
            catch (Exception ex)
            {
                // WriteLog("[FillContrastFromApi] " + ex.Message);
                // OnFutureSiteLogEvent("[등락률진단] 예외: " + ex.Message);
            }
        }

        protected override ERRORCODE Prepare()
        {
            if (this.QuoteList == null)
                this.QuoteList = new List<QuoteInfo>();
            else this.QuoteList.Clear();

            if (this.CurrentList == null)
                this.CurrentList = new List<CurrentInfo>();
            else this.CurrentList.Clear();

            LoginState = LOGINSTATE.OK;
            DChartType = CHARTTYPE.NONE;
            RequestChart();

            return ERRORCODE.SUCCESS;
        }

        bool bQutoteCreated = false;
        protected override ERRORCODE Check()
        {
            //종목변경
            if (CurItemSymbol != null && this.ItemSymbol != CurItemSymbol.Symbol)
            {
                ItemSymbolInfo itemSymbol = ItemList.FirstOrDefault<ItemSymbolInfo>(it => it.Symbol == ItemSymbol);
                if (itemSymbol != null)
                {
                    CurItemSymbol = itemSymbol;
                    ItemPrecision = itemSymbol.Precision;
                    Settings.Default.PriceFormat = Common.GetPriceFormat(CurItemSymbol.Precision);

                    // OnFutureSiteLogEvent(CurItemSymbol.ItemName);
                    return ERRORCODE.PREPARE_FAILED;
                }
                else
                    return ERRORCODE.UNKNOWN_FAILED;
            }
            if (CurItemSymbol != null && CurItemSymbol.MidPrice != 0 && !bQutoteCreated)
            {
                CreateQuoteInfo();
            }

            // MT4 API 기준 주문 리스트 실시간 갱신 (다른 프로그램에서 청산해도 리스트에 반영)
            bool doRefreshOrderList = false;
            lock (_mtLock)
            {
                if (_mtApiClient != null && _mtApiClient.ConnectionState == MtConnectionState.Connected
                    && Math.Abs(Environment.TickCount - m_tickOrderList) >= ORDERLIST_REFRESH_MS)
                {
                    m_tickOrderList = Environment.TickCount;
                    doRefreshOrderList = true;
                }
            }
            if (doRefreshOrderList)
            {
                try { RequestOrderList(false); } catch (Exception ex) { WriteLog("[Check] RequestOrderList " + ex.Message); }
            }

            if (!m_bNeedAcc && Math.Abs(Environment.TickCount - m_tickAccount) < 60000)
                return ERRORCODE.SUCCESS;

            m_tickAccount = Environment.TickCount;
            ERRORCODE error_code = ERRORCODE.UNKNOWN_FAILED;

            m_bNeedAcc = false;

            CONSTATE con_state = RequestAccountData();
            switch (con_state)
            {
                case CONSTATE.SUCCESSS:
                    error_code = ERRORCODE.SUCCESS;

                    break;
                default: break;
            }


            return error_code;
        }

        /// <summary>CMG(MT4)는 서버 로그인 없이 MT4 API 연결만 사용하므로 비밀번호 없이도 Start 가능</summary>
        protected override bool AllowEmptyPassword => true;

        public override bool Start()
        {
            return base.Start();
        }

        protected override void OnStarted()
        {
            base.OnStarted();

        }

        protected override void OnStopped(bool bAutoStop)
        {
            DisconnectSocket();

            base.OnStopped(bAutoStop);
        }

        protected override void OnPrepare()
        {
            base.OnPrepare();

            if (CurItemSymbol != null)
            {
                CurItemSymbol.MidPrice = 0;
                OnFutureSiteLogEvent(string.Format("[종목] {0} => 틱단위:{1}, 주문수량단위:{2}", CurItemSymbol.Symbol, CurItemSymbol.OverTick * CurItemSymbol.TickScale, CurItemSymbol.VolumeStep)); //CurItemSymbol.MinVolume, CurItemSymbol.MaxVolume
            }
            Current = null;

            reqQuote(ItemSymbol, mt_oldSymbol);
            this.ValuationList[0].CurrentProfit = this.ValuationList[0].TotalProfit;
            CurrentList.Clear();
            OrderList.Clear();
            RequestOrderList(false);
        }


        public override bool DoSellOrder(QuoteInfo quoteInfo, double nQuantity = 1, bool bMarketPrice = false)
        {

            if (this.CurrentUserAccount == null || string.IsNullOrEmpty(CurrentUserAccount.UserAccountId))
            {
                OnFutureSiteLogEvent("[주문] 계좌정보 오류!");
                return false;
            }

            if (CurItemSymbol.VolumeStep == 0)
            {
                OnFutureSiteLogEvent("[주문] 주문가능한 종목이 아닙니다!");
                return false;
            }

            //nQuantity = nQuantity * CurItemSymbol.VolumeStep;
            string temp = string.Format("{0:N3}", nQuantity);
            nQuantity = double.Parse(temp);
            if (nQuantity < CurItemSymbol.MinVolume || nQuantity > CurItemSymbol.MaxVolume)
            {
                OnFutureSiteLogEvent(string.Format("[주문] 주문수량 오류!"));
                return false;
            }
            if (CurrentUserAccount.Balance < 1000 * nQuantity)
            {
                OnFutureSiteLogEvent("[주문] 담보금 부족!");
                return false;
            }

            if (Current == null || Current.CurrentPrice <= 0)
            {
                OnFutureSiteLogEvent("[주문] 주문기간이 아닙니다.");
                return false;
            }

            if (!bMarketPrice && quoteInfo != null)
            {
                temp = string.Format("{0:N6}", quoteInfo.Price);
                quoteInfo.Price = double.Parse(temp);

                double nOrderRange = 4000 * CurItemSymbol.OverTick;
                if (quoteInfo.Price < Current.CurrentPrice2 - nOrderRange || quoteInfo.Price > Current.CurrentPrice2 + nOrderRange)
                {
                    OnFutureSiteLogEvent("[주문] 주문 가격이 초과 됨");
                    return false;
                }
            }
            else bMarketPrice = true;

            var param_list = new Dictionary<string, object>();
            if (bMarketPrice)
            {
                param_list.Add("actionType", "ORDER_TYPE_SELL");
                param_list.Add("symbol", ItemSymbol);
                param_list.Add("volume", nQuantity);
                param_list.Add("takeProfit", 0);

                OnFutureSiteLogEvent(string.Format("[매도주문] 주문가:시장가, 주문수량:{0}", nQuantity));
            }
            else
            {
                param_list.Add("actionType", "ORDER_TYPE_SELL_LIMIT");
                param_list.Add("symbol", ItemSymbol);
                param_list.Add("volume", nQuantity);
                param_list.Add("openPrice", quoteInfo.Price);
                OnFutureSiteLogEvent(string.Format("[매도주문] 주문가:{0}, 주문수량:{1}", quoteInfo.Price, nQuantity));
            }

            lock (_mtLock)
            {
                if (_mtApiClient == null || _mtApiClient.ConnectionState != MtConnectionState.Connected)
                {
                    OnFutureSiteLogEvent("[주문] MtApi 미연결");
                    return false;
                }
                try
                {
                    double price = bMarketPrice ? _mtApiClient.SymbolInfoDouble(ItemSymbol, EnumSymbolInfoDouble.SYMBOL_ASK) : quoteInfo.Price;
                    TradeOperation orderType = bMarketPrice ? TradeOperation.OP_SELL : TradeOperation.OP_SELLLIMIT;
                    int ticket = _mtApiClient.OrderSend(ItemSymbol, orderType, nQuantity, price, 30, 0, 0, "CMG", 0, DateTime.MinValue);
                    if (ticket < 0)
                    {
                        int err = _mtApiClient.GetLastError();
                        OnFutureSiteLogEvent(string.Format("[주문] 실패(err={0})", err));
                        return false;
                    }
                    WriteLog(string.Format("[DoSellOrder] ticket={0}", ticket));
                }
                catch (Exception ex)
                {
                    OnFutureSiteLogEvent("[주문] " + ex.Message);
                    return false;
                }
            }
            // 주문 리스트 갱신 후 ORDER 이벤트로 UI 현시 (기존 프로젝트와 동일한 갱신 흐름)
            Thread.Sleep(200);
            RequestOrderList(false);
            return true;
        }

        public override bool DoBuyOrder(QuoteInfo quoteInfo, double nQuantity = 1, bool bMarketPrice = false)
        {
            if (this.CurrentUserAccount == null || string.IsNullOrEmpty(CurrentUserAccount.UserAccountId))
            {
                OnFutureSiteLogEvent("[주문] 계좌정보 오류!");
                return false;
            }
            if (CurItemSymbol.VolumeStep == 0)
            {
                OnFutureSiteLogEvent("[주문] 주문가능한 종목이 아닙니다!");
                return false;
            }
            //nQuantity = nQuantity * CurItemSymbol.VolumeStep;
            string temp = string.Format("{0:N3}", nQuantity);
            nQuantity = double.Parse(temp);
            if (nQuantity < CurItemSymbol.MinVolume || nQuantity > CurItemSymbol.MaxVolume)
            {
                OnFutureSiteLogEvent(string.Format("[주문] 주문수량 오류!"));
                return false;
            }

            if (CurrentUserAccount.Balance < 1000 * nQuantity)
            {
                OnFutureSiteLogEvent("[주문] 담보금 부족!");
                return false;
            }

            if (Current == null || Current.CurrentPrice <= 0)
            {
                OnFutureSiteLogEvent("[주문] 주문기간이 아닙니다.");
                return false;
            }

            if (!bMarketPrice && quoteInfo != null)
            {
                temp = string.Format("{0:N6}", quoteInfo.Price);
                quoteInfo.Price = double.Parse(temp);

                double nOrderRange = 4000 * CurItemSymbol.OverTick;
                if (quoteInfo.Price < Current.CurrentPrice - nOrderRange || quoteInfo.Price > Current.CurrentPrice + nOrderRange)
                {
                    OnFutureSiteLogEvent("[주문] 주문 가격이 초과 됨");
                    return false;
                }
            }
            else bMarketPrice = true;

            var param_list = new Dictionary<string, object>();
            if (bMarketPrice)
            {
                param_list.Add("actionType", "ORDER_TYPE_BUY");
                param_list.Add("symbol", ItemSymbol);
                param_list.Add("volume", nQuantity);
                param_list.Add("takeProfit", 0);
                OnFutureSiteLogEvent(string.Format("[매수주문] 주문가:시장가, 주문수량:{0}", nQuantity));
            }
            else
            {
                param_list.Add("actionType", "ORDER_TYPE_BUY_LIMIT");
                param_list.Add("symbol", ItemSymbol);
                param_list.Add("volume", nQuantity);
                param_list.Add("openPrice", quoteInfo.Price);
                OnFutureSiteLogEvent(string.Format("[매수주문] 주문가:{0}, 주문수량:{1}", quoteInfo.Price, nQuantity));
            }

            lock (_mtLock)
            {
                if (_mtApiClient == null || _mtApiClient.ConnectionState != MtConnectionState.Connected)
                {
                    OnFutureSiteLogEvent("[주문] MtApi 미연결");
                    return false;
                }
                try
                {
                    double price = bMarketPrice ? _mtApiClient.SymbolInfoDouble(ItemSymbol, EnumSymbolInfoDouble.SYMBOL_BID) : quoteInfo.Price;
                    TradeOperation orderType = bMarketPrice ? TradeOperation.OP_BUY : TradeOperation.OP_BUYLIMIT;
                    int ticket = _mtApiClient.OrderSend(ItemSymbol, orderType, nQuantity, price, 30, 0, 0, "CMG", 0, DateTime.MinValue);
                    if (ticket < 0)
                    {
                        int err = _mtApiClient.GetLastError();
                        OnFutureSiteLogEvent(string.Format("[주문] 실패(err={0})", err));
                        return false;
                    }
                    WriteLog(string.Format("[DoBuyOrder] ticket={0}", ticket));
                }
                catch (Exception ex)
                {
                    OnFutureSiteLogEvent("[주문] " + ex.Message);
                    return false;
                }
            }
            // 주문 리스트 갱신 후 ORDER 이벤트로 UI 현시 (기존 프로젝트와 동일한 갱신 흐름)
            Thread.Sleep(200);
            RequestOrderList(false);

            return true;
        }


        public override bool CancelOrder(OrderInfo orderInfo)
        {
            if (this.CurrentUserAccount == null || string.IsNullOrEmpty(CurrentUserAccount.UserAccountId))
            {
                OnFutureSiteLogEvent("[주문취소] 계좌정보 오류!");
                return false;
            }

            lock (_mtLock)
            {
                if (_mtApiClient == null || _mtApiClient.ConnectionState != MtConnectionState.Connected)
                    return false;
                try
                {
                    int ticket = int.Parse(orderInfo.OrderNo);
                    bool ok = _mtApiClient.OrderDelete(ticket);
                    WriteLog(string.Format("[CancelOrder] ticket={0} result={1}", ticket, ok));
                    if (!ok) return false;
                }
                catch (Exception ex)
                {
                    WriteLog("[CancelOrder] " + ex.Message);
                    return false;
                }
            }
            Thread.Sleep(200);
            RequestOrderList(false);
            return true;
        }

        public override bool LiquidateOrder(OrderInfo orderInfo)
        {
            if (this.CurrentUserAccount == null)
                return false;

            lock (_mtLock)
            {
                if (_mtApiClient == null || _mtApiClient.ConnectionState != MtConnectionState.Connected)
                    return false;
                try
                {
                    int ticket = int.Parse(orderInfo.OrderNo);
                    double volume = orderInfo.OrderQty;
                    double price = orderInfo.TradeType == TRADETYPE.SELL ? _mtApiClient.SymbolInfoDouble(ItemSymbol, EnumSymbolInfoDouble.SYMBOL_BID) : _mtApiClient.SymbolInfoDouble(ItemSymbol, EnumSymbolInfoDouble.SYMBOL_ASK);
                    bool ok = _mtApiClient.OrderClose(ticket, volume, price, 30);
                    WriteLog(string.Format("[LiquidateOrder] ticket={0} result={1}", ticket, ok));
                    if (!ok) return false;
                    double profit = orderInfo.Valuation;
                    string logMsg = string.Format("[청산] 청산가:{0}", price);
                    logMsg += " " + (profit >= 0 ? "수익:" : "손실:") + string.Format("{0}", profit);
                    OnFutureSiteLogEvent(logMsg);
                }
                catch (Exception ex)
                {
                    WriteLog("[LiquidateOrder] " + ex.Message);
                    return false;
                }
            }
            Thread.Sleep(200);
            RequestOrderList(false);
            return true;
        }


        protected override List<QuoteInfo> CreateQuoteInfo()
        {
            List<QuoteInfo> list = new List<QuoteInfo>();
            if (CurItemSymbol == null)
                return list;
            if (CurItemSymbol.OverTick <= 0)
                return list;
            bQutoteCreated = true;

            Settings.Default.ItemOverTick = (float)(CurItemSymbol.OverTick * CurItemSymbol.TickScale);

            double upLimitPrice = CurItemSymbol.MidPrice + CurItemSymbol.OverTick * 2000.0;
            double downLimitPrice = CurItemSymbol.MidPrice - CurItemSymbol.OverTick * 2000.0;
            double tick = CurItemSymbol.OverTick;
            if (downLimitPrice < 1E-06)
            {
                downLimitPrice = tick;
            }
            if (upLimitPrice >= 1E-06 && downLimitPrice >= 1E-06)
            {
                int index = 0;
                double curPrice = upLimitPrice;
                while (curPrice > downLimitPrice || Math.Abs(curPrice - downLimitPrice) < 1E-06)
                {
                    list.Add(new QuoteInfo
                    {
                        QuoteInfoId = index,
                        Price = Math.Round(curPrice, CurItemSymbol.Precision),
                        PriceStr = string.Format(Settings.Default.PriceFormat, curPrice)
                    });

                    index++;
                    curPrice -= tick;
                }
            }

            this.QuoteList = list;
            return list;
        }


        private void RequestChart()
        {

            Thread.Sleep(500);
            RequestRChart();
            Thread.Sleep(500);
            RequestDChart((CHARTTYPE)Settings.Default.ChartType);

        }


        public override bool RequestDChart(CHARTTYPE chartType)
        {
            if (this.CurrentUserAccount == null)
                return false;

            if (DChartType == chartType)
                return true;

            string tmFrame = "";
            DChartType = chartType;
            switch (chartType)
            {
                case CHARTTYPE.MIN_1:
                    tmFrame = "1m";
                    break;
                case CHARTTYPE.MIN_5:
                    tmFrame = "5m";
                    break;
                case CHARTTYPE.MIN_15:
                    tmFrame = "15m";
                    break;
                case CHARTTYPE.MIN_30:
                    tmFrame = "30m";
                    break;
                default:
                    break;
            }
            if (tmFrame.Length < 1)
                return false;

            lock (_mtLock)
            {
                if (_mtApiClient == null || _mtApiClient.ConnectionState != MtConnectionState.Connected) return false;
                try
                {
                    int tf = GetMtApiTimeframe(tmFrame);
                    MqlRates[] rates = _mtApiClient.CopyRates(ItemSymbol, (MtApi.ENUM_TIMEFRAMES)tf, 0, 300)?.ToArray() ?? new MqlRates[0];
                    if (rates == null || rates.Length == 0) return false;

                    lock (CtrlProperty._DItemList) lock (CtrlProperty._CItemList)
                    {
                        CtrlProperty._DItemList.Clear();
                        CtrlProperty._CItemList.Clear();
                        for (int i = 0; i < rates.Length; i++)
                        {
                            var r = rates[i];
                            float fCurPrice = (float)r.Close;
                            int nConc = (int)r.TickVolume;
                            DateTime dtStart = r.Time;
                            DateTime dtEnd = CtrlProperty.GetEndTime(CtrlProperty._DTimeType, CtrlProperty._DTimeUnitAmt, dtStart, true);
                            float fStartPrice = (float)r.Open;
                            float fHighPrice = (float)r.High;
                            float fLowPrice = (float)r.Low;

                            var newDItem = new DItem(fStartPrice * CtrlProperty._nValueRate, fCurPrice * CtrlProperty._nValueRate, fLowPrice * CtrlProperty._nValueRate, fHighPrice * CtrlProperty._nValueRate,
                                CtrlProperty.GetTimeStamp(dtStart), CtrlProperty.GetTimeStamp(dtEnd), CtrlProperty._DItemList.Count, 0, nConc);
                            CtrlProperty._DItemList.Add(newDItem);
                            var newCItem = new CItem(CtrlProperty.GetTimeStamp(dtStart), CtrlProperty.GetTimeStamp(dtEnd), 0, nConc);
                            CtrlProperty._CItemList.Add(newCItem);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errMsg = ex.Message;
                    return false;
                }
            }

            return true;
        }

        private static int GetMtApiTimeframe(string tmFrame)
        {
            switch (tmFrame)
            {
                case "1m": return (int)MtApi.ENUM_TIMEFRAMES.PERIOD_M1;
                case "5m": return (int)MtApi.ENUM_TIMEFRAMES.PERIOD_M5;
                case "15m": return (int)MtApi.ENUM_TIMEFRAMES.PERIOD_M15;
                case "30m": return (int)MtApi.ENUM_TIMEFRAMES.PERIOD_M30;
                case "1h": return (int)MtApi.ENUM_TIMEFRAMES.PERIOD_H1;
                case "4h": return (int)MtApi.ENUM_TIMEFRAMES.PERIOD_H4;
                case "1d": return (int)MtApi.ENUM_TIMEFRAMES.PERIOD_D1;
                case "1w": return (int)MtApi.ENUM_TIMEFRAMES.PERIOD_W1;
                case "1mn": return (int)MtApi.ENUM_TIMEFRAMES.PERIOD_MN1;
                default: return (int)MtApi.ENUM_TIMEFRAMES.PERIOD_M30;
            }
        }

        public override bool RequestRChart()
        {
            if (this.CurrentUserAccount == null)
                return false;
            TIMETYPE timeType = CtrlProperty._RTimeType;
            TIMEUNIT timeUnit = CtrlProperty._RTimeUnitAmt;
            string tmFrame = "";
            switch (timeType)
            {
                case TIMETYPE.TIMETYPE_TICK:
                    break;
                case TIMETYPE.TIMETYPE_MIN:
                    if (timeUnit == TIMEUNIT.TIMEUNIT_1)
                        tmFrame = "1m";
                    else if (timeUnit == TIMEUNIT.TIMEUNIT_5)
                        tmFrame = "5m";
                    else if (timeUnit == TIMEUNIT.TIMEUNIT_15)
                        tmFrame = "15m";
                    else if (timeUnit == TIMEUNIT.TIMEUNIT_30)
                        tmFrame = "30m";
                    else if (timeUnit == TIMEUNIT.TIMEUNIT_60)
                        tmFrame = "1h";
                    else if (timeUnit == TIMEUNIT.TIMEUNIT_240)
                        tmFrame = "4h";
                    break;
                case TIMETYPE.TIMETYPE_DAY:
                    tmFrame = "1d";
                    break;
                case TIMETYPE.TIMETYPE_WEEK:
                    tmFrame = "1w";
                    break;
                case TIMETYPE.TIMETYPE_MONTH:
                    tmFrame = "1mn";
                    break;
                case TIMETYPE.TIMETYPE_YEAR:
                    break;
                default:
                    break;
            }
            if (tmFrame.Length < 1)
                return false;

            lock (_mtLock)
            {
                if (_mtApiClient == null || _mtApiClient.ConnectionState != MtConnectionState.Connected) return false;
                try
                {
                    CtrlProperty.SetValueRate(Common.GetPrecisionRate(ItemSymbol, CurItemSymbol.Precision), Common.GetValueFormat(ItemPrecision + 1), (float)CurItemSymbol.OverTick);

                    int tf = GetMtApiTimeframe(tmFrame);
                    MqlRates[] rates = _mtApiClient.CopyRates(ItemSymbol, (MtApi.ENUM_TIMEFRAMES)tf, 0, 300)?.ToArray() ?? new MqlRates[0];
                    if (rates == null || rates.Length == 0) return false;

                    lock (CtrlProperty._RItemList)
                    {
                        CtrlProperty._RItemList.Clear();
                        for (int i = 0; i < rates.Length; i++)
                        {
                            var r = rates[i];
                            float fCurPrice = (float)r.Close;
                            int nConc = (int)r.TickVolume;
                            DateTime dtStart = r.Time;
                            DateTime dtEnd = CtrlProperty.GetEndTime(CtrlProperty._RTimeType, CtrlProperty._RTimeUnitAmt, dtStart, true);
                            float fStartPrice = (float)r.Open;
                            float fHighPrice = (float)r.High;
                            float fLowPrice = (float)r.Low;

                            var itemNew = new RItem(fStartPrice * CtrlProperty._nValueRate, fCurPrice * CtrlProperty._nValueRate, fLowPrice * CtrlProperty._nValueRate, fHighPrice * CtrlProperty._nValueRate,
                                CtrlProperty.GetTimeStamp(dtStart), CtrlProperty.GetTimeStamp(dtEnd), CtrlProperty._RItemList.Count, 0, nConc);
                            CtrlProperty._RItemList.Add(itemNew);
                        }
                    }
                    OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.REDRAW);
                }
                catch (Exception ex)
                {
                    string errMsg = ex.Message;
                    return false;
                }
            }

            return true;
        }

        public override bool ChangePrd(string sSymbol)
        {
            if (PrdList == null || PrdList.Count < 1)
                return false;
            if (sSymbol != CurPrd.Code)
            {
                CurPrd = PrdList.FirstOrDefault(p => p.Code == sSymbol);
                if (CurPrd != null)
                {
                    ItemList = CurPrd.ItemList;
                    return true;
                }
            }

            return false;
        }
        public override bool ChangeItem(string sSymbol)
        {
            if (ItemList == null || ItemList.Count < 1)
                return false;
            if (ItemSymbol != sSymbol)
            {
                mt_oldSymbol = ItemSymbol;
                ItemSymbol = sSymbol;
                Prepared = false;
                bQutoteCreated = false;
                StartPrice = 0;

                return true;
            }

            return false;
        }

        private CONSTATE RequestAccountData()
        {
            if (this.CurrentUserAccount == null)
                return CONSTATE.NO_LOGIN;

            // MtApi: 계정 정보는 Check() 주기에서 필요시 AccountBalance/AccountEquity로 갱신 가능
            return CONSTATE.SUCCESSS;
        }

        private CONSTATE RequestHistoryDeals()
        {
            if (this.CurrentUserAccount == null)
                return CONSTATE.NO_LOGIN;

            try
            {
                lock (_mtLock)
                {
                    if (_mtApiClient == null || _mtApiClient.ConnectionState != MtConnectionState.Connected) return CONSTATE.ERR_REGAPI;
                    DateTime dtNow = DateTime.Now;
                    DateTime startTime = dtNow.Date.AddDays(-1);
                    DateTime endTime = dtNow;
                    var historyOrders = _mtApiClient.GetOrders(OrderSelectSource.MODE_HISTORY) ?? new List<MtOrder>();
                    double dProfit = 0;
                    foreach (var o in historyOrders)
                    {
                        if (o == null) continue;
                        DateTime closeTime = o.CloseTime;
                        if (closeTime >= startTime && closeTime <= endTime)
                            dProfit += o.Profit;
                    }
                    this.ValuationList[0].TotalProfit = dProfit;
                    DayProfitLoss.TotalProfit = (long)this.ValuationList[0].TotalProfit;
                    this.ValuationList[0].CurrentProfit = this.ValuationList[0].TotalProfit + this.ValuationList[0].TotalValuation;
                }
            }
            catch (Exception ex)
            {
                string msgg = ex.Message;
                return CONSTATE.ERR_LOGIN;
            }

            return CONSTATE.SUCCESSS;
        }

        private void RequestOrderList(bool bAccount)
        {

            Thread.Sleep(500);
            RequestOutstandOrder();
            Thread.Sleep(500);
            RequestRequidateOrder();

            if (bAccount)
            {
                m_bNeedAcc = bAccount;
            }
        }

        private CONSTATE RequestRequidateOrder()
        {

            if (this.CurrentUserAccount == null)
                return CONSTATE.NO_LOGIN;

            try
            {
                lock (OrderList)
                {
                    OrderList.RemoveAll(o => o.OrderType == "체결");

                    lock (_mtLock)
                    {
                        if (_mtApiClient == null || _mtApiClient.ConnectionState != MtConnectionState.Connected) return CONSTATE.ERR_REGAPI;
                        var orders = _mtApiClient.GetOrders(OrderSelectSource.MODE_TRADES) ?? new List<MtOrder>();
                        WriteLog(string.Format("[RequestRequidateOrder] cnt={0}", orders.Count));
                        foreach (var o in orders)
                        {
                            if (o == null || o.Operation != TradeOperation.OP_BUY && o.Operation != TradeOperation.OP_SELL) continue;
                            if (o.Ticket <= 0) continue;
                            if (o.Symbol != ItemSymbol) continue;
                            if (OrderList.FirstOrDefault(x => x.OrderNo == o.Ticket.ToString()) != null) continue;

                            string orderTime = o.OpenTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                            string typeStr = (o.Operation == TradeOperation.OP_SELL) ? "POSITION_TYPE_SELL" : "POSITION_TYPE_BUY";
                            OrderInfo orderInfo = new OrderInfo
                            {
                                OrderType = "체결",
                                Symbol = o.Symbol,
                                Qty = string.Format("{0}[{1}]", typeStr == "POSITION_TYPE_SELL" ? "매도" : "매수", o.Lots),
                                AveragePrice = o.OpenPrice.ToString(),
                                MaxAveragePrice = Math.Round(o.OpenPrice, 6),
                                CurrentPrice = o.ClosePrice.ToString(),
                                StartCciPrice = -10000,
                                Valuation = (int)o.Profit,
                                Action = "청산",
                                TradeType = typeStr == "POSITION_TYPE_SELL" ? TRADETYPE.SELL : TRADETYPE.BUY,
                                OrderQty = o.Lots,
                                OrderNo = o.Ticket.ToString(),
                                OrderDate = orderTime,
                            };
                            this.OrderList.Add(orderInfo);
                        }
                    }
                }

                OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.ORDER);
                SetUnliquidationPosition(OrderList);
            }
            catch (Exception ex)
            {
                WriteLog(string.Format("[RequestRequidateOrder] error={0}", ex.Message));
            }

            return CONSTATE.SUCCESSS;
        }

        private CONSTATE RequestOutstandOrder()
        {

            if (this.CurrentUserAccount == null)
                return CONSTATE.NO_LOGIN;

            try
            {
                lock (OrderList)
                {
                    OrderList.RemoveAll(o => o.OrderType == "미체결");

                    lock (_mtLock)
                    {
                        if (_mtApiClient == null || _mtApiClient.ConnectionState != MtConnectionState.Connected) return CONSTATE.ERR_REGAPI;
                        var orders = _mtApiClient.GetOrders(OrderSelectSource.MODE_TRADES) ?? new List<MtOrder>();
                        WriteLog(string.Format("[RequestOutstandOrder] cnt={0}", orders.Count));

                        foreach (var o in orders)
                        {
                            if (o == null) continue;
                            if (o.Operation != TradeOperation.OP_BUYLIMIT && o.Operation != TradeOperation.OP_SELLLIMIT && o.Operation != TradeOperation.OP_BUYSTOP && o.Operation != TradeOperation.OP_SELLSTOP) continue;
                            if (o.Ticket <= 0) continue;
                            if (o.Symbol != ItemSymbol) continue;
                            if (OrderList.FirstOrDefault(x => x.OrderNo == o.Ticket.ToString()) != null) continue;

                            string orderTime = o.OpenTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                            string typeStr = (o.Operation == TradeOperation.OP_SELLLIMIT || o.Operation == TradeOperation.OP_SELLSTOP) ? "POSITION_TYPE_SELL_LIMIT" : "POSITION_TYPE_BUY_LIMIT";
                            OrderInfo orderInfo = new OrderInfo
                            {
                                OrderType = "미체결",
                                Symbol = o.Symbol,
                                Qty = string.Format("{0}[{1}]", typeStr == "POSITION_TYPE_SELL_LIMIT" ? "매도" : "매수", o.Lots),
                                AveragePrice = o.OpenPrice.ToString(),
                                MaxAveragePrice = Math.Round(o.OpenPrice, 6),
                                CurrentPrice = o.ClosePrice.ToString(),
                                Valuation = 0L,
                                Action = "취소",
                                TradeType = typeStr == "POSITION_TYPE_SELL_LIMIT" ? TRADETYPE.SELL : TRADETYPE.BUY,
                                OrderQty = o.Lots,
                                OrderNo = o.Ticket.ToString(),
                                OrderTime = Environment.TickCount,
                                OrderDate = orderTime,
                            };
                            this.OrderList.Add(orderInfo);
                        }
                    }
                }

                SetUnliquidationPosition(OrderList);
                OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.ORDER);
            }
            catch (Exception ex)
            {
                WriteLog(string.Format("[RequestOutstandOrder] error={0}", ex.Message));
            }

            return CONSTATE.SUCCESSS;
        }

        private void onUpdatedOrders(JsonValue orders)
        {
            try
            {

                int cnt = orders.Count;
                for (int i = 0; i < cnt; i++)
                {
                    JsonValue order = orders[i];
                    string symbol = order["symbol"];

                    if (symbol != ItemSymbol)
                        continue;

                    string orderNo = order["id"];

                    OrderInfo orderInfo = OrderList.FirstOrDefault(o => o.OrderType == "미체결" && o.OrderNo == orderNo);
                    if (orderInfo != null)
                        continue;

                    string type = order["type"];
                    string state = order["state"];
                    if (state != "ORDER_STATE_PLACED")
                        continue;
                    double volume = order["volume"];
                    double openPrice = order["openPrice"];
                    double currentPrice = order["currentPrice"];
                    string orderTime = order["time"];

                    orderInfo = new OrderInfo
                    {
                        OrderType = "미체결",
                        Symbol = symbol,
                        Qty = string.Format("{0}[{1}]", type == "ORDER_TYPE_SELL_LIMIT" ? "매도" : "매수", volume),
                        AveragePrice = openPrice.ToString(),
                        MaxAveragePrice = Math.Round(openPrice, 6),
                        CurrentPrice = currentPrice.ToString(),
                        Valuation = 0L,
                        Action = "취소",
                        TradeType = type == "ORDER_TYPE_SELL_LIMIT" ? TRADETYPE.SELL : TRADETYPE.BUY,
                        OrderQty = volume,
                        OrderNo = orderNo,
                        OrderDate = orderTime,
                        OrderTime = Environment.TickCount,
                    };
                    this.OrderList.Add(orderInfo);

                    OnFutureSiteLogEvent(string.Format("[주문접수] {0} 주문가:{1}", type == "ORDER_TYPE_SELL_LIMIT" ? "매도" : "매수", openPrice));

                }

                SetUnliquidationPosition(OrderList);
                OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.ORDER);
            }
            catch (Exception ex)
            {
                WriteLog(string.Format("[onUpdatedOrders] error={0}", ex.Message));
            }
        }

        private void onUpdatedPosition(JsonValue jsonPos)
        {
            string orderNo = jsonPos["id"];
            string symbol = jsonPos["symbol"];

            if (symbol != ItemSymbol)
                return;
            OrderInfo orderInfo = OrderList.FirstOrDefault(o => o.OrderType == "체결" && o.OrderNo == orderNo);
            if (orderInfo != null)
            {
                OrderList.Remove(orderInfo);
            }

            orderInfo = OrderList.FirstOrDefault(o => o.OrderType == "미체결" && o.OrderNo == orderNo);

            try
            {
                string type = jsonPos["type"];
                double volume = jsonPos["volume"];
                double openPrice = jsonPos["openPrice"];
                double currentPrice = jsonPos["currentPrice"];
                double profit = jsonPos["profit"];
                string sTime = jsonPos["time"];

                bool bAdd = false;
                if (orderInfo == null)
                {
                    orderInfo = new OrderInfo();
                    bAdd = true;
                }
                orderInfo.Symbol = symbol;
                orderInfo.OrderType = "체결";
                orderInfo.Qty = string.Format("{0}[{1}]", type == "POSITION_TYPE_SELL" ? "매도" : "매수", volume);
                orderInfo.AveragePrice = openPrice.ToString();
                orderInfo.MaxAveragePrice = Math.Round(openPrice, 6);
                orderInfo.CurrentPrice = currentPrice.ToString();
                orderInfo.StartCciPrice = -10000;
                orderInfo.Valuation = profit;
                orderInfo.Action = "청산";
                orderInfo.TradeType = type == "POSITION_TYPE_SELL" ? TRADETYPE.SELL : TRADETYPE.BUY;
                orderInfo.OrderQty = volume;
                orderInfo.OrderNo = orderNo;
                orderInfo.OrderDate = sTime;

                if (bAdd)
                    OrderList.Add(orderInfo);

                OnFutureSiteLogEvent(string.Format("[체결] {0} 체결가:{1}", type == "POSITION_TYPE_SELL" ? "매도" : "매수", openPrice));

                DateTime time = DateTime.ParseExact(sTime, "yyyy-MM-ddTHH:mm:ss.fffZ", System.Globalization.CultureInfo.InvariantCulture);

                this.LiquidOrder = new OrderVal
                {
                    OrderType = "체결",
                    Symbol = ItemSymbol,
                    AveragePrice = string.Format(Settings.Default.PriceFormat, openPrice),
                    OrderTime = time,
                    OrderTm = Environment.TickCount,
                    ResultState = type == "POSITION_TYPE_SELL" ? RESULTSTATE.SELL : RESULTSTATE.BUY,
                    ConcState = CONCSTATE.CONCLUDE,
                    OrderQty = volume,
                };

                SetUnliquidationPosition(OrderList);
                OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.LIQUID);
            }
            catch (Exception ex)
            {
                WriteLog(string.Format("[onUpdatedPosition] error={0}", ex.Message));
            }

        }
        private void onRemovedPosition(string id, JsonValue deals)
        {
            OrderInfo orderInfo = OrderList.FirstOrDefault(o => o.OrderNo == id);
            if (orderInfo == null)
                return;

            try
            {
                if (orderInfo.OrderType == "체결")
                {
                    int cnt = deals.Count;
                    for (int i = 0; i < cnt; i++)
                    {
                        JsonValue deal = deals[i];
                        if (deal["id"] == id)
                        {
                            string type = deal["type"];
                            double volume = deal["volume"];
                            double price = deal["price"];
                            double profit = deal["profit"];
                            string sTime = deal["time"];
                            DateTime time = DateTime.ParseExact(sTime, "yyyy-MM-ddTHH:mm:ss.fffZ", System.Globalization.CultureInfo.InvariantCulture);

                            this.LiquidOrder = new OrderVal
                            {
                                OrderType = "청산",
                                Symbol = ItemSymbol,
                                AveragePrice = string.Format(Settings.Default.PriceFormat, price),
                                OrderTime = time,
                                OrderTm = Environment.TickCount,
                                ResultState = type == "DEAL_TYPE_SELL" ? RESULTSTATE.BUY : RESULTSTATE.SELL,
                                ConcState = CONCSTATE.LIQUID,
                                OrderQty = volume,
                            };

                            string logMsg = string.Format("[청산] 청산가:{0}", price);
                            logMsg += "(" + (profit > 0 ? "수익:" : "손실:") + string.Format("{0})", profit);

                            this.ValuationList[0].TotalProfit += profit;
                            this.DayProfitLoss.TotalProfit = (long)ValuationList[0].TotalProfit;

                            OnFutureSiteLogEvent(logMsg);
                            break;
                        }
                    }
                }
                OrderList.RemoveAll(o => o.OrderNo == id);
                OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.LIQUID);
            }
            catch (Exception ex)
            {
                WriteLog(string.Format("[onRemovedPosition] error={0}", ex.Message));
            }
        }
        private void onCompletedOrder(string id, JsonValue historyOrders)
        {
            OrderInfo orderInfo = OrderList.FirstOrDefault(o => o.OrderNo == id);
            if (orderInfo == null)
                return;

            try
            {
                if (orderInfo.OrderType == "미체결")
                {
                    int cnt = historyOrders.Count;
                    for (int i = 0; i < cnt; i++)
                    {
                        JsonValue order = historyOrders[i];
                        if (order["id"] == id)
                        {
                            string type = order["type"];
                            string state = order["state"];
                            double volume = order["volume"];
                            double openPrice = order["openPrice"];
                            //double currentPrice = order["currentPrice"];
                            //double profit = order["profit"];
                            //string orderTime = order["time"];

                            OnFutureSiteLogEvent(string.Format("[주문취소] {0}", type == "ORDER_TYPE_BUY_LIMIT" ? "매수" : "매도"));
                            break;
                        }
                    }
                }
                OrderList.RemoveAll(o => o.OrderNo == id);
                OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.LIQUID);
            }
            catch (Exception ex)
            {
                WriteLog(string.Format("[onCompletedOrder] error={0}", ex.Message));
            }
        }

        private void onAccountInformation(JsonValue json)
        {
            try
            {
                double balance = json["balance"];
                this.CurrentUserAccount.Balance = balance;
                double valuation = json["equity"] - balance;
                //this.ValuationList[0].CurrentProfit = this.ValuationList[0].TotalProfit + valuation;

                OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.VALUATION);

                WriteLog(string.Format("[onAccountInformation] balance={0}, valuation={1}", balance, valuation));
            }
            catch (Exception ex)
            {
                WriteLog(string.Format("[onAccountInformation] error={0}", ex.Message));
            }
        }
        private void OnReceiveCurrent(Current current)
        {
            try
            {
                if (Math.Abs(Environment.TickCount - m_tickCurrent) > 50)
                {
                    m_tickCurrent = Environment.TickCount;
                    lock (this.OrderList)
                    {
                        if (this.OrderList != null)
                        {
                            int nOrderCnt = this.OrderList.Count;
                            if (nOrderCnt > 0)
                            {
                                int nReqCnt = 0;
                                OrderInfo orderInfo;
                                double lValSum = 0;
                                double dAveragePrice = 0.0;
                                double dAveragePriceSum = 0.0;
                                for (int iRow = 0; iRow < nOrderCnt; iRow++)
                                {
                                    orderInfo = OrderList[iRow];
                                    if (orderInfo == null)
                                        break;

                                    if (orderInfo.OrderType.StartsWith("미체결"))
                                    {
                                        orderInfo.CurrentPrice = string.Format(Settings.Default.PriceFormat, orderInfo.TradeType == TRADETYPE.SELL ? current.CurrentPrice2 : current.CurrentPrice);
                                        continue;
                                    }

                                    //if (double.Parse(orderInfo.CurrentPrice) != current.CurrentPrice)
                                    {
                                        orderInfo.CurrentPrice = string.Format(Settings.Default.PriceFormat, orderInfo.TradeType == TRADETYPE.SELL ? current.CurrentPrice2 : current.CurrentPrice);
                                        dAveragePrice = double.Parse(orderInfo.AveragePrice);
                                        dAveragePriceSum += dAveragePrice;
                                        orderInfo.Valuation = orderInfo.TradeType == TRADETYPE.SELL ?
                                            ((dAveragePrice - current.CurrentPrice2) / CurItemSymbol.OverTick * CurItemSymbol.ValueTick * CurItemSymbol.Exchange * orderInfo.OrderQty) :
                                            ((current.CurrentPrice - dAveragePrice) / CurItemSymbol.OverTick * CurItemSymbol.ValueTick * CurItemSymbol.Exchange * orderInfo.OrderQty);
                                        if (orderInfo.TradeType == TRADETYPE.SELL)  //매도
                                        {
                                            if (current.CurrentPrice2 < orderInfo.MaxAveragePrice)
                                            {
                                                orderInfo.MaxAveragePrice = current.CurrentPrice2;
                                            }
                                        }
                                        else if (orderInfo.TradeType == TRADETYPE.BUY)  //매수
                                        {
                                            if (current.CurrentPrice > orderInfo.MaxAveragePrice)
                                            {
                                                orderInfo.MaxAveragePrice = current.CurrentPrice;
                                            }
                                        }
                                    }
                                    lValSum += orderInfo.Valuation;
                                    nReqCnt++;
                                }
                                ValuationList[0].Valuation = lValSum;
                                ValuationList[0].TotalValuation = lValSum;
                                if (dAveragePriceSum > 0 && nReqCnt > 0)
                                    ValuationList[0].AverageUnitPrice = Math.Round(dAveragePriceSum / nReqCnt, 6);
                                ValuationList[0].CurrentProfit = ValuationList[0].TotalProfit + lValSum;

                            }
                            else if (nOrderCnt <= 0)
                            {
                                ValuationList[0].Valuation = 0;
                                ValuationList[0].TotalValuation = 0;
                                ValuationList[0].AverageUnitPrice = 0;
                                ValuationList[0].CurrentProfit = ValuationList[0].TotalProfit;
                            }
                        }
                    }
                    SetHiLowPrice(current);
                }

            }
            catch (Exception)
            {

            }

            SetCurrentInfo(current);
            SetItemPriceInfo(current);

            OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.CURRENT);
        }


        private QuoteInfo FindQuoteInfo(List<QuoteInfo> target, double price)
        {
            double num = double.MaxValue;
            int index = 0;
            if (Math.Abs(price) < 1E-06)
            {
                return target[0];
            }
            foreach (QuoteInfo quoteInfo in target)
            {
                if (Math.Abs(quoteInfo.Price - price) < num)
                {
                    num = Math.Abs(quoteInfo.Price - price);
                    index = quoteInfo.QuoteInfoId;
                }
            }
            return target[index];
        }


        private void SetTotalQuoteInfo(Quote quote)
        {
            if (this.TotalQuoteList == null)
                return;

            this.TotalQuoteList[0].TotalAskQty = quote.TotalAskQty;
            this.TotalQuoteList[0].TotalAskCount = quote.TotalAskCount;
            this.TotalQuoteList[0].Difference = quote.TotalBidQty - quote.TotalAskQty;
            this.TotalQuoteList[0].TotalBidQty = quote.TotalBidQty;
            this.TotalQuoteList[0].TotalBidCount = quote.TotalBidCount;
        }
        private void SetItemPriceInfo(Current current)
        {
            if (this.ItemPriceList == null)
                return;

            this.ItemPriceList[0].CurrentPrice = string.Format(Settings.Default.PriceFormat, current.CurrentPrice);
            this.ItemPriceList[0].Contrast = current.Contrast;
            this.ItemPriceList[0].ContrastPer = current.ContrastPer;
            this.ItemPriceList[0].StartPrice = string.Format(Settings.Default.PriceFormat, current.StartPrice);
            this.ItemPriceList[0].HighPrice = string.Format(Settings.Default.PriceFormat, current.HighPrice);
            this.ItemPriceList[0].LowPrice = string.Format(Settings.Default.PriceFormat, current.LowPrice);
        }

        private void SetCurrentInfo(Current current)
        {
            if (this.CurrentList == null)
                return;
            string strPrice = String.Format(Settings.Default.PriceFormat, Math.Round(current.CurrentPrice, 6));
            string strPrice2 = String.Format(Settings.Default.PriceFormat, Math.Round(current.CurrentPrice2, 6));
            this.Current = new CurrentInfo
            {
                Time = current.ReceivedDate,
                CurrentPrice = current.CurrentPrice,
                CurrentPrice2 = current.CurrentPrice2,
                CurrentPriceStr = strPrice,
                CurrentPrice2Str = strPrice2,
                ConclusionQty = this.CurrentList.Any<CurrentInfo>() ? current.ConclusionVolume : 0,
                TradeType = (current.TradeType == TradeType.Sell) ? TRADETYPE.SELL : TRADETYPE.BUY
            };
            this.CurrentList.Insert(0, this.Current);

            if (this.CurrentList.Count > 500)
                this.CurrentList.RemoveAt(500);
        }

        private void SetHiLowPrice(Current current)
        {
            if (this.QuoteList != null && this.QuoteList.Any<QuoteInfo>())
            {
                foreach (QuoteInfo quoteInfo in this._oldHiLowInfo)
                    quoteInfo.PriceSymbol = null;

                List<QuoteInfo> list = new List<QuoteInfo>();
                this.CurrentPriceRow = this.FindQuoteInfo(this.QuoteList, current.CurrentPrice);
                list.Add(this.CurrentPriceRow);
                this.BeforeClosePriceRow = this.FindQuoteInfo(this.QuoteList, current.BeforeClosePrice);
                this.BeforeClosePriceRow.PriceSymbol = "전";
                list.Add(this.BeforeClosePriceRow);
                this.LowPriceRow = this.FindQuoteInfo(this.QuoteList, current.LowPrice);
                this.LowPriceRow.PriceSymbol = "저";
                list.Add(this.LowPriceRow);
                this.HighPriceRow = this.FindQuoteInfo(this.QuoteList, current.HighPrice);
                this.HighPriceRow.PriceSymbol = "고";
                list.Add(this.HighPriceRow);
                this.StartPriceRow = this.FindQuoteInfo(this.QuoteList, current.StartPrice);
                this.StartPriceRow.PriceSymbol = "시";
                list.Add(this.StartPriceRow);
                this._oldHiLowInfo = list;
            }
        }

        private void SetUnliquidationPosition(List<OrderInfo> orders)
        {
            int num = (this.PositionRow == null) ? 0 : this.PositionRow.QuoteInfoId;
            List<QuoteInfo> list = new List<QuoteInfo>();
            foreach (OrderInfo order in orders)
            {
                list.Add(this.FindQuoteInfo(this.QuoteList, Double.Parse(order.AveragePrice)));
            }
            if (list.Any<QuoteInfo>())
            {
                this.PositionRow = this.QuoteList[Convert.ToInt32(list.Average((QuoteInfo q) => q.QuoteInfoId))];
                this.PositionTradeType = new TRADETYPE?(
                    (from o in orders select o.TradeTypeNo).FirstOrDefault<string>() == "1" ?
                     TRADETYPE.SELL : TRADETYPE.BUY
                );
            }
            else
            {
                this.PositionRow = null;
                this.PositionTradeType = null;
            }
        }

        private void reqItemlist()
        {
            if (PrdList.Count > 0) return;
            PrdList.Clear();

            PrdInfo newPrd;
            ItemSymbolInfo newItem;

            List<string> symbols = new List<string>();
            lock (_mtLock)
            {
                if (_mtApiClient == null || _mtApiClient.ConnectionState != MtConnectionState.Connected) return;
                try
                {
                    int total = _mtApiClient.SymbolsTotal(false);
                    for (int i = 0; i < total; i++)
                    {
                        string name = _mtApiClient.SymbolName(i, false);
                        if (!string.IsNullOrEmpty(name))
                            symbols.Add(name);
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    return;
                }
            }

            List<ItemSymbolInfo> allItems = getAllItem();

            foreach (string symbol in symbols)
            {
                newItem = allItems.FirstOrDefault(i => i.ItemName == symbol);
                if (newItem == null)
                    continue;

                newPrd = PrdList.FirstOrDefault(p => p.Name == newItem.PrdName);
                if (newPrd == null)
                {
                    newPrd = new PrdInfo();
                    newPrd.Code = newItem.PrdName;
                    newPrd.Name = newItem.PrdName;
                    newPrd.ItemList = new List<ItemSymbolInfo>();

                    PrdList.Add(newPrd);
                }

                newPrd.ItemList.Add(newItem);
                if (ItemSymbol.Length > 0 && ItemSymbol == newItem.Symbol)
                {
                    CurPrd = newPrd;
                    ItemSymbol = newItem.Symbol;
                    CurItemSymbol = newItem;
                    ItemList = newPrd.ItemList;
                }
                else if (ItemSymbol.Length < 1 && newItem.Symbol.IndexOf("NAS100") >= 0)
                {
                    CurPrd = newPrd;
                    ItemSymbol = newItem.Symbol;
                    CurItemSymbol = newItem;
                    ItemList = newPrd.ItemList;
                }

            }

            if (ItemSymbol.Length < 1 && PrdList.Count > 0)
            {
                CurPrd = PrdList.First();
                ItemList = CurPrd.ItemList;
                CurItemSymbol = CurPrd.ItemList.First();
                ItemSymbol = CurItemSymbol.Symbol;
            }

            if (CurItemSymbol != null)
            {
                ItemPrecision = CurItemSymbol.Precision;
                if (!Settings.Default.SignalSiteOn)
                {
                    Settings.Default.PriceFormat = Common.GetPriceFormat(ItemPrecision);
                    OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.PREPAREITEM);
                }
            }
        }

        private List<ItemSymbolInfo> getAllItem()
        {

            ItemSymbolInfo newItem;

            List<ItemSymbolInfo> itemList = new List<ItemSymbolInfo>();
            //FUTURE CFDs.ecn Count=16
            newItem = new ItemSymbolInfo() { Symbol = "CAC40.ecn", PrdName = "FUTURE CFDs.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.1, ItemName = "CAC40.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "CHINA50.ecn", PrdName = "FUTURE CFDs.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "CHINA50.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "COCOA.ecn", PrdName = "FUTURE CFDs.ecn", Precision = 0, TickScale = 1, OverTick = 1, Exchange = 1, ValueTick = 2, ItemName = "COCOA.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "COFFEE.ecn", PrdName = "FUTURE CFDs.ecn", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "COFFEE.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "COPPER.ecn", PrdName = "FUTURE CFDs.ecn", Precision = 4, TickScale = 1, OverTick = 0.0001, Exchange = 1, ValueTick = 0.01, ItemName = "COPPER.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "DAX40.ecn", PrdName = "FUTURE CFDs.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.25, ItemName = "DAX40.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "DJ30.ecn", PrdName = "FUTURE CFDs.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.05, ItemName = "DJ30.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EUSTX50.ecn", PrdName = "FUTURE CFDs.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.1, ItemName = "EUSTX50.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "FT100.ecn", PrdName = "FUTURE CFDs.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.1, ItemName = "FT100.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "HSI.ecn", PrdName = "FUTURE CFDs.ecn", Precision = 0, TickScale = 1, OverTick = 1, Exchange = 1, ValueTick = 6.5, ItemName = "HSI.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "NAS100.ecn", PrdName = "FUTURE CFDs.ecn", Precision = 2, TickScale = 25, OverTick = 0.01, Exchange = 1, ValueTick = 0.2, ItemName = "NAS100.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "NK225.ecn", PrdName = "FUTURE CFDs.ecn", Precision = 0, TickScale = 1, OverTick = 1, Exchange = 1, ValueTick = 3.3, ItemName = "NK225.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "S&P.ecn", PrdName = "FUTURE CFDs.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.5, ItemName = "S&P.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "SOYBEAN.ecn", PrdName = "FUTURE CFDs.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "SOYBEAN.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "SPI200.ecn", PrdName = "FUTURE CFDs.ecn", Precision = 1, TickScale = 1, OverTick = 0.1, Exchange = 1, ValueTick = 2.5, ItemName = "SPI200.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "VIX.ecn", PrdName = "FUTURE CFDs.ecn", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 1, ItemName = "VIX.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);

            //FUTURE CFDs.ins Count=19
            newItem = new ItemSymbolInfo() { Symbol = "CAC40.ins", PrdName = "FUTURE CFDs.ins", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.1, ItemName = "CAC40.ins", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "CHINA50.ins", PrdName = "FUTURE CFDs.ins", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "CHINA50.ins", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "COCOA.ins", PrdName = "FUTURE CFDs.ins", Precision = 0, TickScale = 1, OverTick = 1, Exchange = 1, ValueTick = 2, ItemName = "COCOA.ins", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "COFFEE.ins", PrdName = "FUTURE CFDs.ins", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "COFFEE.ins", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "COPPER.ins", PrdName = "FUTURE CFDs.ins", Precision = 4, TickScale = 1, OverTick = 0.0001, Exchange = 1, ValueTick = 0.01, ItemName = "COPPER.ins", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "DAX40.ins", PrdName = "FUTURE CFDs.ins", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.25, ItemName = "DAX40.ins", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "DJ30.ins", PrdName = "FUTURE CFDs.ins", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.05, ItemName = "DJ30.ins", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EUSTX50.ins", PrdName = "FUTURE CFDs.ins", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.1, ItemName = "EUSTX50.ins", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "FT100.ins", PrdName = "FUTURE CFDs.ins", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.1, ItemName = "FT100.ins", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "HSI.ins", PrdName = "FUTURE CFDs.ins", Precision = 0, TickScale = 1, OverTick = 1, Exchange = 1, ValueTick = 6.5, ItemName = "HSI.ins", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "NAS100.ins", PrdName = "FUTURE CFDs.ins", Precision = 2, TickScale = 25, OverTick = 0.01, Exchange = 1, ValueTick = 0.2, ItemName = "NAS100.ins", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "NATGAS.ins", PrdName = "FUTURE CFDs.ins", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 1, ItemName = "NATGAS.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "NK225.ins", PrdName = "FUTURE CFDs.ins", Precision = 0, TickScale = 1, OverTick = 1, Exchange = 1, ValueTick = 3.3, ItemName = "NK225.ins", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "S&P.ins", PrdName = "FUTURE CFDs.ins", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.5, ItemName = "S&P.ins", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "SOYBEAN.ins", PrdName = "FUTURE CFDs.ins", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "SOYBEAN.ins", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "SPI200.ins", PrdName = "FUTURE CFDs.ins", Precision = 1, TickScale = 1, OverTick = 0.1, Exchange = 1, ValueTick = 2.5, ItemName = "SPI200.ins", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "VIX.ins", PrdName = "FUTURE CFDs.ins", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 1, ItemName = "VIX.ins", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "WTI.ins", PrdName = "FUTURE CFDs.ins", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 10, ItemName = "WTI.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);

            //FX MAJOR STD Count=10
            newItem = new ItemSymbolInfo() { Symbol = "AUDUSD", PrdName = "FX MAJOR STD", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "AUDUSD", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURUSD", PrdName = "FX MAJOR STD", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "EURUSD", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPUSD", PrdName = "FX MAJOR STD", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "GBPUSD", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "NZDUSD", PrdName = "FX MAJOR STD", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "NZDUSD", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDCAD", PrdName = "FX MAJOR STD", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "USDCAD", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDCHF", PrdName = "FX MAJOR STD", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "USDCHF", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 }; ;
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDCNH", PrdName = "FX MAJOR STD", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "USDCNH", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDHKD", PrdName = "FX MAJOR STD", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "USDHKD", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDJPY", PrdName = "FX MAJOR STD", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 1, ItemName = "USDJPY", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDSGD", PrdName = "FX MAJOR STD", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "USDSGD", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            itemList.Add(newItem);

            //FOREX.ins Count=61
            newItem = new ItemSymbolInfo() { Symbol = "AUDCAD.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "AUDCAD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "AUDCHF.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "AUDCHF.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "AUDJPY.ins", PrdName = "FOREX.ins", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 1, ItemName = "AUDJPY.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "AUDNZD.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "AUDNZD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "AUDSGD.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "AUDSGD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "AUDUSD.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "AUDUSD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "CADCHF.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "CADCHF.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "CADJPY.ins", PrdName = "FOREX.ins", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 1, ItemName = "CADJPY.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "CADSGD.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "CADSGD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "CHFJPY.ins", PrdName = "FOREX.ins", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 1, ItemName = "CHFJPY.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "CHFSEK.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "CHFSEK.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "CHFSGD.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "CHFSGD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURAUD.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "EURAUD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURCAD.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "EURCAD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURCHF.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "EURCHF.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURCZK.ins", PrdName = "FOREX.ins", Precision = 4, TickScale = 1, OverTick = 0.0001, Exchange = 1, ValueTick = 1, ItemName = "EURCZK.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURGBP.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "EURGBP.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURJPY.ins", PrdName = "FOREX.ins", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 1, ItemName = "EURJPY.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURNOK.ins", PrdName = "FOREX.ins", Precision = 4, TickScale = 1, OverTick = 0.0001, Exchange = 1, ValueTick = 1, ItemName = "EURNOK.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURNZD.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "EURNZD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURPLN.ins", PrdName = "FOREX.ins", Precision = 4, TickScale = 1, OverTick = 0.0001, Exchange = 1, ValueTick = 1, ItemName = "EURPLN.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURSEK.ins", PrdName = "FOREX.ins", Precision = 4, TickScale = 1, OverTick = 0.0001, Exchange = 1, ValueTick = 1, ItemName = "EURSEK.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURSGD.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "EURSGD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURUSD.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "EURUSD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPAUD.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "GBPAUD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPCAD.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "GBPCAD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPCHF.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "GBPCHF.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPJPY.ins", PrdName = "FOREX.ins", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 1, ItemName = "GBPJPY.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPMXN.ins", PrdName = "FOREX.ins", Precision = 4, TickScale = 1, OverTick = 0.0001, Exchange = 1, ValueTick = 1, ItemName = "GBPMXN.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPNOK.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "GBPNOK.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPNZD.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "GBPNZD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPPLN.ins", PrdName = "FOREX.ins", Precision = 4, TickScale = 1, OverTick = 0.0001, Exchange = 1, ValueTick = 1, ItemName = "GBPPLN.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPSEK.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "GBPSEK.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPSGD.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "GBPSGD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPUSD.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "GBPUSD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "MXNJPY.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "MXNJPY.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "NOKJPY.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "NOKJPY.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "NZDCAD.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "NZDCAD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "NZDCHF.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "NZDCHF.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "NZDJPY.ins", PrdName = "FOREX.ins", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 1, ItemName = "NZDJPY.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "NZDUSD.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "NZDUSD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "SGDJPY.ins", PrdName = "FOREX.ins", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 1, ItemName = "SGDJPY.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDBRL.ins", PrdName = "FOREX.ins", Precision = 4, TickScale = 1, OverTick = 0.0001, Exchange = 1, ValueTick = 2, ItemName = "USDBRL.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDCAD.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "USDCAD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDCHF.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "USDCHF.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDCNH.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "USDCNH.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDCZK.ins", PrdName = "FOREX.ins", Precision = 4, TickScale = 1, OverTick = 0.0001, Exchange = 1, ValueTick = 1, ItemName = "USDCZK.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDHKD.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "USDHKD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDIDR.ins", PrdName = "FOREX.ins", Precision = 1, TickScale = 1, OverTick = 0.1, Exchange = 1, ValueTick = 1, ItemName = "USDIDR.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDILS.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "USDILS.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDINR.ins", PrdName = "FOREX.ins", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 1, ItemName = "USDINR.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDJPY.ins", PrdName = "FOREX.ins", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 1, ItemName = "USDJPY.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDKRW.ins", PrdName = "FOREX.ins", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 1, ItemName = "USDKRW.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDMXN.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "USDMXN.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDNOK.ins", PrdName = "FOREX.ins", Precision = 4, TickScale = 1, OverTick = 0.0001, Exchange = 1, ValueTick = 1, ItemName = "USDNOK.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDPLN.ins", PrdName = "FOREX.ins", Precision = 4, TickScale = 1, OverTick = 0.0001, Exchange = 1, ValueTick = 1, ItemName = "USDPLN.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDSEK.ins", PrdName = "FOREX.ins", Precision = 4, TickScale = 1, OverTick = 0.0001, Exchange = 1, ValueTick = 1, ItemName = "USDSEK.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDSGD.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "USDSGD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDTHB.ins", PrdName = "FOREX.ins", Precision = 4, TickScale = 1, OverTick = 0.0001, Exchange = 1, ValueTick = 1, ItemName = "USDTHB.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDZAR.ins", PrdName = "FOREX.ins", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 1, ItemName = "USDZAR.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "ZARJPY.ins", PrdName = "FOREX.ins", Precision = 4, TickScale = 1, OverTick = 0.0001, Exchange = 1, ValueTick = 1, ItemName = "ZARJPY.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            itemList.Add(newItem);

            //METALS.ins Count=7
            newItem = new ItemSymbolInfo() { Symbol = "XAGUSD.ins", PrdName = "METALS.ins", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 5, ItemName = "XAGUSD.ins", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "XAUAUD.ins", PrdName = "METALS.ins", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 1, ItemName = "XAUAUD.ins", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "XAUCHF.ins", PrdName = "METALS.ins", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 1, ItemName = "XAUCHF.ins", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "XAUEUR.ins", PrdName = "METALS.ins", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 1, ItemName = "XAUEUR.ins", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "XAUGBP.ins", PrdName = "METALS.ins", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 1, ItemName = "XAUGBP.ins", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "XAUUSD.ins", PrdName = "METALS.ins", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 1, ItemName = "XAUUSD.ins", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "XPTUSD.ins", PrdName = "METALS.ins", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 0.1, ItemName = "XPTUSD.ins", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            itemList.Add(newItem);

            //CRYPTO.ecn Count=1
            newItem = new ItemSymbolInfo() { Symbol = "BTCUSD.ecn", PrdName = "CRYPTO.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "BTCUSD.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            itemList.Add(newItem);

            //CRYPTO2.ecn Count=9
            newItem = new ItemSymbolInfo() { Symbol = "ADAUSD.ecn", PrdName = "CRYPTO2.ecn", Precision = 4, TickScale = 1, OverTick = 0.0001, Exchange = 1, ValueTick = 0.0001, ItemName = "ADAUSD.ecn", MinVolume = 0.1, MaxVolume = 200, VolumeStep = 0.1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "BCHUSD.ecn", PrdName = "CRYPTO2.ecn", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 0.001, ItemName = "BCHUSD.ecn", MinVolume = 0.1, MaxVolume = 200, VolumeStep = 0.1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "DOGUSD.ecn", PrdName = "CRYPTO2.ecn", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 0.001, ItemName = "DOGUSD.ecn", MinVolume = 0.1, MaxVolume = 200, VolumeStep = 0.1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "DOTUSD.ecn", PrdName = "CRYPTO2.ecn", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 0.001, ItemName = "DOTUSD.ecn", MinVolume = 0.1, MaxVolume = 200, VolumeStep = 0.1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "ETHUSD.ecn", PrdName = "CRYPTO2.ecn", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 0.001, ItemName = "ETHUSD.ecn", MinVolume = 0.1, MaxVolume = 200, VolumeStep = 0.1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "LNKUSD.ecn", PrdName = "CRYPTO2.ecn", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 0.001, ItemName = "LNKUSD.ecn", MinVolume = 0.1, MaxVolume = 200, VolumeStep = 0.1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "LTCUSD.ecn", PrdName = "CRYPTO2.ecn", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 0.001, ItemName = "LTCUSD.ecn", MinVolume = 0.1, MaxVolume = 200, VolumeStep = 0.1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "XLMUSD.ecn", PrdName = "CRYPTO2.ecn", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "XLMUSD.ecn", MinVolume = 0.1, MaxVolume = 200, VolumeStep = 0.1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "XRPUSD.ecn", PrdName = "CRYPTO2.ecn", Precision = 5, TickScale = 1, OverTick = 0.00001, Exchange = 1, ValueTick = 0.001, ItemName = "XRPUSD.ecn", MinVolume = 0.1, MaxVolume = 200, VolumeStep = 0.1 };
            itemList.Add(newItem);

            //CASH CFD.ecn Count=19
            newItem = new ItemSymbolInfo() { Symbol = "AUS200.ecn", PrdName = "CASH CFD.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "AUS200.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "CN50.ecn", PrdName = "CASH CFD.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "CN50.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EU50.ecn", PrdName = "CASH CFD.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "EU50.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "FRA40.ecn", PrdName = "CASH CFD.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "FRA40.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GER40.ecn", PrdName = "CASH CFD.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "GER40.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "HK50.ecn", PrdName = "CASH CFD.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "HK50.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "IT40.ecn", PrdName = "CASH CFD.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "IT40.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "JPN225.ecn", PrdName = "CASH CFD.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.005, ItemName = "JPN225.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "NETH25.ecn", PrdName = "CASH CFD.ecn", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "NETH25.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "SGFREE.ecn", PrdName = "CASH CFD.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "SGFREE.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "SPA35.ecn", PrdName = "CASH CFD.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "SPA35.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "SWI20.ecn", PrdName = "CASH CFD.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "SWI20.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "UK100.ecn", PrdName = "CASH CFD.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "UK100.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "UKOIL.ecn", PrdName = "CASH CFD.ecn", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "UKOIL.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "US2000.ecn", PrdName = "CASH CFD.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "US2000.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "US30.ecn", PrdName = "CASH CFD.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "US30.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "US500.ecn", PrdName = "CASH CFD.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "US500.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USOIL.ecn", PrdName = "CASH CFD.ecn", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "USOIL.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USTECH.ecn", PrdName = "CASH CFD.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "USTECH.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            itemList.Add(newItem);

            //OIL.ecn Count=3
            newItem = new ItemSymbolInfo() { Symbol = "BRENT.ecn", PrdName = "OIL.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 10, ItemName = "BRENT.ecn", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "NATGAS.ecn", PrdName = "OIL.ecn", Precision = 3, TickScale = 1, OverTick = 0.001, Exchange = 1, ValueTick = 1, ItemName = "NATGAS.ecn", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            itemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "WTI.ecn", PrdName = "OIL.ecn", Precision = 2, TickScale = 1, OverTick = 0.01, Exchange = 1, ValueTick = 10, ItemName = "WTI.ecn", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            itemList.Add(newItem);

            return itemList;
        }

        public void ConnectSocket()
        {
            if (_mtApiClient == null || _mtApiClient.ConnectionState != MtConnectionState.Connected)
                return;
            try
            {
                if (ItemSymbol.Length > 0)
                    _mtApiClient.SymbolSelect(ItemSymbol, true);
                WriteLog("[ConnectSocket] SymbolSelect " + ItemSymbol);
            }
            catch (Exception ex)
            {
                WriteLog("[ConnectSocket] " + ex.Message);
            }
        }

        public void DisconnectSocket()
        {
            try
            {
                if (_mtApiClient != null)
                {
                    if (ItemSymbol.Length > 0)
                    {
                        try { _mtApiClient.SymbolSelect(ItemSymbol, false); } catch { }
                        Thread.Sleep(300);
                    }
                    _mtApiClient.ConnectionStateChanged -= MtApiClient_ConnectionStateChanged;
                    _mtApiClient.QuoteUpdated -= MtApiClient_QuoteUpdated;
                    _mtApiClient.BeginDisconnect();
                    Thread.Sleep(200);
                    _mtApiClient = null;
                }
                _mtApiConnected = false;
            }
            catch (Exception ex)
            {
                WriteLog("[DisconnectSocket] " + ex.Message);
            }
        }

        public bool reqQuote(string newSymbol, string oldSymbol)
        {
            try
            {
                lock (_mtLock)
                {
                    if (_mtApiClient == null || _mtApiClient.ConnectionState != MtConnectionState.Connected) return false;
                    if (oldSymbol.Length > 0)
                    {
                        _mtApiClient.SymbolSelect(oldSymbol, false);
                        WriteLog(string.Format("[reqQuote] unsubscribe symbol={0}", oldSymbol));
                        Thread.Sleep(500);
                    }
                    _mtApiClient.SymbolSelect(newSymbol, true);
                    WriteLog(string.Format("[reqQuote] subscribe symbol={0}", newSymbol));
                }
                return true;
            }
            catch (Exception ex)
            {
                WriteLog("[reqQuote] " + ex.Message);
                return false;
            }
        }

    }
}
