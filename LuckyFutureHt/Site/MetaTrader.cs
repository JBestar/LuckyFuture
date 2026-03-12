// #define WRITE_LOG

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Json;
using System.Linq;
using System.Net.Http.Headers;
using System.ServiceModel.Configuration;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ChartCtrl;
using Goodbyte.TradingSystem.Domain.Entities;
using LuckyFuture.Models.ValueObjects;
using LuckyFuture.Properties;
using LuckyFutureLib.Include;
using SocketIOClient;

namespace LuckyFuture.Site
{

    class MetaTrader : FutureSite
    {
        public override SITETYPE Type { get; set; }
        private readonly HttpClientFx _httpClient = new HttpClientFx();
        private SocketIOClient.SocketIO _socketClient = null;

        public const string URL_MAIN = "https://mt-client-api-v1.new-york.agiliumtrade.ai";
        public const string URL_SOCK = "https://mt-client-api-v1.new-york-a.agiliumtrade.ai";
        public const string URL_PROV = "https://mt-provisioning-api-v1.agiliumtrade.agiliumtrade.ai";
        public const string URL_DATA = "https://mt-market-data-client-api-v1.new-york.agiliumtrade.ai";

        private string mt_application = "MetaApi";
        private string mt_userId = "";
        private string mt_host = "";
        private string mt_oldSymbol = "";

        private int m_tickCurrent = 0;
        private int m_tickAccount = 0;
        private bool m_bNeedAcc = false;

        public MetaTrader()
        {
            Type = SITETYPE.Prime;
            _httpClient.Reset();

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
            string url = String.Format("{0}/users/current/accounts?offset=0&limit=1000&state=DEPLOYED", URL_PROV);
            string token = UserPassword;
            if (!_httpClient.SendRequest(out string body, out HttpHeaders headers, HTTPREQUEST_TYPE.GET, url, token))
                return ERRORCODE.CANT_CONNECT;
            try
            {
                mt_userId = "";
                JsonDocument doc = JsonDocument.Parse(body);
                JsonElement rootElement = doc.RootElement;
                int cnt = rootElement.GetArrayLength();
                for (int i = 0; i < cnt; i++)
                {
                    string loginId = rootElement[i].GetProperty("login").GetString();
                    if (loginId == UserId)
                    {
                        mt_userId = rootElement[i].GetProperty("_id").GetString();
                        break;
                    }
                }


            }
            catch (Exception ex)
            {
                string msgg = ex.Message;
                return ERRORCODE.UNKNOWN_FAILED;
            }
            if (mt_userId.Length < 1)
                return ERRORCODE.LOGIN_NO_ID;

            url = String.Format("{0}/users/current/accounts/{1}/account-information", URL_MAIN, mt_userId);
            token = UserPassword;
            if (!_httpClient.SendRequest(out body, out headers, HTTPREQUEST_TYPE.GET, url, token))
                return ERRORCODE.CANT_CONNECT;

            double balance = 0, valuation = 0, profit = 0;
            try
            {
                JsonDocument doc = JsonDocument.Parse(body);
                UserAcc = doc.RootElement.GetProperty("server").GetString();
                balance = doc.RootElement.GetProperty("balance").GetDouble();
                profit = doc.RootElement.GetProperty("equity").GetDouble() - balance;
                valuation = 0;
                WriteLog(String.Format("Login Balance={0}, Valuation={1}, Profit={2}", balance, valuation, profit));
                profit = 0;
            }
            catch (Exception ex)
            {
                string msgg = ex.Message;
                return ERRORCODE.UNKNOWN_FAILED;
            }

            this.CurrentUserAccount = new UserAccountInfo
            {
                UserAccountId = UserAcc,
                UserAccountStr = UserAcc,
                Balance = (balance - valuation)
            };
            this.UserAccounts = new List<UserAccountInfo>();
            this.UserAccounts.Add(this.CurrentUserAccount);
            DayProfitLoss = new DayProfitLossInfo();
            OnLogin();

            DayProfitLoss.TotalProfit = (long)profit;
            this.ValuationList[0].TotalValuation = valuation;
            this.ValuationList[0].TotalProfit = DayProfitLoss.TotalProfit;
            this.ValuationList[0].CurrentProfit = DayProfitLoss.TotalProfit + (long)valuation;


            return ERRORCODE.SUCCESS;
        }


        protected override void OnLogin()
        {
            base.OnLogin();
            bQutoteCreated = false;
            loadItemlist();
            //reqItemlist();
            ConnectSocket();
            Thread.Sleep(4000);
        }

        protected override ERRORCODE LogOut()
        {
            return ERRORCODE.SUCCESS;
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


        public override bool Start()
        {
            if (String.IsNullOrEmpty(UserPassword))
                return false;
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
                OnFutureSiteLogEvent(string.Format("[종목] {0} => 틱단위:{1}, 주문수량단위:{2}", CurItemSymbol.Symbol, CurItemSymbol.OverTick, CurItemSymbol.VolumeStep)); //CurItemSymbol.MinVolume, CurItemSymbol.MaxVolume
            }
            Current = null;

            reqQuote(ItemSymbol, mt_oldSymbol);

            CurrentList.Clear();
            OrderList.Clear();
            RequestOrderList(false, false);
            RequestOrderList(false, true);
        }


        public override bool DoSellOrder(QuoteInfo quoteInfo, double nQuantity = 1, bool bMarketPrice = false)
        {

            if (this.CurrentUserAccount == null || string.IsNullOrEmpty(CurrentUserAccount.UserAccountId))
            {
                OnFutureSiteLogEvent("[주문] 계좌정보 오류!");
                return false;
            }

            if(CurItemSymbol.VolumeStep == 0)
            {
                OnFutureSiteLogEvent("[주문] 주문가능한 종목이 아닙니다!");
                return false;
            }

            nQuantity = nQuantity * CurItemSymbol.VolumeStep;
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

            if (quoteInfo != null)
            {
                temp = string.Format("{0:N6}", quoteInfo.Price);
                quoteInfo.Price = double.Parse(temp);

                double nOrderRange = 2000 * CurItemSymbol.OverTick;
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
                param_list.Add("actionType", "ORDER_TYPE_SELL");
                param_list.Add("symbol", ItemSymbol);
                param_list.Add("volume", nQuantity);
                param_list.Add("takeProfit", 0);
            }
            else
            {
                param_list.Add("actionType", "ORDER_TYPE_SELL_LIMIT");
                param_list.Add("symbol", ItemSymbol);
                param_list.Add("volume", nQuantity);
                param_list.Add("openPrice", quoteInfo.Price);
            }

            string jsonParam = JsonSerializer.Serialize(param_list);

            string url = String.Format("{0}/users/current/accounts/{1}/trade", URL_MAIN, mt_userId);
            string token = UserPassword;
            if (!_httpClient.SendRequest(out string body, out HttpHeaders headers, HTTPREQUEST_TYPE.JSON, url, token, jsonParam))
                return false;

            WriteLog(string.Format("[DoSellOrder] param={0} resp={1}", jsonParam, body));

            try
            {
                JsonDocument doc = JsonDocument.Parse(body);
                int code = doc.RootElement.GetProperty("numericCode").GetInt32();
                if(code != 0)
                {
                    string message = doc.RootElement.GetProperty("message").GetString();
                    OnFutureSiteLogEvent(string.Format("[주문] 실패({0})", message));
                }

            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

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
            nQuantity = nQuantity * CurItemSymbol.VolumeStep;
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

            if (quoteInfo != null)
            {
                temp = string.Format("{0:N6}", quoteInfo.Price);
                quoteInfo.Price = double.Parse(temp);

                double nOrderRange = 2000 * CurItemSymbol.OverTick;
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
            }
            else
            {
                param_list.Add("actionType", "ORDER_TYPE_BUY_LIMIT");
                param_list.Add("symbol", ItemSymbol);
                param_list.Add("volume", nQuantity);
                param_list.Add("openPrice", quoteInfo.Price);
            }

            string jsonParam = JsonSerializer.Serialize(param_list);

            string url = String.Format("{0}/users/current/accounts/{1}/trade", URL_MAIN, mt_userId);
            string token = UserPassword;
            if (!_httpClient.SendRequest(out string body, out HttpHeaders headers, HTTPREQUEST_TYPE.JSON, url, token, jsonParam))
                return false;

            WriteLog(string.Format("[DoSellOrder] param={0} resp={1}", jsonParam, body));
            try
            {
                JsonDocument doc = JsonDocument.Parse(body);
                int code = doc.RootElement.GetProperty("numericCode").GetInt32();
                if (code != 0)
                {
                    string message = doc.RootElement.GetProperty("message").GetString();
                    OnFutureSiteLogEvent(string.Format("[주문] 실패({0})", message));
                }
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

            return true;
        }


        public override bool CancelOrder(OrderInfo orderInfo)
        {
            if (this.CurrentUserAccount == null || string.IsNullOrEmpty(CurrentUserAccount.UserAccountId))
            {
                OnFutureSiteLogEvent("[주문취소] 계좌정보 오류!");
                return false;
            }

            var param_list = new Dictionary<string, string>
            {
                { "actionType", "ORDER_CANCEL"},
                { "orderId", orderInfo.OrderNo},
            };
            string jsonParam = JsonSerializer.Serialize(param_list);

            string url = String.Format("{0}/users/current/accounts/{1}/trade", URL_MAIN, mt_userId);
            string token = UserPassword;
            if (!_httpClient.SendRequest(out string body, out HttpHeaders headers, HTTPREQUEST_TYPE.JSON, url, token, jsonParam))
                return false;

            WriteLog(string.Format("[CancelOrder] {0}", body));

            return true;
        }

        public override bool LiquidateOrder(OrderInfo orderInfo)
        {
            if (this.CurrentUserAccount == null)
                return false;

            var param_list = new Dictionary<string, string>
            {
                { "actionType", "POSITION_CLOSE_ID"},
                { "positionId", orderInfo.OrderNo},
            };
            string jsonParam = JsonSerializer.Serialize(param_list);

            string url = String.Format("{0}/users/current/accounts/{1}/trade", URL_MAIN, mt_userId);
            string token = UserPassword;
            if (!_httpClient.SendRequest(out string body, out HttpHeaders headers, HTTPREQUEST_TYPE.JSON, url, token, jsonParam))
                return false;

            WriteLog(string.Format("[LiquidateOrder] {0}", body));

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

            Settings.Default.ItemOverTick = (float)CurItemSymbol.OverTick;

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

            string url = String.Format("{0}/users/current/accounts/{1}/historical-market-data/symbols/{2}/timeframes/{3}/candles?limit=300", URL_DATA, mt_userId, ItemSymbol, tmFrame);
            string token = UserPassword;
            if (!_httpClient.SendRequest(out string body, out HttpHeaders headers, HTTPREQUEST_TYPE.GET, url, token))
                return false;
            try
            {
                DItem newDItem = null;
                CItem newCItem = null;
                int nConc = 0;
                float fCurPrice = 0, fStartPrice = 0, fHighPrice = 0, fLowPrice = 0;
                string sDateTime = "";
                DateTime dtStart = DateTime.MinValue, dtEnd = DateTime.MinValue;

                JsonDocument doc = JsonDocument.Parse(body);
                JsonElement rootElement = doc.RootElement;
                int cnt = rootElement.GetArrayLength();
                int nTickCnt = 0;

                lock (CtrlProperty._DItemList) lock (CtrlProperty._CItemList)
                    {
                        CtrlProperty._DItemList.Clear();
                        CtrlProperty._CItemList.Clear();
                        for (int i = 0; i < cnt; i++)
                        {
                            fCurPrice = (float)rootElement[i].GetProperty("close").GetDouble();
                            nConc = rootElement[i].GetProperty("tickVolume").GetInt32();
                            sDateTime = rootElement[i].GetProperty("time").GetString();
                            fStartPrice = (float)rootElement[i].GetProperty("open").GetDouble();
                            fHighPrice = (float)rootElement[i].GetProperty("high").GetDouble();
                            fLowPrice = (float)rootElement[i].GetProperty("low").GetDouble();
                            if (sDateTime.Length > 14)
                            {
                                dtStart = DateTime.ParseExact(sDateTime, "yyyy-MM-ddTHH:mm:ss.fffZ", System.Globalization.CultureInfo.InvariantCulture);
                                dtEnd = CtrlProperty.GetEndTime(CtrlProperty._DTimeType, CtrlProperty._DTimeUnitAmt, dtStart, true);
                            }

                            newDItem = new DItem(fStartPrice * CtrlProperty._nValueRate, fCurPrice * CtrlProperty._nValueRate, fLowPrice * CtrlProperty._nValueRate, fHighPrice * CtrlProperty._nValueRate,
                            CtrlProperty.GetTimeStamp(dtStart), CtrlProperty.GetTimeStamp(dtEnd), CtrlProperty._DItemList.Count(), nTickCnt, nConc);
                            CtrlProperty._DItemList.Add(newDItem);

                            newCItem = new CItem(CtrlProperty.GetTimeStamp(dtStart), CtrlProperty.GetTimeStamp(dtEnd), nTickCnt, nConc);
                            CtrlProperty._CItemList.Add(newCItem);
                        }
                    }


            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
            }

            return true;
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

            string url = String.Format("{0}/users/current/accounts/{1}/historical-market-data/symbols/{2}/timeframes/{3}/candles?limit=300", URL_DATA, mt_userId, ItemSymbol, tmFrame);
            string token = UserPassword;
            if (!_httpClient.SendRequest(out string body, out HttpHeaders headers, HTTPREQUEST_TYPE.GET, url, token))
                return false;
            try
            {
                CtrlProperty.SetValueRate(Common.GetPrecisionRate(ItemSymbol, CurItemSymbol.Precision), Common.GetValueFormat(ItemPrecision + 1), (float)CurItemSymbol.OverTick);

                RItem itemNew = null;
                int nConc = 0;
                float fCurPrice = 0, fStartPrice = 0, fHighPrice = 0, fLowPrice = 0;
                string sDateTime = "";
                DateTime dtStart = DateTime.MinValue, dtEnd = DateTime.MinValue;

                JsonDocument doc = JsonDocument.Parse(body);
                JsonElement rootElement = doc.RootElement;
                int cnt = rootElement.GetArrayLength();
                int nTickCnt = 0;

                lock (CtrlProperty._RItemList)
                {
                    CtrlProperty._RItemList.Clear();

                    for (int i = 0; i < cnt; i++)
                    {
                        fCurPrice = (float)rootElement[i].GetProperty("close").GetDouble();
                        nConc = rootElement[i].GetProperty("tickVolume").GetInt32();
                        sDateTime = rootElement[i].GetProperty("time").GetString();
                        fStartPrice = (float)rootElement[i].GetProperty("open").GetDouble();
                        fHighPrice = (float)rootElement[i].GetProperty("high").GetDouble();
                        fLowPrice = (float)rootElement[i].GetProperty("low").GetDouble();
                        if (sDateTime.Length > 14)
                        {
                            dtStart = DateTime.ParseExact(sDateTime, "yyyy-MM-ddTHH:mm:ss.fffZ", System.Globalization.CultureInfo.InvariantCulture);
                            dtEnd = CtrlProperty.GetEndTime(CtrlProperty._RTimeType, CtrlProperty._RTimeUnitAmt, dtStart, true);
                        }

                        itemNew = new RItem(fStartPrice * CtrlProperty._nValueRate, fCurPrice * CtrlProperty._nValueRate, fLowPrice * CtrlProperty._nValueRate, fHighPrice * CtrlProperty._nValueRate,
                        CtrlProperty.GetTimeStamp(dtStart), CtrlProperty.GetTimeStamp(dtEnd), CtrlProperty._RItemList.Count(), nTickCnt, nConc);
                        CtrlProperty._RItemList.Add(itemNew);
                    }
                }
                OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.REDRAW);
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
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

            string strAccount = this.CurrentUserAccount.UserAccountId;
            string strAccPwd = UserPassword;

            return CONSTATE.SUCCESSS;
        }

        private void RequestOrderList(bool bAccount, bool bRequidate)
        {
            if (Settings.Default.SignalSiteOn)
                return;

            Thread.Sleep(200);
            if (bRequidate)
            {
                RequestRequidateOrder();
            }
            else
            {
                RequestOutstandOrder();
            }

            if (bAccount)
            {
                m_bNeedAcc = bAccount;
            }
        }

        private CONSTATE RequestRequidateOrder()
        {

            if (this.CurrentUserAccount == null)
                return CONSTATE.NO_LOGIN;

            OrderList.RemoveAll(o => o.OrderType == "체결");

            string token = UserPassword;
            string url = String.Format("{0}/users/current/accounts/{1}/positions?refreshTerminalState=true", URL_MAIN, mt_userId);
            if (!_httpClient.SendRequest(out string body, out HttpHeaders headers, HTTPREQUEST_TYPE.GET, url, token))
                return CONSTATE.ERR_REGAPI;

            try
            {
                JsonDocument doc = JsonDocument.Parse(body);
                JsonElement rootElement = doc.RootElement;
                int cnt = rootElement.GetArrayLength();
                for (int i = 0; i < cnt; i++)
                {
                    string orderNo = rootElement[i].GetProperty("id").GetString();
                    string type = rootElement[i].GetProperty("type").GetString();
                    string symbol = rootElement[i].GetProperty("symbol").GetString();
                    double volume = rootElement[i].GetProperty("volume").GetDouble();
                    double openPrice = rootElement[i].GetProperty("openPrice").GetDouble();
                    double currentPrice = rootElement[i].GetProperty("currentPrice").GetDouble();
                    double profit = rootElement[i].GetProperty("profit").GetDouble();
                    string orderTime = rootElement[i].GetProperty("time").GetString();

                    if (symbol != ItemSymbol)
                        continue;
                    // if (OrderList.FirstOrDefault(o => o.OrderNo == orderNo) != null)
                    //    continue;

                    OrderInfo orderInfo = new OrderInfo
                    {
                        OrderType = "체결",
                        Symbol = symbol,
                        Qty = string.Format("{0}[{1}]", type == "POSITION_TYPE_SELL" ? "매도" : "매수", volume),
                        AveragePrice = openPrice.ToString(),
                        MaxAveragePrice = Math.Round(openPrice, 6),
                        CurrentPrice = currentPrice.ToString(),
                        StartCciPrice = -10000,
                        Valuation = (int)profit,
                        Action = "청산",
                        TradeType = type == "POSITION_TYPE_SELL" ? TRADETYPE.SELL : TRADETYPE.BUY,
                        OrderQty = volume,
                        OrderNo = orderNo,
                        OrderDate = orderTime,
                    };
                    this.OrderList.Add(orderInfo);
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

            OrderList.RemoveAll(o => o.OrderType == "미체결");

            string token = UserPassword;
            string url = String.Format("{0}/users/current/accounts/{1}/orders?refreshTerminalState=true", URL_MAIN, mt_userId);
            if (!_httpClient.SendRequest(out string body, out HttpHeaders headers, HTTPREQUEST_TYPE.GET, url, token))
                return CONSTATE.ERR_REGAPI;

            try
            {
                JsonDocument doc = JsonDocument.Parse(body);
                JsonElement rootElement = doc.RootElement;
                int cnt = rootElement.GetArrayLength();
                for (int i = 0; i < cnt; i++)
                {
                    string orderNo = rootElement[i].GetProperty("id").GetString();
                    string type = rootElement[i].GetProperty("type").GetString();
                    string state = rootElement[i].GetProperty("state").GetString();
                    string symbol = rootElement[i].GetProperty("symbol").GetString();
                    double volume = rootElement[i].GetProperty("volume").GetDouble();
                    double openPrice = rootElement[i].GetProperty("openPrice").GetDouble();
                    double currentPrice = rootElement[i].GetProperty("currentPrice").GetDouble();
                    string orderTime = rootElement[i].GetProperty("time").GetString();

                    if (symbol != ItemSymbol)
                        continue;
                    // if (OrderList.FirstOrDefault(o => o.OrderNo == orderNo) != null)
                    //    continue;

                    OrderInfo orderInfo = new OrderInfo
                    {
                        OrderType = "미체결",
                        Symbol = symbol,
                        Qty = string.Format("{0}[{1}]", type == "POSITION_TYPE_SELL_LIMIT" ? "매도" : "매수", volume),
                        AveragePrice = openPrice.ToString(),
                        MaxAveragePrice = Math.Round(openPrice, 6),
                        CurrentPrice = currentPrice.ToString(),
                        Valuation = 0L,
                        Action = "취소",
                        TradeType = type == "POSITION_TYPE_SELL" ? TRADETYPE.SELL : TRADETYPE.BUY,
                        OrderQty = volume,
                        OrderNo = orderNo,
                        OrderTime = Environment.TickCount,
                        OrderDate = orderTime,
                    };
                    this.OrderList.Add(orderInfo);
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

                    OnFutureSiteLogEvent(string.Format("[주문접수] {0} 주문가:{1}", type == "ORDER_TYPE_SELL_LIMIT" ? "매도":"매수", openPrice));

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

            OrderInfo orderInfo = OrderList.FirstOrDefault(o => o.OrderType == "미체결" && o.OrderNo == orderNo);

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

                            string logMsg = string.Format("청산가:{0}", price);
                            logMsg += "(" + (profit > 0 ? "수익:" : "손실:") + string.Format("{0})", profit);

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
                WriteLog(string.Format("[onAccountInformation] balance={0}", balance));
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

                                    if (double.Parse(orderInfo.CurrentPrice) != current.CurrentPrice)
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
                                }
                                ValuationList[0].Valuation = lValSum;
                                ValuationList[0].TotalValuation = lValSum;
                                if (dAveragePriceSum > 0)
                                    ValuationList[0].AverageUnitPrice = dAveragePriceSum / nOrderCnt;
                                ValuationList[0].TotalProfit = DayProfitLoss.TotalProfit;
                                ValuationList[0].CurrentProfit = DayProfitLoss.TotalProfit + lValSum;

                            }
                            else if (nOrderCnt <= 0)
                            {
                                ValuationList[0].Valuation = 0;
                                ValuationList[0].TotalValuation = 0;
                                ValuationList[0].AverageUnitPrice = 0;
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

            newPrd = new PrdInfo();
            newPrd.Code = "Prime";
            newPrd.Name = "Prime";
            newPrd.ItemList = new List<ItemSymbolInfo>();

            string url = String.Format("{0}/users/current/accounts/{1}/symbols", URL_MAIN, mt_userId);
            string token = UserPassword;
            if (!_httpClient.SendRequest(out string body, out HttpHeaders headers, HTTPREQUEST_TYPE.GET, url, token))
                return;

            List<string> symbols = new List<string>();
            try
            {
                JsonDocument doc = JsonDocument.Parse(body);
                JsonElement rootElement = doc.RootElement;
                int cnt = rootElement.GetArrayLength();
                for (int i = 0; i < cnt; i++)
                {
                    symbols.Add(rootElement[i].GetString());
                }
            }
            catch (Exception ex)
            {
                string msgg = ex.Message;
                return;
            }


            foreach (string symbol in symbols)
            {

                url = String.Format("{0}/users/current/accounts/{1}/symbols/{2}/specification", URL_MAIN, mt_userId, symbol);
                token = UserPassword;
                if (!_httpClient.SendRequest(out body, out headers, HTTPREQUEST_TYPE.GET, url, token))
                    continue;

                try
                {
                    JsonDocument doc = JsonDocument.Parse(body);
                    JsonElement rootElement = doc.RootElement;

                    newItem = new ItemSymbolInfo();
                    newItem.Symbol = rootElement.GetProperty("symbol").GetString();
                    newItem.Precision = rootElement.GetProperty("digits").GetInt32();
                    newItem.OverTick = rootElement.GetProperty("tickSize").GetDouble();
                    newItem.ValueTick = rootElement.GetProperty("pipSize").GetDouble();
                    newItem.Exchange = 1;
                    int contractSize = rootElement.GetProperty("contractSize").GetInt32();
                    int initialMargin = rootElement.GetProperty("initialMargin").GetInt32();
                    string priceCalculationMode = rootElement.GetProperty("priceCalculationMode").GetString();
                    string baseCurrency = rootElement.GetProperty("baseCurrency").GetString();
                    string swapMode = rootElement.GetProperty("swapMode").GetString();
                    string description = rootElement.GetProperty("description").GetString();
                    double point = rootElement.GetProperty("point").GetDouble();
                    string executionMode = rootElement.GetProperty("executionMode").GetString();
                    double maxVolume = rootElement.GetProperty("maxVolume").GetDouble();
                    double minVolume = rootElement.GetProperty("minVolume").GetDouble();
                    double volumeStep = rootElement.GetProperty("volumeStep").GetDouble();
                    string tradeMode = rootElement.GetProperty("tradeMode").GetString();
                    string path = rootElement.GetProperty("path").GetString();

                    WriteLog(String.Format("Symbol={0}, Precision={1}, OverTick={2}, ValueTick={3}, contractSize={4}, initialMargin={5}, priceCalculationMode={6}, baseCurrency={7}, swapMode={8}, description={9}, point={10}, executionMode={11}, volumes={12},{13},{14}, tradeMode={15}, path={16}",
                        newItem.Symbol, newItem.Precision, newItem.OverTick, newItem.ValueTick,
                        contractSize, initialMargin, priceCalculationMode, baseCurrency, swapMode, description, point, executionMode,
                        maxVolume, minVolume, volumeStep, tradeMode, path));
                }
                catch (Exception ex)
                {
                    string msgg = ex.Message;
                    continue;
                }

                newItem.ItemName = newItem.Symbol;

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
            PrdList.Add(newPrd);

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
        private void loadItemlist()
        {
            if (PrdList.Count > 0) return;
            PrdList.Clear();

            PrdInfo newPrd;
            ItemSymbolInfo newItem;

            newPrd = new PrdInfo();
            newPrd.Code = "FUTURE CFDs.ecn";
            newPrd.Name = "FUTURE CFDs.ecn";
            newPrd.ItemList = new List<ItemSymbolInfo>();
            newItem = new ItemSymbolInfo() { Symbol = "CAC40.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.1, ItemName = "CAC40.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "CHINA50.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "CHINA50.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "COCOA.ecn", Precision = 0, OverTick = 1, Exchange = 1, ValueTick = 2, ItemName = "COCOA.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "COFFEE.ecn", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "COFFEE.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "COPPER.ecn", Precision = 4, OverTick = 0.0001, Exchange = 1, ValueTick = 0.01, ItemName = "COPPER.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "DAX40.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.25, ItemName = "DAX40.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "DJ30.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.05, ItemName = "DJ30.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EUSTX50.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.1, ItemName = "EUSTX50.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "FT100.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.1, ItemName = "FT100.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "HSI.ecn", Precision = 0, OverTick = 1, Exchange = 1, ValueTick = 6.5, ItemName = "HSI.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "NAS100.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.2, ItemName = "NAS100.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            CurPrd = newPrd;
            ItemSymbol = newItem.Symbol;
            CurItemSymbol = newItem;
            ItemList = newPrd.ItemList;
            newItem = new ItemSymbolInfo() { Symbol = "NK225.ecn", Precision = 0, OverTick = 1, Exchange = 1, ValueTick = 3.3, ItemName = "NK225.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "S&P.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.5, ItemName = "S&P.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "SOYBEAN.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "SOYBEAN.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "SPI200.ecn", Precision = 1, OverTick = 0.1, Exchange = 1, ValueTick = 2.5, ItemName = "SPI200.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "VIX.ecn", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 1, ItemName = "VIX.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            PrdList.Add(newPrd);


            newPrd = new PrdInfo();
            newPrd.Code = "FX MAJOR STD";
            newPrd.Name = "FX MAJOR STD";
            newPrd.ItemList = new List<ItemSymbolInfo>();
            newItem = new ItemSymbolInfo() { Symbol = "AUDUSD", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "AUDUSD", };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURUSD", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "EURUSD", };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPUSD", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "GBPUSD", };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "NZDUSD", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "NZDUSD", };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDCAD", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "USDCAD", };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDCHF", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "USDCHF", };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDCNH", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.01, ItemName = "USDCNH", };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDHKD", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "USDHKD", };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDJPY", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "USDJPY", };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDSGD", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "USDSGD", };
            newPrd.ItemList.Add(newItem);
            PrdList.Add(newPrd);

            newPrd = new PrdInfo();
            newPrd.Code = "FOREX.ins";
            newPrd.Name = "FOREX.ins";
            newPrd.ItemList = new List<ItemSymbolInfo>();
            newItem = new ItemSymbolInfo() { Symbol = "AUDCAD.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "AUDCAD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "AUDCHF.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "AUDCHF.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "AUDJPY.ins", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "AUDJPY.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "AUDNZD.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "AUDNZD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "AUDSGD.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "AUDSGD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "AUDUSD.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "AUDUSD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "CADCHF.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "CADCHF.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "CADJPY.ins", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "CADJPY.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "CADSGD.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.00, ItemName = "CADSGD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "CHFJPY.ins", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "CHFJPY.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "CHFSEK.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "CHFSEK.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "CHFSGD.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "CHFSGD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURAUD.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "EURAUD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURCAD.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "EURCAD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURCHF.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "EURCHF.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURCZK.ins", Precision = 4, OverTick = 0.0001, Exchange = 1, ValueTick = 0.0001, ItemName = "EURCZK.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURGBP.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "EURGBP.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURJPY.ins", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "EURJPY.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURNOK.ins", Precision = 4, OverTick = 0.0001, Exchange = 1, ValueTick = 0.0001, ItemName = "EURNOK.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURNZD.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "EURNZD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURPLN.ins", Precision = 4, OverTick = 0.0001, Exchange = 1, ValueTick = 0.0001, ItemName = "EURPLN.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURSEK.ins", Precision = 4, OverTick = 0.0001, Exchange = 1, ValueTick = 0.0001, ItemName = "EURSEK.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURSGD.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "EURSGD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EURUSD.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "EURUSD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPAUD.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "GBPAUD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPCAD.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "GBPCAD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPCHF.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "GBPCHF.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPJPY.ins", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "GBPJPY.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPMXN.ins", Precision = 4, OverTick = 0.0001, Exchange = 1, ValueTick = 0.0001, ItemName = "GBPMXN.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPNOK.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "GBPNOK.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPNZD.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "GBPNZD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPPLN.ins", Precision = 4, OverTick = 0.0001, Exchange = 1, ValueTick = 0.0001, ItemName = "GBPPLN.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPSEK.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "GBPSEK.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPSGD.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "GBPSGD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GBPUSD.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "GBPUSD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "MXNJPY.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.01, ItemName = "MXNJPY.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "NOKJPY.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.01, ItemName = "NOKJPY.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "NZDCAD.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "NZDCAD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "NZDCHF.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "NZDCHF.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "NZDJPY.ins", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "NZDJPY.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "NZDUSD.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "NZDUSD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "SGDJPY.ins", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "SGDJPY.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDBRL.ins", Precision = 4, OverTick = 0.0001, Exchange = 1, ValueTick = 0.0001, ItemName = "USDBRL.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDCAD.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "USDCAD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDCHF.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "USDCHF.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDCNH.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.01, ItemName = "USDCNH.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDCZK.ins", Precision = 4, OverTick = 0.0001, Exchange = 1, ValueTick = 0.0001, ItemName = "USDCZK.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDHKD.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "USDHKD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDIDR.ins", Precision = 1, OverTick = 0.1, Exchange = 1, ValueTick = 0.0001, ItemName = "USDIDR.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDILS.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "USDILS.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDINR.ins", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 0.0001, ItemName = "USDINR.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDJPY.ins", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "USDJPY.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDKRW.ins", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.0001, ItemName = "USDKRW.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDMXN.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "USDMXN.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDNOK.ins", Precision = 4, OverTick = 0.0001, Exchange = 1, ValueTick = 0.0001, ItemName = "USDNOK.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDPLN.ins", Precision = 4, OverTick = 0.0001, Exchange = 1, ValueTick = 0.0001, ItemName = "USDPLN.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDSEK.ins", Precision = 4, OverTick = 0.0001, Exchange = 1, ValueTick = 0.0001, ItemName = "USDSEK.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDSGD.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "USDSGD.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDTHB.ins", Precision = 4, OverTick = 0.0001, Exchange = 1, ValueTick = 0.0001, ItemName = "USDTHB.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USDZAR.ins", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.0001, ItemName = "USDZAR.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "ZARJPY.ins", Precision = 4, OverTick = 0.0001, Exchange = 1, ValueTick = 0.01, ItemName = "ZARJPY.ins", MinVolume = 0.01, MaxVolume = 50, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            PrdList.Add(newPrd);


            newPrd = new PrdInfo();
            newPrd.Code = "METALS.ins";
            newPrd.Name = "METALS.ins";
            newPrd.ItemList = new List<ItemSymbolInfo>();
            newItem = new ItemSymbolInfo() { Symbol = "XAGUSD.ins", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "XAGUSD.ins", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "XAUAUD.ins", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "XAUAUD.ins", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "XAUCHF.ins", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "XAUCHF.ins", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "XAUEUR.ins", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "XAUEUR.ins", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "XAUGBP.ins", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "XAUGBP.ins", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "XAUUSD.ins", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "XAUUSD.ins", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "XPTUSD.ins", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "XPTUSD.ins", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            PrdList.Add(newPrd);


            newPrd = new PrdInfo();
            newPrd.Code = "CRYPTO.ecn";
            newPrd.Name = "CRYPTO.ecn";
            newPrd.ItemList = new List<ItemSymbolInfo>();
            newItem = new ItemSymbolInfo() { Symbol = "BTCUSD.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "BTCUSD.ecn", MinVolume = 0.01, MaxVolume = 20, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            PrdList.Add(newPrd);


            newPrd = new PrdInfo();
            newPrd.Code = "CRYPTO2.ecn";
            newPrd.Name = "CRYPTO2.ecn";
            newPrd.ItemList = new List<ItemSymbolInfo>();
            newItem = new ItemSymbolInfo() { Symbol = "ADAUSD.ecn", Precision = 4, OverTick = 0.0001, Exchange = 1, ValueTick = 0.0001, ItemName = "ADAUSD.ecn", MinVolume = 0.1, MaxVolume = 200, VolumeStep = 0.1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "BCHUSD.ecn", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "BCHUSD.ecn", MinVolume = 0.1, MaxVolume = 200, VolumeStep = 0.1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "DOGUSD.ecn", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.01, ItemName = "DOGUSD.ecn", MinVolume = 0.1, MaxVolume = 200, VolumeStep = 0.1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "DOTUSD.ecn", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "DOTUSD.ecn", MinVolume = 0.1, MaxVolume = 200, VolumeStep = 0.1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "ETHUSD.ecn", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "ETHUSD.ecn", MinVolume = 0.1, MaxVolume = 200, VolumeStep = 0.1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "LNKUSD.ecn", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "LNKUSD.ecn", MinVolume = 0.1, MaxVolume = 200, VolumeStep = 0.1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "LTCUSD.ecn", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "LTCUSD.ecn", MinVolume = 0.1, MaxVolume = 200, VolumeStep = 0.1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "XLMUSD.ecn", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.01, ItemName = "XLMUSD.ecn", MinVolume = 0.1, MaxVolume = 200, VolumeStep = 0.1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "XRPUSD.ecn", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.01, ItemName = "XRPUSD.ecn", MinVolume = 0.1, MaxVolume = 200, VolumeStep = 0.1 };
            newPrd.ItemList.Add(newItem);
            PrdList.Add(newPrd);


            newPrd = new PrdInfo();
            newPrd.Code = "CASH CFD.ecn";
            newPrd.Name = "CASH CFD.ecn";
            newPrd.ItemList = new List<ItemSymbolInfo>();
            newItem = new ItemSymbolInfo() { Symbol = "AUS200.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "AUS200.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "CN50.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "CN50.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "EU50.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "EU50.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "FRA40.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "FRA40.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "GER40.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "GER40.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "HK50.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "HK50.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "IT40.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "IT40.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "JPN225.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 1, ItemName = "JPN225.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "NETH25.ecn", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "NETH25.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "SGFREE.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "SGFREE.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "SPA35.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "SPA35.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "SWI20.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "SWI20.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "UK100.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "UK100.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "UKOIL.ecn", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "UKOIL.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "US2000.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "US2000.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "US30.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "US30.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "US500.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "US500.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USOIL.ecn", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "USOIL.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "USTECH.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "USTECH.ecn", MinVolume = 1, MaxVolume = 1000, VolumeStep = 1 };
            newPrd.ItemList.Add(newItem);
            PrdList.Add(newPrd);


            newPrd = new PrdInfo();
            newPrd.Code = "OIL.ecn";
            newPrd.Name = "OIL.ecn";
            newPrd.ItemList = new List<ItemSymbolInfo>();
            newItem = new ItemSymbolInfo() { Symbol = "BRENT.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 10, ItemName = "BRENT.ecn", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "NATGAS.ecn", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "NATGAS.ecn", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            newItem = new ItemSymbolInfo() { Symbol = "WTI.ecn", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 10, ItemName = "WTI.ecn", MinVolume = 0.01, MaxVolume = 10, VolumeStep = 0.01 };
            newPrd.ItemList.Add(newItem);
            PrdList.Add(newPrd);



            // newPrd = new PrdInfo();
            // newPrd.Code = "";
            // newPrd.Name = "";
            // newPrd.ItemList = new List<ItemSymbolInfo>();
            // newItem = new ItemSymbolInfo() { Symbol = "UK100", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "UK100", };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "FT100.fs", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.1, ItemName = "FT100.fs" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "AstonMarti+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "AstonMarti+" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "Aviva+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "Aviva+" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "Barclays+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "Barclays+" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "Boohoo+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "Boohoo+" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "BT+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "BT+" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "BP+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "BP+" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "EasyJet+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "EasyJet+" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "Flutter+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "Flutter+" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "Fresnillo+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "Fresnillo+" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "GBXUSD", Precision = 5, OverTick = 0.00001, Exchange = 1, ValueTick = 0.01, ItemName = "GBXUSD", };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "Glencore+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "Glencore+" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "GSK+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "GSK+" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "HSBC_UK+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "HSBC_UK+" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "Petrofac+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "Petrofac+" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "S4Capital+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "S4Capital+" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "JDSports+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "JDSports+" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "Tesco+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "Tesco+" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "COCOA.fs", Precision = 0, OverTick = 1, Exchange = 1, ValueTick = 10, ItemName = "COCOA.fs" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "IAG+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "IAG+" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "ReckittBen+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "ReckittBen+" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "HutGroup+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "HutGroup+" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "Vodafone+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "Vodafone+" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "RioTinto+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "RioTinto+" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "Lloyds+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "Lloyds+" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "Ocado+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "Ocado+" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "COFFEE.fs", Precision = 3, OverTick = 0.001, Exchange = 1, ValueTick = 0.01, ItemName = "COFFEE.fs" };
            // newPrd.ItemList.Add(newItem);
            // newItem = new ItemSymbolInfo() { Symbol = "RollsRoyce+", Precision = 2, OverTick = 0.01, Exchange = 1, ValueTick = 0.01, ItemName = "RollsRoyce+" };
            // newPrd.ItemList.Add(newItem);
            // PrdList.Add(newPrd);

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

        public void ConnectSocket()
        {
            if (_socketClient != null)
            {
                DisconnectSocket();
            }
            mt_host = "";
            var rand = new Random();
            string clientId = string.Format("{0:N10}", rand.NextDouble());
            string token = UserPassword;

            SocketIOOptions options = new SocketIOOptions
            {
                Path = "/ws",
                ExtraHeaders = new Dictionary<string, string>
                {
                    { "Client-Id", clientId},
                },
                // ConnectionTimeout = new TimeSpan(5000),
                EIO = SocketIO.Core.EngineIO.V3,
            };
            options.Query = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("auth-token", token),
                new KeyValuePair<string, string>("clientId", clientId),
                new KeyValuePair<string, string>("protocol", "3")
            };

            _socketClient = new SocketIOClient.SocketIO(URL_SOCK, options);

            _socketClient.On("response", response =>
            {
                socket_onResponse(response.GetValue<string>());
            });

            _socketClient.On("synchronization", response =>
            {
                socket_onSyncronization(response.GetValue<string>());
            });

            _socketClient.OnConnected += socket_onConnected;
            _socketClient.OnError += socket_onError;
            _socketClient.OnDisconnected += socket_onDisconnected;

            // Connect to the server (make this asynchronous)
            _socketClient.ConnectAsync();

        }

        public void DisconnectSocket()
        {
            if (_socketClient != null)
            {
                if (_socketClient.Connected)
                {
                    if (ItemSymbol.Length > 0)
                    {
                        // var request = new Dictionary<string, object>
                        // {
                        //     { "type", "unsubscribeFromMarketData"},
                        //     { "symbol", ItemSymbol},
                        //     { "instanceIndex", 0},
                        //     { "accountId", mt_userId},
                        //     { "application", m_application},
                        //     { "requestId", GetRandomId()},
                        //     { "timestamps",  new Dictionary<string, string> {{ "clientProcessingStarted", GetIsoFormat(DateTime.Now.ToUniversalTime()) }}}
                        // };
                        // 
                        // _socketClient.EmitAsync("request", request);
                        // WriteLog(string.Format("[Send] {0}", JsonSerializer.Serialize(request)));
                        string token = UserPassword;
                        string url = String.Format("{0}/users/current/accounts/{1}/symbols/{2}/unsubscribe", URL_MAIN, mt_userId, ItemSymbol);
                        if (!_httpClient.SendRequest(out string body, out HttpHeaders headers, HTTPREQUEST_TYPE.POST, url, token))
                        {
                        }

                        Thread.Sleep(500);
                    }

                    _socketClient.Dispose();
                }

                _socketClient.OnConnected -= socket_onConnected;
                _socketClient.OnDisconnected -= socket_onDisconnected;
                _socketClient.OnError -= socket_onError;

                _socketClient = null;
            }
        }

        public string GetRandomId()
        {
            int length = 32;
            string allowedChars = "abcdefghijklmnopqrstuvwxyz";

            Random random = new Random();
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(allowedChars.Length);
                result.Append(allowedChars[index]);
            }

            return result.ToString();
        }
        public string GetIsoFormat(DateTime dt)
        {
            return dt.ToString("yyyy-MM-ddTHH:mm:ss.fff") + "Z";
        }
        private void socket_onConnected(object sender, EventArgs e)
        {
            var request = new Dictionary<string, object>
                {
                    { "type", "subscribe"},
                    { "instanceIndex", 0},
                    { "sessionId", GetRandomId()},
                    { "accountId", mt_userId},
                    { "application", mt_application},
                    { "requestId", GetRandomId()},
                    { "timestamps",  new Dictionary<string, string> {{ "clientProcessingStarted", GetIsoFormat(DateTime.Now.ToUniversalTime()) }} }
                };

            _socketClient.EmitAsync("request", request);
        }

        private void socket_onError(object sender, string e)
        {
            WriteLog("[socket_onError] " + e);
        }

        private void socket_onDisconnected(object sender, string e)
        {
            WriteLog("[socket_onDisconnected] " + e);
        }

        private void socket_onResponse(string msg)
        {
            WriteLog(string.Format("[RESP] {0}", msg));
            try
            {
                if (mt_host.Length == 0 && msg.Contains("\"type\":\"response\""))
                {
                    JsonValue json = JsonValue.Parse(msg);
                    if (json.ContainsKey("host"))
                    {
                        mt_host = json["host"];
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(string.Format("[RESP] Error = {0}", ex.Message));
            }
        }

        private void socket_onSyncronization(string msg)
        {
            try
            {
                if (msg.Contains("\"type\":\"prices\""))
                {
                    // WriteLog(string.Format("[SYNC] {0}", msg));

                    if (msg.Contains("\"prices\":"))
                    {
                        JsonValue json = JsonValue.Parse(msg);
                        if (json.ContainsKey("prices") && json["prices"].JsonType == JsonType.Array)
                        {
                            JsonValue jsonPrice = json["prices"][0];
                            onReceiveCurrent(jsonPrice);
                        }
                    }

                }
                else if (msg.Contains("\"type\":\"update\""))
                {
                    WriteLog(string.Format("[SYNC] {0}", msg));
                    JsonValue json = JsonValue.Parse(msg);
                    if (json.ContainsKey("updatedPositions") && json["updatedPositions"].JsonType == JsonType.Array)
                    {
                        int cnt = json["updatedPositions"].Count;
                        for (int i = 0; i < cnt; i++)
                        {
                            JsonValue jsonPos = json["updatedPositions"][i];
                            onUpdatedPosition(jsonPos);
                        }

                    }
                    else if (json.ContainsKey("removedPositionIds") && json.ContainsKey("historyOrders") && json["removedPositionIds"].JsonType == JsonType.Array)
                    {
                        int cnt = json["removedPositionIds"].Count;
                        for (int i = 0; i < cnt; i++)
                        {
                            string id = json["removedPositionIds"][i];
                            onRemovedPosition(id, json["deals"]);
                        }
                    }
                    else if (json.ContainsKey("updatedOrders"))
                    {
                        onUpdatedOrders(json["updatedOrders"]);
                    }
                    else if (json.ContainsKey("completedOrderIds") && json.ContainsKey("historyOrders") && json["completedOrderIds"].JsonType == JsonType.Array)
                    {
                        int cnt = json["completedOrderIds"].Count;
                        for (int i = 0; i < cnt; i++)
                        {
                            string id = json["completedOrderIds"][i];
                            onCompletedOrder(id, json["historyOrders"]);
                        }
                    }

                    if (json.ContainsKey("accountInformation"))
                    {
                        onAccountInformation(json["accountInformation"]);
                    }
                }
                else
                {
                    WriteLog(string.Format("[SYNC] {0}", msg));
                }
            }
            catch (Exception ex)
            {
                string exp = ex.Message;
            }

        }
        public bool reqQuote(string newSymbol, string oldSymbol)
        {
            /*
            var request = new Dictionary<string, object>();
            if (oldSymbol.Length > 0)
            {
                request = new Dictionary<string, object>
                {
                    { "type", "unsubscribeFromMarketData"},
                    { "symbol", oldSymbol},
                    { "instanceIndex", 0},
                    { "accountId", mt_userId},
                    { "application", m_application},
                    { "requestId", GetRandomId()},
                    { "timestamps",  new Dictionary<string, string> {{ "clientProcessingStarted", GetIsoFormat(DateTime.Now.ToUniversalTime()) }}}
                };

                _socketClient.EmitAsync("request", request);
                WriteLog(string.Format("[Send] {0}", JsonSerializer.Serialize(request)));

                Thread.Sleep(1000);
            }

            var subscriptions = new List<object>
            {
                new Dictionary<string, object>{
                    { "type", "quotes" }
                },
                // new Dictionary<string, object>{
                //     { "type", "quotes" }, {"intervalInMilliseconds", 5000}
                // },
                // new Dictionary<string, object>{
                //     { "type", "candles" }, {"timeframe", "1m"}, {"intervalInMilliseconds", 10000}
                // },
                // new Dictionary<string, object>{
                //     { "type", "ticks" }
                // },
                // new Dictionary<string, object>{
                //     { "type", "marketDepth" }, {"intervalInMilliseconds", 5000}
                // },
            };


            request = new Dictionary<string, object>
                {
                    { "type", "subscribeToMarketData"},
                    { "symbol", newSymbol},
                    { "subscriptions", subscriptions},
                    { "instanceIndex", 0},
                    { "accountId", mt_userId},
                    { "application", m_application},
                    { "requestId", GetRandomId()},
                    { "timestamps",  new Dictionary<string, string> {{ "clientProcessingStarted", GetIsoFormat(DateTime.Now.ToUniversalTime()) }}}
                };

            _socketClient.EmitAsync("request", request);
            WriteLog(string.Format("[Send] {0}", JsonSerializer.Serialize(request)));
            */
            string token = UserPassword;
            string url = "";
            string body = "";
            HttpHeaders headers = null;
            if (oldSymbol.Length > 0)
            {
                url = String.Format("{0}/users/current/accounts/{1}/symbols/{2}/unsubscribe", URL_MAIN, mt_userId, oldSymbol);
                if (!_httpClient.SendRequest(out body, out headers, HTTPREQUEST_TYPE.POST, url, token))
                    return false;

                Thread.Sleep(1000);
            }


            url = String.Format("{0}/users/current/accounts/{1}/symbols/{2}/current-tick?keepSubscription=true", URL_MAIN, mt_userId, newSymbol);
            if (!_httpClient.SendRequest(out body, out headers, HTTPREQUEST_TYPE.GET, url, token))
                return false;

            return true;
        }
        private void onReceiveCurrent(JsonValue jsonPrice)
        {
            try
            {
                string symbol = jsonPrice["symbol"];
                if (symbol != ItemSymbol)
                {
                    WriteLog(string.Format("[Current] otherSymbol={0}", symbol));
                    return;
                }

                double ask = jsonPrice["ask"];
                double bid = jsonPrice["bid"];
                string sTime = jsonPrice["time"];
                string sBrokerTime = jsonPrice["brokerTime"];
                double equity = jsonPrice["equity"];

                DateTime time = DateTime.ParseExact(sTime, "yyyy-MM-ddTHH:mm:ss.fffZ", System.Globalization.CultureInfo.InvariantCulture);
                DateTime timeBroker = DateTime.ParseExact(sBrokerTime, "yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);

                Current current = new Current();
                current.ReceivedDate = time;

                current.CurrentPrice = bid;
                current.CurrentPrice2 = ask;
                current.ConclusionVolume = 1;

                WriteLog(string.Format("[Current] time={0}, ask={1}, bid={2}, equity={3}", sTime, ask, bid, equity));

                if (CurItemSymbol != null && CurItemSymbol.MidPrice == 0)
                {
                    CurItemSymbol.MidPrice = ask;
                }

                if (bQutoteCreated)
                    OnReceiveCurrent(current);

            }
            catch (Exception ex)
            {
                string exp = ex.Message;
            }

        }

    }
}
