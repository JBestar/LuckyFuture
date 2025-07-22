#define WRITE_LOG

using ChartCtrl;
using LuckyFuture.Models.ValueObjects;
using LuckyFuture.Models.Reanteck;
using LuckyFutureLib.Include;
using LuckyFutureLib.AsynSocket;
using LuckyFuture.Properties;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using Goodbyte.TradingSystem.Domain.Entities;

namespace LuckyFuture.Site
{


    class SiteReantek : FutureSite
    {
        public override SITETYPE Type { get ; set; }

        ConnectSocket _ConnSock = null;
        CurrencySocket _CurrSock = null;

        LOGINSTATE _ConnectState = LOGINSTATE.NONE;
        LOGINSTATE _KeepAlive = LOGINSTATE.NONE;

        string _AppName = "";
        string _IpAddr = "";
        const int _ConnPort = 5335;
        const int _CurrPort = 5334;

        public const string RECV_RESULT_SUCCESS = "success";
        public const string RECV_RESULT_NO_ID = "NO_MBR_ID";
        public const string RECV_RESULT_NO_PWD = "NO_PWD";
        public const string RECV_RESULT_DUP_ID = "DUP_ID";

        public const string PRESENT_HEAD_M6 = "M6";   //해외선물 호가
        public const string PRESENT_HEAD_M3 = "M3";   //해외선물 체결정보

        public const int PRESENT_SIZE_LEN = 5;
        public const int PRESENT_HEAD_LEN = 2;
        public const int PRESENT_SYMBOL_LEN = 15;

        public const int PRESENT_M6_LEN = 0x1D4 - PRESENT_SIZE_LEN;   //해외선물 호가 길이
        public const int PRESENT_M3_LEN = 0x92 - PRESENT_SIZE_LEN;   //해외선물 체결가 길이

        public const int PACKET_BODYLEN_1 = 1;
        public const int PACKET_BODYLEN_2 = 2;
        public const int PACKET_BODYLEN_3 = 3;
        public const int PACKET_BODYLEN_4 = 4;
        public const int PACKET_BODYLEN_5 = 5;
        public const int PACKET_BODYLEN_6 = 6;
        public const int PACKET_BODYLEN_7 = 7;
        public const int PACKET_BODYLEN_8 = 8;
        public const int PACKET_BODYLEN_9 = 9;
        public const int PACKET_BODYLEN_10 = 10;
        public const int PACKET_BODYLEN_11 = 11;
        public const int PACKET_BODYLEN_12 = 12;
        public const int PACKET_BODYLEN_15 = 15;
        public const int PACKET_BODYLEN_16 = 16;
        public const int PACKET_BODYLEN_20 = 20;
        public const int PACKET_BODYLEN_22 = 22;
        public const int PACKET_BODYLEN_30 = 30;

        bool _ordered = false;
        private double FEE_RATE = 6.5;

        int _aliveTime = 0;
        int _orderTick = 0;
        int _currencyTick = 0;
        TRADETYPE _orderTrade = TRADETYPE.NONE;

        Random _random = new Random();
        DateTime _currencyDt;
        public SiteReantek(SITETYPE siteType)
        {
            Type = siteType;
            if(siteType == SITETYPE.TOPASSET)
            {
                _AppName = "/NEW_MST/몬스타 HTS/TeamViewer, /@";
                //_AppName = "몬스타 HTS/TeamViewer, /@";
                _IpAddr = "222.239.252.47";
            }
            else if (siteType == SITETYPE.MIRAE2)
            {
                _AppName = "/NEW/미래선물 HTS/N/@";
                _IpAddr = "117.52.11.89";
                FEE_RATE = 7;

            }
            else if (siteType == SITETYPE.DREAM)
            {
                 FEE_RATE = 2;
                _AppName = "/NEW/TD/N/@";
                _IpAddr = "172.65.218.121"; //172.65.237.26 // 172.65.237.42
            }
#if WRITE_LOG
            CreateLogFile();
#endif
        }
        string _logPath = "";

        private void CreateLogFile()
        {
            _logPath = "C://BinHts/KFOpen_" + DateTime.Now.ToString("yyyyMMddHHmmss");
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
            ERRORCODE login_result = ERRORCODE.UNKNOWN_FAILED;
            _ConnSock.ConnectSocket();
            Thread.Sleep(200);
            _ConnectState = LOGINSTATE.NONE;
            _ConnSock.RequestDialogLogin(id, password, _AppName);            
            Thread.Sleep(1000);

            if (_ConnectState == LOGINSTATE.OK)
            {
                _KeepAlive = LOGINSTATE.OK;
                login_result = ERRORCODE.SUCCESS;
            } else if(_ConnectState == LOGINSTATE.NO_ID)
            {
                login_result = ERRORCODE.LOGIN_NO_ID;
            } else if(_ConnectState == LOGINSTATE.NO_PWD)
            {
                login_result = ERRORCODE.LOGIN_NO_PWD;
            } else if (_ConnectState == LOGINSTATE.DUP_ID)
            {
                _ConnSock.RequestDupLogin(id);
                Thread.Sleep(500);
                if (_ConnectState == LOGINSTATE.OK)
                {
                    _KeepAlive = LOGINSTATE.OK;
                    login_result = ERRORCODE.SUCCESS;
                } else login_result = ERRORCODE.LOGIN_NO_CON;
            }
            else if (_ConnectState == LOGINSTATE.NONE)
            {
                login_result = ERRORCODE.LOGIN_NO_CON;
            }

            _ConnSock.CloseSocket();
            Thread.Sleep(200);
            OnLogin();
            return login_result;
        }


        protected override ERRORCODE LogOut()
        {
            _ConnSock.RequestLogout();
            Thread.Sleep(200);
            _ConnSock.RequestLogoutEnd();
            Thread.Sleep(200);
            _ConnSock.CloseSocket();
            _CurrSock.CloseSocket();
            return ERRORCODE.SUCCESS;
        }

        protected override ERRORCODE Prepare()
        {
            
            if (CurrentUserAccount == null || CurrentUserAccount.UserAccountStr == null)
                return ERRORCODE.UNKNOWN_FAILED;
            
            if (ItemList.Count < 1)
                return ERRORCODE.UNKNOWN_FAILED;
            
            if (ItemSymbol != CurItemSymbol.Symbol)
            {
                foreach (ItemSymbolInfo itemSymbol in ItemList)
                {
                    if (ItemSymbol == itemSymbol.Symbol)
                    {
                        CurItemSymbol = itemSymbol;
                        break;
                    }
                }

                if (ItemSymbol != CurItemSymbol.Symbol)
                    return ERRORCODE.UNKNOWN_FAILED;

                ItemPrecision = CurItemSymbol.Precision;
                Settings.Default.PriceFormat = Common.GetPriceFormat(ItemPrecision);
                OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.PREPAREITEM);

                Thread.Sleep(1000);
            }
            

            if (CurItemSymbol.MidPrice == 0)
            {
                Thread.Sleep(1000);
                return ERRORCODE.PREPARE_FAILED;
            }
                
            this.QuoteList = CreateQuoteInfo();
            Prepared = true;
            
            _ConnSock.RequestProfit(CurrentUserAccount.UserAccountStr);
            Thread.Sleep(500);

            return ERRORCODE.SUCCESS;
        }

        

        protected override ERRORCODE Check()
        {
            if(_ConnSock.CheckConnection() && _CurrSock.CheckConnection())
            {
                //종목변경
                if (CurItemSymbol != null && this.ItemSymbol != CurItemSymbol.Symbol)
                {
                    _ConnSock.RequestMsg("주문창#[클릭주문] 1번  종목변경: "+ this.ItemSymbol + " -> "+ CurItemSymbol.Symbol + "#"+AppConfig._PhysicalAddr);
                    return ERRORCODE.PREPARE_FAILED;
                }
                else
                {
                    if(_KeepAlive != LOGINSTATE.OK)
                    {
                        LogOut();
                        return ERRORCODE.UNKNOWN_FAILED;
                    }
                    else if (_ordered)
                    {
                        _ordered = false;
                        Thread.Sleep(500);
                        _ConnSock.RequestProfit();                        
                        Thread.Sleep(500);
                        _ConnSock.RequestReceiptlist();
                        Thread.Sleep(500);
                        _ConnSock.RequestOrderlist();

                    } else if (Math.Abs(Environment.TickCount - _aliveTime) > 300000)
                    {
                        _KeepAlive = LOGINSTATE.NONE;
                        _aliveTime = Environment.TickCount;
                        _ConnSock.RequestKeepAlive();
                        
                    } else if(_currencyTick != 0 && Math.Abs(Environment.TickCount - _currencyTick) > 120000)
                    {
                        LogOut();
                        return ERRORCODE.UNKNOWN_FAILED;
                    }
                    
                    return ERRORCODE.SUCCESS;
                    
                }
            }

            return ERRORCODE.UNKNOWN_FAILED;
        }



        protected override void OnStarted()
        {
            base.OnStarted();

            _ConnSock = new ConnectSocket(_IpAddr, _ConnPort);
            _CurrSock = new CurrencySocket(_IpAddr, _CurrPort);

            _ConnSock.DataReceiveEvent += OnSiteNoticeReceive;
            _CurrSock.DataReceiveEvent += OnSiteCurrencyReceive;
        }

        protected override void OnStopped(bool bAutoStop)
        {
            _ConnSock.DataReceiveEvent -= OnSiteNoticeReceive;
            _CurrSock.DataReceiveEvent -= OnSiteCurrencyReceive;

            base.OnStopped(bAutoStop);
        }

        protected override void OnLogin()
        {
            base.OnLogin();

            Thread.Sleep(200);
            _ConnectState = LOGINSTATE.NONE;
            _ConnSock.ConnectSocket();
            Thread.Sleep(200);
            _ConnSock.RequestMainLogin(UserId, UserPassword);
            
            this.UserAccounts = new List<UserAccountInfo>();
            this.CurrentUserAccount = new UserAccountInfo();
            DayProfitLoss = new DayProfitLossInfo();
            // ItemSymbol = "";
            Thread.Sleep(500);

            _currencyDt = DateTime.Now;
            _currencyTick = 0;
            StartPrice = 0;
            _CurrSock.ConnectSocket();
            Thread.Sleep(200);
            _CurrSock.RequestLogin(UserId);

            int delayMs = 200;
            Thread.Sleep(delayMs);
            _ConnSock.RequestAccount();
            Thread.Sleep(delayMs);
            _ConnSock.Request401();
//             Thread.Sleep(delayMs);
//             _ConnSock.Request005();
            Thread.Sleep(delayMs);
            _ConnSock.Request301();
            Thread.Sleep(delayMs);
            _ConnSock.Request302();
            Thread.Sleep(delayMs);
            _ConnSock.Request402();
            Thread.Sleep(delayMs);
            _ConnSock.Request202();
            Thread.Sleep(delayMs);
            _ConnSock.RequestItemlist();
            Thread.Sleep(delayMs);
            _ConnSock.Request001();
//            Thread.Sleep(delayMs);
//            _ConnSock.Request307();

            Thread.Sleep(delayMs);
            _ConnSock.Request421();
            Thread.Sleep(delayMs);
            _ConnSock.Request411();
            Thread.Sleep(delayMs);
            _ConnSock.Request504();
            Thread.Sleep(200);

            _ConnSock.RequestMsg("접속#로그인");

        }

        protected override void OnPrepare()
        {
            base.OnPrepare();

            Settings.Default.ServerTimeDelay = 0;

            Current = null;
            CurrentList.Clear();
            OrderList.Clear();
            _ConnSock.RequestReceiptlist();
            Thread.Sleep(500);
            _ConnSock.RequestOrderlist();
            Thread.Sleep(500);
            //Prepared = true ;

        }

        public override bool ChangeItem(string sSymbol)
        {
            if (ItemList == null || ItemList.Count < 1)
                return false;
            if (ItemSymbol != sSymbol)
            {
                Prepared = false;
                ItemSymbol = sSymbol;
                StartPrice = 0;
                return true;
            }

            return false;
        }

        private void OnSiteNoticeReceive(object sender, SocketEventArgs e)
        {

            List<string> packetList;
            lock (_ConnSock.ResponseList)
            {
                if (_ConnSock.ResponseList.Count < 1)
                    return;
                
                packetList = new List<string>(_ConnSock.ResponseList);
                _ConnSock.ResponseList.Clear();
            }

            string rvHead = "";
            int nCurPos = 0;
            foreach (string recvMsg in packetList)
            {
                rvHead = recvMsg.Substring(0, ConnectSocket.HEAD_NAME_LENGTH);
                nCurPos = ConnectSocket.HEAD_NAME_LENGTH + ConnectSocket.HEAD_PACKETNO_LENGTH;
               
                if (rvHead == ConnectSocket.HEAD_POPUPLOGIN)
                {
                    // NO_MBR_ID#Y
                    // NO_PWD/1#Y
                    // success#Y, EVAL_SCRT_AMT#Y
                    // DUP_ID#Y
                   string[] splitMsg = recvMsg.Substring(nCurPos += PACKET_BODYLEN_30).Split('#');
                    if (splitMsg[0].ToLower() == RECV_RESULT_SUCCESS || splitMsg[0].StartsWith("EVAL_SCRT_AMT") )
                        _ConnectState = LOGINSTATE.OK;
                    else
                    {
                        string[] splitResult = splitMsg[0].Split('/');
                        if (splitResult[0] == RECV_RESULT_NO_ID)
                            _ConnectState = LOGINSTATE.NO_ID;
                        else if (splitResult[0] == RECV_RESULT_NO_PWD && splitResult.Length > 1)
                        {
                            login_errors = 0;
                            _ConnectState = LOGINSTATE.NO_PWD;
                            if (int.TryParse(splitResult[1], out login_errors))
                            {

                            }

                        }
                        else if (splitResult[0] == RECV_RESULT_DUP_ID)
                        {
                            _ConnectState = LOGINSTATE.DUP_ID;
                        }

                    }
                }
                else if (rvHead == ConnectSocket.HEAD_DUPLOGIN)
                {
                    if(recvMsg.Substring(nCurPos += PACKET_BODYLEN_30).ToLower() == RECV_RESULT_SUCCESS)
                    {
                        _ConnectState = LOGINSTATE.OK;
                    }
                }
                else if (rvHead == ConnectSocket.HEAD_KEEPALIVE)
                {
                    
                    string[] splitMsg = recvMsg.Substring(nCurPos += PACKET_BODYLEN_30).Split('/');
                    if (splitMsg.Length > 1) {
                        if(splitMsg[1].Trim().ToLower() == RECV_RESULT_SUCCESS)
                            _KeepAlive = LOGINSTATE.OK;
                        else _KeepAlive = LOGINSTATE.NONE;
                    }
                }
                else if (rvHead == ConnectSocket.HEAD_MAINLOGIN)
                {
                    if (recvMsg.Substring(nCurPos += PACKET_BODYLEN_30).ToLower() == RECV_RESULT_SUCCESS)
                    {
                        _aliveTime = Environment.TickCount;
                        //_ConnectState = LOGINSTATE.OK;
                    }

                    //else _ConnectState = LOGINSTATE.FAILED;
                }
                else if (rvHead == ConnectSocket.HEAD_RECEIPT_R)
                {
                    nCurPos += PACKET_BODYLEN_30;
                    if (recvMsg.Substring(nCurPos).IndexOf(ItemSymbol) >= 0)
                    {
                        ParseReceiptMsg(recvMsg.Substring(nCurPos));
                    }

                }
                else if (rvHead == ConnectSocket.HEAD_ORDER_R)
                {
                    nCurPos += PACKET_BODYLEN_30;
                    if (recvMsg.Substring(nCurPos).IndexOf(ItemSymbol) >= 0)
                    {
                        ParseOrderMsg(recvMsg.Substring(nCurPos));
                    }
                }
                else if (rvHead == ConnectSocket.HEAD_CANCEL_R)
                {
                    nCurPos += PACKET_BODYLEN_30;
                    if (recvMsg.Substring(nCurPos).IndexOf(ItemSymbol) >= 0)
                    {
                        ParseCancelMsg(recvMsg.Substring(nCurPos));
                    }
                }
                else if (rvHead == ConnectSocket.HEAD_ACCOUNT)
                {
                    ParseAccountMsg(recvMsg.Substring(nCurPos += PACKET_BODYLEN_30));
                }
                else if (rvHead == ConnectSocket.HEAD_PROFIT)
                {
                    nCurPos += PACKET_BODYLEN_30 + PACKET_BODYLEN_8;

                    ParseValuationMsg(recvMsg.Substring(nCurPos));
                }
                else if (rvHead == ConnectSocket.HEAD_ITEMLIST)
                {
                    nCurPos += PACKET_BODYLEN_30 + PACKET_BODYLEN_8;
                    ParseItemlistMsg(recvMsg.Substring(nCurPos));
                }
                else if (rvHead == ConnectSocket.HEAD_ITEMQUOTE_RANGE)
                {
                    nCurPos += PACKET_BODYLEN_30 + PACKET_BODYLEN_2;
                    if (recvMsg.Substring(nCurPos).IndexOf(ItemSymbol) >= 0)
                    {
                        ParseItemRangeMsg(recvMsg.Substring(nCurPos));
                    }

                }
                else if (rvHead == ConnectSocket.HEAD_ORDERLIST)
                {
                    nCurPos += PACKET_BODYLEN_30;
                    ParseOrderlistMsg(recvMsg.Substring(nCurPos));

                }
                else if (rvHead == ConnectSocket.HEAD_RECEIPTLIST)
                {
                    nCurPos += PACKET_BODYLEN_30;
                    ParseReceiptlistMsg(recvMsg.Substring(nCurPos));

                }
                
            }
            
        }

        private void OnSiteCurrencyReceive(object sender, SocketEventArgs e)
        {
            List<string> packetList;
            lock (_CurrSock.ResponseList)
            {
                if (_CurrSock.ResponseList.Count < 1)
                    return;
                _currencyTick = Environment.TickCount;
                packetList = new List<string>(_CurrSock.ResponseList);
                _CurrSock.ResponseList.Clear();
            }

            if (CurItemSymbol == null )
                return;

            string rvHead = "";
            string rvItemSymbol = "";
            int iCurPos = 0;
            foreach (string packetMsg in packetList)
            {
                iCurPos = 0;
                if (packetMsg.Length < PRESENT_HEAD_LEN)
                    continue;
                rvHead = packetMsg.Substring(iCurPos, PRESENT_HEAD_LEN);
                iCurPos += PRESENT_HEAD_LEN;
                if (rvHead == PRESENT_HEAD_M3)         //해외선물 체결가
                {
                    if (packetMsg.Length != PRESENT_M3_LEN)
                        continue;

                    rvItemSymbol = packetMsg.Substring(iCurPos, PRESENT_SYMBOL_LEN).Trim();
                    
                    if (CurItemSymbol == null || rvItemSymbol != ItemSymbol)
                        continue;

                    ParseCurrentMsg(packetMsg.Substring(iCurPos));

                } else if(rvHead == PRESENT_HEAD_M6)   //해외선물 호가
                {
                    if (packetMsg.Length != PRESENT_M6_LEN)
                        continue;

                    rvItemSymbol = packetMsg.Substring(iCurPos, PRESENT_SYMBOL_LEN).Trim();
                    if (CurItemSymbol == null || rvItemSymbol != ItemSymbol)
                        continue;

                    ParseQuoteMsg(packetMsg.Substring(iCurPos));
                }
            }
        }

        protected void ParseQuoteMsg(string strQuote)
        {
            Quote quote = new Quote();
            int iCurPos = 0;

            try
            {
                quote.Symbol = strQuote.Substring(iCurPos, PRESENT_SYMBOL_LEN).Trim();

                quote.TotalBidQty = int.Parse(strQuote.Substring(iCurPos += PRESENT_SYMBOL_LEN, PACKET_BODYLEN_15));
                quote.Bid1 = double.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_15, PACKET_BODYLEN_20));
                quote.BidQty1 = int.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_20, PACKET_BODYLEN_12));
                quote.Bid2 = double.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_12, PACKET_BODYLEN_20));
                quote.BidQty2 = int.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_20, PACKET_BODYLEN_12));
                quote.Bid3 = double.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_12, PACKET_BODYLEN_20));
                quote.BidQty3 = int.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_20, PACKET_BODYLEN_12));
                quote.Bid4 = double.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_12, PACKET_BODYLEN_20));
                quote.BidQty4 = int.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_20, PACKET_BODYLEN_12));
                quote.Bid5 = double.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_12, PACKET_BODYLEN_20));
                quote.BidQty5 = int.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_20, PACKET_BODYLEN_12));

                quote.TotalAskQty = int.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_12, PACKET_BODYLEN_15));
                quote.Ask1 = double.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_15, PACKET_BODYLEN_20));
                quote.AskQty1 = int.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_20, PACKET_BODYLEN_12));
                quote.Ask2 = double.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_12, PACKET_BODYLEN_20));
                quote.AskQty2 = int.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_20, PACKET_BODYLEN_12));
                quote.Ask3 = double.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_12, PACKET_BODYLEN_20));
                quote.AskQty3 = int.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_20, PACKET_BODYLEN_12));
                quote.Ask4 = double.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_12, PACKET_BODYLEN_20));
                quote.AskQty4 = int.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_20, PACKET_BODYLEN_12));
                quote.Ask5 = double.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_12, PACKET_BODYLEN_20));
                quote.AskQty5 = int.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_20, PACKET_BODYLEN_12));

                quote.TotalBidCount = int.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_12, PACKET_BODYLEN_10));
                quote.BidCount1 = int.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_10, PACKET_BODYLEN_7));
                quote.BidCount2 = int.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_7, PACKET_BODYLEN_7));
                quote.BidCount3 = int.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_7, PACKET_BODYLEN_7));
                quote.BidCount4 = int.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_7, PACKET_BODYLEN_7));
                quote.BidCount5 = int.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_7, PACKET_BODYLEN_7));

                quote.TotalAskCount = int.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_7, PACKET_BODYLEN_10));
                quote.AskCount1 = int.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_10, PACKET_BODYLEN_7));
                quote.AskCount2 = int.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_7, PACKET_BODYLEN_7));
                quote.AskCount3 = int.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_7, PACKET_BODYLEN_7));
                quote.AskCount4 = int.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_7, PACKET_BODYLEN_7));
                quote.AskCount5 = int.Parse(strQuote.Substring(iCurPos += PACKET_BODYLEN_7, PACKET_BODYLEN_7));


                quote.QuoteTime = strQuote.Substring(iCurPos += PACKET_BODYLEN_7, PACKET_BODYLEN_6);
                quote.ReceivedDate = DateTime.ParseExact(quote.QuoteTime, "HHmmss", System.Globalization.CultureInfo.InvariantCulture);

                if(CurItemSymbol.MidPrice == 0)
                {
                    CurItemSymbol.MidPrice = quote.Ask1;
                }
                

                OnReceiveQuote(quote);
            }
            catch (Exception)
            {

            }
            
        }


        protected void ParseCurrentMsg(string strCurrent)
        {
            Current current = new Current();
            int iCurPos = 0;
            try
            {
                current.Symbol = strCurrent.Substring(iCurPos, PRESENT_SYMBOL_LEN).Trim();
                current.CurrentPrice = double.Parse(strCurrent.Substring(iCurPos += PRESENT_SYMBOL_LEN, PACKET_BODYLEN_20));
                current.ConclusionVolume = int.Parse(strCurrent.Substring(iCurPos += PACKET_BODYLEN_20, PACKET_BODYLEN_12));
                current.StartPrice = double.Parse(strCurrent.Substring(iCurPos += PACKET_BODYLEN_12, PACKET_BODYLEN_20));
                current.HighPrice = double.Parse(strCurrent.Substring(iCurPos += PACKET_BODYLEN_20, PACKET_BODYLEN_20));
                current.LowPrice = double.Parse(strCurrent.Substring(iCurPos += PACKET_BODYLEN_20, PACKET_BODYLEN_20));
                current.ContrastPer = double.Parse(strCurrent.Substring(iCurPos += PACKET_BODYLEN_20, PACKET_BODYLEN_6));
                current.Contrast = double.Parse(strCurrent.Substring(iCurPos += PACKET_BODYLEN_6, PACKET_BODYLEN_20));
                current.CurrentTime = strCurrent.Substring(iCurPos += PACKET_BODYLEN_20, PACKET_BODYLEN_6);
                current.ReceivedDate = DateTime.ParseExact(current.CurrentTime, "HHmmss", System.Globalization.CultureInfo.InvariantCulture);

                TimeSpan timeSpan = current.ReceivedDate - _currencyDt ;
                
                if (timeSpan.TotalSeconds >= 0 && timeSpan.TotalSeconds < 4000)
                    _currencyDt = current.ReceivedDate;
                else 
                    current.ReceivedDate = _currencyDt;

                current.TradeType = _random.Next(10)%2 == 1 ?  TradeType.Buy:TradeType.Sell;

                if (CurItemSymbol.MidPrice == 0)
                {
                    CurItemSymbol.MidPrice = current.CurrentPrice;
                }
                // WriteLog(string.Format("{0} | {1}", current.CurrentTime, current.CurrentPrice));

                OnReceiveCurrent(current);
            }
            catch (Exception)
            {

            }
        }

        private void ParseAccountMsg(string msg)
        {
            string[] splitMsg = msg.Split('/');
            if (Type == SITETYPE.DREAM)
                splitMsg = msg.Split('|');
            if (splitMsg[0] == RECV_RESULT_SUCCESS)
            {
                _ConnectState = LOGINSTATE.OK;
                CurrentUserAccount.UserAccountStr = splitMsg[7];
                int nTemp = 0;
                if (int.TryParse(splitMsg[5], out nTemp))
                {
                    CurrentUserAccount.Leverage = nTemp;
                }

                this.UserAccounts.Clear();
                this.UserAccounts.Add(this.CurrentUserAccount);

            }
        }
        
        private void ParseValuationMsg(string sValuation)
        {
            if (sValuation.Length == 111)
            {
                int iCurPos = 0;
                string sProfit = sValuation.Substring(iCurPos, PACKET_BODYLEN_22);
                string sLoss = sValuation.Substring(iCurPos += PACKET_BODYLEN_22, PACKET_BODYLEN_22);
                string sBalance = sValuation.Substring(iCurPos += PACKET_BODYLEN_22, PACKET_BODYLEN_22);

                long lBalance = 0, lProfit = 0, lLoss = 0, lValuation = 0;
                try
                {
                    lProfit = Common.StrToLong(sProfit);
                    lLoss = Common.StrToLong(sLoss);
                    lBalance = Common.StrToLong(sBalance);
                }
                catch (Exception)
                {
                    return;
                }
                if (this.CurrentUserAccount != null)
                {
                    DayProfitLoss.TotalProfit = lProfit - lLoss;

                    this.CurrentUserAccount.Balance = lBalance - lValuation;

                    this.ValuationList[0].TotalValuation = lValuation;
                    this.ValuationList[0].TotalProfit = DayProfitLoss.TotalProfit;
                    this.ValuationList[0].CurrentProfit = DayProfitLoss.TotalProfit;


                }
            }
        }

        private void ParseItemlistMsg(string sItemlist)
        {
            if (ItemList.Count > 0) return;

            //6EH23   1Y0.0000520230313202212096.25 1000000000000000000000001235Euro FX(23 - 03 / USD)                                00000000000000125000202303CME À¯·Î(23 - 03 / USD)               @
            //string strExp = @"(\w+)\s*.{2}(.{7}).{16}(.{5}).{8}(.{20})([^\)]+\))\s*.{36}([^\(]+\([^\)]+\))\s*@";
            string strExp = @"(\w+)\s*.{2}(.{7}).{16}(.{5}).{8}(.{20})(.{50}).{36}([^\(]+\([^\)]+\))\s*@";

            Regex regex = new Regex(strExp);

            MatchCollection mc = regex.Matches(sItemlist);
            ItemSymbolInfo newItem = null;
            // OnFutureSiteLogEvent("ParseItemlistMsg()");
            ItemList.Clear();
            string strTemp = "";
            int nTemp = 0;
            double dTemp = 0;
            foreach (Match m in mc)
            {
                if (m.Groups.Count == 7)
                {
                    newItem = new ItemSymbolInfo();
                    newItem.Symbol = m.Groups[1].Value;                     //6EH23

                    strTemp = m.Groups[2].Value;                            //0.00005
                    nTemp = strTemp.IndexOf('.');
                    nTemp = nTemp < 0 ? 6 : nTemp;
                    newItem.Precision = 6 - nTemp;
                    dTemp = 0;
                    if (double.TryParse(strTemp, out dTemp))
                    {
                        newItem.OverTick = dTemp;
                    }

                    strTemp = m.Groups[3].Value;                            //6.25
                    if (double.TryParse(strTemp, out dTemp))
                    {
                        newItem.ValueTick = dTemp;
                    }
                    strTemp = m.Groups[4].Value;                            //00000000000000001235
                    if (double.TryParse(strTemp, out dTemp))
                    {
                        newItem.Exchange = dTemp;
                    }
                    strTemp = m.Groups[6].Value;                   //유로(23-03/USD)
                    strTemp = strTemp.Replace("/USD", "");
                    strTemp = strTemp.Replace("/HKD", "");
                    newItem.ItemName = strTemp;

                    if (ItemSymbol.Length > 0 && ItemSymbol == newItem.Symbol)
                    {
                        ItemSymbol = newItem.Symbol;
                        CurItemSymbol = newItem;
                    } 
                    else if (ItemSymbol.Length < 1 && newItem.Symbol.IndexOf("NQ") >= 0)
                    {
                        ItemSymbol = newItem.Symbol;
                        CurItemSymbol = newItem;
                    }
//                     OnFutureSiteLogEvent("ParseItemlistMsg() Item="+newItem.Symbol+ " Precision="+newItem.Precision+
//                         " OverTick="+newItem.OverTick+" Exchange="+newItem.Exchange+" ValueTick="+newItem.ValueTick);
                    ItemList.Add(newItem);
                }
            }

            if (ItemSymbol.Length < 1 && ItemList.Count > 0)
            {
                CurItemSymbol = ItemList.First();
                ItemSymbol = CurItemSymbol.Symbol;
            }

            if (CurItemSymbol != null)
            {
                ItemPrecision = CurItemSymbol.Precision;
                Settings.Default.PriceFormat = Common.GetPriceFormat(ItemPrecision);
                OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.PREPAREITEM);
            }

        }

        protected void ParseItemRangeMsg(string sMsg)
        {
            string[] itemRanges = sMsg.Split('#');
            string[] itemInfo;
            double dTemp = 0;
            try
            {
                foreach (string itemRange in itemRanges)
                {
                    itemInfo = itemRange.Split('/');
                    foreach(ItemSymbolInfo itemSymbol in ItemList)
                    {
                        if (itemInfo.Length == 5 && itemSymbol.Symbol == itemInfo[0])
                        {
                            dTemp = 0;
                            if (double.TryParse(itemInfo[4], out dTemp))
                            {
                                itemSymbol.MidPrice = dTemp;
                            }
                            break;
                        }
                    }

                }   
            }
            catch (Exception) { }
        }

        public override bool DoSellOrder(QuoteInfo quoteInfo, int nQuantity = 1, bool bMarketPrice = false)
        {

            if (this.CurrentUserAccount == null)
            {
                OnFutureSiteLogEvent("[주문] 계좌정보 오류!");
                return false;
            }
            if (nQuantity < 1 || nQuantity > 10)
            {
                OnFutureSiteLogEvent("[주문] 주문수량 오류!");
                return false;
            }
            if (CurrentUserAccount.Balance < 300000 * nQuantity)
            {
                OnFutureSiteLogEvent("[주문] 담보금 부족!");
                return false;
            }

            /*
            if (quoteInfo.Price < this.Ask1Row.Price && quoteInfo.Price > this.Bid1Row.Price)
            {
                OnFutureSiteLogEvent("빈 호가엔 주문할 수 없습니다.");
                return false;
            }
            */
            if (Current == null || Current.CurrentPrice <= 0)
            {
                OnFutureSiteLogEvent("[주문] 주문기간이 아닙니다.");
                return false;
            }

            if (quoteInfo != null)
            {
                double nOrderRange = 40 * CurItemSymbol.OverTick;
                if (quoteInfo.Price < Current.CurrentPrice - nOrderRange || quoteInfo.Price > Current.CurrentPrice + nOrderRange)
                {
                    OnFutureSiteLogEvent("[주문] 주문 가격이 초과 됨");
                    return false;
                }
            }
            else bMarketPrice = true;
            string msg = "주문창#[클릭주문] 1번  [Mileage : 0] 시장가매도버튼 클릭 " + CurItemSymbol.Symbol + "#";
            if (!bMarketPrice)
            {
                msg = "주문창#[클릭주문] 1번  [Mileage : 0] 지장가매도버튼 클릭 " + CurItemSymbol.Symbol + "#";
            }
            if (_ConnSock.RequestOrder(TRADETYPE.SELL, CurItemSymbol.Symbol, 
                string.Format(Settings.Default.PriceFormat, bMarketPrice ? 0 : quoteInfo.Price), nQuantity, bMarketPrice))
            {
                _orderTrade = TRADETYPE.SELL;
                _orderTick = Environment.TickCount;

                OnFutureSiteLogEvent("[주문] 매도주문이 접수되었습니다.");
                OnFutureSiteLogEvent("[주문] 주문시가격:" + Current.CurrentPrice);
            } else
            {
                OnFutureSiteLogEvent("[주문] 매도주문 실패!!!");                
            }

            return true;

        }

        public override bool DoBuyOrder(QuoteInfo quoteInfo, int nQuantity = 1, bool bMarketPrice = false)
        {


            if (this.CurrentUserAccount == null )
            {
                OnFutureSiteLogEvent("[주문] 계좌정보 오류!");
                return false;
            }

            if (nQuantity < 1 || nQuantity > 10)
            {
                OnFutureSiteLogEvent("[주문] 주문수량 오류!");
                return false;
            }

            if (CurrentUserAccount.Balance < 300000 * nQuantity)
            {
                OnFutureSiteLogEvent("[주문] 담보금 부족!");
                return false;
            }
            /*
            if (quoteInfo.Price < this.Ask1Row.Price && quoteInfo.Price > this.Bid1Row.Price)
            {
                OnFutureSiteLogEvent("빈 호가엔 주문할 수 없습니다.");
                return false;
            }
            */
            if (Current == null || Current.CurrentPrice <= 0)
            {
                OnFutureSiteLogEvent("[주문] 주문기간이 아닙니다.");
                return false;
            }

            if (quoteInfo != null)
            {
                double nOrderRange = 40 * CurItemSymbol.OverTick;
                if (quoteInfo.Price < Current.CurrentPrice - nOrderRange || quoteInfo.Price > Current.CurrentPrice + nOrderRange)
                {
                    OnFutureSiteLogEvent("[주문] 주문 가격이 초과 됨");
                    return false;
                }
            }
            else bMarketPrice = true;

            string msg = "주문창#[클릭주문] 1번  [Mileage : 0] 시장가매수버튼 클릭 " + CurItemSymbol.Symbol + "#";
            if (!bMarketPrice)
            {
                msg = "주문창#[클릭주문] 1번  [Mileage : 0] 지장가매수버튼 클릭 " + CurItemSymbol.Symbol + "#";
            }
            if (_ConnSock.RequestOrder(TRADETYPE.BUY, CurItemSymbol.Symbol,
                string.Format(Settings.Default.PriceFormat, bMarketPrice ? 0 : quoteInfo.Price), nQuantity, bMarketPrice))
            {
                _orderTrade = TRADETYPE.BUY;
                _orderTick = Environment.TickCount;

                OnFutureSiteLogEvent("[주문] 매수주문이 접수되었습니다.");
                OnFutureSiteLogEvent("[주문] 주문시가격:" + Current.CurrentPrice);

            } else
            {
                OnFutureSiteLogEvent("[주문] 매수주문 실패!!!");
            }


            return true;
        }


        public override bool CancelOrder(OrderInfo orderInfo)
        {
            if (this.CurrentUserAccount == null)
            {
                OnFutureSiteLogEvent("계좌정보 오류!");
                return false;
            }

            if(_ConnSock.RequestCancel(orderInfo.TradeType, orderInfo.Symbol, orderInfo.AveragePrice, orderInfo.OrderQty, long.Parse(orderInfo.OrderNo)))
            {
                if (orderInfo.TradeType == TRADETYPE.SELL)
                    OnFutureSiteLogEvent("[주문취소] 매도주문이 취소되었습니다.");
                else if (orderInfo.TradeType == TRADETYPE.BUY)
                    OnFutureSiteLogEvent("[주문취소] 매수주문이 취소되었습니다.");
            } else
            {
                OnFutureSiteLogEvent("[주문취소] 취소주문 실패!!!");
            }

            return true;
        }

        public override bool LiquidateOrder(OrderInfo orderInfo)
        {
            if (this.CurrentUserAccount == null)
                return false;
            
            if(_ConnSock.RequestOrder(orderInfo.TradeType == TRADETYPE.SELL ? TRADETYPE.BUY:TRADETYPE.SELL,
                orderInfo.Symbol, string.Format(Settings.Default.PriceFormat, 0), orderInfo.OrderQty, true))
            {
                if (orderInfo.TradeType == TRADETYPE.SELL)
                    OnFutureSiteLogEvent("[청산] 매도주문이 청산되었습니다.");
                else if (orderInfo.TradeType == TRADETYPE.BUY)
                    OnFutureSiteLogEvent("[청산] 매수주문이 청산되었습니다.");

                OnFutureSiteLogEvent("[청산] 청산시가격:" + Current.CurrentPrice);

            } else
            {
                OnFutureSiteLogEvent("[청산] 청산 실패!!!");
            }


            return true;
        }

        protected override List<QuoteInfo> CreateQuoteInfo()
        {
            List<QuoteInfo> list = new List<QuoteInfo>();
            if (CurItemSymbol == null)
                return list;
            if (CurItemSymbol.OverTick <= 0)
                return list;

            Settings.Default.ItemOverTick = (float)CurItemSymbol.OverTick;

            double upLimitPrice = CurItemSymbol.MidPrice + CurItemSymbol.OverTick * 1000.0;
            double downLimitPrice = CurItemSymbol.MidPrice - CurItemSymbol.OverTick * 1000.0;
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

            return list;
        }


        private void OnReceiveQuote(Quote quote)
        {
            if (!Prepared)
                return;

            SetQuoteInfo(quote);
            SetTotalQuoteInfo(quote);
            OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.QUOTE);
        }

        private void OnReceiveCurrent(Current current)
        {
            if (!Prepared)
                return;
            try
            {

                if (StartPrice == 0)
                {
                    StartPrice = current.CurrentPrice;
                }

                lock (this.OrderList)
                {
                    if (this.OrderList != null)
                    {
                        int nOrderCnt = this.OrderList.Count;
                        if (nOrderCnt > 0)
                        {

                            OrderInfo orderInfo;
                            long? lValSum = 0;
                            double dAveragePrice = 0.0;
                            double dAveragePriceSum = 0.0;
                            for (int iRow = 0; iRow < nOrderCnt; iRow++)
                            {
                                orderInfo = OrderList[iRow];
                                if (orderInfo == null)
                                    break;

                                if (orderInfo.OrderType.StartsWith("미체결"))
                                {
                                    orderInfo.CurrentPrice = string.Format(Settings.Default.PriceFormat, current.CurrentPrice);
                                    continue;
                                }

                                if (double.Parse(orderInfo.CurrentPrice) != current.CurrentPrice)
                                {
                                    orderInfo.CurrentPrice = string.Format(Settings.Default.PriceFormat, current.CurrentPrice);
                                    dAveragePrice = double.Parse(orderInfo.AveragePrice);
                                    dAveragePriceSum += dAveragePrice;
                                    orderInfo.Valuation = orderInfo.TradeType == TRADETYPE.SELL ?
                                        (long?)((dAveragePrice - current.CurrentPrice) / CurItemSymbol.OverTick * CurItemSymbol.ValueTick * CurItemSymbol.Exchange * orderInfo.OrderQty) :
                                        (long?)((current.CurrentPrice - dAveragePrice) / CurItemSymbol.OverTick * CurItemSymbol.ValueTick * CurItemSymbol.Exchange * orderInfo.OrderQty);

                                    if (orderInfo.TradeType == TRADETYPE.SELL)  //매도
                                    {
                                        if( current.CurrentPrice < orderInfo.MaxAveragePrice)
                                        {
                                            orderInfo.MaxAveragePrice = current.CurrentPrice;
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
                            ValuationList[0].Valuation = (long)lValSum;
                            ValuationList[0].TotalValuation = (long)lValSum;
                            if (dAveragePriceSum > 0)
                                ValuationList[0].AverageUnitPrice = dAveragePriceSum / nOrderCnt;
                            ValuationList[0].TotalProfit = DayProfitLoss.TotalProfit;
                            ValuationList[0].CurrentProfit = DayProfitLoss.TotalProfit + (long)lValSum;

                        }
                        else if (nOrderCnt < 1)
                        {
                            ValuationList[0].Valuation = 0;
                            ValuationList[0].TotalValuation = 0;
                            ValuationList[0].AverageUnitPrice = 0;
                        }
                    }
                }
                

            }
            catch (Exception)
            {

            }

            SetCurrentInfo(current);
            SetItemPriceInfo(current);
            SetHiLowPrice(current);

            OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.CURRENT);
        }


        private void ParseOrderlistMsg(string msg)
        {

            //TA40400012test001                       00020134test001   405102911-01HSIV21  1   0000124208.0    항셍(21-10/HKD)               24183     00000000000000001893620000000000006052000000
            //test001   405102911-01NQZ21   2   0000114824.250  나스닥(21-12/USD)             14775.50  00000000000000-11500120000000000003706062500
            try
            {
                int iCurPos = 0;
                //                 int nOrderLen = 134;
                //                 int nOrderCnt = int.Parse(msg.Substring(iCurPos, PACKET_BODYLEN_4));
                lock (OrderList)
                {
                    iCurPos += PACKET_BODYLEN_4 + PACKET_BODYLEN_4;
                    this.OrderList.RemoveAll(o => o.OrderType == "체결");
                    msg = msg.Substring(iCurPos);
                    if (msg.IndexOf(ItemSymbol) >= 0)
                    {

                        string strExp = @".{10}[^\-]+\-.{2}(\w+)\s*(\d+)\s*(.{5})(.{11})(.{10}).{44}([^\(]+\([^\)]+\))\s*@";

                        Regex regex = new Regex(strExp);

                        MatchCollection mc = regex.Matches(msg);
                        string sSymbol = "";
                        TRADETYPE tradeType = TRADETYPE.NONE;
                        int nQty = 0;
                        string sAveragePrice = "", sCurrentPrice = "";
                        double dAveragePrice = 0, dCurrentPrice = 0;
                        foreach (Match m in mc)
                        {
                            if (m.Groups.Count == 7)
                            {
                                sSymbol = m.Groups[1].Value.Trim();
                                tradeType = m.Groups[2].Value.Trim() == "1" ? TRADETYPE.SELL : TRADETYPE.BUY;
                                nQty = 0;
                                if (!int.TryParse(m.Groups[3].Value, out nQty))
                                {
                                    continue;
                                }
                                sAveragePrice = m.Groups[4].Value.Trim();
                                sCurrentPrice = m.Groups[5].Value.Trim();
                                if (sSymbol != CurItemSymbol.Symbol)
                                    continue;
                                dAveragePrice = double.Parse(sAveragePrice);
                                dCurrentPrice = double.Parse(sCurrentPrice);
                                long lValuation = (long)((dAveragePrice - dCurrentPrice) / CurItemSymbol.OverTick * CurItemSymbol.ValueTick * CurItemSymbol.Exchange * nQty);
                                if (tradeType == TRADETYPE.BUY)
                                {
                                    lValuation = 0 - lValuation;
                                }
                                OrderInfo orderInfo = new OrderInfo
                                {
                                    OrderType = "체결",
                                    Symbol = sSymbol,
                                    Qty = string.Format("{0}[{1}]", tradeType == TRADETYPE.SELL ? "매도" : "매수", nQty),
                                    AveragePrice = string.Format(Settings.Default.PriceFormat, dAveragePrice),
                                    MaxAveragePrice = Math.Round(dAveragePrice, 6),
                                    CrossAveragePrice = 0,
                                    StartCciPrice = -10000,
                                    CurrentPrice = string.Format(Settings.Default.PriceFormat, dCurrentPrice),
                                    Valuation = lValuation,
                                    Action = "청산",
                                    TradeType = tradeType,
                                    OrderQty = nQty,
                                    OrderTime = Environment.TickCount,
                                    OrderNo = "0",

                                };
                                this.OrderList.Add(orderInfo);

                            }
                        }

                    }
                }
                SetUnliquidationPosition(OrderList);
                OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.ORDER);
                

            }
            catch (Exception)
            {

            }

        }


        private void ParseReceiptlistMsg(string msg)
        {

            //TA40500021test003   086204217-01        00010082192203  0009670704NQZ21   10000114920.00 나스닥(21-12/USD)             00000000000
            
            try
            {
                int iCurPos = 0;

                lock (OrderList)
                {
                    this.OrderList.RemoveAll(o => o.OrderType == "미체결");

                    iCurPos += PACKET_BODYLEN_4 + PACKET_BODYLEN_4;
                    msg = msg.Substring(iCurPos);
                    if (msg.IndexOf(ItemSymbol) >= 0)
                    {

                        string strExp = @".{8}(.{10})(\w+)\s*(.{1})(.{5})(.{9}).{11}([^\(]+\([^\)]+\))\s*@";

                        Regex regex = new Regex(strExp);

                        MatchCollection mc = regex.Matches(msg);
                        string sSymbol = "";
                        long lOrderNo = 0;
                        TRADETYPE tradeType = TRADETYPE.NONE;
                        int nQty = 0;
                        string sAveragePrice = "";
                        double dAveragePrice = 0;
                        foreach (Match m in mc)
                        {
                            if (m.Groups.Count == 7)
                            {
                                lOrderNo = long.Parse(m.Groups[1].Value.Trim());
                                sSymbol = m.Groups[2].Value.Trim();
                                tradeType = m.Groups[3].Value.Trim() == "1" ? TRADETYPE.SELL : TRADETYPE.BUY;
                                nQty = int.Parse(m.Groups[4].Value);
                                sAveragePrice = m.Groups[5].Value;
                                dAveragePrice = double.Parse(sAveragePrice);

                                if (sSymbol != CurItemSymbol.Symbol)
                                    continue;

                                OrderInfo orderInfo = new OrderInfo
                                {
                                    OrderType = "미체결",
                                    Symbol = sSymbol,
                                    Qty = string.Format("{0}[{1}]", tradeType == TRADETYPE.SELL ? "매도" : "매수", nQty),
                                    AveragePrice = string.Format(Settings.Default.PriceFormat, dAveragePrice),
                                    MaxAveragePrice = Math.Round(dAveragePrice, 6),
                                    CrossAveragePrice = 0,
                                    StartCciPrice = -10000,
                                    CurrentPrice = "0",
                                    Valuation = 0L,
                                    Action = "취소",
                                    TradeType = tradeType,
                                    OrderQty = nQty,
                                    OrderTime = Environment.TickCount,
                                    OrderNo = lOrderNo.ToString(),

                                };
                                this.OrderList.Add(orderInfo);

                            }

                        }
                    }

                }

                SetUnliquidationPosition(OrderList);
                OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.ORDER);
            }
            catch (Exception)
            {

            }

        }



        private void ParseOrderMsg(string msg)
        {
            
            //TA15300000test001   405102911-01        success/0068R101405102911-010000       ESZ210000600040100004425.750000000001/test001#9669888#4425.75#1#1#405102911-01#18:08:21#0#0.00#9669888
            try
            {
                int iCurPos = 0;
                string[] splitMsg = msg.Split('/');


                if (splitMsg.Length < 3)
                    return;

                if (splitMsg[0] != RECV_RESULT_SUCCESS)
                    return;

                iCurPos += PACKET_BODYLEN_8 + PACKET_BODYLEN_16;

                string sSymbol = splitMsg[1].Substring(iCurPos, PACKET_BODYLEN_12).Trim();
                if (sSymbol != CurItemSymbol.Symbol)
                    return;
                iCurPos += PACKET_BODYLEN_12 + PACKET_BODYLEN_1 + PACKET_BODYLEN_10;

                string sCurrentPrice = splitMsg[1].Substring(iCurPos, PACKET_BODYLEN_11);
                double dCurrentPrice = double.Parse(sCurrentPrice);
                iCurPos += PACKET_BODYLEN_11 + PACKET_BODYLEN_5;
                string sQty = splitMsg[1].Substring(iCurPos, PACKET_BODYLEN_5);
                int nQty = int.Parse(sQty);

                string[] splitData = splitMsg[2].Split('#');

                if (splitData.Length < 10)
                    return;

                if (splitData[0] != UserId)
                    return;

                string logMsg = "";
                long lOrderNo = long.Parse(splitData[1]);
                string sAveragePrice = splitData[2];
                double dAveragePrice = double.Parse(sAveragePrice);
                string sOrderOrLiquid = splitData[3];       //체결>0 청산-0
                                
                TRADETYPE tradeType = splitData[4] == "1" ? TRADETYPE.SELL : TRADETYPE.BUY;
                long lValuation = (long)((dAveragePrice - Current.CurrentPrice) / CurItemSymbol.OverTick * CurItemSymbol.ValueTick * CurItemSymbol.Exchange * nQty);
                if (tradeType == TRADETYPE.BUY)
                {
                    lValuation = 0 - lValuation;
                }

                lock (OrderList)
                {
                    if (int.Parse(sOrderOrLiquid) > 0)
                    {
                        
                        OrderInfo orderInfo = OrderList.FirstOrDefault<OrderInfo>(
                            (OrderInfo o) => o.TradeType == tradeType &&
                            double.Parse(o.AveragePrice) == dAveragePrice && o.OrderType == "미체결");

                        if (orderInfo != null)
                        {
                            this.OrderList.Remove(orderInfo);
                        }

                        orderInfo = OrderList.FirstOrDefault<OrderInfo>(
                            (OrderInfo o) => o.OrderType == "체결");

                        this.LiquidOrder = new OrderVal
                        {
                            OrderType = "체결",
                            Symbol = sSymbol,
                            AveragePrice = string.Format(Settings.Default.PriceFormat, dAveragePrice),
                            OrderTime = this.Current.Time/*DateTime.Now*/,
                            ResultState = tradeType == TRADETYPE.BUY ? RESULTSTATE.BUY : RESULTSTATE.SELL,
                            ConcState = CONCSTATE.CONCLUDE,
                            OrderQty = nQty,
                        };

                        if (orderInfo != null)
                        {
                            if (orderInfo.TradeType == tradeType)
                                nQty += orderInfo.OrderQty;
                            else if(nQty > orderInfo.OrderQty)
                            {
                                this.LiquidOrder.OrderType = "되돌림";
                                this.LiquidOrder.ConcState = CONCSTATE.RECONC;
                                this.LiquidOrder.ResultState = tradeType == TRADETYPE.BUY ? RESULTSTATE.BUY : RESULTSTATE.SELL;
                                nQty = nQty - orderInfo.OrderQty;
                            } else
                            {
                                this.LiquidOrder.OrderType = "청산";
                                this.LiquidOrder.ConcState = CONCSTATE.LIQUID;
                                this.LiquidOrder.ResultState = orderInfo.TradeType == TRADETYPE.BUY ? RESULTSTATE.BUY : RESULTSTATE.SELL;
                                tradeType = orderInfo.TradeType;
                                nQty = orderInfo.OrderQty - nQty ;
                            }
                            this.OrderList.Remove(orderInfo);
                            
                        }

                        orderInfo = new OrderInfo
                        {
                            OrderType = "체결",
                            Symbol = sSymbol,
                            Qty = string.Format("{0}[{1}]", tradeType == TRADETYPE.SELL ? "매도" : "매수", nQty),
                            AveragePrice = string.Format(Settings.Default.PriceFormat, dAveragePrice),
                            MaxAveragePrice = Math.Round(dAveragePrice, 6),
                            CrossAveragePrice = 0,
                            StartCciPrice = -10000,
                            CurrentPrice = Current.CurrentPrice.ToString(),
                            Valuation = lValuation,
                            Action = "청산",
                            TradeType = tradeType,
                            OrderQty = nQty,
                            OrderTime = Environment.TickCount,
                            OrderNo = lOrderNo.ToString(),

                        };
                        this.OrderList.Add(orderInfo);

                        logMsg += "체결가:" + orderInfo.AveragePrice.ToString();
                    } else
                    {
                        OrderInfo orderInfo = OrderList.FirstOrDefault<OrderInfo>(
                            (OrderInfo o) => o.TradeType == tradeType && 
                            double.Parse(o.AveragePrice) == dAveragePrice &&  o.OrderType == "체결");

                        logMsg += "청산가:" + string.Format(Settings.Default.PriceFormat, dCurrentPrice).ToString();
                        
                        if (orderInfo != null)
                        {
                            this.LiquidOrder = new OrderVal
                            {
                                OrderType = "청산",
                                Symbol = orderInfo.Symbol,
                                AveragePrice = string.Format(Settings.Default.PriceFormat, dCurrentPrice).ToString(),
                                OrderTime = this.Current.Time/*DateTime.Now*/,
                                ResultState = orderInfo.TradeType == TRADETYPE.BUY ? RESULTSTATE.BUY: RESULTSTATE.SELL,
                                ConcState = CONCSTATE.LIQUID,
                                OrderQty = orderInfo.OrderQty,
                            };
                            OrderList.Remove(orderInfo);


                            long? dValuation = orderInfo.TradeType == TRADETYPE.SELL ?
                                            (long?)((dAveragePrice - dCurrentPrice) / CurItemSymbol.OverTick * CurItemSymbol.ValueTick * CurItemSymbol.Exchange * orderInfo.OrderQty) :
                                            (long?)((dCurrentPrice - dAveragePrice) / CurItemSymbol.OverTick * CurItemSymbol.ValueTick * CurItemSymbol.Exchange * orderInfo.OrderQty);
                            dValuation -= (long?)(FEE_RATE * 2 * CurItemSymbol.Exchange * orderInfo.OrderQty);

                            logMsg += "(" + (dValuation > 0 ? "수익:" : "손실:") + string.Format("{0:N0}원)", (long)dValuation);
                        }
                    }
                }
                _ordered = true;

                SetUnliquidationPosition(OrderList);
                OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.LIQUID);
                OnFutureSiteLogEvent(logMsg);
            }
            catch (Exception)
            {
                _ordered = true;
            }

        }


        private void ParseReceiptMsg(string msg)
        {
            //success/0088T101405102911-01000000006000131       CLX2100000075.740000000002                 New/test001#9670264#111937#0#405102911-01#0
            try
            {
                int iCurPos = 0;
                string[] splitMsg = msg.Split('/');
                
                if (splitMsg.Length < 3)
                    return;

                if (splitMsg[0] != RECV_RESULT_SUCCESS)
                    return;

                iCurPos += PACKET_BODYLEN_8 + PACKET_BODYLEN_16 + 1 + PACKET_BODYLEN_10;

                string sSymbol = splitMsg[1].Substring(iCurPos, PACKET_BODYLEN_12).Trim();
                if (sSymbol != CurItemSymbol.Symbol)
                    return;

                string sAveragePrice = splitMsg[1].Substring(iCurPos += PACKET_BODYLEN_12, PACKET_BODYLEN_16);
                string sQty = splitMsg[1].Substring(iCurPos += PACKET_BODYLEN_16, PACKET_BODYLEN_5);
                string sNew = splitMsg[1].Substring(iCurPos += PACKET_BODYLEN_5).Trim();
                double dAveragePrice = double.Parse(sAveragePrice);
                if (sNew != "New")
                    return;

                
                if (Math.Abs (Environment.TickCount - _orderTick) > 500)
                    return;
                TRADETYPE tradeType = _orderTrade;

                if (tradeType == TRADETYPE.NONE)
                    return;

                string[] splitData = splitMsg[2].Split('#');

                if (splitData.Length < 6)
                    return;

                if (splitData[0] != UserId)
                    return;
                int nQty = int.Parse(sQty);

                long lOrderNo = long.Parse(splitData[1]);
   
                OrderInfo orderInfo = new OrderInfo
                {
                    OrderType = "미체결",
                    Symbol = sSymbol,
                    Qty = string.Format("{0}[{1}]", tradeType == TRADETYPE.SELL ? "매도" : "매수", nQty),
                    AveragePrice = string.Format(Settings.Default.PriceFormat, dAveragePrice),
                    CurrentPrice = Current.CurrentPrice.ToString(),
                    Valuation = 0L,
                    Action = "취소",
                    TradeType = tradeType,
                    OrderQty = nQty,
                    OrderTime = Environment.TickCount,
                    OrderNo = lOrderNo.ToString(),
                        
                };
                this.OrderList.Add(orderInfo);

                
                SetUnliquidationPosition(OrderList);
                OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.ORDER);
                
            }
            catch (Exception)
            {

            }
            
        }

        private void ParseCancelMsg(string msg)
        {
            try
            {
                int iCurPos = 0;
                string[] splitMsg = msg.Split('/');

                if (splitMsg.Length < 3)
                    return;

                if (splitMsg[0] != RECV_RESULT_SUCCESS)
                    return;

                iCurPos += PACKET_BODYLEN_8 + PACKET_BODYLEN_16 + 2;

                string sSymbol = splitMsg[1].Substring(iCurPos, PACKET_BODYLEN_12).Trim();
                if (sSymbol != CurItemSymbol.Symbol)
                    return;

                string[] splitData = splitMsg[2].Split('#');

                if (splitData.Length < 4)
                    return;

                if (splitData[0] != UserId)
                    return;

                //_ConnSock.RequestReceiptlist();
                long lOrderNo = long.Parse(splitData[2]);

                OrderInfo orderInfo = OrderList.FirstOrDefault<OrderInfo>(
                    (OrderInfo o) => long.Parse(o.OrderNo) == lOrderNo && o.OrderType == "미체결");

                if (orderInfo != null)
                {
                    OrderList.Remove(orderInfo);

                    SetUnliquidationPosition(OrderList);
                    OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.ORDER);
                }
                
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// ////////////////////
        /// </summary>
        /// <param name="quote"></param>
        private void SetQuoteInfo(Quote quote)
        {
            if (this.QuoteList == null)
                return;

            if (this.QuoteList.Any<QuoteInfo>())
            {
                if (this._oldAskInfo != null)
                {
                    foreach (QuoteInfo quoteInfo1 in this._oldAskInfo)
                    {
                        quoteInfo1.AskCount = null;
                        quoteInfo1.AskQty = null;
                        quoteInfo1.BidQty = null;
                    }
                }
                if (this._oldBidInfo != null)
                {
                    foreach (QuoteInfo quoteInfo2 in this._oldBidInfo)
                    {
                        quoteInfo2.BidCount = null;
                        quoteInfo2.BidQty = null;
                        quoteInfo2.AskQty = null;
                    }
                }

                List<QuoteInfo> askList = new List<QuoteInfo>();
                List<QuoteInfo> bidList = new List<QuoteInfo>();

                double[,] priceList =
                {
                    {
                        quote.Ask1, quote.Ask2, quote.Ask3, quote.Ask4, quote.Ask5,
                        quote.Ask6, quote.Ask7, quote.Ask8, quote.Ask9, quote.Ask10
                    },
                    {
                        quote.Bid1, quote.Bid2, quote.Bid3, quote.Bid4, quote.Bid5,
                        quote.Bid6, quote.Bid7, quote.Bid8, quote.Bid9, quote.Bid10
                    },
                };

                int[,] qtyList =
                {
                    {
                        quote.AskQty1, quote.AskQty2, quote.AskQty3, quote.AskQty4, quote.AskQty5,
                        quote.AskQty6, quote.AskQty7, quote.AskQty8, quote.AskQty9, quote.AskQty10
                    },
                    {
                        quote.BidQty1, quote.BidQty2, quote.BidQty3, quote.BidQty4, quote.BidQty5,
                        quote.BidQty6, quote.BidQty7, quote.BidQty8, quote.BidQty9, quote.BidQty10
                    }
                };

                int[,] countList =
                {
                    {
                        quote.AskCount1, quote.AskCount2, quote.AskCount3, quote.AskCount4, quote.AskCount5,
                        quote.AskCount6, quote.AskCount7, quote.AskCount8, quote.AskCount9, quote.AskCount10
                    },
                    {
                        quote.BidCount1, quote.BidCount2, quote.BidCount3, quote.BidCount4, quote.BidCount5,
                        quote.BidCount6, quote.BidCount7, quote.BidCount8, quote.BidCount9, quote.BidCount10
                    }
                };

                QuoteInfo quoteInfo;
                for (int i = 0; i < priceList.GetLength(0); i++)
                {
                    for (int j = 0; j < priceList.GetLength(1); j++)
                    {
                        quoteInfo = this.FindQuoteInfo(this.QuoteList, priceList[i, j]);
                        if (i == 0)
                        {
                            quoteInfo.AskQty = new int?(qtyList[i, j]);
                            quoteInfo.AskCount = new int?(countList[i, j]);
                            askList.Add(quoteInfo);
                        }
                        else
                        {
                            quoteInfo.BidQty = new int?(qtyList[i, j]);
                            quoteInfo.BidCount = new int?(countList[i, j]);
                            bidList.Add(quoteInfo);
                        }
                    }
                }
                this._oldAskInfo = askList;
                this._oldBidInfo = bidList;
                this.Ask1Row = askList[0];
                this.Bid1Row = bidList[0];

                if(Current != null)
                    this.CurrentPriceRow = this.FindQuoteInfo(this.QuoteList, Current.CurrentPrice);
            }
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

            this.Current = new CurrentInfo
            {
                Time = current.ReceivedDate,
                CurrentPrice = current.CurrentPrice,
                CurrentPriceStr = strPrice,
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
                for (int i = 0; i < order.OrderQty; i++)
                    list.Add(this.FindQuoteInfo(this.QuoteList, Double.Parse(order.AveragePrice)));                
            }
            if (list.Any<QuoteInfo>())
            {
                this.PositionRow = this.QuoteList[Convert.ToInt32(list.Average((QuoteInfo q) => q.QuoteInfoId))];
                
                this.PositionTradeType = new TRADETYPE?(
                    (from o in orders select o.TradeType).FirstOrDefault<TRADETYPE>()
                );
            }
            else
            {
                this.PositionRow = null;
                this.PositionTradeType = null;
            }
        }


    }
}
