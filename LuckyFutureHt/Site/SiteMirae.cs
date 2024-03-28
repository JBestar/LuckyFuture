// #define WRITE_LOG

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


    class SiteMirae : FutureSite
    {

        public override SITETYPE Type { get; set; }

        LoginSocket _LoginSock = null;
        TraderSocket _TraderSock = null;

        LOGINSTATE _ConnectState = LOGINSTATE.NONE;
        LOGINSTATE _KeepAlive = LOGINSTATE.NONE;

        string _IpAddr = "";
        const int _LoginPort = 56011;
        const int _TraderPort = 50601;

        public const string RECV_RESULT_SUCCESS = "success";
        public const string RECV_RESULT_NO_ID = "NO_MBR_ID";
        public const string RECV_RESULT_NO_PWD = "NO_PWD";
        public const string RECV_RESULT_DUP_ID = "DUP_ID";

        public const string PRESENT_HEAD_M6 = "M6";   //해외선물 호가
        public const string PRESENT_HEAD_M3 = "M3";   //해외선물 체결정보

        public const int PRESENT_SIZE_LEN = 5;
        public const int PRESENT_HEAD_LEN = 2;
        public const int PRESENT_SYMBOL_LEN = 32;

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
        public const int PACKET_BODYLEN_14 = 14;
        public const int PACKET_BODYLEN_15 = 15;
        public const int PACKET_BODYLEN_16 = 16;
        public const int PACKET_BODYLEN_20 = 20;
        public const int PACKET_BODYLEN_22 = 22;
        public const int PACKET_BODYLEN_30 = 30;

        bool _ordered = false;
        private double FEE_RATE = 6.5;

        int _aliveTime = 0;
        int _orderTick = 0;
        // int _currencyTick = 0;

        Random _random = new Random();
        public SiteMirae(SITETYPE siteType)
        {
            Type = siteType;
//             if (siteType == SITETYPE.MIRAE)
//             {
//                 _IpAddr = "218.50.1.4";
//             }

#if WRITE_LOG
            CreateLogFile();
#endif
        }
        string _logPath = "";

        private void CreateLogFile()
        {
            _logPath = "D://BinHts/Mirae_" + DateTime.Now.ToString("yyyyMMdd");
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
            _LoginSock.ConnectSocket();
            Thread.Sleep(500);
            _ConnectState = LOGINSTATE.NONE;
            _LoginSock.RequestDialogLogin(id, password);
            Thread.Sleep(3000);

            //_ConnectState = LOGINSTATE.OK;
            if (_ConnectState == LOGINSTATE.OK)
            {
                _KeepAlive = LOGINSTATE.OK;
                login_result = ERRORCODE.SUCCESS;
            }
            else if (_ConnectState == LOGINSTATE.NO_ID)
            {
                login_result = ERRORCODE.LOGIN_NO_ID;
            }
            else if (_ConnectState == LOGINSTATE.NO_PWD)
            {
                login_result = ERRORCODE.LOGIN_NO_PWD;
            }
            else if (_ConnectState == LOGINSTATE.NONE)
            {
                login_result = ERRORCODE.LOGIN_NO_CON;
            }

            _LoginSock.CloseSocket();
            if (_ConnectState == LOGINSTATE.OK)
            {
                Thread.Sleep(500);
                OnLogin();
            }
            return login_result;
        }

        protected override ERRORCODE LogOut()
        {
            _TraderSock.RequestLogout();
            Thread.Sleep(200);
            _TraderSock.RequestLogoutEnd();
            Thread.Sleep(200);
            _TraderSock.CloseSocket();
            return ERRORCODE.SUCCESS;
        }

        protected override ERRORCODE Prepare()
        {

            if (CurrentUserAccount == null || CurrentUserAccount.UserAccountStr == null)
                return ERRORCODE.UNKNOWN_FAILED;

            if (ItemList.Count < 1)
                return ERRORCODE.UNKNOWN_FAILED;

            if (CurItemSymbol != null)
            {

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
                }
                this.QuoteList = CreateQuoteInfo();
                //_TraderSock.RequestCM001A();
                //Thread.Sleep(200);
                _TraderSock.RequestCurrency(CurItemSymbol.Symbol, CurItemSymbol.index);
                Thread.Sleep(400);
                _TraderSock.RequestControlTick(CurItemSymbol.Symbol, true);
                _TraderSock.RequestControlQuote(CurItemSymbol.Symbol, true);

                Thread.Sleep(500);
            }

            Prepared = true;

            Thread.Sleep(500);

            return ERRORCODE.SUCCESS;
        }



        protected override ERRORCODE Check()
        {
            if (_TraderSock.CheckConnection())
            {
                //종목변경
                if (CurItemSymbol != null && this.ItemSymbol != CurItemSymbol.Symbol)
                {
                    _TraderSock.RequestControlTick(ItemSymbol, false);
                    _TraderSock.RequestControlQuote(ItemSymbol, false);
                    return ERRORCODE.PREPARE_FAILED;
                }
                else
                {
                    if (_KeepAlive != LOGINSTATE.OK)
                    {
                        LogOut();
                        return ERRORCODE.UNKNOWN_FAILED;
                    }
                    else if (_ordered)
                    {
                        _ordered = false;
                        Thread.Sleep(500);
                        _TraderSock.RequestAccount();
                        Thread.Sleep(500);
                        _TraderSock.RequestReceiptlist();
                        Thread.Sleep(500);
                        _TraderSock.RequestOrderlist();

                    }
                    else if (Math.Abs(Environment.TickCount - _aliveTime) > 10000)
                    {
                        //_KeepAlive = LOGINSTATE.NONE;
                        _aliveTime = Environment.TickCount;
                        _TraderSock.RequestKeepAlive();

                    }
//                     else if (_currencyTick != 0 && Math.Abs(Environment.TickCount - _currencyTick) > 120000)
//                     {
//                         LogOut();
//                         return ERRORCODE.UNKNOWN_FAILED;
//                     }

                    return ERRORCODE.SUCCESS;

                }
            }

            return ERRORCODE.UNKNOWN_FAILED;
        }



        protected override void OnStarted()
        {
            base.OnStarted();

            _LoginSock = new LoginSocket(_IpAddr, _LoginPort);
            _TraderSock = new TraderSocket(_IpAddr, _TraderPort);

            _LoginSock.DataReceiveEvent += OnSiteLoginReceive;
            _TraderSock.DataReceiveEvent += OnSiteTraderReceive;
        }

        protected override void OnStopped(bool bAutoStop)
        {
            _LoginSock.DataReceiveEvent -= OnSiteLoginReceive;
            _TraderSock.DataReceiveEvent -= OnSiteTraderReceive;

            base.OnStopped(bAutoStop);
        }

        protected override void OnLogin()
        {
            base.OnLogin();

            Thread.Sleep(200);
            _ConnectState = LOGINSTATE.NONE;
            _TraderSock.ConnectSocket();
            Thread.Sleep(200);
            _TraderSock.RequestLogin(UserId);
            int delayMs = 500;

            this.UserAccounts = new List<UserAccountInfo>();
            this.CurrentUserAccount = new UserAccountInfo();
            DayProfitLoss = new DayProfitLossInfo();

            Thread.Sleep(500);
            _TraderSock.RequestTr0010();
            // _currencyTick = 0;
            StartPrice = 0;
            Thread.Sleep(300);
            _TraderSock.RequestItemlist(); //HEAD_BQ0006
            _TraderSock.RequestCommon(TraderSocket.HEAD_BQ0009);
            _TraderSock.RequestBq0005();
            _TraderSock.RequestCommon(TraderSocket.HEAD_BQ0008);
            _TraderSock.RequestAccount(); //
            _TraderSock.RequestCq3001();

            Thread.Sleep(delayMs);

            Thread.Sleep(500);

        }

        protected override void OnPrepare()
        {
            base.OnPrepare();

            Settings.Default.ServerTimeDelay = 0;

            Current = null;
            CurrentList.Clear();
            OrderList.Clear();
            _TraderSock.RequestReceiptlist(); //CQ2001
            Thread.Sleep(200);
            _TraderSock.RequestOrderlist(); //CQ4001
            Thread.Sleep(200);
            _TraderSock.RequestCommon(TraderSocket.HEAD_CQ3003);
            Thread.Sleep(500);
            _TraderSock.RequestTr0030(true);

            //Prepared = true ;

        }
        public override bool RequestRChart()
        {
            return false;
        }
        public override bool RequestDChart(bool bDChart = true)
        {
            return false;
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

        private void OnSiteLoginReceive(object sender, SocketEventArgs e)
        {
            List<string> packetList;
            lock (_LoginSock.ResponseList)
            {
                if (_LoginSock.ResponseList.Count < 1)
                    return;

                packetList = new List<string>(_LoginSock.ResponseList);
                _LoginSock.ResponseList.Clear();
            }
            string headRecv = "";
            foreach (string recvMsg in packetList)
            {
                if (recvMsg.Length < 0x26)
                    continue;

                headRecv = recvMsg.Substring(29, 9).Trim();

                WriteLog(string.Format("Receive Header = {0}", headRecv));

                if (headRecv == LoginSocket.PACK_AT0001)
                {
                    _LoginSock.RequestAt002();
                }
                else if (headRecv == LoginSocket.PACK_AT0002)
                {
                    string loginResult = recvMsg.Substring(38, 6).Trim();
                    WriteLog(string.Format("Receive loginResult = {0}", loginResult));

                    if (loginResult == "U20003")
                        _ConnectState = LOGINSTATE.NO_PWD;
                    else if (loginResult == "U20002")
                        _ConnectState = LOGINSTATE.NO_ID;
                    else if (loginResult.Substring(0, 5) == "00000")
                    {
                        _ConnectState = LOGINSTATE.OK;
                        _LoginSock.RequestAq003();
                    }
                }
                else if (headRecv == LoginSocket.PACK_AQ0003)
                {
                    _LoginSock.RequestAt006();
                }
                else if (headRecv == LoginSocket.PACK_AT0006)
                {

                }
            }
        }

        private void OnSiteTraderReceive(object sender, SocketEventArgs e)
        {

            List<byte[]> packetList;
            lock (_TraderSock._ResponsePkList)
            {
                if (_TraderSock._ResponsePkList.Count < 1)
                    return;

                packetList = new List<byte[]>(_TraderSock._ResponsePkList);
                _TraderSock._ResponsePkList.Clear();
            }

            string bodyId = "";
            int nCurPos = 0;
            foreach (byte[] recvBytes in packetList)
            {
                nCurPos = TraderSocket.HEAD_INFO_LENGTH;
                bodyId = _TraderSock.getBodyId(recvBytes);
                if (bodyId.Length < 1)
                    continue;

                if (recvBytes[nCurPos + 0x34] != 'O' && recvBytes[nCurPos + 0x34] != 'Q')
                {
                    if (bodyId == TraderSocket.BODY_ID_TICK)
                    {
                        ParseCurrentMsg(recvBytes);
                    }
                    else if (bodyId == TraderSocket.BODY_ID_QUOT)
                    {
                        ParseQuoteMsg(recvBytes);
                    }
                    else if (bodyId == TraderSocket.BODY_ID_ORDR)
                    {
                        ParseReceiptMsg(recvBytes);
                    }
                    else if (bodyId == TraderSocket.BODY_ID_NTRD)
                    {

                    }
                }
                else
                {
                    if (bodyId == TraderSocket.HEAD_CQ3002) //Account
                    {
                        ParseAccountMsg(recvBytes);
                    }
                    else if (bodyId == TraderSocket.HEAD_BQ0006) //ItemList
                    {
                        ParseItemlistMsg(recvBytes);
                    }
                    else if (bodyId == TraderSocket.HEAD_TR0099) //KeepAlive
                    {
                        _KeepAlive = LOGINSTATE.OK;
                    }
                    else if (bodyId == TraderSocket.HEAD_CQ2001) //Receipt List
                    {
                        //_TraderSock.WriteBytes(Extension.ByteArrayToString(recvBytes, recvBytes.Length), false, TraderSocket.HEAD_CQ2001);
                        ParseReceiptlistMsg(recvBytes);
                    }
                    else if (bodyId == TraderSocket.HEAD_CQ4001) //Ordered List
                    {
                        //_TraderSock.WriteBytes(Extension.ByteArrayToString(recvBytes, recvBytes.Length), false, TraderSocket.HEAD_CQ4001);
                        ParseOrderlistMsg(recvBytes);
                    }
                    else if (bodyId == TraderSocket.HEAD_CMO101)
                    {
                        _TraderSock.WriteBytes(Extension.ByteArrayToString(recvBytes, recvBytes.Length), false, TraderSocket.HEAD_CQ4001);
                    }

                }

                /*
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
               
                */
            }

        }

        protected void ParseQuoteMsg(byte[] recvBytes)
        {

            string sPacket = Encoding.ASCII.GetString(recvBytes);

            if (sPacket.Length < 0x54D - 100)
                return;
            //0000001347RDPKT      2030400007          00000                                                      
            // 0000001247N R_QUOTR  000000
            // 20231218235847000000
            // NQH24
            // 0000000002
            // 0000000000
            // 000000000000035
            // 000000000000032
            // 000000000000033
            // 000000000000033
            // 000000016873.00
            // 000000016873.25000000000000004000000000000004
            // 000000016873.00000000000000001000000000000001
            // 000000016873.50000000000000009000000000000009
            // 000000016872.75000000000000009000000000000008
            // 000000016873.75000000000000009000000000000008
            // 000000016872.50000000000000009000000000000009
            // 000000016874.00000000000000006000000000000005
            // 000000016872.25000000000000006000000000000006
            // 000000016874.25000000000000007000000000000007
            // 000000016872.00000000000000008000000000000008
            // 000000000000.00000000000000000000000000000000
            // 000000000000.00000000000000000000000000000000
            // 000000000000.00000000000000000000000000000000
            // 000000000000.00000000000000000000000000000000
            // 000000000000.00000000000000000000000000000000
            // 000000000000.00000000000000000000000000000000
            // 000000000000.00000000000000000000000000000000
            // 000000000000.00000000000000000000000000000000
            // 000000000000.00000000000000000000000000000000
            // 000000000000.00000000000000000000000000000000



            int iCurPos = TraderSocket.HEAD_INFO_LENGTH + TraderSocket.BODY_INFO_LENGTH;
            string sBody = sPacket.Substring(iCurPos);
            Quote quote = new Quote();

            try
            {

                string strExp = @"(.{20})(.{32}).{20}(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})(.{15})";
                Regex regex = new Regex(strExp);

                MatchCollection mc = regex.Matches(sBody);

                foreach (Match m in mc)
                {

                    quote.QuoteTime = m.Groups[1].Value.Substring(0, PACKET_BODYLEN_14); //20231221175339000000

                    quote.Symbol = m.Groups[2].Value.Trim();
                    if (CurItemSymbol == null || quote.Symbol != ItemSymbol)
                        return;

                    quote.TotalBidQty = int.Parse(m.Groups[3].Value.Trim());
                    quote.TotalAskQty = int.Parse(m.Groups[4].Value.Trim());
                    quote.TotalBidCount = int.Parse(m.Groups[5].Value.Trim());
                    quote.TotalAskCount = int.Parse(m.Groups[6].Value.Trim());


                    quote.Ask1 = double.Parse(m.Groups[8].Value.Trim());
                    quote.AskQty1 = int.Parse(m.Groups[9].Value.Trim());
                    quote.AskCount1 = int.Parse(m.Groups[10].Value.Trim());

                    quote.Bid1 = double.Parse(m.Groups[11].Value.Trim());
                    quote.BidQty1 = int.Parse(m.Groups[12].Value.Trim());
                    quote.BidCount1 = int.Parse(m.Groups[13].Value.Trim());

                    quote.Ask2 = double.Parse(m.Groups[14].Value.Trim());
                    quote.AskQty2 = int.Parse(m.Groups[15].Value.Trim());
                    quote.AskCount2 = int.Parse(m.Groups[16].Value.Trim());

                    quote.Bid2 = double.Parse(m.Groups[17].Value.Trim());
                    quote.BidQty2 = int.Parse(m.Groups[18].Value.Trim());
                    quote.BidCount2 = int.Parse(m.Groups[19].Value.Trim());

                    quote.Ask3 = double.Parse(m.Groups[20].Value.Trim());
                    quote.AskQty3 = int.Parse(m.Groups[21].Value.Trim());
                    quote.AskCount3 = int.Parse(m.Groups[22].Value.Trim());

                    quote.Bid3 = double.Parse(m.Groups[23].Value.Trim());
                    quote.BidQty3 = int.Parse(m.Groups[24].Value.Trim());
                    quote.BidCount3 = int.Parse(m.Groups[25].Value.Trim());

                    quote.Ask4 = double.Parse(m.Groups[26].Value.Trim());
                    quote.AskQty4 = int.Parse(m.Groups[27].Value.Trim());
                    quote.AskCount4 = int.Parse(m.Groups[28].Value.Trim());

                    quote.Bid4 = double.Parse(m.Groups[29].Value.Trim());
                    quote.BidQty4 = int.Parse(m.Groups[30].Value.Trim());
                    quote.BidCount4 = int.Parse(m.Groups[31].Value.Trim());

                    quote.Ask5 = double.Parse(m.Groups[32].Value.Trim());
                    quote.AskQty5 = int.Parse(m.Groups[33].Value.Trim());
                    quote.AskCount5 = int.Parse(m.Groups[34].Value.Trim());

                    quote.Bid5 = double.Parse(m.Groups[35].Value.Trim());
                    quote.BidQty5 = int.Parse(m.Groups[36].Value.Trim());
                    quote.BidCount5 = int.Parse(m.Groups[37].Value.Trim());

                    quote.ReceivedDate = DateTime.ParseExact(quote.QuoteTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                    break;
                }

                //                 if (CurItemSymbol.MidPrice == 0)
                //                 {
                //                     CurItemSymbol.MidPrice = quote.Ask1;
                //                 }

                OnReceiveQuote(quote);
            }
            catch (Exception)
            {

            }

        }


        protected void ParseCurrentMsg(byte[] recvBytes)
        {

            string sPacket = Encoding.ASCII.GetString(recvBytes);

            if (sPacket.Length < 0x28E - 20)
                return;

            // R_TICK 28E
            // 
            // 0000000644RDPKT      1190400004          00000
            // 0000000544N R_TICKR  000000
            // 20231221165719000000
            // NQH24
            // 0016864.5000000000000000000001
            // 0016790.25000000016869.0000000
            // 0016790.25000000016864.5000000
            // 000000000039832                    000000098.2500000000000000000.5900000000020000000000


            int iCurPos = TraderSocket.HEAD_INFO_LENGTH + TraderSocket.BODY_INFO_LENGTH;
            string sBody = sPacket.Substring(iCurPos);

            Current current = new Current();
            int concCnt = 0;
            try
            {
                string strExp = @"(.{20})(.{32})(.{15})(.{15}).{45}(.{15})(.{15})(.{15}).{30}.{20}(.{12}).{12}(.{12})";
                Regex regex = new Regex(strExp);

                MatchCollection mc = regex.Matches(sBody);

                foreach (Match m in mc)
                {

                    iCurPos = 0;
                    current.CurrentTime = m.Groups[1].Value.Substring(0, PACKET_BODYLEN_14); //20231221175339000000

                    current.Symbol = m.Groups[2].Value.Trim();

                    if (CurItemSymbol == null || current.Symbol != ItemSymbol)
                        return;

                    current.CurrentPrice = double.Parse(m.Groups[3].Value.Trim());

                    concCnt = int.Parse(m.Groups[4].Value.Trim());
                    current.ConclusionVolume = Math.Abs(concCnt);

                    current.StartPrice = double.Parse(m.Groups[5].Value.Trim());

                    current.HighPrice = double.Parse(m.Groups[6].Value.Trim());

                    current.LowPrice = double.Parse(m.Groups[7].Value.Trim());


                    current.Contrast = double.Parse(m.Groups[8].Value.Trim());

                    current.ContrastPer = double.Parse(m.Groups[9].Value.Trim());

                    current.ReceivedDate = DateTime.ParseExact(current.CurrentTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);

                    current.TradeType = concCnt > 0 ? TradeType.Buy : TradeType.Sell;

                    //                 if (CurItemSymbol.MidPrice == 0)
                    //                 {
                    //                     CurItemSymbol.MidPrice = current.CurrentPrice;
                    //                 }
                    // WriteLog(string.Format("{0} | {1}", current.CurrentTime, current.CurrentPrice));
                }
                OnReceiveCurrent(current);
            }
            catch (Exception)
            {

            }
        }

        private void ParseAccountMsg(byte[] recvBytes)
        {

            int lenZip = _TraderSock.GetZipLen(recvBytes);
            int lenDat = _TraderSock.GetDatLen(recvBytes);
            byte[] zipBytes = new byte[lenZip];
            byte[] datBytes = new byte[lenDat];
            Buffer.BlockCopy(recvBytes, TraderSocket.HEAD_INFO_LENGTH + 0xD2, zipBytes, 0, lenZip);

            try
            {
                IntPtr[] ptrLenDat = new IntPtr[1] { (IntPtr)lenDat };
                int result = TraderSocket.ts_uncompress(zipBytes, lenZip, datBytes, ptrLenDat);
                if (result != 0)
                    return;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return;
            }
            try
            {
                string msg = Encoding.Default.GetString(datBytes);
                string strExp = @"(.{15}).{11}(.{15}).{150}.{300}(.{15})(.{15})";
                Regex regex = new Regex(strExp);

                MatchCollection mc = regex.Matches(msg);

                long nTemp = 0;

                long lBalance = 0, lProfit = 0, lLoss = 0, lValuation = 0;

                foreach (Match m in mc)
                {
                    _ConnectState = LOGINSTATE.OK;
                    CurrentUserAccount.UserAccountStr = m.Groups[1].Value.Trim();

                    string sProfit = m.Groups[3].Value.Trim();
                    string sBalance = m.Groups[4].Value.Trim();


                    if (long.TryParse(sProfit, out nTemp))
                    {
                        lProfit = nTemp;
                    }

                    if (long.TryParse(sBalance, out nTemp))
                    {
                        lBalance = nTemp;
                    }

                    this.UserAccounts.Clear();
                    this.UserAccounts.Add(this.CurrentUserAccount);

                    break;
                }

                if (this.CurrentUserAccount != null)
                {
                    _TraderSock.SetUserInfo(UserId, CurrentUserAccount.UserAccountStr);
                    DayProfitLoss.TotalProfit = lProfit - lLoss;

                    this.CurrentUserAccount.Balance = lBalance - lValuation;

                    this.ValuationList[0].TotalValuation = lValuation;
                    this.ValuationList[0].TotalProfit = DayProfitLoss.TotalProfit;
                    this.ValuationList[0].CurrentProfit = DayProfitLoss.TotalProfit;
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return;
            }
        }


        private void ParseItemlistMsg(byte[] recvBytes)
        {
            if (ItemList.Count > 0) return;


            int lenZip = _TraderSock.GetZipLen(recvBytes);
            int lenDat = _TraderSock.GetDatLen(recvBytes);
            byte[] zipBytes = new byte[lenZip];
            byte[] datBytes = new byte[lenDat];
            Buffer.BlockCopy(recvBytes, TraderSocket.HEAD_INFO_LENGTH + 0xD2, zipBytes, 0, lenZip);

            try
            {
                IntPtr[] ptrLenDat = new IntPtr[1] { (IntPtr)lenDat };
                int result = TraderSocket.ts_uncompress(zipBytes, lenZip, datBytes, ptrLenDat);
                if (result != 0)
                    return;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return;
            }
            try
            {
                List<string> itemList = _TraderSock.GetItemList(datBytes);
                string strExp = @"0{28}\d+\s*0{28}(\w+)\s+(.{50})\s+.\w+\s+\w+\s+\w+\s+.{50}\s+(\w+)\s+(.{30})\s+\w+\s+.{30}\s+\w+\s+\w+\s+(\d*\.{0,1}\d*)\s+(\d*\.{0,1}\d*)\s+(\d*\.{0,1}\d*)\s+(\d*\.{0,1}\d*)\s+(\d*\.{0,1}\d*)\s+(\d*\.{0,1}\d*)\s+(\d*\.{0,1}\d*)\s+(\d*\.{0,1}\d*)\s+\w+\s+\w+\s+\w+\s+\w+\s+\w+\s+(\d*\.{0,1}\d*)\s+";
                Regex regex = new Regex(strExp);

                ItemSymbolInfo newItem = null;
                ItemList.Clear();
                string strTemp = "";
                int nTemp = 0;
                double dTemp = 0;
                int index = 0;
                foreach (string msg in itemList)
                {
                    index++;

                    if (msg.IndexOf("CME") < 0 && msg.IndexOf("HKEX") < 0)
                        continue;
                    WriteLog(msg);
                    MatchCollection mc = regex.Matches(msg);
                    foreach (Match m in mc)
                    {
                        newItem = new ItemSymbolInfo();
                        newItem.Symbol = m.Groups[1].Value;                     //6AH24

                        if (newItem.Symbol.IndexOf("201") >= 0)
                            break;

                        newItem.index = index;
                        strTemp = m.Groups[2].Value.Trim();                   //호주 달러 2024.03
                        newItem.ItemName = strTemp;

                        strTemp = m.Groups[6].Value.Trim();                            //0.00005
                        if (double.TryParse(strTemp, out dTemp))
                        {
                            newItem.OverTick = dTemp;
                        }

                        strTemp = m.Groups[7].Value.Trim();                            //6.25
                        if (double.TryParse(strTemp, out dTemp))
                        {
                            newItem.ValueTick = dTemp;
                        }
                        strTemp = m.Groups[5].Value.Trim();                            //00000000000000001235
                        if (double.TryParse(strTemp, out dTemp))
                        {
                            newItem.Exchange = dTemp;
                        }
                        strTemp = m.Groups[10].Value.Trim();                            //00000000000000001235
                        if (int.TryParse(strTemp, out nTemp))
                        {
                            newItem.Precision = nTemp;
                        }

                        strTemp = m.Groups[13].Value.Trim();                            //6.25
                        if (double.TryParse(strTemp, out dTemp))
                        {
                            newItem.MidPrice = dTemp;
                        }

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
                        //                     OnFutureSiteLogEvent("ParseItemlistMsg() Item="+newItem.Symbol + " Index=" + index + " Precision="+newItem.Precision+
                        //                          " OverTick="+newItem.OverTick+" Exchange="+newItem.Exchange+" ValueTick="+newItem.ValueTick);
                        ItemList.Add(newItem);
                        break;
                    }
                }

                if (ItemSymbol.Length < 1 && ItemList.Count > 0)
                {
                    CurItemSymbol = ItemList.First();
                    ItemSymbol = CurItemSymbol.Symbol;
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return;
            }
            if (CurItemSymbol != null)
            {
                ItemPrecision = CurItemSymbol.Precision;
                Settings.Default.PriceFormat = Common.GetPriceFormat(ItemPrecision);
                OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.PREPAREITEM);
            }

        }


        public override bool DoSellOrder(QuoteInfo quoteInfo, int nQuantity = 1, bool bMarketPrice = false)
        {

            if (this.CurrentUserAccount == null)
            {
                OnFutureSiteLogEvent("계좌정보 오류!");
                return false;
            }
            if (nQuantity < 1 || nQuantity > 10)
            {
                OnFutureSiteLogEvent("주문수량 오류!");
                return false;
            }
            if (CurrentUserAccount.Balance < 300000 * nQuantity)
            {
                OnFutureSiteLogEvent("담보금 부족!");
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
                OnFutureSiteLogEvent("주문기간이 아닙니다.");
                return false;
            }

            if (quoteInfo != null)
            {
                double nOrderRange = 40 * CurItemSymbol.OverTick;
                if (quoteInfo.Price < Current.CurrentPrice - nOrderRange || quoteInfo.Price > Current.CurrentPrice + nOrderRange)
                {
                    OnFutureSiteLogEvent("주문 가격이 초과 됨");
                    return false;
                }
            }
            else bMarketPrice = true;

            if (_TraderSock.RequestOrder(CurrentUserAccount.UserAccountStr, TRADETYPE.SELL, CurItemSymbol.Symbol,
                bMarketPrice ? 0 : quoteInfo.Price, nQuantity, bMarketPrice, Current.CurrentPrice))
            {
                _orderTick = Environment.TickCount;

                OnFutureSiteLogEvent("매도주문이 접수되었습니다.");
                OnFutureSiteLogEvent("주문시가격:" + Current.CurrentPrice);
            }
            else
            {
                OnFutureSiteLogEvent("매도주문 실패!!!");
            }
            return true;
        }

        public override bool DoBuyOrder(QuoteInfo quoteInfo, int nQuantity = 1, bool bMarketPrice = false)
        {


            if (this.CurrentUserAccount == null)
            {
                OnFutureSiteLogEvent("계좌정보 오류!");
                return false;
            }

            if (nQuantity < 1 || nQuantity > 10)
            {
                OnFutureSiteLogEvent("주문수량 오류!");
                return false;
            }

            if (CurrentUserAccount.Balance < 300000 * nQuantity)
            {
                OnFutureSiteLogEvent("담보금 부족!");
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
                OnFutureSiteLogEvent("주문기간이 아닙니다.");
                return false;
            }

            if (quoteInfo != null)
            {
                double nOrderRange = 40 * CurItemSymbol.OverTick;
                if (quoteInfo.Price < Current.CurrentPrice - nOrderRange || quoteInfo.Price > Current.CurrentPrice + nOrderRange)
                {
                    OnFutureSiteLogEvent("주문 가격이 초과 됨");
                    return false;
                }
            }
            else bMarketPrice = true;

            if (_TraderSock.RequestOrder(CurrentUserAccount.UserAccountStr, TRADETYPE.BUY, CurItemSymbol.Symbol,
                bMarketPrice ? 0 : quoteInfo.Price, nQuantity, bMarketPrice, Current.CurrentPrice))
            {
                _orderTick = Environment.TickCount;

                OnFutureSiteLogEvent("매수주문이 접수되었습니다.");
                OnFutureSiteLogEvent("주문시가격:" + Current.CurrentPrice);

            }
            else
            {
                OnFutureSiteLogEvent("매수주문 실패!!!");
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

            if (_TraderSock.RequestCancel(CurrentUserAccount.UserAccountStr, orderInfo.TradeType, orderInfo.Symbol, orderInfo.AveragePrice, orderInfo.OrderQty, long.Parse(orderInfo.OrderNo)))
            {
                if (orderInfo.TradeType == TRADETYPE.SELL)
                    OnFutureSiteLogEvent("매도주문이 취소되었습니다.");
                else if (orderInfo.TradeType == TRADETYPE.BUY)
                    OnFutureSiteLogEvent("매수주문이 취소되었습니다.");
            }
            else
            {
                OnFutureSiteLogEvent("취소주문 실패!!!");
            }

            return true;
        }

        public override bool LiquidateOrder(OrderInfo orderInfo)
        {
            if (this.CurrentUserAccount == null)
                return false;

            if (_TraderSock.RequestOrder(CurrentUserAccount.UserAccountStr, orderInfo.TradeType == TRADETYPE.SELL ? TRADETYPE.BUY : TRADETYPE.SELL,
                orderInfo.Symbol, 0, orderInfo.OrderQty, true, 0))
            {
                if (orderInfo.TradeType == TRADETYPE.SELL)
                    OnFutureSiteLogEvent("매도주문이 청산되었습니다.");
                else if (orderInfo.TradeType == TRADETYPE.BUY)
                    OnFutureSiteLogEvent("매수주문이 청산되었습니다.");

                OnFutureSiteLogEvent("청산시가격:" + Current.CurrentPrice);

            }
            else
            {
                OnFutureSiteLogEvent("청산 실패!!!");
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
                                        if (current.CurrentPrice < orderInfo.MaxAveragePrice)
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


        private void ParseOrderlistMsg(byte[] recvBytes)
        {


            int lenZip = _TraderSock.GetZipLen(recvBytes);
            int lenDat = _TraderSock.GetDatLen(recvBytes);
            if (lenDat < 1)
                return;

            byte[] zipBytes = new byte[lenZip];
            byte[] datBytes = new byte[lenDat];
            Buffer.BlockCopy(recvBytes, TraderSocket.HEAD_INFO_LENGTH + 0xD2, zipBytes, 0, lenZip);

            try
            {
                IntPtr[] ptrLenDat = new IntPtr[1] { (IntPtr)lenDat };
                int result = TraderSocket.ts_uncompress(zipBytes, lenZip, datBytes, ptrLenDat);
                if (result != 0)
                    return;
            }
            catch (Exception ex)
            {
                _ordered = true;
                string err = ex.Message;
                return;
            }
            try
            {
                string msg = Encoding.ASCII.GetString(datBytes);

                lock (OrderList)
                {
                    OrderList.RemoveAll(o => o.OrderType == "체결");
                }

                string strExp = @".{15}.{1}(.{32})(.{20}).{421}(.{20}).{25}(.{20})(.{20}).{194}";

                Regex regex = new Regex(strExp);

                MatchCollection mc = regex.Matches(msg);
                string sSymbol = "";
                TRADETYPE tradeType = TRADETYPE.NONE;
                int nQty = 0;
                string sAveragePrice = "", sCurrentPrice = "";
                string sTrade = "";
                double dAveragePrice = 0, dCurrentPrice = 0;
                foreach (Match m in mc)
                {

                    sSymbol = m.Groups[1].Value.Trim();
                    if (sSymbol != CurItemSymbol.Symbol)
                        continue;

                    sTrade = m.Groups[2].Value.Trim();
                    tradeType = sTrade == "S" ? TRADETYPE.SELL : TRADETYPE.BUY;
                    nQty = 0;
                    if (!int.TryParse(m.Groups[3].Value, out nQty))
                    {
                        continue;
                    }
                    sAveragePrice = m.Groups[4].Value.Trim();
                    sCurrentPrice = m.Groups[5].Value.Trim();

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
                        CurrentPrice = string.Format(Settings.Default.PriceFormat, dCurrentPrice),
                        Valuation = lValuation,
                        Action = "청산",
                        TradeType = tradeType,
                        OrderQty = nQty,
                        OrderTime = Environment.TickCount,
                        OrderNo = "0",

                    };
                    OrderList.Add(orderInfo);
                }

                SetUnliquidationPosition(OrderList);
                OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.ORDER);

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

        }


        private void ParseReceiptlistMsg(byte[] recvBytes)
        {
            int lenZip = _TraderSock.GetZipLen(recvBytes);
            int lenDat = _TraderSock.GetDatLen(recvBytes);
            if (lenDat < 1)
                return;

            byte[] zipBytes = new byte[lenZip];
            byte[] datBytes = new byte[lenDat];
            Buffer.BlockCopy(recvBytes, TraderSocket.HEAD_INFO_LENGTH + 0xD2, zipBytes, 0, lenZip);

            try
            {
                IntPtr[] ptrLenDat = new IntPtr[1] { (IntPtr)lenDat };
                int result = TraderSocket.ts_uncompress(zipBytes, lenZip, datBytes, ptrLenDat);
                if (result != 0)
                    return;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return;
            }
            try
            {
                string msg = Encoding.ASCII.GetString(datBytes);

                lock (OrderList)
                {
                    OrderList.RemoveAll(o => o.OrderType == "미체결");
                }

                string strExp = @".{15}.{9}(.{10}).{20}(.{32}).{10}(.{1})(.{20}).{60}(.{20}).{25}(.{20}).{50}\d*\s+";
                Regex regex = new Regex(strExp);

                string sSymbol = "";
                string sTrade = "";
                string sAveragePrice = "";
                string sQty = "";
                string sOrderNo = "";
                //string sOrderTime = "";

                MatchCollection mc = regex.Matches(msg);
                foreach (Match m in mc)
                {
                    sOrderNo = m.Groups[1].Value.Trim();
                    sSymbol = m.Groups[2].Value.Trim();
                    if (sSymbol != CurItemSymbol.Symbol)
                        return;
                    sTrade = m.Groups[3].Value.Trim();
                    sQty = m.Groups[4].Value.Trim();
                    sAveragePrice = m.Groups[6].Value.Trim();
                    //sOrderTime = m.Groups[7].Value.Trim();

                    double dAveragePrice = double.Parse(sAveragePrice);

                    int nQty = int.Parse(sQty);

                    long lOrderNo = long.Parse(sOrderNo);

                    TRADETYPE tradeType = sTrade == "S" ? TRADETYPE.SELL : TRADETYPE.BUY;

                    OrderInfo orderInfo = new OrderInfo
                    {
                        OrderType = "미체결",
                        Symbol = sSymbol,
                        Qty = string.Format("{0}[{1}]", tradeType == TRADETYPE.SELL ? "매도" : "매수", nQty),
                        AveragePrice = string.Format(Settings.Default.PriceFormat, dAveragePrice),
                        MaxAveragePrice = Math.Round(dAveragePrice, 6),
                        CrossAveragePrice = 0,
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

                SetUnliquidationPosition(OrderList);
                OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.ORDER);
            }
            catch (Exception)
            {

            }

        }


        private void ParseReceiptMsg(byte[] recvBytes)
        {
            try
            {
                string msg = Encoding.ASCII.GetString(recvBytes);
                WriteLog(msg);
                string strExp = @".{310}.{15}(.{2})(.{32})(.{1})(.{8})(.{10})(.{10}).{20}.{90}(.{10})(.{20}).{16}(.{14}).{140}(.{10})(.{20})";
                Regex regex = new Regex(strExp);

                string sOrdType = "";
                string sSymbol = "";
                string sTrade = "";
                string sAveragePrice = "";
                string sQty = "";
                string sOrderNo = "";
                string sOrderNo2 = "";
                string sOrderTime = "";
                string sConcQty = "";
                string sConcPrice = "";

                MatchCollection mc = regex.Matches(msg);
                foreach (Match m in mc)
                {
                    sOrdType = m.Groups[1].Value.Trim(); //21-주문 22-체결

                    sSymbol = m.Groups[2].Value.Trim();
                    if (sSymbol != CurItemSymbol.Symbol)
                        return;
                    sTrade = m.Groups[3].Value.Trim();
                    sOrderNo = m.Groups[5].Value.Trim();
                    sOrderNo2 = m.Groups[6].Value.Trim();
                    sQty = m.Groups[7].Value.Trim();
                    sAveragePrice = m.Groups[8].Value.Trim();
                    sOrderTime = m.Groups[9].Value.Trim();
                    sConcQty = m.Groups[10].Value.Trim();
                    sConcPrice = m.Groups[11].Value.Trim();
                    break;
                }
                if (sSymbol.Length < 1)
                    return;

                double dAveragePrice = double.Parse(sAveragePrice);

                int nQty = int.Parse(sQty);

                long lOrderNo = long.Parse(sOrderNo);
                long lOrderNo2 = long.Parse(sOrderNo2);
                TRADETYPE tradeType = sTrade == "S" ? TRADETYPE.SELL : TRADETYPE.BUY;

                WriteLog("ParseReceiptMsg() OrdType=" + sOrdType + " Trade=" + sTrade + " OrderNo=" + sOrderNo +
                        "," + sOrderNo2 + " Qty=" + sQty + "," + sConcQty + " Price=" + sAveragePrice + ", " + sConcPrice);

                if (sOrdType == "21") //주문
                {
                    if (nQty > 0) //주문접수
                    {
                        if (dAveragePrice == 0)
                            return;
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
                        OrderInfo orInfo = OrderList.FirstOrDefault<OrderInfo>(
                           (OrderInfo o) => long.Parse(o.OrderNo) == lOrderNo && o.OrderType == "미체결");

                        lock (OrderList)
                        {
                            if (orInfo != null)
                            {
                                OrderList.Remove(orInfo);
                            }
                            OrderList.Add(orderInfo);
                        }

                    }
                    else //취소
                    {
                        OrderInfo orInfo = OrderList.FirstOrDefault<OrderInfo>(
                           (OrderInfo o) => long.Parse(o.OrderNo) == lOrderNo2 && o.OrderType == "미체결");
                        lock (OrderList)
                        {
                            if (orInfo != null)
                            {
                                OrderList.Remove(orInfo);
                            }
                        }
                    }

                    SetUnliquidationPosition(OrderList);
                    OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.ORDER);

                }
                else if (sOrdType == "22") //체결, 청산
                {
                    nQty = int.Parse(sConcQty);
                    double dConcPrice = double.Parse(sConcPrice);
                    string logMsg = "";

                    lock (OrderList)
                    {

                        OrderInfo orderInfo = OrderList.FirstOrDefault<OrderInfo>(
                            (OrderInfo o) => o.OrderType == "체결" && o.Symbol == sSymbol);

                        if (orderInfo != null && orderInfo.TradeType != tradeType && orderInfo.OrderQty == nQty) //청산
                        {

                            logMsg += "청산가:" + string.Format(Settings.Default.PriceFormat, dConcPrice).ToString();

                            this.LiquidOrder = new OrderVal
                            {
                                OrderType = "청산",
                                Symbol = orderInfo.Symbol,
                                AveragePrice = string.Format(Settings.Default.PriceFormat, dConcPrice).ToString(),
                                OrderTime = this.Current.Time/*DateTime.Now*/,
                                ResultState = orderInfo.TradeType == TRADETYPE.BUY ? RESULTSTATE.BUY : RESULTSTATE.SELL,
                                ConcState = CONCSTATE.LIQUID,
                                OrderQty = orderInfo.OrderQty,
                            };
                            OrderList.Remove(orderInfo);
                            dAveragePrice = double.Parse(orderInfo.AveragePrice);

                            long? dValuation = orderInfo.TradeType == TRADETYPE.SELL ?
                                            (long?)((dAveragePrice - dConcPrice) / CurItemSymbol.OverTick * CurItemSymbol.ValueTick * CurItemSymbol.Exchange * orderInfo.OrderQty) :
                                            (long?)((dConcPrice - dAveragePrice) / CurItemSymbol.OverTick * CurItemSymbol.ValueTick * CurItemSymbol.Exchange * orderInfo.OrderQty);
                            dValuation -= (long?)(FEE_RATE * 2 * CurItemSymbol.Exchange * orderInfo.OrderQty);

                            logMsg += "(" + (dValuation > 0 ? "수익:" : "손실:") + string.Format("{0:N0}원)", (long)dValuation);
                        }
                        else
                        {

                            OrderInfo orderReceipt = OrderList.FirstOrDefault<OrderInfo>(
                                (OrderInfo o) => o.TradeType == tradeType && o.OrderType == "미체결");

                            if (orderReceipt != null)
                            {
                                this.OrderList.Remove(orderReceipt);
                            }

                            this.LiquidOrder = new OrderVal
                            {
                                OrderType = "체결",
                                Symbol = sSymbol,
                                AveragePrice = string.Format(Settings.Default.PriceFormat, dConcPrice),
                                OrderTime = this.Current.Time/*DateTime.Now*/,
                                ResultState = tradeType == TRADETYPE.BUY ? RESULTSTATE.BUY : RESULTSTATE.SELL,
                                ConcState = CONCSTATE.CONCLUDE,
                                OrderQty = nQty,
                            };

                            if (orderInfo != null)
                            {
                                if (orderInfo.TradeType == tradeType)
                                    nQty += orderInfo.OrderQty;
                                else if (nQty > orderInfo.OrderQty)
                                {
                                    this.LiquidOrder.OrderType = "되돌림";
                                    this.LiquidOrder.ConcState = CONCSTATE.RECONC;
                                    this.LiquidOrder.ResultState = tradeType == TRADETYPE.BUY ? RESULTSTATE.BUY : RESULTSTATE.SELL;
                                    nQty = nQty - orderInfo.OrderQty;
                                }
                                else
                                {
                                    this.LiquidOrder.OrderType = "청산";
                                    this.LiquidOrder.ConcState = CONCSTATE.LIQUID;
                                    this.LiquidOrder.ResultState = orderInfo.TradeType == TRADETYPE.BUY ? RESULTSTATE.BUY : RESULTSTATE.SELL;
                                    tradeType = orderInfo.TradeType;
                                    nQty = orderInfo.OrderQty - nQty;
                                }
                                this.OrderList.Remove(orderInfo);
                            }

                            orderInfo = new OrderInfo
                            {
                                OrderType = "체결",
                                Symbol = sSymbol,
                                Qty = string.Format("{0}[{1}]", tradeType == TRADETYPE.SELL ? "매도" : "매수", nQty),
                                AveragePrice = string.Format(Settings.Default.PriceFormat, dConcPrice),
                                MaxAveragePrice = Math.Round(dConcPrice, 6),
                                CrossAveragePrice = 0,
                                CurrentPrice = Current.CurrentPrice.ToString(),
                                Valuation = 0,
                                Action = "청산",
                                TradeType = tradeType,
                                OrderQty = nQty,
                                OrderTime = Environment.TickCount,
                                OrderNo = lOrderNo.ToString(),

                            };
                            this.OrderList.Add(orderInfo);

                            logMsg += "체결가:" + orderInfo.AveragePrice.ToString();
                        }

                    }
                    _ordered = true;

                    SetUnliquidationPosition(OrderList);
                    OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.LIQUID);
                    OnFutureSiteLogEvent(logMsg);

                    WriteLog("ParseReceiptMsg() RequestOrderlist");
                }
            }
            catch (Exception)
            {
                _ordered = true;
            }

        }

        private void ParseOrderMsg(byte[] recvBytes)
        {

            try
            {
                string[] splitMsg = null;


                if (splitMsg.Length < 3)
                    return;

                if (splitMsg[0] != RECV_RESULT_SUCCESS)
                    return;

                int iCurPos = PACKET_BODYLEN_8 + PACKET_BODYLEN_16;

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
                            else if (nQty > orderInfo.OrderQty)
                            {
                                this.LiquidOrder.OrderType = "되돌림";
                                this.LiquidOrder.ConcState = CONCSTATE.RECONC;
                                this.LiquidOrder.ResultState = tradeType == TRADETYPE.BUY ? RESULTSTATE.BUY : RESULTSTATE.SELL;
                                nQty = nQty - orderInfo.OrderQty;
                            }
                            else
                            {
                                this.LiquidOrder.OrderType = "청산";
                                this.LiquidOrder.ConcState = CONCSTATE.LIQUID;
                                this.LiquidOrder.ResultState = orderInfo.TradeType == TRADETYPE.BUY ? RESULTSTATE.BUY : RESULTSTATE.SELL;
                                tradeType = orderInfo.TradeType;
                                nQty = orderInfo.OrderQty - nQty;
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
                    }
                    else
                    {
                        OrderInfo orderInfo = OrderList.FirstOrDefault<OrderInfo>(
                            (OrderInfo o) => o.TradeType == tradeType &&
                            double.Parse(o.AveragePrice) == dAveragePrice && o.OrderType == "체결");

                        logMsg += "청산가:" + string.Format(Settings.Default.PriceFormat, dCurrentPrice).ToString();

                        if (orderInfo != null)
                        {
                            this.LiquidOrder = new OrderVal
                            {
                                OrderType = "청산",
                                Symbol = orderInfo.Symbol,
                                AveragePrice = string.Format(Settings.Default.PriceFormat, dCurrentPrice).ToString(),
                                OrderTime = this.Current.Time/*DateTime.Now*/,
                                ResultState = orderInfo.TradeType == TRADETYPE.BUY ? RESULTSTATE.BUY : RESULTSTATE.SELL,
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

                if (Current != null)
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
