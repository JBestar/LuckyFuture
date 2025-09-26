// #define WRITE_LOG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuckyFuture.Models.ValueObjects;
using System.Threading;
using System.IO;
using System.Diagnostics;
using LuckyFuture.Properties;
using ChartCtrl;
using LuckyFutureLib.Include;
using Goodbyte.TradingSystem.Domain.Entities;
using System.Text.RegularExpressions;

namespace LuckyFuture.Site
{
    enum ERRORCOM :Int32
    {
        SUCCESS = 0,            //성공
        NOTRCODE = -103,        //TrCode 없음
        OVERREQUEST = -200,     //조회과부하
        OVERORDER = -201,       //주문과부하
        NOPARAMETER = -202,     //조회입력값(명칭/누락)오류
        NOPRCODE = -203,        //종목코드 미존재
        NOORDERINPUT = -300,    //주문입력값 오류
        NOACCPWD = -301,        //계좌비번 미입력
    }

    class FutureObject
    {

        public string stk_code { get; set; } //	12	종목코드
        public string arti_code { get; set; } // 6	품목코드(6A, ES, ….)
        public string arti_hnm { get; set; } //	40	품목명
        public string arti_tp { get; set; } //	3	품목구분(IDX, CUR, ….)
        public string crnc_code { get; set; } // 3	결제통화코드(USD, JPY, …)
        public string tick_unit { get; set; } //	15	TICK 단위
        public string tick_value { get; set; } //	15	TICK 가치
        public string deal_unit { get; set; } //	15	거래단위
        public string deal_mtal { get; set; } //	15	거래승수
        public string ntt_code { get; set; } //	1	진법코드로서 1~9, A ~Z으로 표현되며, 실수형 또는 진법표현을 위한 구분값임.
        public string  ntt_calc_unit { get; set; } //	15	가격표시조정계수
        public string  frgn_exch_code { get; set; } //	10	해외거래소코드
        public string  expr_dt { get; set; } //	8	만기일자
        public string  fprc { get; set; } //	10	소숫점자리수
        public string  gubun { get; set; } //	1	최근월물구분
        public string  atv_code { get; set; } //	1	액티브월물구분
        public string  pre_gvol { get; set; } //	16	전일누적거래량

    }
    class KFOpen : FutureSite
    {
        public override SITETYPE Type { get; set; }

        protected AxKFOpenAPILib.AxKFOpenAPI axKFOpenAPI;

        // private Current m_current;
        private string m_sCheDate = "";
        private double EXCH_RATE = 1300.0;
        private int m_tickCurrent = 0;
        // private int m_tickChart = 0;
        private int m_tickQuote = 0;
        private int m_tickAccount = 0;
        private bool m_bNeedAcc = false;
        private int m_tickOrderList = 0;

        public KFOpen()
        {
            Type = SITETYPE.KIWOOM;
        }

        public KFOpen(AxKFOpenAPILib.AxKFOpenAPI axKFOpenAPI)
        {
            Type = SITETYPE.KIWOOM;
            this.axKFOpenAPI = axKFOpenAPI;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Trace.TraceInformation("<KFOpen> Register KF_OnReceiver");
            /*
            //Connection Event Handler
            axKFOpenAPI.OnEventConnect += KF_OnEventConnect;
            */
            //ReceiveTrData Event Handler
            axKFOpenAPI.OnReceiveTrData += KF_OnReceiveTrData;
            //ReceiveRealData Event Handler
            axKFOpenAPI.OnReceiveRealData += KF_OnReceiveRealData;
            //ReceiveMsg Event Handler
            //axKFOpenAPI.OnReceiveMsg += KF_OnReceiveMsg;
            //ReceiveChejanData Event Handler
            axKFOpenAPI.OnReceiveChejanData += KF_OnReceiveChejanData;
            

#if WRITE_LOG
            CreateLogFile();
#endif
        }
        string _logPath = "";

        private void CreateLogFile()
        {
            _logPath = "D://BinHts/KFOpen_" + DateTime.Now.ToString("yyyyMMdd");
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
            ERRORCODE error_code = ERRORCODE.UNKNOWN_FAILED;
            this.CurrentUserAccount = new UserAccountInfo
            {
                UserAccountId = UserAcc,
                UserAccountStr = UserAcc,
            };
            this.UserAccounts = new List<UserAccountInfo>();
            this.UserAccounts.Add(this.CurrentUserAccount);
            DayProfitLoss = new DayProfitLossInfo();
            OnLogin();

            CONSTATE con_state = RequestAccountData();
            switch (con_state)
            {
                case CONSTATE.SUCCESSS:
                    error_code = ERRORCODE.SUCCESS;
                    //OnFutureSiteLogEvent("계좌에 접속되었습니다.");
                    break;
                case CONSTATE.NO_LOGIN:
                    error_code = ERRORCODE.ACCOUNT_STANDBY;
                    OnFutureSiteLogEvent("키움에 로그인 해주십시오.");
                    break;
                case CONSTATE.ERR_REGAPI:
                    error_code = ERRORCODE.CANT_CONNECT;
                    OnFutureSiteLogEvent("OpenAPI 미신청중입니다.");
                    break;
                case CONSTATE.ERR_ACCPWD:
                    error_code = ERRORCODE.CANT_CONNECT;
                    OnFutureSiteLogEvent("키움에 계좌비번을 입력하십시오.");
                    break;
                case CONSTATE.ERR_ACCNO:
                    error_code = ERRORCODE.CANT_CONNECT;
                    OnFutureSiteLogEvent("타인 계좌를 사용할 수 없습니다.");
                    break;
                default:
                    error_code = ERRORCODE.CANT_CONNECT;
                    OnFutureSiteLogEvent("접속실패!");
                    break;
            }


            return error_code;
        }


        protected override void OnLogin()
        {
            base.OnLogin();
            bQutoteCreated = false;
            RequestRealData();
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
                // Trace.TraceInformation("<KFOpen> Check() ChangeItem");
                ItemSymbolInfo itemSymbol = ItemList.FirstOrDefault<ItemSymbolInfo>(it => it.Symbol == ItemSymbol);
                if(itemSymbol != null)
                {
                    CurItemSymbol = itemSymbol;
                    ItemPrecision = itemSymbol.Precision;
                    Settings.Default.PriceFormat = Common.GetPriceFormat(CurItemSymbol.Precision);

                    // OnFutureSiteLogEvent(CurItemSymbol.ItemName);
                    return ERRORCODE.PREPARE_FAILED;
                } else
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

//             if(Environment.TickCount - m_tickChart < 600000)
//             {
//                 if (this.OrderList.Count < 1)
//                     RequestChart();
//             }

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
            Trace.TraceInformation("<KFOpen> Remove KF_OnReceiver");

            axKFOpenAPI.OnReceiveTrData -= KF_OnReceiveTrData;
            //ReceiveRealData Event Handler
            axKFOpenAPI.OnReceiveRealData -= KF_OnReceiveRealData;
            //ReceiveMsg Event Handler
            //axKFOpenAPI.OnReceiveMsg += KF_OnReceiveMsg;
            //ReceiveChejanData Event Handler
            axKFOpenAPI.OnReceiveChejanData -= KF_OnReceiveChejanData;

            base.OnStopped(bAutoStop);
        }

        protected override void OnPrepare()
        {   
            base.OnPrepare();

            if (CurItemSymbol != null)
            {
                Thread.Sleep(500);
                CurItemSymbol.MidPrice = 0;
            }
            Current = null;

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
            if (Current == null || Current.CurrentPrice < 1)
            {
                OnFutureSiteLogEvent("[주문] 주문기간이 아닙니다.");
                return false;
            }

            if (quoteInfo != null)
            {
                double nOrderRange = 400 * CurItemSymbol.OverTick;
                if (quoteInfo.Price < Current.CurrentPrice - nOrderRange || quoteInfo.Price > Current.CurrentPrice + nOrderRange)
                {
                    OnFutureSiteLogEvent("[주문] 주문 가격이 초과 됨");
                    return false;
                }
            }
            else bMarketPrice = true;


            string strRQName = "RQ_5";
            string strScrNo = "0105";
            string strAccNo = CurrentUserAccount.UserAccountId;
            int iOrderType = 1;             //1:신규매도, 2:신규매수 3:매도취소, 4:매수취소, 5:매도정정, 6:매수정정
            string strCode = ItemSymbol;
            int iQty = (int)nQuantity;
            string strPrice = "0";
            string strStopPrice = "0";      // 주문구분 3:STOP, 4:STOP LIMIT 인 경우, 값 셋팅(단, 2:STOP 인 경우, strPrice = "0" 셋팅) 
            string strOrderGubun = "1";     // 2:지정가, 1:시장가, 3:STOP, 4:STOP LIMIT
            string strOrgNo = "";           // 주문타입 3:매도취소, 4:매수취소, 5:매도정정, 6:매수정정 인 경우에 주문번호 셋팅
            if (!bMarketPrice)
            {
                strOrderGubun = "2";
                strPrice = quoteInfo.Price.ToString();
            }
                
            int iRet = axKFOpenAPI.SendOrder(strRQName, strScrNo, strAccNo, iOrderType, strCode, iQty, strPrice, strStopPrice, strOrderGubun, strOrgNo);

            if (iRet == (int)ERRORCOM.SUCCESS)
            {
                OnFutureSiteLogEvent("[주문] 매도주문 요청");
                OnFutureSiteLogEvent("[주문] 주문시가격:" + Current.CurrentPrice);
                return true;
            }
            else ShowErrorLog((ERRORCOM)iRet);
            return true;

        }
        
        public override bool DoBuyOrder(QuoteInfo quoteInfo, double nQuantity = 1, bool bMarketPrice = false)
        {

            if (this.CurrentUserAccount == null || string.IsNullOrEmpty(CurrentUserAccount.UserAccountId))
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
            if (Current == null || Current.CurrentPrice < 1)
            {
                OnFutureSiteLogEvent("[주문] 주문기간이 아닙니다.");
                return false;
            }

            if (quoteInfo != null)
            {
                double nOrderRange = 400 * CurItemSymbol.OverTick;
                if (quoteInfo.Price < Current.CurrentPrice - nOrderRange || quoteInfo.Price > Current.CurrentPrice + nOrderRange)
                {
                    OnFutureSiteLogEvent("[주문] 주문 가격이 초과 됨");
                    return false;
                }
            }
            else bMarketPrice = true;

            string strRQName = "RQ_5";
            string strScrNo = "0105";
            string strAccNo = CurrentUserAccount.UserAccountId;
            int iOrderType = 2;             //1:신규매도, 2:신규매수 3:매도취소, 4:매수취소, 5:매도정정, 6:매수정정
            string strCode = ItemSymbol;
            int iQty = (int)nQuantity;
            string strPrice = "0";
            string strStopPrice = "0";      // 주문구분 3:STOP, 4:STOP LIMIT 인 경우, 값 셋팅(단, 2:STOP 인 경우, strPrice = "0" 셋팅) 
            string strOrderGubun = "1";     // 2:지정가, 1:시장가, 3:STOP, 4:STOP LIMIT
            string strOrgNo = "";           // 주문타입 3:매도취소, 4:매수취소, 5:매도정정, 6:매수정정 인 경우에 주문번호 셋팅

            if (!bMarketPrice)      //지정가주문
            {
                strPrice = quoteInfo.Price.ToString();
                strOrderGubun = "2";
            }
                
            
            int iRet = axKFOpenAPI.SendOrder(strRQName, strScrNo, strAccNo, iOrderType, strCode, iQty, strPrice, strStopPrice, strOrderGubun, strOrgNo);

            if (iRet == (int)ERRORCOM.SUCCESS)
            {
                // _orderTrade = TRADETYPE.BUY;
                OnFutureSiteLogEvent("[주문] 매수주문 요청");
                OnFutureSiteLogEvent("[주문] 주문시가격:" + Current.CurrentPrice);
                return true;
            }
            else ShowErrorLog((ERRORCOM)iRet);
            return true;
        }


        public override bool CancelOrder(OrderInfo orderInfo)
        {
            if (this.CurrentUserAccount == null || string.IsNullOrEmpty(CurrentUserAccount.UserAccountId))
            {
                OnFutureSiteLogEvent("[주문취소] 계좌정보 오류!");
                return false;
            }

            string strRQName = "RQ_5";
            string strScrNo = "0105";
            string strAccNo = CurrentUserAccount.UserAccountId;  //계좌번호
            int iOrderType = orderInfo.TradeTypeNo == "1"? 3:4;             //주문유형 (1:신규매도, 2:신규매수 3:매도취소, 4:매수취소, 5:매도정정, 6:매수정정)
            string strCode = orderInfo.Symbol;       //종목코드
            int iQty = (int)orderInfo.OrderQty;                   //주문수량
            string strPrice = "0";          //주문단가
            string strStopPrice = "0";      //stop단가 주문구분 3:STOP, 4:STOP LIMIT 인 경우, 값 셋팅(단, 2:STOP 인 경우, strPrice = "0" 셋팅) 
            string strOrderGubun = "2";     //거래구분( 2:지정가, 1:시장가, 3:STOP, 4:STOP LIMIT)
            string strOrgNo = orderInfo.OrderNo; //원주문번호 (주문타입 3:매도취소, 4:매수취소, 5:매도정정, 6:매수정정 인 경우에 주문번호 셋팅)


            int iRet = axKFOpenAPI.SendOrder(strRQName, strScrNo, strAccNo, iOrderType, strCode, iQty, strPrice, strStopPrice, strOrderGubun, strOrgNo);

            if (iRet == (int)ERRORCOM.SUCCESS)
            {
                if (orderInfo.TradeTypeNo == "1")
                    OnFutureSiteLogEvent("[주문취소] 매도주문");
                else if (orderInfo.TradeTypeNo == "2")
                    OnFutureSiteLogEvent("[주문취소] 매수주문");
            }
            else ShowErrorLog((ERRORCOM)iRet);

            return true;
        }

        public override bool LiquidateOrder(OrderInfo orderInfo)
        {
            if (this.CurrentUserAccount == null)
                return false;

            string strAccount = CurrentUserAccount.UserAccountId;
            string strAccPwd = UserPassword;
            axKFOpenAPI.SetInputValue("계좌번호", strAccount);
            axKFOpenAPI.SetInputValue("비밀번호", strAccPwd);
            axKFOpenAPI.SetInputValue("비밀번호입력매체", "00");
            axKFOpenAPI.SetInputValue("통신주문구분", "AP");
            axKFOpenAPI.SetInputValue("입력건수", "1");
            axKFOpenAPI.SetInputValue("FCM코드", orderInfo.FCMCode);
            axKFOpenAPI.SetInputValue("종목코드", orderInfo.Symbol);
            axKFOpenAPI.SetInputValue("종목명", orderInfo.SymbolName);
            axKFOpenAPI.SetInputValue("매도수구분", orderInfo.TradeTypeNo);    //1:매도 2:매수
            axKFOpenAPI.SetInputValue("매입표시가격", orderInfo.AveragePrice);
            axKFOpenAPI.SetInputValue("매입일자", orderInfo.OrderDate);
            axKFOpenAPI.SetInputValue("해외주문유형", "1");   //1:시장가
            axKFOpenAPI.SetInputValue("주문수량", orderInfo.OrderQty.ToString());
            axKFOpenAPI.SetInputValue("주문표시가격", orderInfo.OrderPrice);
            axKFOpenAPI.SetInputValue("STOP표시가격", orderInfo.StopPrice);
            axKFOpenAPI.SetInputValue("주문번호", orderInfo.OrderNo);
            axKFOpenAPI.SetInputValue("에러코드", orderInfo.ErrorCode);
            axKFOpenAPI.SetInputValue("처리메세지", orderInfo.OrderMsg);

            int iResCode = axKFOpenAPI.CommRqData("RQ_4", "opw10007", "", "0104");

            if (iResCode == (int)ERRORCOM.SUCCESS)
            {
                if (orderInfo.TradeTypeNo == "1")
                {
                    OnFutureSiteLogEvent("[청산] 매도주문이 청산되었습니다.");
                }
                else if (orderInfo.TradeTypeNo == "2")
                {
                    OnFutureSiteLogEvent("[청산] 매수주문이 청산되었습니다.");
                }

                OnFutureSiteLogEvent("[청산] 청산시가격:" + Current.CurrentPrice);

            }
            else ShowErrorLog((ERRORCOM)iResCode);


            return true;
        }

        protected void ShowErrorLog(ERRORCOM errorCom)
        {
            if (errorCom == ERRORCOM.OVERREQUEST)
            {
                OnFutureSiteLogEvent("조회과부하!");
            }
            else if (errorCom == ERRORCOM.OVERORDER)
            {
                OnFutureSiteLogEvent("주문과부하!");
            }
            else if (errorCom == ERRORCOM.NOPARAMETER)
            {
                OnFutureSiteLogEvent("조회입력값오류!");
            }
            else if (errorCom == ERRORCOM.NOPRCODE)
            {
                OnFutureSiteLogEvent("종목코드미존재!");
            }
            else if (errorCom == ERRORCOM.NOORDERINPUT)
            {
                OnFutureSiteLogEvent("주문입력값오류!");
            }
            else if (errorCom == ERRORCOM.NOACCPWD)
            {
                OnFutureSiteLogEvent("계좌비번을 입력해주세요!");
            }
            else
            {
                OnFutureSiteLogEvent("조회오류!");
            }
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


        /// <summary>
        /// //////////////////
        /// </summary>
        /// <returns></returns>

        private CONSTATE RequestRealData()
        {
            if (this.CurrentUserAccount == null)
                return CONSTATE.NO_LOGIN;

            string strAccount = this.CurrentUserAccount.UserAccountId;
            string strAccPwd = UserPassword;

            axKFOpenAPI.SetInputValue("계좌번호", strAccount);
            axKFOpenAPI.SetInputValue("비밀번호", strAccPwd);
            axKFOpenAPI.SetInputValue("비밀번호입력매체", "00");

            int iResCode = axKFOpenAPI.CommRqData("RQ_3", "opw60003", "", "0103");      //원화외화예수금잔액조회

            WriteLog("RequestRealData=" + iResCode);

            if (iResCode != 0)
                return (CONSTATE)iResCode;
            return CONSTATE.SUCCESSS;
            
        }

        private void RequestChart()
        {
            
            Thread.Sleep(500);
            RequestRChart();
            Thread.Sleep(500);
            RequestDChart((CHARTTYPE)Settings.Default.ChartType);
            
            /*
            RequestData("NQU25", "RQ_13", "0113");

            Thread.Sleep(10000);

            RequestData("ESU25", "RQ_13", "0114");
            */
        }

        private bool RequestData(string symbol, string sRQName, string sScreenNo)
        {
            if (this.CurrentUserAccount == null)
                return false;

            string sTrCode = "opt10001";

            axKFOpenAPI.SetInputValue("종목코드", symbol);

            TIMETYPE timeType = TIMETYPE.TIMETYPE_TICK;
            string sTimeUnit = "60";
            
            switch (timeType)
            {
                case TIMETYPE.TIMETYPE_TICK:
                    sTrCode = "opc10001";
                    axKFOpenAPI.SetInputValue("시간단위", sTimeUnit);
                    break;
                case TIMETYPE.TIMETYPE_MIN:
                    sTrCode = "opc10002";
                    axKFOpenAPI.SetInputValue("시간단위", sTimeUnit);
                    break;
                case TIMETYPE.TIMETYPE_DAY:
                    sTrCode = "opc10003";
                    axKFOpenAPI.SetInputValue("조회일자", DateTime.Now.ToString("yyyyMMdd"));
                    break;
                case TIMETYPE.TIMETYPE_WEEK:
                    sTrCode = "opc10004";
                    axKFOpenAPI.SetInputValue("조회일자", DateTime.Now.ToString("yyyyMMdd"));
                    break;
                case TIMETYPE.TIMETYPE_MONTH:
                    sTrCode = "opc10005";
                    axKFOpenAPI.SetInputValue("조회일자", DateTime.Now.ToString("yyyyMMdd"));
                    break;
                case TIMETYPE.TIMETYPE_YEAR:
                    sTrCode = "opc10006";
                    axKFOpenAPI.SetInputValue("조회일자", DateTime.Now.ToString("yyyyMMdd"));
                    break;
                default:
                    break;
            }

            int iResCode = axKFOpenAPI.CommRqData(sRQName, sTrCode, "", sScreenNo);      //종목정보조회

            WriteLog(symbol + " RequestData=" + iResCode);

            if (iResCode == 0)
                return true;
            return false;

        }

        public override bool RequestDChart(CHARTTYPE chartType)
        {
            if (this.CurrentUserAccount == null)
                return false;

            if (DChartType == chartType)
                return true;

            DChartType = chartType;

            axKFOpenAPI.SetInputValue("종목코드", ItemSymbol);
            string sTrCode = "";
            string sTimeUnit = "";
            
            switch (chartType)
            {
                case CHARTTYPE.MIN_1:
                    sTrCode = "opc10002";
                    sTimeUnit = ((int)TIMEUNIT.TIMEUNIT_1).ToString();
                    break;
                case CHARTTYPE.MIN_5:
                    sTrCode = "opc10002";
                    sTimeUnit = ((int)TIMEUNIT.TIMEUNIT_5).ToString();
                    break;
                case CHARTTYPE.MIN_15:
                    sTrCode = "opc10002";
                    sTimeUnit = ((int)TIMEUNIT.TIMEUNIT_15).ToString();
                    break;
                case CHARTTYPE.MIN_30:
                    sTrCode = "opc10002";
                    sTimeUnit = ((int)TIMEUNIT.TIMEUNIT_30).ToString();
                    break;
                case CHARTTYPE.TICK_60:
                    sTrCode = "opc10001";
                    sTimeUnit = ((int)TIMEUNIT.TIMEUNIT_60).ToString();
                    break;
                case CHARTTYPE.TICK_90:
                    sTrCode = "opc10001";
                    sTimeUnit = ((int)TIMEUNIT.TIMEUNIT_90).ToString();
                    break;
                case CHARTTYPE.TICK_120:
                    sTrCode = "opc10001";
                    sTimeUnit = ((int)TIMEUNIT.TIMEUNIT_120).ToString();
                    break;
                case CHARTTYPE.TICK_240:
                    sTrCode = "opc10001";
                    sTimeUnit = ((int)TIMEUNIT.TIMEUNIT_240).ToString();
                    break;
                case CHARTTYPE.TICK_350:
                    sTrCode = "opc10001";
                    sTimeUnit = ((int)TIMEUNIT.TIMEUNIT_350).ToString();
                    break;
                case CHARTTYPE.TICK_400:
                    sTrCode = "opc10001";
                    sTimeUnit = ((int)TIMEUNIT.TIMEUNIT_400).ToString();
                    break;
                case CHARTTYPE.TICK_600:
                    sTrCode = "opc10001";
                    sTimeUnit = ((int)TIMEUNIT.TIMEUNIT_600).ToString();
                    break;
                case CHARTTYPE.TICK_750:
                    sTrCode = "opc10001";
                    sTimeUnit = ((int)TIMEUNIT.TIMEUNIT_750).ToString();
                    break;
                case CHARTTYPE.TICK_990:
                    sTrCode = "opc10001";
                    sTimeUnit = ((int)TIMEUNIT.TIMEUNIT_990).ToString();
                    break;
                default:
                    break;
            }
            WriteLog(string.Format("<KFOpen> RequestDChart sTrCode = {0}, sTimeUnit = {1}", sTrCode, sTimeUnit));
            if (sTrCode.Length < 1)
                return false;
            axKFOpenAPI.SetInputValue("시간단위", sTimeUnit);
            int iResCode = axKFOpenAPI.CommRqData("RQ_0", sTrCode, "", "0100");

            if (iResCode == 0)
                return true;
            return false;
        }
        
        public override bool RequestRChart()
        {
            if (this.CurrentUserAccount == null)
                return false;
            TIMETYPE timeType = CtrlProperty._RTimeType;
            TIMEUNIT timeUnit = CtrlProperty._RTimeUnitAmt;
            string sTimeUnit = ((int)timeUnit).ToString();
            axKFOpenAPI.SetInputValue("종목코드", ItemSymbol);
            string sTrCode = "";
            switch (timeType)
            {
                case TIMETYPE.TIMETYPE_TICK:
                    sTrCode = "opc10001";
                    axKFOpenAPI.SetInputValue("시간단위", sTimeUnit);
                    break;
                case TIMETYPE.TIMETYPE_MIN:
                    sTrCode = "opc10002";
                    axKFOpenAPI.SetInputValue("시간단위", sTimeUnit);
                    break;
                case TIMETYPE.TIMETYPE_DAY:
                    sTrCode = "opc10003";
                    axKFOpenAPI.SetInputValue("조회일자", DateTime.Now.ToString("yyyyMMdd"));
                    break;
                case TIMETYPE.TIMETYPE_WEEK:
                    sTrCode = "opc10004";
                    axKFOpenAPI.SetInputValue("조회일자", DateTime.Now.ToString("yyyyMMdd"));
                    break;
                case TIMETYPE.TIMETYPE_MONTH:
                    sTrCode = "opc10005";
                    axKFOpenAPI.SetInputValue("조회일자", DateTime.Now.ToString("yyyyMMdd"));
                    break;
                case TIMETYPE.TIMETYPE_YEAR:
                    sTrCode = "opc10006";
                    axKFOpenAPI.SetInputValue("조회일자", DateTime.Now.ToString("yyyyMMdd"));
                    break;
                default:
                    break;
            }

            WriteLog(string.Format("<KFOpen> RequestRChart sTrCode = {0}, sTimeUnit = {1}", sTrCode, sTimeUnit));
            if (sTrCode.Length < 1)
                return false;

            int iResCode = axKFOpenAPI.CommRqData("RQ_1", sTrCode, "", "0101"); 
             
            if (iResCode == 0)
                return true;
            return false ;
        }

        public override bool ChangePrd(string sSymbol)
        {
            if (PrdList == null || PrdList.Count < 1)
                return false;
            if (sSymbol != CurPrd.Code)
            {
                CurPrd = PrdList.FirstOrDefault(p => p.Code == sSymbol);
                if(CurPrd != null)
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

            axKFOpenAPI.SetInputValue("계좌번호", strAccount);
            axKFOpenAPI.SetInputValue("비밀번호", strAccPwd);
            axKFOpenAPI.SetInputValue("비밀번호입력매체", "00");

            int iResCode = axKFOpenAPI.CommRqData("RQ_9", "opw30009", "", "0109");      //예수금및증거금현황조회
            if (iResCode == 0)
                return CONSTATE.SUCCESSS;
            return (CONSTATE)iResCode;
        }

        private void RequestOrderList(bool bAccount, bool bRequidate)
        {
            if (Settings.Default.SignalSiteOn)
                return;

            // OrderList.Clear();
            // OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.ORDER);            
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

            string strAccount = this.CurrentUserAccount.UserAccountId;
            string strAccPwd = UserPassword;

            axKFOpenAPI.SetInputValue("계좌번호", strAccount);
            axKFOpenAPI.SetInputValue("비밀번호", strAccPwd);
            axKFOpenAPI.SetInputValue("비밀번호입력매체", "00");
            axKFOpenAPI.SetInputValue("통신주문구분", "AP");

            int iResCode = axKFOpenAPI.CommRqData("RQ_12", "opw30023", "", "0112");     //해외파생지정청산대상조회
            WriteLog("<<RequestRequidateOrder");

            if (iResCode == 0)
                return CONSTATE.SUCCESSS;
            return (CONSTATE)iResCode;
        }

        private CONSTATE RequestOutstandOrder()
        {

            if (this.CurrentUserAccount == null)
                return CONSTATE.NO_LOGIN;

            if (Math.Abs(Environment.TickCount - m_tickOrderList) < 500)
                return CONSTATE.SUCCESSS;

            OrderList.RemoveAll(o => o.OrderType == "미체결");

            m_tickOrderList = Environment.TickCount;
            string strAccount = this.CurrentUserAccount.UserAccountId;
            string strAccPwd = UserPassword;

            axKFOpenAPI.SetInputValue("계좌번호", strAccount);
            axKFOpenAPI.SetInputValue("비밀번호", strAccPwd);
            axKFOpenAPI.SetInputValue("비밀번호입력매체", "00");
            axKFOpenAPI.SetInputValue("종목코드", " ");
            axKFOpenAPI.SetInputValue("통화코드", " ");
            axKFOpenAPI.SetInputValue("매도수구분", " ");

            int iResCode = axKFOpenAPI.CommRqData("RQ_6", "opw30001", "", "0112");     //미체결내역조회

            if (iResCode == 0)
                return CONSTATE.SUCCESSS;
            return (CONSTATE)iResCode;
        }

        /// <summary>
        /// //////////////////
        /// </summary>
        /// <param name="sValuation"></param>
        private void OnReceiveValuation(string sValuation, string sMValue)
        {
            if (sValuation.Length == 60)
            {
                string sBalance = sValuation.Substring(15, 15);
                string sTotalValuation = sValuation.Substring(30, 15);
                string sTotalProfit = sValuation.Substring(45, 15);
                double lBalance = 0, lValuation = 0, lProfit = 0;
                try
                {
                    lBalance = long.Parse(sBalance);
                    lValuation = long.Parse(sTotalValuation) / 100.0;
                    lProfit = long.Parse(sTotalProfit) / 100.0;
                }
                catch (Exception)
                {
                    return;
                }
                if (lBalance >= 0 && this.CurrentUserAccount != null)
                {
                    this.CurrentUserAccount.Balance = lBalance - lValuation;
                    DayProfitLoss.TotalProfit = (long)lProfit;
                    this.ValuationList[0].TotalValuation = lValuation;
                    this.ValuationList[0].TotalProfit = lProfit;
                    this.ValuationList[0].CurrentProfit = lProfit + lValuation;

                    if(!Settings.Default.SignalSiteOn)
                        OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.VALUATION);
                    WriteLog(String.Format("OnReceiveValuation Balance={0}, Valuation={1}, Profit={2}", lBalance, lValuation, lProfit));
                }

                if (sMValue.Length > 180)
                {
                    try
                    {
                        string sCurrencyCode = sMValue.Substring(0, 3).Trim();   //통화코드

                        //WriteLog("OnReceiveExchangeRate" + sCurrencyCode);

                        if (sCurrencyCode == "USD")
                        {
                            sMValue = sMValue.Substring(3+135);
                            //WriteLog("OnReceiveExchangeRate" + sRateValue);

                            string sFCurrent = sMValue.Substring(0, 15).Trim();   //외화예수금//sRateValue.Substring(19, 12).Trim();   //외화예수금
                            double lFCurent = double.Parse(sFCurrent);

                            if (lFCurent > 0)
                                EXCH_RATE = Math.Round(lBalance * 100 / lFCurent, 1);
                            WriteLog("OnReceiveValuation=" + EXCH_RATE);

                            ParseItemlistMsg();
                        }


                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        private void OnReceiveQuote(Quote quote)
        {
            SetQuoteInfo(quote);
            SetTotalQuoteInfo(quote);
            if (!Settings.Default.SignalSiteOn)
                OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.QUOTE);
//             else
//                 OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.QUOTE_SIGNAL);
        }

        private void OnReceiveCurrent(Current current)
        {
            try { 
                if(Math.Abs(Environment.TickCount - m_tickCurrent) > 50)
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
                                        orderInfo.CurrentPrice = current.CurrentPrice.ToString();
                                        continue;
                                    }

                                    if (double.Parse(orderInfo.CurrentPrice) != current.CurrentPrice)
                                    {
                                        orderInfo.CurrentPrice = string.Format(Settings.Default.PriceFormat, current.CurrentPrice);
                                        dAveragePrice = double.Parse(orderInfo.AveragePrice);
                                        dAveragePriceSum += dAveragePrice;
                                        orderInfo.Valuation = orderInfo.TradeTypeNo == "1" ?
                                            ((dAveragePrice - current.CurrentPrice) / CurItemSymbol.OverTick * CurItemSymbol.ValueTick * CurItemSymbol.Exchange * orderInfo.OrderQty) :
                                            ((current.CurrentPrice - dAveragePrice) / CurItemSymbol.OverTick * CurItemSymbol.ValueTick * CurItemSymbol.Exchange * orderInfo.OrderQty);
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
                                ValuationList[0].Valuation = lValSum;
                                ValuationList[0].TotalValuation = lValSum;
                                if (dAveragePriceSum > 0)
                                    ValuationList[0].AverageUnitPrice = dAveragePriceSum / nOrderCnt;
                                //ValuationList[0].TotalProfit = DayProfitLoss.TotalProfit;
                                ValuationList[0].CurrentProfit = ValuationList[0].TotalProfit + lValSum;

                            }
                            else if (nOrderCnt < 1)
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

            if(!Settings.Default.SignalSiteOn)
                OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.CURRENT); 
            else OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.CURRENT_SIGNAL);
        }

        private void OnRecieveDChartData(bool bDChart, string sTrCode, string sSValue, string sMValues)
        {

            if (sMValues == null)
                return;
            if (sMValues.Length < 140 || sMValues.Length % 140 != 0)
                return;
            int nLen = sMValues.Length / 140;
            WriteLog("<KFOpen> OnRecieveDChartData sTrCode = " + sTrCode);
            // Trace.TraceInformation("<KFOpen> OnRecieveDChartData = " + nLen);

            CtrlProperty.SetValueRate(Common.GetPrecisionRate(ItemSymbol, CurItemSymbol.Precision), Common.GetValueFormat(ItemPrecision + 1), (float)CurItemSymbol.OverTick);
            try
            {
                bool bMin = false;
                if (sTrCode == "opc10001" || sTrCode == "opc10002")
                    bMin = true;

                DItem newDItem = null;
                CItem newCItem = null;
                int nConc = 0;
                float fCurPrice = 0, fStartPrice = 0, fHighPrice = 0, fLowPrice = 0;
                string sDateTime = "";
                DateTime dtStart = DateTime.MinValue, dtEnd = DateTime.MinValue;

                int nLastTick = 0;
                if (sSValue.Trim().Length > 0)
                {
                    nLastTick = int.Parse(sSValue.Trim());
                }
                int nTickCnt = 0;
                
                lock (CtrlProperty._DItemList) lock(CtrlProperty._CItemList)
                {
                    nLen = nLen > 300 ? 300 : nLen;
                    CtrlProperty._DItemList.Clear();
                    CtrlProperty._CItemList.Clear();
                    
                    for (int iRow = nLen - 1; iRow >= 0; iRow--)
                    {

                        if (bMin)
                        {
                            fCurPrice = float.Parse(sMValues.Substring(iRow * 140, 20).Trim());
                            nConc = int.Parse(sMValues.Substring(iRow * 140 + 20, 20).Trim());
                            sDateTime = sMValues.Substring(iRow * 140 + 40, 20).Trim();
                            fStartPrice = float.Parse(sMValues.Substring(iRow * 140 + 60, 20).Trim());
                            fHighPrice = float.Parse(sMValues.Substring(iRow * 140 + 80, 20).Trim());
                            fLowPrice = float.Parse(sMValues.Substring(iRow * 140 + 100, 20).Trim());
                            if (sDateTime.Length == 14)
                            {
                                dtStart = DateTime.ParseExact(sDateTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                                dtEnd = CtrlProperty.GetEndTime(CtrlProperty._DTimeType, CtrlProperty._DTimeUnitAmt, dtStart, true);
                            }
                        }
                        else
                        {
                            fCurPrice = float.Parse(sMValues.Substring(iRow * 140, 20).Trim());
                            fStartPrice = float.Parse(sMValues.Substring(iRow * 140 + 20, 20).Trim());
                            fHighPrice = float.Parse(sMValues.Substring(iRow * 140 + 40, 20).Trim());
                            fLowPrice = float.Parse(sMValues.Substring(iRow * 140 + 60, 20).Trim());
                            sDateTime = sMValues.Substring(iRow * 140 + 80, 20).Trim();
                            if (sDateTime.Length == 8)
                            {
                                dtStart = DateTime.ParseExact(sDateTime, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                                dtEnd = CtrlProperty.GetEndTime(CtrlProperty._DTimeType, CtrlProperty._DTimeUnitAmt, dtStart, true);
                            }
                            nConc = int.Parse(sMValues.Substring(iRow * 140 + 100, 20).Trim());
                        }
                        if (iRow == 0)
                            nTickCnt = nLastTick;
                        else nTickCnt = (int)CtrlProperty._DTimeUnitAmt;

                        newDItem = new DItem(fStartPrice * CtrlProperty._nValueRate, fCurPrice * CtrlProperty._nValueRate, fLowPrice * CtrlProperty._nValueRate, fHighPrice * CtrlProperty._nValueRate,
                        CtrlProperty.GetTimeStamp(dtStart), CtrlProperty.GetTimeStamp(dtEnd), CtrlProperty._DItemList.Count(), nTickCnt, nConc);
                        CtrlProperty._DItemList.Add(newDItem);
                        
                        newCItem = new CItem(CtrlProperty.GetTimeStamp(dtStart), CtrlProperty.GetTimeStamp(dtEnd), nTickCnt, nConc);
                        CtrlProperty._CItemList.Add(newCItem);

                    }
                }
                
            }
            catch (Exception) { }

        }
        private void OnRecieveRChartData(string sTrCode, string sSValue, string sMValues)
        {
            
            if (sMValues == null)
                return;
            if (sMValues.Length < 140 || sMValues.Length % 140 != 0)
                return;
            int nLen = sMValues.Length / 140;
            CtrlProperty.SetValueRate(Common.GetPrecisionRate(ItemSymbol, CurItemSymbol.Precision), Common.GetValueFormat(ItemPrecision + 1), (float)CurItemSymbol.OverTick);
            WriteLog("<KFOpen> OnRecieveRChartData sTrCode = " + sTrCode);
            // Trace.TraceInformation("<KFOpen> OnRecieveRChartData = " + nLen);

            try
            {
                bool bMin = false;
                if (sTrCode == "opc10001" || sTrCode == "opc10002")
                    bMin = true;
                
                RItem itemNew = null;
                int nConc = 0;
                float fCurPrice = 0, fStartPrice = 0, fHighPrice = 0, fLowPrice = 0 ; 
                string sDateTime = "";
                DateTime dtStart = DateTime.MinValue, dtEnd = DateTime.MinValue;

                int nLastTick = 0;
                if(sSValue.Trim().Length > 0)
                {
                    nLastTick = int.Parse(sSValue.Trim());
                }
                int nTickCnt = 0;
                lock (CtrlProperty._RItemList) {
                    CtrlProperty._RItemList.Clear();
                    for (int iRow = nLen-1; iRow >= 0 ; iRow--)
                    {

                        if (bMin)
                        {
                            fCurPrice = float.Parse(sMValues.Substring(iRow * 140, 20).Trim());
                            nConc = int.Parse(sMValues.Substring(iRow * 140 + 20, 20).Trim());
                            sDateTime = sMValues.Substring(iRow * 140 + 40, 20).Trim();
                            fStartPrice = float.Parse(sMValues.Substring(iRow * 140 + 60, 20).Trim());
                            fHighPrice = float.Parse(sMValues.Substring(iRow * 140 + 80, 20).Trim());
                            fLowPrice = float.Parse(sMValues.Substring(iRow * 140 + 100, 20).Trim());
                            if (sDateTime.Length == 14)
                            {
                                dtStart = DateTime.ParseExact(sDateTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                                dtEnd = CtrlProperty.GetEndTime(CtrlProperty._RTimeType, CtrlProperty._RTimeUnitAmt, dtStart, true);
                             }
                         } else
                        {
                            fCurPrice = float.Parse(sMValues.Substring(iRow * 140, 20).Trim());                            
                            fStartPrice = float.Parse(sMValues.Substring(iRow * 140 + 20, 20).Trim());
                            fHighPrice = float.Parse(sMValues.Substring(iRow * 140 + 40, 20).Trim());
                            fLowPrice = float.Parse(sMValues.Substring(iRow * 140 + 60, 20).Trim());
                            sDateTime = sMValues.Substring(iRow * 140 + 80, 20).Trim();
                            if (sDateTime.Length == 8)
                            {
                                dtStart = DateTime.ParseExact(sDateTime, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                                dtEnd = CtrlProperty.GetEndTime(CtrlProperty._RTimeType, CtrlProperty._RTimeUnitAmt, dtStart, true);
                            }
                            nConc = int.Parse(sMValues.Substring(iRow * 140 + 100, 20).Trim());
                        }
                        if (iRow == 0)
                            nTickCnt = nLastTick;
                        else nTickCnt = (int)CtrlProperty._RTimeUnitAmt;
                        itemNew = new RItem(fStartPrice * CtrlProperty._nValueRate, fCurPrice * CtrlProperty._nValueRate, fLowPrice * CtrlProperty._nValueRate, fHighPrice * CtrlProperty._nValueRate, 
                            CtrlProperty.GetTimeStamp(dtStart), CtrlProperty.GetTimeStamp(dtEnd), CtrlProperty._RItemList.Count(), nTickCnt, nConc);
                        CtrlProperty._RItemList.Add(itemNew);

                    }
                }
                OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.REDRAW);
                
            }
            catch (Exception) { }
            
        }
        /// <summary>
        /// Receive Currency Rate
        /// </summary>
        /// <param name="sRateValue"></param>
        private void OnReceiveExchangeRate(string sRateValue)
        {
            // WriteLog("OnReceiveExchangeRate()");

            if (sRateValue == null)
                return;
            //"USD미국달러            000009979000000009974500000000000000000000009974500000000000000000000000000000000000000000000000126783195"
            WriteLog("OnReceiveExchangeRate() len=" + sRateValue.Length +" value="+ sRateValue);
            if (sRateValue.Length < 110)
                return;

            string pattern = @"([A-Za-z]+)\S+\s+(\d+)";
            string usdRate = "";
            foreach (Match match in Regex.Matches(sRateValue, pattern, RegexOptions.IgnoreCase))
            {
                if (match.Groups[1].Value == "USD")
                    usdRate = match.Value;
            }

            if (usdRate.Length == 0)
                return;

            sRateValue = usdRate;
            try
            {
                string sCurrencyCode = sRateValue.Substring(0, 3).Trim();   //통화코드

                WriteLog("OnReceiveExchangeRate=" + sCurrencyCode);

                if (sCurrencyCode == "USD")
                {
                    sRateValue = sRateValue.Substring(sRateValue.Length - 105);
                    //WriteLog("OnReceiveExchangeRate" + sRateValue);

                    string sFCurrent = sRateValue.Substring(0, 12).Trim();   //외화예수금//sRateValue.Substring(19, 12).Trim();   //외화예수금
                    string sKCurrent = sRateValue.Substring(90, 15).Trim();   //외화예수금//sRateValue.Substring(109, 15).Trim();   //외화예수금

                    double lFCurent = double.Parse(sFCurrent);
                    double lKCurent = double.Parse(sKCurrent);


                    if(lFCurent > 0 && lKCurent > 0)
                        EXCH_RATE = Math.Round(lKCurent * 100 / lFCurent, 1);
                    WriteLog("OnReceiveExchangeRate=" + EXCH_RATE);


                    ParseItemlistMsg();
                }
                

            }
            catch (Exception)
            {

            }

        }

        private void OnReceiveRequidateOrder(string sTrCode, string sRQName)
        {
            
            int nRowCnt = axKFOpenAPI.GetRepeatCnt(sTrCode, sRQName);
            WriteLog(">>OnReceiveRequidateOrder cnt="+nRowCnt);

            try
            {
                double dAveragePrice = 0;

                for (int iRow=0; iRow < nRowCnt; iRow++)
                {
                    string sAveragePrice = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "매입표시가격").Trim();
                    dAveragePrice = double.Parse(sAveragePrice);

                    string sSymbol = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "종목코드").Trim();
                    if (sSymbol != ItemSymbol)
                        continue;
                    if (OrderList.FirstOrDefault(o => double.Parse(o.AveragePrice) == dAveragePrice && o.OrderType == "체결") != null)
                        continue;

                    string sTradeTypeNo = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "매도수구분").Trim();
                    string sQty = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "주문수량").Trim();
                    int nQty = int.Parse(sQty);
                    string sCurrentPrice = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "현재표시가격").Trim();
                    string sValuation = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "평가손익").Trim();
                    long lValuation = (long)Math.Round(long.Parse(sValuation) * EXCH_RATE / 100);
                    string sFCMCode = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "FCM코드").Trim();
                    string sSymbolName = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "종목명").Trim();
                    string sOrderPrice = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "주문표시가격").Trim();
                    string sStopPrice = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "STOP표시가격").Trim();
                    string sOrderNo = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "주문번호").Trim();
                    string sErrorCode = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "에러코드").Trim();
                    string sOrderMsg = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "처리메세지").Trim();
                    string sOrderDate = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "매입일자").Trim();


                    OrderInfo orderInfo = new OrderInfo
                    {
                        OrderType = "체결",
                        Symbol = sSymbol,
                        Qty = string.Format("{0}[{1}]", sTradeTypeNo == "1" ? "매도" : "매수", nQty),
                        AveragePrice = sAveragePrice,
                        MaxAveragePrice = Math.Round(dAveragePrice, 6),
                        CurrentPrice = sCurrentPrice,
                        StartCciPrice = -10000,
                        Valuation = lValuation,
                        Action = "청산",
                        TradeType = sTradeTypeNo == "1" ? TRADETYPE.SELL : TRADETYPE.BUY,
                        FCMCode = sFCMCode,
                        SymbolName = sSymbolName,
                        TradeTypeNo = sTradeTypeNo,
                        OrderQty = nQty,
                        OrderPrice = sOrderPrice,
                        StopPrice = sStopPrice,
                        OrderNo = sOrderNo,
                        OrderDate = sOrderDate,
                        ErrorCode = sErrorCode,
                        OrderMsg = sOrderMsg
                    };
                    this.OrderList.Add(orderInfo);
                    
                }
                SetUnliquidationPosition(OrderList);
                OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.ORDER);
            }
            catch(Exception)
            {

            }
        }


        private void OnReceiveOutstandOrder(string sTrCode, string sRQName)
        {
            int nRowCnt = axKFOpenAPI.GetRepeatCnt(sTrCode, sRQName);
            WriteLog(">>OnReceiveOutstandOrder cnt=" + nRowCnt);

            try
            {
                double dAveragePrice = 0;

                for (int iRow = 0; iRow < nRowCnt; iRow++)
                {
                    string sOrderNo = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "주문번호").Trim();
                    string sSymbol = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "종목코드").Trim();
                    if (sSymbol != ItemSymbol)
                        continue;
                    if (OrderList.FirstOrDefault(o => o.OrderNo == sOrderNo && o.OrderType == "미체결") != null)
                        continue;

                    string sOrderKind = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "주문유형").Trim();
                    string sTradeTypeNo = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "매도수구분").Trim();
                    string sQty = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "주문수량").Trim();
                    int nQty = int.Parse(sQty);
                    string sAveragePrice = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "주문표시가격").Trim();
                    string sStopPrice = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "조건표시가격").Trim();
                    string sOrderState = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "상태구분").Trim();
                    string sOrderTime = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "주문시각").Trim();
                    string sOrderOrgNo = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "원주문번호").Trim();
                    dAveragePrice = double.Parse(sAveragePrice);

                    DateTime dtOrderTime = DateTime.Now;
                    if(sOrderTime.Length == 14)
                    {
                        dtOrderTime = DateTime.ParseExact(sOrderTime, "MM/dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    }

                    OrderInfo orderInfo = new OrderInfo
                    {
                        OrderType = "미체결",
                        Symbol = sSymbol,
                        Qty = string.Format("{0}[{1}]", sTradeTypeNo == "1" ? "매도" : "매수", nQty),
                        AveragePrice = sAveragePrice,
                        MaxAveragePrice = Math.Round(dAveragePrice, 6),
                        CurrentPrice = Current != null ? Current.CurrentPrice.ToString(): "0",
                        Valuation = 0L,
                        Action = "취소",
                        TradeType = sTradeTypeNo == "1" ? TRADETYPE.SELL:TRADETYPE.BUY,
                        TradeTypeNo = sTradeTypeNo,
                        OrderQty = nQty,
                        OrderTime = Environment.TickCount,
                        StopPrice = sStopPrice,
                        OrderNo = sOrderNo,
                        
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
                try
                {
                    if(Current != null)
                    this.CurrentPriceRow = this.FindQuoteInfo(this.QuoteList, Current.CurrentPrice);
                }
                catch (Exception)
                {

                }
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

        private void ParseItemlistMsg()
        {
            if (PrdList.Count > 0) return;
            PrdList.Clear();

            string[] items; 
            if (Settings.Default.SignalSiteOn)
            {
                items = new string[] { "NQ", "ES", "6E", "6B", "6J", "6A", "6C" };
            } else
            {
                items = new string[] { "NQ", "ES", "6E", "6B", "6J", "6A", "6C", "6M", "6S", "6L", "6N", "GD", "GF", "HE", "LE", "NKD", "RTY", "RS1", "SR3" }; //19
            }

            string futureItemList = axKFOpenAPI.GetGlobalFutureItemlist();
            // WriteLog(string.Format("FutureItemList={0}", futureItemList));

            string[] futureItems = futureItemList.Split(';');
            double dTemp = 0;
            int nTemp = 0;
            string log = "";
            PrdInfo newPrd;
            ItemSymbolInfo newItem;

            foreach (string item in futureItems)
            {

                if (Settings.Default.SignalSiteOn && !Array.Exists(items, it => it == item))
                    continue;

                newPrd = new PrdInfo();
                newPrd.Code = item;
                if (item == "6M")
                {
                    newPrd.Name = "멕시코 페소";
                } else if (item == "6S")
                {
                    newPrd.Name = "스위스 프랑";
                } else if (item == "6L")
                {
                    newPrd.Name = "브라질 헤알";
                }
                else if (item == "NKD")
                {
                    newPrd.Name = "니케이225 달러";
                }
                else if (item == "EMD")
                {
                    newPrd.Name = "미니 S&P MidCap 400";
                }
                else if (item == "6C")
                {
                    newPrd.Name = "캐나다 달러";
                }
                else if (item == "6B")
                {
                    newPrd.Name = "파운드";
                }
                else if (item == "6A")
                {
                    newPrd.Name = "호주달러";
                }
                else if (item == "RTY")
                {
                    newPrd.Name = "미니 Russell 2000";
                }
                else if (item == "GD")
                {
                    newPrd.Name = "S&P GSCI";
                }
                else if (item == "6E")
                {
                    newPrd.Name = "유로 FX";
                }
                else if (item == "M6A")
                {
                    newPrd.Name = "Micro AUD";
                }
                else if (item == "RS1")
                {
                    newPrd.Name = "미니 Russell 1000";
                }
                else if (item == "M6B")
                {
                    newPrd.Name = "Micro GBP";
                }
                else if (item == "LE")
                {
                    newPrd.Name = "Live Cattle";    //생우
                }
                else if (item == "GF")
                {
                    newPrd.Name = "Feeder Cattle";    //비육우
                }
                else if (item == "HE")
                {
                    newPrd.Name = "Lean Hogs";      //돈육
                }
                else if (item == "ES")
                {
                    newPrd.Name = "미니 S&P 500";      
                }
                else if (item == "MCD")
                {
                    newPrd.Name = "Micro CAD";
                }
                else if (item == "6J")
                {
                    newPrd.Name = "일본엔";
                }
                else if (item == "NQ")
                {
                    newPrd.Name = "미니 나스닥 100";
                }
                else if (item == "MNQ")
                {
                    newPrd.Name = "Micro 미니 나스닥 100";
                }
                else if (item == "M6E")
                {
                    newPrd.Name = "Micro 유로";
                }
                else if (item == "M2K")
                {
                    newPrd.Name = "Micro 미니 Russell 2000";
                }
                else if (item == "E7")
                {
                    newPrd.Name = "미니 유로 FX";
                }
                else if (item == "SR3")
                {
                    newPrd.Name = "3-Month SOFR";
                }
                else if (item == "MSF")
                {
                    newPrd.Name = "Micro CHF";
                }
                else if (item == "MES")
                {
                    newPrd.Name = "Micro 미니 S&P 500";
                }
                else if (item == "6N")
                {
                    newPrd.Name = "뉴질랜드 달러";
                } else
                {
                    newPrd.Name = item;
                }
                newPrd.ItemList = new List<ItemSymbolInfo>();
                string futureCodeList = axKFOpenAPI.GetGlobalFutureCodelist(item);
                string[] futureCodes = futureCodeList.Split(';');
                WriteLog(string.Format(">>>Item={0} codeCnt={1}", item, futureCodes.Length));
                foreach(string code in futureCodes)
                {
                    string codeInfo = axKFOpenAPI.GetGlobalFutOpCodeInfoByCode(code);
                    FutureObject futureObject = new FutureObject
                    {
                        stk_code = codeInfo.Substring(0, 12).Trim(),    //F0NQU23
                        arti_code = codeInfo.Substring(12, 6).Trim(),   //NQ
                        arti_hnm = codeInfo.Substring(18, 40).Trim(),   //Mini NASDAQ 100(23.09)
                        arti_tp = codeInfo.Substring(58, 3).Trim(),     //IDX
                        crnc_code = codeInfo.Substring(61, 3).Trim(),   //USD
                        tick_unit = codeInfo.Substring(64, 15).Trim(),  //0.25
                        tick_value = codeInfo.Substring(79, 15).Trim(), //5
                        deal_unit = codeInfo.Substring(94, 15).Trim(),  //20
                        deal_mtal = codeInfo.Substring(109, 15).Trim(), //20
                        ntt_code = codeInfo.Substring(124, 1).Trim(),   //2
                        ntt_calc_unit = codeInfo.Substring(125, 15).Trim(), //1
                        frgn_exch_code = codeInfo.Substring(140, 10).Trim(),    //CME
                        expr_dt = codeInfo.Substring(150, 8).Trim(),    //20230915
                        fprc = codeInfo.Substring(158, 10).Trim(),      //0.010000
                        gubun = codeInfo.Substring(168, 1).Trim(),      //1
                        atv_code = codeInfo.Substring(169, 1).Trim(),   //1
                        pre_gvol = codeInfo.Substring(170, 12).Trim(),  //513589
                    };

                    newItem = new ItemSymbolInfo();
                    newItem.Symbol = futureObject.stk_code.Substring(2);        //F0NQU23             
                    if (int.TryParse(futureObject.ntt_code, out nTemp))
                    {
                        newItem.Precision = nTemp;
                    }
                    if (double.TryParse(futureObject.tick_unit, out dTemp))
                    {
                        newItem.OverTick = dTemp;
                    }
                    if (double.TryParse(futureObject.tick_value, out dTemp))
                    {
                        newItem.ValueTick = dTemp;
                    }
                    newItem.Exchange = EXCH_RATE;
                    newItem.ItemName = futureObject.arti_hnm;
                    //if(futureObject.arti_hnm.IndexOf("Euro FX") == 0)
                    //{
                    //    newItem.ItemName = newItem.ItemName.Replace("Euro FX", "유로");
                    //} else if (futureObject.arti_hnm.IndexOf("British Pound") == 0)
                    //{
                    //    newItem.ItemName = newItem.ItemName.Replace("British Pound", "파운드");
                    //}
                    //else if (futureObject.arti_hnm.IndexOf("Japanese Yen") == 0)
                    //{
                    //    newItem.ItemName = newItem.ItemName.Replace("Japanese Yen", "일본엔");
                    //}
                    //else if (futureObject.arti_hnm.IndexOf("Australian Dollar") == 0)
                    //{
                    //    newItem.ItemName = newItem.ItemName.Replace("Australian Dollar", "호주달러");
                    //}
                    //else if (futureObject.arti_hnm.IndexOf("Mini S&P 500") == 0)
                    //{
                    //    newItem.ItemName = newItem.ItemName.Replace("Mini S&P 500", "미니S&P");
                    //}
                    //else if (futureObject.arti_hnm.IndexOf("Mini NASDAQ 100") == 0)
                    //{
                    //    newItem.ItemName = newItem.ItemName.Replace("Mini NASDAQ 100", "나스닥");
                    //}
                    //else if (futureObject.arti_hnm.IndexOf("Canadian Dollar") == 0)
                    //{
                    //    newItem.ItemName = newItem.ItemName.Replace("Canadian Dollar", "캐나다 달러");
                    //}
                    //else if (futureObject.arti_hnm.IndexOf("Mexican pesos") == 0)
                    //{
                    //    newItem.ItemName = newItem.ItemName.Replace("Mexican pesos", "멕시코 페소");
                    //}
                    //else if (futureObject.arti_hnm.IndexOf("Swiss Franc") == 0)
                    //{
                    //    newItem.ItemName = newItem.ItemName.Replace("Swiss Franc", "스위스 프랑");
                    //}
                    //else if (futureObject.arti_hnm.IndexOf("Brazilian Real") == 0)
                    //{
                    //    newItem.ItemName = newItem.ItemName.Replace("Brazilian Real", "브라질 헤알");
                    //}
                    //else if (futureObject.arti_hnm.IndexOf("New Zealand Dollars") == 0)
                    //{
                    //    newItem.ItemName = newItem.ItemName.Replace("New Zealand Dollars", "뉴질랜드 달러");
                    //}
                    //else if (futureObject.arti_hnm.IndexOf("Feeder Cattle") == 0)
                    //{
                    //    newItem.ItemName = newItem.ItemName.Replace("Feeder Cattle", "비육우");
                    //}
                    //else if (futureObject.arti_hnm.IndexOf("Lean Hogs") == 0)
                    //{
                    //    newItem.ItemName = newItem.ItemName.Replace("Lean Hogs", "돈육");
                    //}
                    //else if (futureObject.arti_hnm.IndexOf("Live Cattle") == 0)
                    //{
                    //    newItem.ItemName = newItem.ItemName.Replace("Live Cattle", "생우");
                    //}
                    //else if (futureObject.arti_hnm.IndexOf("Nikkei 225 Dollar") == 0)
                    //{
                    //    newItem.ItemName = newItem.ItemName.Replace("Nikkei 225 Dollar", "니케이225 달러");
                    //}

                    newPrd.ItemList.Add(newItem);

                    if (ItemSymbol.Length > 0 && ItemSymbol == newItem.Symbol)
                    {
                        CurPrd = newPrd;
                        ItemSymbol = newItem.Symbol;
                        CurItemSymbol = newItem;
                        ItemList = newPrd.ItemList;
                    }
                    else if (ItemSymbol.Length < 1 && newItem.Symbol.IndexOf("NQ") >= 0)
                    {
                        CurPrd = newPrd;
                        ItemSymbol = newItem.Symbol;
                        CurItemSymbol = newItem;
                        ItemList = newPrd.ItemList;
                    }

                    log = string.Format("{0} | {1} | {2} | {3} | {4} | {5} | {6} | {7} | {8} | {9} | {10} | {11} | {12} | {13} | {14} | {15} | {16}",
                        futureObject.stk_code,
                        futureObject.arti_code,
                        futureObject.arti_hnm,
                        futureObject.arti_tp,
                        futureObject.crnc_code,
                        futureObject.tick_unit,
                        futureObject.tick_value,
                        futureObject.deal_unit,
                        futureObject.deal_mtal,
                        futureObject.ntt_code,
                        futureObject.ntt_calc_unit,
                        futureObject.frgn_exch_code,
                        futureObject.expr_dt,
                        futureObject.fprc,
                        futureObject.gubun,
                        futureObject.atv_code,
                        futureObject.pre_gvol
                        );
                    // WriteLog(log);
                    WriteLog("ParseItemlistMsg() Item=" + newItem.Symbol + " Precision=" + newItem.Precision +
                        " OverTick=" + newItem.OverTick + " Exchange=" + newItem.Exchange + " ValueTick=" + newItem.ValueTick);
                    //if(!Settings.Default.SignalSiteOn)
                    //    break;
                }
                PrdList.Add(newPrd);
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
                // WriteLog(string.Format("ItemSymbol={0}", ItemSymbol));
                if (!Settings.Default.SignalSiteOn)
                {
                    Settings.Default.PriceFormat = Common.GetPriceFormat(ItemPrecision);
                    OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.PREPAREITEM);
                }
            }
        }


        private void KF_OnReceiveTrData(object sender, AxKFOpenAPILib._DKFOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            //string sScrNo, string sRQName, string sTrCode, string sRecName, string sPreNext
            //string sScrNo = e.sScrNo;       //CommRqData의 sScrNo
            string sRQName = e.sRQName;     //CommRqData의 sRQName
            string sTrCode = e.sTrCode;     //CommRqData의 sTrCode		
            string sRecName = e.sRecordName;
            //string sPreNext = e.sPreNext;
            
            try
            {

                string tSValue = "";
                string tMValue = "";

                if (sRQName == "RQ_0")      //DChart
                {
                    tSValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 1); //싱글
                    tMValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 2); //멀티
                    OnRecieveDChartData(true, sTrCode, tSValue, tMValue);
                }
                else if (sRQName == "RQ_1")  //RChart
                {

                    tSValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 1); //싱글
                    tMValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 2); //멀티
                    OnRecieveRChartData(sTrCode, tSValue, tMValue);
                }
                else if (sRQName == "RQ_2")    //CChart					
                {

                    tSValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 1); //싱글
                    tMValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 2); //멀티
                    OnRecieveDChartData(false, sTrCode, tSValue, tMValue);

                }
                else if (sRQName == "RQ_3") //원화외화예수금잔액조회			opw60003
                {
                    //tValues = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 1); //싱글
                    tMValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 2); //멀티
                    OnReceiveExchangeRate(tMValue);
                }
                else if (sRQName == "RQ_4")		//청산
                {
                    tSValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 1); //싱글
                    tMValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 2); //멀티
                    WriteLog("청산상태: sValue=" + tSValue + " mValue=" + tMValue);

                    if(tSValue.Trim().Length == 0)
                    {
                        OnFutureSiteLogEvent("[청산] 요청 실패");
                    }

                    // RequestOrderList(true, true);

                }
                else if (sRQName == "RQ_5")     //주문, 취소상태
                {
                    tSValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 1); //싱글(거래번호)
                    tMValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 2); //멀티
                    if(tSValue.Trim().Length == 0)
                    {
                        OnFutureSiteLogEvent("[주문] 요청 실패");
                    }
                    WriteLog("주문상태: sValue=" + tSValue + " mValue=" + tMValue);

                }
                else if (sRQName == "RQ_6")     //미체결내역조회				opw30001
                {
                    //tValues = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 1); //싱글
                    //tValues = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 2); //멀티
                    OnReceiveOutstandOrder(sTrCode, sRQName);
                }
                else if (sRQName == "RQ_7")     //미결제잔고내역조회			opw30003
                {
                    //tValues = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 1); //싱글
                    //tValues = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 2); //멀티
                }
                else if (sRQName == "RQ_8")     //주문체결내역조회			opw30005		
                {
                    //tValues = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 2); //멀티
                }
                else if (sRQName == "RQ_9")     //예수금및증거금현황조회		opw30009
                {
                    tSValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 1); //싱글
                    tMValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 2); //멀티
                    OnReceiveValuation(tSValue, tMValue);
                }
                else if (sRQName == "RQ_10")     //증거금상세조회				opw30010
                {
                    //tValues = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 1); //싱글
                                                                                //tValues = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 2); //멀티
                }
                else if (sRQName == "RQ_11")     //미결제내역상세조회			opw30012
                {
                    //tValues = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 2); //멀티
                }
                else if (sRQName == "RQ_12")     //해외파생지정청산대상조회	opw30023 ; 체결리스트
                {
                    tMValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 2); //멀티
                    OnReceiveRequidateOrder(sTrCode, sRQName);
                }
                else if (sRQName == "RQ_13")		//종목정보조회 1
                {
                    tSValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 1); //싱글
                    tMValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 2); //멀티
                    WriteLog("종목정보1: sValue=" + tSValue + " mValue=" + tMValue);
                }
                else if (sRQName == "RQ_14")		//종목정보조회 2
                {
                    tSValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 1); //싱글
                    tMValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 2); //멀티
                    WriteLog("종목정보2: sValue=" + tSValue + " mValue=" + tMValue);
                }

            }
            catch (Exception ex)
            {
                OnFutureSiteLogEvent(ex.Message);
            }
        }


        private void KF_OnReceiveRealData(object sender, AxKFOpenAPILib._DKFOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            string sJongmok = e.sJongmokCode;
            string sRealType = e.sRealType;
            string sRealData = e.sRealData;
            // WriteLog("실시간자료: 종목=" + sJongmok + " 자료=" + sRealType);
            if (sJongmok != ItemSymbol)
                return;
            //string sValue = "";
            if (sRealType.Trim() == "해외선물시세")
            {
                Current current = new Current();
                current.CurrentTime = axKFOpenAPI.GetCommRealData(sRealType, 20);   //체결시간

                string sCurrentPrice = axKFOpenAPI.GetCommRealData(sRealType, 10);   //현재가(진법)
                
                //sValue += axKFOpenAPI.GetCommRealData(sRealType, 140);  //현재가	
                
                string sContrast = axKFOpenAPI.GetCommRealData(sRealType, 11);   //전일대비	
                
                string sContrastPer = axKFOpenAPI.GetCommRealData(sRealType, 12);    //등락율
                
                string sSellValue = axKFOpenAPI.GetCommRealData(sRealType, 27);    //매도호가
                
                string sBuyValue = axKFOpenAPI.GetCommRealData(sRealType, 28);    //매수호가
                
                string sConclusion = axKFOpenAPI.GetCommRealData(sRealType, 15);    //체결량
                
                //sValue += axKFOpenAPI.GetCommRealData(sRealType, 13);    //누적거래량
                
                string sStartPrice = axKFOpenAPI.GetCommRealData(sRealType, 16);    //시가
                
                string sHighPrice = axKFOpenAPI.GetCommRealData(sRealType, 17);    //고가
                
                string sLowPrice = axKFOpenAPI.GetCommRealData(sRealType, 18);    //저가
                
                //sValue += axKFOpenAPI.GetCommRealData(sRealType, 25);    //전일대비기호
                
                //sValue += axKFOpenAPI.GetCommRealData(sRealType, 26);    //대비
                
                //sValue += axKFOpenAPI.GetCommRealData(sRealType, 30);    //전일거래량등락율
                
                string sCheDate = axKFOpenAPI.GetCommRealData(sRealType, 22);    //체결일자

                //sValue += axKFOpenAPI.GetCommRealData(sRealType, 761);   //영업일

                //sValue = axKFOpenAPI.GetCommRealData(sRealType, 10);    //현재가(진법)
                try
                {
                    if (sCheDate.Length == 8 && current.CurrentTime.Length == 6)
                    {
                        m_sCheDate = sCheDate;
                        current.ReceivedDate = DateTime.ParseExact(sCheDate + current.CurrentTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);

                    }
                    else return;
                
                    //m_fCurVal = Convert.ToSingle(sValue);
                    current.CurrentPrice = Math.Abs(double.Parse(sCurrentPrice));
                    current.StartPrice = Math.Abs(double.Parse(sStartPrice));
                    current.HighPrice = Math.Abs(double.Parse(sHighPrice));
                    current.LowPrice = Math.Abs(double.Parse(sLowPrice));
                    current.Contrast = double.Parse(sContrast);
                    current.ContrastPer = double.Parse(sContrastPer);
                    current.ConclusionVolume = Math.Abs(int.Parse(sConclusion));
                    if (sCurrentPrice == sBuyValue)
                        current.TradeType = TradeType.Buy;
                    else if (sCurrentPrice == sSellValue)
                        current.TradeType = TradeType.Sell;

                    if (bQutoteCreated)
                        OnReceiveCurrent(current);
                }
                catch (Exception)
                {
                    return;

                }


            }
            else if (sRealType.Trim() == "해외선물호가")
            {
                if (Settings.Default.SignalSiteOn && bQutoteCreated)
                    return;

                if (Math.Abs(Environment.TickCount - m_tickQuote) < 100)
                    return;
                m_tickQuote = Environment.TickCount;

                Quote quote = new Quote();

                quote.QuoteTime = axKFOpenAPI.GetCommRealData(sRealType, 21);    //호가시간
                
                //sValue += axKFOpenAPI.GetCommRealData(sRealType, 27);    //최우선매도호가
                //sValue += axKFOpenAPI.GetCommRealData(sRealType, 28);    //최우선매수호가
                
                string sAsk1 = axKFOpenAPI.GetCommRealData(sRealType, 41);    //매도호가1
                string sAskQty1 = axKFOpenAPI.GetCommRealData(sRealType, 61);    //매도호가잔량1
                //sValue += axKFOpenAPI.GetCommRealData(sRealType, 81);    //매도호가대비1
                string sAskCount1 = axKFOpenAPI.GetCommRealData(sRealType, 101);    //매도호가건수1
                string sBid1 = axKFOpenAPI.GetCommRealData(sRealType, 51);    //매수호가1
                string sBidQty1 = axKFOpenAPI.GetCommRealData(sRealType, 71);    //매수호가잔량1
                //sValue += axKFOpenAPI.GetCommRealData(sRealType, 91);    //매수호가대비1
                string sBidCount1 = axKFOpenAPI.GetCommRealData(sRealType, 111);    //매수호가건수1
                
                string sAsk2 = axKFOpenAPI.GetCommRealData(sRealType, 42);    //매도호가2
                string sAskQty2 = axKFOpenAPI.GetCommRealData(sRealType, 62);    //매도호가잔량2
                //sValue += axKFOpenAPI.GetCommRealData(sRealType, 82);    //매도호가대비2
                string sAskCount2 = axKFOpenAPI.GetCommRealData(sRealType, 102);    //매도호가건수2
                string sBid2 = axKFOpenAPI.GetCommRealData(sRealType, 52);    //매수호가2
                string sBidQty2 = axKFOpenAPI.GetCommRealData(sRealType, 72);    //매수호가잔량2
                //sValue += axKFOpenAPI.GetCommRealData(sRealType, 92);    //매수호가대비2
                string sBidCount2 = axKFOpenAPI.GetCommRealData(sRealType, 112);    //매수호가건수2
                
                string sAsk3 = axKFOpenAPI.GetCommRealData(sRealType, 43);    //매도호가3
                string sAskQty3 = axKFOpenAPI.GetCommRealData(sRealType, 63);    //매도호가잔량3
                //sValue += axKFOpenAPI.GetCommRealData(sRealType, 83);    //매도호가대비3
                string sAskCount3 = axKFOpenAPI.GetCommRealData(sRealType, 103);    //매도호가건수3
                string sBid3 = axKFOpenAPI.GetCommRealData(sRealType, 53);    //매수호가3
                string sBidQty3 = axKFOpenAPI.GetCommRealData(sRealType, 73);    //매수호가잔량3
                //sValue += axKFOpenAPI.GetCommRealData(sRealType, 93);    //매수호가대비3
                string sBidCount3 = axKFOpenAPI.GetCommRealData(sRealType, 113);    //매수호가건수3
                
                string sAsk4 = axKFOpenAPI.GetCommRealData(sRealType, 44);    //매도호가4
                string sAskQty4 = axKFOpenAPI.GetCommRealData(sRealType, 64);    //매도호가잔량4
                //sValue += axKFOpenAPI.GetCommRealData(sRealType, 84);    //매도호가대비4
                string sAskCount4 = axKFOpenAPI.GetCommRealData(sRealType, 104);    //매도호가건수4
                string sBid4 = axKFOpenAPI.GetCommRealData(sRealType, 54);    //매수호가4
                string sBidQty4 = axKFOpenAPI.GetCommRealData(sRealType, 74);    //매수호가잔량4
                //sValue += axKFOpenAPI.GetCommRealData(sRealType, 94);    //매수호가대비4
                string sBidCount4 = axKFOpenAPI.GetCommRealData(sRealType, 114);    //매수호가건수4
                
                string sAsk5 = axKFOpenAPI.GetCommRealData(sRealType, 45);    //매도호가5
                string sAskQty5 = axKFOpenAPI.GetCommRealData(sRealType, 65);    //매도호가잔량5
                //sValue += axKFOpenAPI.GetCommRealData(sRealType, 85);    //매도호가대비5
                string sAskCount5 = axKFOpenAPI.GetCommRealData(sRealType, 105);    //매도호가건수5
                string sBid5 = axKFOpenAPI.GetCommRealData(sRealType, 55);    //매수호가5
                string sBidQty5 = axKFOpenAPI.GetCommRealData(sRealType, 75);    //매수호가잔량5
                //sValue += axKFOpenAPI.GetCommRealData(sRealType, 95);    //매수호가대비5
                string sBidCount5 = axKFOpenAPI.GetCommRealData(sRealType, 115);    //매수호가건수5
                
                string sTotalAskQty = axKFOpenAPI.GetCommRealData(sRealType, 121);    //매도호가총잔량
                //sValue += axKFOpenAPI.GetCommRealData(sRealType, 122);    //매도호가총잔량대비
                string sTotalAskCount = axKFOpenAPI.GetCommRealData(sRealType, 123);    //매도호가총건수

                string sTotalBidQty = axKFOpenAPI.GetCommRealData(sRealType, 125);    //매수호가총잔량
                //sValue += axKFOpenAPI.GetCommRealData(sRealType, 126);    //매수호가총잔량대비
                string sTotalBidCount = axKFOpenAPI.GetCommRealData(sRealType, 127);    //매수호가총건수

                //                 sValue += axKFOpenAPI.GetCommRealData(sRealType, 137);    //호가순잔량
                //                 
                //                 sValue += axKFOpenAPI.GetCommRealData(sRealType, 128);    //순매수잔량
                //                 
                //                 sValue += axKFOpenAPI.GetCommRealData(sRealType, 600);    //매도1호가등락율
                //                 
                //                 sValue += axKFOpenAPI.GetCommRealData(sRealType, 601);    //매도2호가등락율
                //                 
                //                 sValue += axKFOpenAPI.GetCommRealData(sRealType, 602);    //매도3호가등락율
                //                 
                //                 sValue += axKFOpenAPI.GetCommRealData(sRealType, 603);    //매도4호가등락율
                //                 
                //                 sValue += axKFOpenAPI.GetCommRealData(sRealType, 604);    //매도5호가등락율
                //                 
                //                 sValue += axKFOpenAPI.GetCommRealData(sRealType, 610);    //매수1호가등락율
                //                 
                //                 sValue += axKFOpenAPI.GetCommRealData(sRealType, 611);    //매수2호가등락율
                //                 
                //                 sValue += axKFOpenAPI.GetCommRealData(sRealType, 612);    //매수3호가등락율
                //                 
                //                 sValue += axKFOpenAPI.GetCommRealData(sRealType, 613);    //매수4호가등락율
                //                 
                //                 sValue += axKFOpenAPI.GetCommRealData(sRealType, 614);    //매수5호가등락율
                //                 
                if (m_sCheDate.Length == 8 && quote.QuoteTime.Length == 6)
                {
                    quote.ReceivedDate = DateTime.ParseExact(m_sCheDate + quote.QuoteTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);

                }
                else return;
                try
                {
                    //m_fCurVal = Convert.ToSingle(sValue);
                    quote.Ask1 = Math.Abs(double.Parse(sAsk1));
                    quote.Ask2 = Math.Abs(double.Parse(sAsk2));
                    quote.Ask3 = Math.Abs(double.Parse(sAsk3));
                    quote.Ask4 = Math.Abs(double.Parse(sAsk4));
                    quote.Ask5 = Math.Abs(double.Parse(sAsk5));

                    quote.AskQty1 = int.Parse(sAskQty1);
                    quote.AskQty2 = int.Parse(sAskQty2);
                    quote.AskQty3 = int.Parse(sAskQty3);
                    quote.AskQty4 = int.Parse(sAskQty4);
                    quote.AskQty5 = int.Parse(sAskQty5);

                    quote.AskCount1 = int.Parse(sAskCount1);
                    quote.AskCount2 = int.Parse(sAskCount2);
                    quote.AskCount3 = int.Parse(sAskCount3);
                    quote.AskCount4 = int.Parse(sAskCount4);
                    quote.AskCount5 = int.Parse(sAskCount5);

                    quote.TotalAskQty = int.Parse(sTotalAskQty);
                    quote.TotalAskCount = int.Parse(sTotalAskCount);

                    quote.Bid1 = Math.Abs(double.Parse(sBid1));
                    quote.Bid2 = Math.Abs(double.Parse(sBid2));
                    quote.Bid3 = Math.Abs(double.Parse(sBid3));
                    quote.Bid4 = Math.Abs(double.Parse(sBid4));
                    quote.Bid5 = Math.Abs(double.Parse(sBid5));

                    quote.BidQty1 = int.Parse(sBidQty1);
                    quote.BidQty2 = int.Parse(sBidQty2);
                    quote.BidQty3 = int.Parse(sBidQty3);
                    quote.BidQty4 = int.Parse(sBidQty4);
                    quote.BidQty5 = int.Parse(sBidQty5);

                    quote.BidCount1 = int.Parse(sBidCount1);
                    quote.BidCount2 = int.Parse(sBidCount2);
                    quote.BidCount3 = int.Parse(sBidCount3);
                    quote.BidCount4 = int.Parse(sBidCount4);
                    quote.BidCount5 = int.Parse(sBidCount5);

                    quote.TotalBidQty = int.Parse(sTotalBidQty);
                    quote.TotalBidCount = int.Parse(sTotalBidCount);

                    
                }
                catch (Exception)
                {
                    return;

                }
                if (CurItemSymbol != null && CurItemSymbol.MidPrice == 0)
                {
                    CurItemSymbol.MidPrice = quote.Ask5;
                }
                if(bQutoteCreated)
                    OnReceiveQuote(quote);

            }

        }

        private void KF_OnReceiveChejanData(object sender, AxKFOpenAPILib._DKFOpenAPIEvents_OnReceiveChejanDataEvent e)
        {

            int nItemCnt = e.nItemCnt;
            string strFidlist = e.sFidList;
            string strGubun = e.sGubun;
            /*
			string strData, strGubunName;

            if (int.Parse(strGubun) == 0)
                strGubunName = "주문내역";
			else if (int.Parse(strGubun) == 1)
				strGubunName = "체결내역";
			else if (int.Parse(strGubun) == 3)
				strGubunName = "마진콜";
			*/
//             string[] strArrData = null, arrData = null;
//             string strData = "";
//             char[] cSplit = { ';' };
//             strArrData = strFidlist.Split(cSplit);
//             if (strArrData.Length > 0)
//                 arrData = new string[strArrData.Length];
// 
//             for (int nCol = 0; nCol < strArrData.Length - 1; nCol++)
//             {
//                 strData = axKFOpenAPI.GetChejanData(int.Parse(strArrData[nCol]));
//                 strData.Trim();
//                 arrData[nCol] = strData;
//             }

            if (int.Parse(strGubun) == 0)
            {
                string sAccountNo = axKFOpenAPI.GetChejanData(9201); //계좌번호
                string sOrderNo = axKFOpenAPI.GetChejanData(9203); //주문번호
                string sItemSymbol = axKFOpenAPI.GetChejanData(9001); //종목코드
                string sTradeType = axKFOpenAPI.GetChejanData(907); //매도수구분
                string sOrderType = axKFOpenAPI.GetChejanData(905); //주문구분
                string sOrderOrg = axKFOpenAPI.GetChejanData(904); //원주문번호
                string sItemName = axKFOpenAPI.GetChejanData(302); //종목명
                string sOrderKind = axKFOpenAPI.GetChejanData(906); //주문유형
                string sOrderCnt = axKFOpenAPI.GetChejanData(900); //주문수량
                string sOrderPrice = axKFOpenAPI.GetChejanData(901); //주문가격
                string sCondPrice = axKFOpenAPI.GetChejanData(13333); //조건가격
                string sMarkPrice = axKFOpenAPI.GetChejanData(13330); //주문표시가격
                string sComPrice = axKFOpenAPI.GetChejanData(13332); //조건표시가격
                string sLiqCnt = axKFOpenAPI.GetChejanData(902); //미체결수량
                string sOrderState = axKFOpenAPI.GetChejanData(913); //주문상태
                string sReverseOrd = axKFOpenAPI.GetChejanData(919); //반대매매여부
                string sMarketCode = axKFOpenAPI.GetChejanData(8046); //거래소코드
                string sCurrencyCode = axKFOpenAPI.GetChejanData(8043); //통화코드
                string sOrderTime = axKFOpenAPI.GetChejanData(908); //주문시간
                string sOrdCondSort = axKFOpenAPI.GetChejanData(50713); //해외주문조건구분
                string sOrdEnd = axKFOpenAPI.GetChejanData(50714); //주문조건종료일자


                WriteLog(string.Format("주문내역=>계좌번호:{0}, 주문번호:{1}, 종목코드:{2}, 매도수구분:{3}, 주문구분:{4}, 원주문번호:{5}, 종목명:{6}, 주문유형:{7}, 주문수량:{8}, 주문가격:{9}, " +
                    "조건가격:{10}, 주문표시가격:{11}, 조건표시가격:{12}, 미체결수량:{13}, 주문상태:{14}, 반대매매여부:{15}, 거래소코드:{16}, 통화코드:{17}, 주문시간:{18}, 해외주문조건:{19}, 주문조건종료일자:{20}",
                    sAccountNo, sOrderNo, sItemSymbol, sTradeType, sOrderType, sOrderOrg, sItemName, sOrderKind, sOrderCnt, sOrderPrice,
                    sCondPrice, sMarkPrice, sComPrice, sLiqCnt, sOrderState, sReverseOrd, sMarketCode, sCurrencyCode, sOrderTime, sOrdCondSort, sOrdEnd
                    ));


                RequestOrderList(false, false);
            }
            else if (int.Parse(strGubun) == 1)
            {
//                 string sAccountNo = axKFOpenAPI.GetChejanData(9201); //계좌번호
                 string sOrderNo = axKFOpenAPI.GetChejanData(9203); //주문번호
//                 string sItemSymbol = axKFOpenAPI.GetChejanData(9001); //종목코드
                 string sTradeType = axKFOpenAPI.GetChejanData(907); //매도수구분
//                 string sOrderType = axKFOpenAPI.GetChejanData(905); //주문체결구분
//                 string sMarketCode = axKFOpenAPI.GetChejanData(8046); //거래소코드
//                 string sOrderOrg = axKFOpenAPI.GetChejanData(904); //원주문번호
//                 string sItemName = axKFOpenAPI.GetChejanData(302); //종목명
//                 string sOrderKind = axKFOpenAPI.GetChejanData(906); //주문유형
//                 string sOrderCnt = axKFOpenAPI.GetChejanData(900); //주문수량
//                 string sOrderPrice = axKFOpenAPI.GetChejanData(901); //주문가격
//                 string sCondPrice = axKFOpenAPI.GetChejanData(13333); //조건가격
//                 string sMarkPrice = axKFOpenAPI.GetChejanData(13330); //주문표시가격
//                 string sComPrice = axKFOpenAPI.GetChejanData(13332); //조건표시가격
//                 
//                 string sLiqNo = axKFOpenAPI.GetChejanData(909); //체결번호
//                 string sLiqCnt = axKFOpenAPI.GetChejanData(911); //체결수량
                 string sLiqPrice = axKFOpenAPI.GetChejanData(910); //체결가격
//                 string sLiqMark = axKFOpenAPI.GetChejanData(13331); //체결표시가격
//                 string sLiqMoney = axKFOpenAPI.GetChejanData(13329); //체결금액
//                 string sCancelCnt = axKFOpenAPI.GetChejanData(13326); //거부수량
//                 string sOrderState = axKFOpenAPI.GetChejanData(913); //주문상태
//                 string sOrderRemain = axKFOpenAPI.GetChejanData(902); //주문잔량
//                 string sOrderFee = axKFOpenAPI.GetChejanData(935); //체결수수료
                 string sAddCnt = axKFOpenAPI.GetChejanData(13327); //신규수량
                 string sLiquidCnt = axKFOpenAPI.GetChejanData(13328); //청산수량
                 string sProfit = axKFOpenAPI.GetChejanData(8018); //실현손익
//                 string sCurrencyCode = axKFOpenAPI.GetChejanData(8043); //통화코드
//                 string sOrderTime = axKFOpenAPI.GetChejanData(908); //체결수신시간
//                 string sOrdCondSort = axKFOpenAPI.GetChejanData(50713); //해외주문조건구분
//                 string sOrdEnd = axKFOpenAPI.GetChejanData(50714); //주문조건종료일자
                 string sItemFee = axKFOpenAPI.GetChejanData(50717); //종목수수료
                 string sCurrentyRate = axKFOpenAPI.GetChejanData(50718); //원화환율
//                 string sProfit2 = axKFOpenAPI.GetChejanData(50719); //실현손익

                int nTemp = 0;
                int nAddCnt = 0, nLiquidCnt = 0;
                if (int.TryParse(sLiquidCnt, out nTemp))
                {
                    nLiquidCnt = nTemp;
                }
                if (int.TryParse(sAddCnt, out nTemp))
                {
                    nAddCnt = nTemp;
                }
                double dAveragePrice = 0;
                if (!double.TryParse(sLiqPrice, out dAveragePrice))
                {
                    return;
                }

//                 WriteLog(string.Format("주문내역=>계좌번호:{0}, 주문번호:{1}, 종목코드:{2}, 매도수구분:{3}, 주문체결구분:{4}, 원주문번호:{5}, 종목명:{6}, 주문유형:{7}, 주문수량:{8}, 주문가격:{9}, " +
//                     "조건가격:{10}, 주문표시가격:{11}, 조건표시가격:{12}, 거래소코드:{13}, 체결번호:{14}, 체결수량:{15}, 체결가격:{16}, 체결표시가격:{17}, 체결금액:{18}, " +
//                     "거부수량:{19}, 주문상태:{20}, 주문잔량:{21}, 체결수수료:{22}, 신규수량:{23}, 청산수량:{24}, 실현손익:{25}, 통화코드:{26}, 체결수신시간:{27}, " +
//                     "해외주문조건구분:{28}, 주문조건종료일자:{29}, 종목수수료:{30}, 원화환율:{31}, 실현손익:{32}",
//                     sAccountNo, sOrderNo, sItemSymbol, sTradeType, sOrderType, sOrderOrg, sItemName, sOrderKind, sOrderCnt, sOrderPrice,
//                     sCondPrice, sMarkPrice, sComPrice, sMarketCode, sLiqNo, sLiqCnt, sLiqPrice, sLiqMark, sLiqMoney, 
//                     sCancelCnt, sOrderState, sOrderRemain, sOrderFee, sAddCnt, sLiquidCnt, sProfit, sCurrencyCode, sOrderFee, 
//                     sOrdCondSort, sOrdEnd, sItemFee, sCurrentyRate, sProfit2
//                     ));
                WriteLog(string.Format("주문내역=>주문번호:{0}, 매도수구분:{1}, 체결가격:{2}" +
                    "신규수량:{3}, 청산수량:{4}, 실현손익:{5}, 종목수수료:{6}, 원화환율:{7} ",
                    sOrderNo, sTradeType, sLiqPrice, sAddCnt, sLiquidCnt, sProfit, sItemFee, sCurrentyRate));

                if (this.LiquidOrder != null && Math.Abs(Environment.TickCount - this.LiquidOrder.OrderTm) < 1000)
                    return;
                WriteLog(string.Format("신규수량:{0}, 청산수량:{1}", nAddCnt, nLiquidCnt));

                string orderType = "";
                int qty = 0;
                string logMsg = "";
                CONCSTATE concState = CONCSTATE.LIQUID;
                RESULTSTATE resultState = RESULTSTATE.IGNORE;
                if (nAddCnt > 0 && nLiquidCnt == 0)
                {
                    //sTradeType = 1:SELL 2:BUY
                    resultState = sTradeType == "1" ? RESULTSTATE.SELL : RESULTSTATE.BUY;
                    concState = CONCSTATE.CONCLUDE;
                    orderType = "체결";
                    qty = nAddCnt;
                    logMsg += "체결가:" + dAveragePrice.ToString();
                }
                else if (nAddCnt == 0 && nLiquidCnt > 0)
                {
                    resultState = sTradeType == "1" ? RESULTSTATE.BUY : RESULTSTATE.SELL;
                    concState = CONCSTATE.LIQUID;
                    orderType = "청산";
                    qty = nLiquidCnt;
                    logMsg += "청산가:" + dAveragePrice.ToString();
                    double dValuation = 0;
                    try
                    {
                        dValuation = (double.Parse(sProfit) - double.Parse(sItemFee)*nLiquidCnt*2) * double.Parse(sCurrentyRate);
                        logMsg += "(" + (dValuation > 0 ? "수익:" : "손실:") + string.Format("{0:N0}원)", (long)dValuation);
                    }
                    catch (Exception)
                    {
                    }
                }
                else if (nAddCnt > 0 && nLiquidCnt > 0)
                {
                    resultState = sTradeType == "1" ? RESULTSTATE.SELL : RESULTSTATE.BUY;
                    concState = CONCSTATE.RECONC;
                    orderType = "되돌림";
                    qty = nAddCnt ;
                    logMsg += "체결가:" + dAveragePrice.ToString();
                    double dValuation = 0;
                    try
                    {
                        dValuation = (double.Parse(sProfit) - double.Parse(sItemFee) * nLiquidCnt * 2) * double.Parse(sCurrentyRate);
                        logMsg += "(" + (dValuation > 0 ? "수익:" : "손실:") + string.Format("{0:N0}원)", (long)dValuation);
                    }
                    catch (Exception)
                    {
                    }
                }
                else return;

                WriteLog(logMsg);
                this.LiquidOrder = new OrderVal
                {
                    OrderType = orderType,
                    Symbol = ItemSymbol,
                    AveragePrice = string.Format(Settings.Default.PriceFormat, dAveragePrice),
                    OrderTime = this.Current.Time/*DateTime.Now*/,
                    OrderTm = Environment.TickCount,
                    ResultState = resultState,
                    ConcState = concState,
                    OrderQty = qty,
                };
                RequestOrderList(true, true);
                RequestOrderList(false, false);

                OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.LIQUID);
                if (logMsg.Length > 0)
                    OnFutureSiteLogEvent(logMsg);

            }
        }
        

    }
}
