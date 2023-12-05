using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuckyFuture.Models.ValueObjects;
using LuckyFuture.Models.DataObjects;
using System.Threading;
using LuckyFuture.Properties;
using LuckyFuture.Logic;
using ChartCtrl;
using LuckyFutureLib.Include;

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


    class KFOpen : FutureSite
    {
        public override SITETYPE Type { get => SITETYPE.KF_OPEN; }

        private Current m_current;
        private string m_sCheDate = "";
        private double EXCH_RATE = 1114.0;
        private const double TICK_RATE = 20.0;
        private int m_tickCurrent = 0;
        private int m_tickChart = 0;
        //private int m_tickOrder = 0;
        private int m_tickQuote = 0;
        private int m_tickAccount = 0;
        private bool m_bInitalize = true;
        private bool m_bNeedAcc = false;
        private readonly object _objLock = new object();

        public KFOpen()
        {
            
        }

        public KFOpen(AxKFOpenAPILib.AxKFOpenAPI axKFOpenAPI)
        {
            this.axKFOpenAPI = axKFOpenAPI;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
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

            ProductCode = "NQU23";
            
        }

        public override ERRORCODE Login()
        {
            ERRORCODE error_code = ERRORCODE.UNKNOWN_FAILED;

            this.CurrentUserAccount = new UserAccountInfo
            {
                UserAccountId = UserId,                
            };
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
                    OnFutureSiteLogEvent("로그인 해주십시오.");
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

        }

        protected override ERRORCODE LogOut()
        {
            return ERRORCODE.SUCCESS;
        }

        protected override ERRORCODE Prepare()
        {

            LoginState = LOGINSTATE.OK;
            
            if (RequestRealData() != CONSTATE.SUCCESSS)
                return ERRORCODE.PREPARE_FAILED;

            return ERRORCODE.SUCCESS;
        }

        protected override ERRORCODE Check()
        {
            
            if (!m_bNeedAcc && Environment.TickCount - m_tickAccount < 60000)
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

            if(Environment.TickCount - m_tickChart < 600000)
            {
                if (this.OrderList.Count < 1)
                    RequestChart();
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
            
            base.OnStopped(bAutoStop);
        }

        protected override void OnPrepare()
        {   
            base.OnPrepare();
            RequestOrderList(false, true);
        }


        public override bool DoSellOrder(QuoteInfo quoteInfo, int nQuantity = 1, bool bMarketPrice = false)
        {
            /*
            lock (_objLock)
            {
                if (Environment.TickCount - m_tickOrder <= 5000)
                {
                    return false;
                }
                m_tickOrder = Environment.TickCount;
            }
            */
            
            if (this.CurrentUserAccount == null || string.IsNullOrEmpty(CurrentUserAccount.UserAccountId))
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
            if (Current == null || Current.CurrentPrice < 1)
            {
                OnFutureSiteLogEvent("주문기간이 아닙니다.");
                return false;
            }

            int nOrderRange = 5;
            if (quoteInfo.Price < Current.CurrentPrice - nOrderRange || quoteInfo.Price > Current.CurrentPrice + nOrderRange)
            {
                OnFutureSiteLogEvent("주문 가격이 초과 됨");
                return false;
            }


            string strRQName = "RQ_5";
            string strScrNo = "0105";
            string strAccNo = CurrentUserAccount.UserAccountId;
            int iOrderType = 1;             //1:신규매도, 2:신규매수 3:매도취소, 4:매수취소, 5:매도정정, 6:매수정정
            string strCode = ProductCode;
            int iQty = nQuantity;
            string strPrice = "0";
            string strStopPrice = "0";      // 주문구분 3:STOP, 4:STOP LIMIT 인 경우, 값 셋팅(단, 2:STOP 인 경우, strPrice = "0" 셋팅) 
            string strOrderGubun = "2";     // 2:지정가, 1:시장가, 3:STOP, 4:STOP LIMIT
            string strOrgNo = "";           // 주문타입 3:매도취소, 4:매수취소, 5:매도정정, 6:매수정정 인 경우에 주문번호 셋팅
            if (!bMarketPrice)
            {
                strOrderGubun = "1";
                strPrice = quoteInfo.Price.ToString();
            }
                
            int iRet = axKFOpenAPI.SendOrder(strRQName, strScrNo, strAccNo, iOrderType, strCode, iQty, strPrice, strStopPrice, strOrderGubun, strOrgNo);

            if (iRet == (int)ERRORCOM.SUCCESS)
            {
                OnFutureSiteLogEvent("매도주문이 접수되었습니다.");
                return true;
            }
            else ShowErrorLog((ERRORCOM)iRet);
            return true;

        }
        
        public override bool DoBuyOrder(QuoteInfo quoteInfo, int nQuantity = 1, bool bMarketPrice = false)
        {
            /*
            lock (_objLock)
            {
                if (Environment.TickCount - m_tickOrder <= 5000)
                {
                    return false;
                }
                m_tickOrder = Environment.TickCount;
            }
            */

            if (this.CurrentUserAccount == null || string.IsNullOrEmpty(CurrentUserAccount.UserAccountId))
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
            if (Current == null || Current.CurrentPrice < 1)
            {
                OnFutureSiteLogEvent("주문기간이 아닙니다.");
                return false;
            }

            int nOrderRange = 5;
            if (quoteInfo.Price < Current.CurrentPrice - nOrderRange || quoteInfo.Price > Current.CurrentPrice + nOrderRange)
            {
                OnFutureSiteLogEvent("주문 가격이 초과 됨");
                return false;
            }
            
            string strRQName = "RQ_5";
            string strScrNo = "0105";
            string strAccNo = CurrentUserAccount.UserAccountId;
            int iOrderType = 2;             //1:신규매도, 2:신규매수 3:매도취소, 4:매수취소, 5:매도정정, 6:매수정정
            string strCode = ProductCode;
            int iQty = nQuantity;
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
                OnFutureSiteLogEvent("매수주문이 접수되었습니다.");
                return true;
            }
            else ShowErrorLog((ERRORCOM)iRet);
            return true;
        }


        public override bool CancelOrder(OrderInfo orderInfo)
        {
            if (this.CurrentUserAccount == null || string.IsNullOrEmpty(CurrentUserAccount.UserAccountId))
            {
                OnFutureSiteLogEvent("계좌정보 오류!");
                return false;
            }


            string strRQName = "RQ_5";
            string strScrNo = "0105";
            string strAccNo = CurrentUserAccount.UserAccountId;  //계좌번호
            int iOrderType = orderInfo.TradeTypeNo == "1"? 3:4;             //주문유형 (1:신규매도, 2:신규매수 3:매도취소, 4:매수취소, 5:매도정정, 6:매수정정)
            string strCode = orderInfo.Symbol;       //종목코드
            int iQty = orderInfo.OrderQty;                   //주문수량
            string strPrice = "0";          //주문단가
            string strStopPrice = "0";      //stop단가 주문구분 3:STOP, 4:STOP LIMIT 인 경우, 값 셋팅(단, 2:STOP 인 경우, strPrice = "0" 셋팅) 
            string strOrderGubun = "2";     //거래구분( 2:지정가, 1:시장가, 3:STOP, 4:STOP LIMIT)
            string strOrgNo = orderInfo.OrderNo; //원주문번호 (주문타입 3:매도취소, 4:매수취소, 5:매도정정, 6:매수정정 인 경우에 주문번호 셋팅)


            int iRet = axKFOpenAPI.SendOrder(strRQName, strScrNo, strAccNo, iOrderType, strCode, iQty, strPrice, strStopPrice, strOrderGubun, strOrgNo);

            if (iRet == (int)ERRORCOM.SUCCESS)
            {
                if (orderInfo.TradeTypeNo == "1")
                    OnFutureSiteLogEvent("매도주문이 취소되었습니다.");
                else if (orderInfo.TradeTypeNo == "2")
                    OnFutureSiteLogEvent("매수주문이 취소되었습니다.");
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
                    OnFutureSiteLogEvent("매도주문이 청산되었습니다.");
                else if (orderInfo.TradeTypeNo == "2")
                    OnFutureSiteLogEvent("매수주문이 청산되었습니다.");
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
            if (m_current == null)
                return list;

            double upLimitPrice = m_current.CurrentPrice + 0.25 * 1000.0;
            double downLimitPrice = m_current.CurrentPrice - 0.25 * 1000.0;
            double tick = 0.25;
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
                        Price = Math.Round(curPrice, 2)
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

            CurrentBufList.Clear();

            string strAccount = this.CurrentUserAccount.UserAccountId;
            string strAccPwd = UserPassword;

            axKFOpenAPI.SetInputValue("계좌번호", strAccount);
            axKFOpenAPI.SetInputValue("비밀번호", strAccPwd);
            axKFOpenAPI.SetInputValue("비밀번호입력매체", "00");

            int iResCode = axKFOpenAPI.CommRqData("RQ_3", "opw60003", "", "0103");      //원화외화예수금잔액조회

            if (iResCode != 0)
                return (CONSTATE)iResCode;
            RequestChart();
            return CONSTATE.SUCCESSS;
            
        }

        private void RequestChart()
        {
            
            m_tickChart = Environment.TickCount;
            Thread.Sleep(500);
            RequestRChart();
            Thread.Sleep(500);
            RequestDChart();
        }

        public override bool RequestDChart()
        {
            if (this.CurrentUserAccount == null)
                return false;
            
            axKFOpenAPI.SetInputValue("종목코드", ProductCode);
            string sTrCode = "";

            CHARTTYPE dchartType = (CHARTTYPE)Settings.Default.ChartType;
            
            switch (dchartType)
            {
                case CHARTTYPE.MIN_1:
                    sTrCode = "opc10002";
                    axKFOpenAPI.SetInputValue("시간단위", "1");
                    break;
                case CHARTTYPE.TICK_60:
                    sTrCode = "opc10001";
                    axKFOpenAPI.SetInputValue("시간단위", "60");
                    break;
                case CHARTTYPE.TICK_120:
                    sTrCode = "opc10001";
                    axKFOpenAPI.SetInputValue("시간단위", "120");
                    break;

                default:
                    break;
            }

            if (sTrCode.Length < 1)
                return false;

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
            axKFOpenAPI.SetInputValue("종목코드", ProductCode);
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

            if (sTrCode.Length < 1)
                return false;

            int iResCode = axKFOpenAPI.CommRqData("RQ_1", sTrCode, "", "0101"); 
             
            if (iResCode == 0)
                return true;
            return false ;
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
            
            OrderList.Clear();
            OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.ORDER);            
            Thread.Sleep(200);
            if(bRequidate)
                RequestRequidateOrder();
            else //Thread.Sleep(100);
                RequestOutstandOrder();

            if (bAccount)
            {
                m_bNeedAcc = bAccount;
                //Thread.Sleep(100);
                //RequestAccountData();
            }
        }

        private CONSTATE RequestRequidateOrder()
        {
            if (this.CurrentUserAccount == null)
                return CONSTATE.NO_LOGIN;
            string strAccount = this.CurrentUserAccount.UserAccountId;
            string strAccPwd = UserPassword;

            axKFOpenAPI.SetInputValue("계좌번호", strAccount);
            axKFOpenAPI.SetInputValue("비밀번호", strAccPwd);
            axKFOpenAPI.SetInputValue("비밀번호입력매체", "00");
            axKFOpenAPI.SetInputValue("통신주문구분", "AP");

            int iResCode = axKFOpenAPI.CommRqData("RQ_12", "opw30023", "", "0112");     //해외파생지정청산대상조회

            if (iResCode == 0)
                return CONSTATE.SUCCESSS;
            return (CONSTATE)iResCode;
        }

        private CONSTATE RequestOutstandOrder()
        {
            if (this.CurrentUserAccount == null)
                return CONSTATE.NO_LOGIN;
            string strAccount = this.CurrentUserAccount.UserAccountId;
            string strAccPwd = UserPassword;

            axKFOpenAPI.SetInputValue("계좌번호", strAccount);
            axKFOpenAPI.SetInputValue("비밀번호", strAccPwd);
            axKFOpenAPI.SetInputValue("비밀번호입력매체", "00");
            axKFOpenAPI.SetInputValue("종목코드", " ");
            axKFOpenAPI.SetInputValue("통화코드", " ");
            axKFOpenAPI.SetInputValue("매도수구분", " ");

            int iResCode = axKFOpenAPI.CommRqData("RQ_6", "opw30001", "", "0112");     //해외파생지정청산대상조회

            if (iResCode == 0)
                return CONSTATE.SUCCESSS;
            return (CONSTATE)iResCode;
        }

        /// <summary>
        /// //////////////////
        /// </summary>
        /// <param name="sValuation"></param>
        private void OnReceiveValuation(string sValuation)
        {
            if (sValuation.Length == 60)
            {
                string sBalance = sValuation.Substring(15, 15);
                string sTotalValuation = sValuation.Substring(30, 15);
                string sTotalProfit = sValuation.Substring(45, 15);
                long lBalance = 0, lValuation = 0, lProfit = 0;
                try
                {
                    lBalance = long.Parse(sBalance);
                    lValuation = long.Parse(sTotalValuation) / 100;
                    lProfit = long.Parse(sTotalProfit) / 100;
                }
                catch (Exception)
                {
                    return;
                }
                if (lBalance >= 0 && this.CurrentUserAccount != null)
                {
                    this.CurrentUserAccount.Balance = lBalance - lValuation;

                    this.ValuationList[0].TotalValuation = lValuation;
                    this.ValuationList[0].TotalProfit = lProfit;

                    OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.VALUATION);
                }
            }
        }

        private void OnReceiveQuote(Quote quote)
        {
            if (Environment.TickCount - m_tickQuote < 20)
                return;
            m_tickQuote = Environment.TickCount;
            SetQuoteInfo(quote);
            SetTotalQuoteInfo(quote);
            OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.QUOTE);
        }

        private void OnReceiveCurrent(Current current)
        {
            try { 
                if(Environment.TickCount - m_tickCurrent > 10)
                {
                    m_tickCurrent = Environment.TickCount;
                    lock (this.OrderList)
                    {
                        if (this.OrderList != null)
                        {
                            int nOrderCnt = this.OrderList.Count;
                            if (nOrderCnt > 0)
                            {
                                m_tickCurrent = Environment.TickCount;
                                OrderInfo orderInfo;
                                long? lValSum = 0;
                                double lAveragePrice = 0.0;
                                double lAveragePriceSum = 0.0;
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
                                        orderInfo.CurrentPrice = current.CurrentPrice.ToString();
                                        lAveragePrice = double.Parse(orderInfo.AveragePrice);
                                        lAveragePriceSum += lAveragePrice;
                                        orderInfo.Valuation = orderInfo.TradeTypeNo == "1" ?
                                            (long?)((lAveragePrice - current.CurrentPrice) * TICK_RATE * EXCH_RATE * orderInfo.OrderQty) :
                                            (long?)((current.CurrentPrice - lAveragePrice) * TICK_RATE * EXCH_RATE * orderInfo.OrderQty);

                                    }
                                    lValSum += orderInfo.Valuation;
                                }
                                ValuationList[0].Valuation = (long)lValSum;
                                ValuationList[0].TotalValuation = (long)lValSum;
                                if (lAveragePriceSum > 0)
                                    ValuationList[0].AverageUnitPrice = lAveragePriceSum / nOrderCnt;
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
            /*
            if (Environment.TickCount - m_tickChart <= 20)
            {
                CurrentBufList.Add(this.Current);
                return;
            }
            */
            //m_tickChart = Environment.TickCount;

            OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.CURRENT);

            OnFutureSiteLogicEvent(SITE_NOTICEEVENTTYPE.CURRENT);
        }

        private void OnRecieveInitialData(string sInitialValues)
        {

            if (sInitialValues == null)
                return;
            if (sInitialValues.Length < 140 || sInitialValues.Length % 140 != 0)
                return;
            int nLen = sInitialValues.Length / 140;
            
            try { 
                
                CurrentInfo currentInfo = null;
                string sCurPrice, sDateTime;
                for (int iRow=0; iRow < nLen; iRow++)
                {

                    currentInfo = new CurrentInfo();

                    sCurPrice = sInitialValues.Substring(iRow * 140, 20).Trim();
                    sDateTime = sInitialValues.Substring(iRow * 140 + 40, 20).Trim();
                    if (sDateTime.Length == 14 )
                    {
                        currentInfo.Time = DateTime.ParseExact(sDateTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    currentInfo.CurrentPrice = double.Parse(sCurPrice);
                    currentInfo.ConclusionQty = 1;
                    currentInfo.TradeType = iRow % 3 == 1 ? TRADETYPE.SELL : TRADETYPE.BUY;
                   
                    this.CurrentList.Add(currentInfo);
                    if (iRow == 0)
                        this.Current = currentInfo;

                }

                if (m_bInitalize)
                {
                    OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.INITCURRENT);
                    m_bInitalize = false;
                }
                    
            }
            catch (Exception) { }
        }
        private void OnRecieveDChartData(string sTrCode, string sSValue, string sMValues)
        {

            if (sMValues == null)
                return;
            if (sMValues.Length < 140 || sMValues.Length % 140 != 0)
                return;
            int nLen = sMValues.Length / 140;

            try
            {
                bool bMin = false;
                if (sTrCode == "opc10001" || sTrCode == "opc10002")
                    bMin = true;

                DItem itemNew = null;
                
                float fCurPrice = 0, fStartPrice = 0, fHighPrice = 0, fLowPrice = 0;
                string sDateTime = "";
                DateTime dtStart = DateTime.MinValue, dtEnd = DateTime.MinValue;

                int nLastTick = 0;
                if (sSValue.Trim().Length > 0)
                {
                    nLastTick = int.Parse(sSValue.Trim());
                }
                int nTickCnt = 0;
                lock (CtrlProperty._DItemList)
                {
                    nLen = nLen > 200 ? 200 : nLen;
                    CtrlProperty._DItemList.Clear();
                    for (int iRow = nLen - 1; iRow >= 0; iRow--)
                    {

                        if (bMin)
                        {
                            fCurPrice = float.Parse(sMValues.Substring(iRow * 140, 20).Trim());
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
                        }
                        if (iRow == 0)
                            nTickCnt = nLastTick;
                        else nTickCnt = (int)CtrlProperty._DTimeUnitAmt;
                        itemNew = new DItem(fStartPrice * 100, fCurPrice * 100, fLowPrice * 100, fHighPrice * 100,
                            CtrlProperty.GetTimeStamp(dtStart), CtrlProperty.GetTimeStamp(dtEnd), CtrlProperty._DItemList.Count(), nTickCnt);
                        CtrlProperty._DItemList.Add(itemNew);

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

            try
            {
                bool bMin = false;
                if (sTrCode == "opc10001" || sTrCode == "opc10002")
                    bMin = true;
                
                RItem itemNew = null;
                
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
                        }
                        if (iRow == 0)
                            nTickCnt = nLastTick;
                        else nTickCnt = (int)CtrlProperty._RTimeUnitAmt;
                        itemNew = new RItem(fStartPrice*100, fCurPrice * 100, fLowPrice * 100, fHighPrice * 100, 
                            CtrlProperty.GetTimeStamp(dtStart), CtrlProperty.GetTimeStamp(dtEnd), CtrlProperty._RItemList.Count(), nTickCnt);
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
            if (sRateValue == null)
                return;
            if (sRateValue.Length != 124)
                return;
            

            try
            {
                string sCurrencyCode = sRateValue.Substring(0, 3).Trim();   //통화코드
                
                if(sCurrencyCode == "USD")
                {
                    string sFCurrent = sRateValue.Substring(19, 12).Trim();   //외화예수금
                    string sKCurrent = sRateValue.Substring(109, 15).Trim();   //외화예수금

                    double lFCurent = double.Parse(sFCurrent);
                    double lKCurent = double.Parse(sKCurrent);


                    if(lFCurent > 0 && lKCurent > 0)
                        EXCH_RATE = Math.Round(lKCurent * 100 / lFCurent, 1);

                }
                

            }
            catch (Exception)
            {

            }

        }

        private void OnReceiveRequidateOrder(string sTrCode, string sRQName)
        {
            
            int nRowCnt = axKFOpenAPI.GetRepeatCnt(sTrCode, sRQName);
            
            try
            {            
                for(int iRow=0; iRow < nRowCnt; iRow++)
                {
                    string sAveragePrice = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "매입표시가격").Trim();
                    if (OrderList.FirstOrDefault(o => o.AveragePrice == sAveragePrice && o.OrderType == "체결") != null)
                        continue;

                    string sTradeTypeNo = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "매도수구분").Trim();
                    string sSymbol = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "종목코드").Trim();
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
                        CurrentPrice = sCurrentPrice,
                        Valuation = lValuation,
                        Action = "청산",
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
            
            try
            {
                for (int iRow = 0; iRow < nRowCnt; iRow++)
                {
                    string sOrderNo = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "주문번호").Trim();

                    if (OrderList.FirstOrDefault(o => o.OrderNo == sOrderNo && o.OrderType == "미체결") != null)
                        continue;

                    string sSymbol = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "종목코드").Trim();
                    string sOrderKind = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "주문유형").Trim();
                    string sTradeTypeNo = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "매도수구분").Trim();
                    string sQty = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "주문수량").Trim();
                    int nQty = int.Parse(sQty);
                    string sAveragePrice = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "주문표시가격").Trim();
                    string sStopPrice = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "조건표시가격").Trim();
                    string sOrderState = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "상태구분").Trim();
                    string sOrderTime = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "주문시각").Trim();
                    string sOrderOrgNo = axKFOpenAPI.GetCommData(sTrCode, sRQName, iRow, "원주문번호").Trim();

                    DateTime dtOrderTime = DateTime.Now;
                    if(sOrderTime.Length == 14)
                    {
                        dtOrderTime = DateTime.ParseExact(sOrderTime, "MM/dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    //if (sOrderState != "1")
                    //    return;

                    OrderInfo orderInfo = new OrderInfo
                    {
                        OrderType = "미체결",
                        Symbol = sSymbol,
                        Qty = string.Format("{0}[{1}]", sTradeTypeNo == "1" ? "매도" : "매수", nQty),
                        AveragePrice = sAveragePrice,
                        CurrentPrice = m_current.CurrentPrice.ToString(),
                        Valuation = 0L,
                        Action = "취소",
                        TradeTypeNo = sTradeTypeNo,
                        OrderQty = nQty,
                        OrderTime = dtOrderTime,
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

                this.CurrentPriceRow = this.FindQuoteInfo(this.QuoteList, m_current.CurrentPrice);
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

            this.ItemPriceList[0].CurrentPrice = current.CurrentPrice;
            this.ItemPriceList[0].Contrast = current.Contrast;//current.CurrentPrice - current.BeforeClosePrice;
            this.ItemPriceList[0].ContrastPer = current.ContrastPer;//(current.CurrentPrice - current.BeforeClosePrice) / current.CurrentPrice * 100.0;
            this.ItemPriceList[0].StartPrice = current.StartPrice;
            this.ItemPriceList[0].HighPrice = current.HighPrice;
            this.ItemPriceList[0].LowPrice = current.LowPrice;
        }

        private void SetCurrentInfo(Current current)
        {
            if (this.CurrentList == null)
                return;

            this.Current = new CurrentInfo
            {
                Time = current.ReceivedDate,
                CurrentPrice = current.CurrentPrice,
                ConclusionQty = this.CurrentList.Any<CurrentInfo>() ? current.ConclusionVolume : 0,
                TradeType = (current.TradeType == TradeType.Sell) ? TRADETYPE.SELL : TRADETYPE.BUY
            };
            this.CurrentList.Insert(0, this.Current);

            if (this.CurrentList.Count > 601)
                this.CurrentList.RemoveAt(601);
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

                if (sRQName == "RQ_0")      //실시간
                {
                    tSValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 1); //싱글
                    tMValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 2); //멀티
                    OnRecieveDChartData(sTrCode, tSValue, tMValue);
                }
                else if (sRQName == "RQ_1")  //차트1조회
                {
                    //tValue = axKFOpenAPI.GetCommData(sTrCode, sRQName, 0, "현재가");

                    //tValues = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 0); //전체
                    tSValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 1); //싱글
                    tMValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 2); //멀티
                    OnRecieveRChartData(sTrCode, tSValue, tMValue);
                }
                else if (sRQName == "RQ_2")    //차트2조회					
                {
                    //tValue = axKFOpenAPI.GetCommData(sTrCode, sRQName, 0, "종목코드");

                    //tValues = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 1); //싱글
                    //tValues = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 2); //멀티


                }
                else if (sRQName == "RQ_3") //원화외화예수금잔액조회			opw60003
                {
                    //tValues = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 1); //싱글
                    tMValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 2); //멀티
                    OnReceiveExchangeRate(tMValue);
                }
                else if (sRQName == "RQ_4")		//청산
                {
                    //tValues = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 1); //싱글
                    //tValues = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 2); //멀티

                    RequestOrderList(true, true);

                }
                else if (sRQName == "RQ_5")     //주문상태
                {
                    //tValues = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 1); //싱글(거래번호)
                    //tValues = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 2); //멀티
                    

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
                    //tMValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 2); //멀티

                    OnReceiveValuation(tSValue);


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
                else if (sRQName == "RQ_12")     //해외파생지정청산대상조회	opw30023
                {
                    tMValue = axKFOpenAPI.GetCommFullData(sTrCode, sRQName, 2); //멀티
                    OnReceiveRequidateOrder(sTrCode, sRQName);
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
                    
                }
                catch (Exception)
                {
                    return;

                }
                if(m_current == null)
                {
                    m_current = current;
                    CreateQuoteInfo();
                }
                m_current = current;

                OnReceiveCurrent(current);

            }
            else if (sRealType.Trim() == "해외선물호가")
            {
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

            string[] strArrData = null, arrData = null;
            string strData = "";
            char[] cSplit = { ';' };
            strArrData = strFidlist.Split(cSplit);
            if (strArrData.Length > 0)
                arrData = new string[strArrData.Length];

            for (int nCol = 0; nCol < strArrData.Length - 1; nCol++)
            {
                strData = axKFOpenAPI.GetChejanData(int.Parse(strArrData[nCol]));
                strData.Trim();
                arrData[nCol] = strData;

            }

            if (int.Parse(strGubun) == 0 )
            {
                RequestOrderList(false, false);
            } else if(int.Parse(strGubun) == 1)
            {
                RequestOrderList(false, true);
            }

        }

    }
}
