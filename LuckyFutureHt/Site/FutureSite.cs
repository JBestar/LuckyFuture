using ChartCtrl;
using LuckyFutureLib.Include;
using LuckyFuture.Models.ValueObjects;
using LuckyFuture.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyFuture.Site
{ 
	public enum ERRORCODE: Int32
	{
		SUCCESS,
		CANT_CONNECT,
		SERVER_BUSY,
		INVALID_ACCOUNT,
		ACCOUNT_STANDBY,
		PREPARE_FAILED,
		UNKNOWN_FAILED,
		LOGIN_NO_ID,
		LOGIN_NO_PWD,
		LOGIN_NO_CON,
		LOGIN_DUP_ID,
	}

	public enum SITETYPE
	{
		NONE = -1,
        DREAM = 0,
        TOPASSET = 1,
        KIWOOM = 2,
        MIRAE = 3,
        VISION_ASSET = 4,
        RELEASE = 100,
	}

	public enum SITE_NOTICEEVENTTYPE
	{
		LOGIN,
		NOLOGIN,
        PREPARE,
        PREPAREITEM,
		CURRENT,
        CURRENT_SIGNAL,
        INITCURRENT,
        QUOTE,
        QUOTE_SIGNAL,
        ORDER,
		VALUATION,
        LIQUID,
        DISCONNECT,
		LOGOUT,
		STOP,
        MANUAL,
        AUTO,
        REDRAW,
    }

    public enum LOGINSTATE
	{
		NONE,
		OK,
		FAILED,
		NO_ID,
		NO_PWD,
		DUP_ID,
	}
    public enum CONSTATE
    {
        NONE = 0,
        SUCCESSS = 1,
        NO_LOGIN = -1,      //미접속
        ERR_LOGIN = -100,   //로그인시 접속실패
        ERR_CONNECT = -101, //서버 접속 실패
        ERR_VERSION = -102, //버전처리 실패
        ERR_TRCODE = -103,  //TrCode 존재안함
        ERR_REGAPI = -104,  //해외 OpenAPI 미신청
        ERR_ACCPWD = -301,  //계좌비밀번호입력			
        ERR_ACCNO = -302,   //타인계좌사용			
    }

    public class FutureSiteEventArgs : EventArgs
	{
		public FutureSiteEventArgs(object data)
		{
			this.Data = data;
		}

		public object Data { get; set; }
	}

	public class FutureSiteLogArgs : EventArgs
	{
		public FutureSiteLogArgs(string Log)
		{
			this.Log = Log;
		}
		public string Log { get; set; }
	}
	abstract class FutureSite: StageThreadEx
	{
        public string UserId {get => user_id; set => user_id = value; }
		public string UserPassword { get => user_pwd; set => user_pwd = value; }
        public string UserAcc { get => user_acc; set => user_acc = value; }
        public UInt64 UserMoney { get => user_money; set => user_money = value; }
		public ERRORCODE LastErrorCode { get => last_error_code; }
		public abstract SITETYPE Type { get; set; }

		protected abstract ERRORCODE Login(string id, string password);
		protected abstract ERRORCODE LogOut();
		protected abstract ERRORCODE Prepare();
		protected abstract ERRORCODE Check();

		public event EventHandler<FutureSiteLogArgs> LogEvent;
		public event EventHandler<FutureSiteEventArgs> NoticeEvent;
		public event EventHandler<FutureSiteEventArgs> LogicEvent;
		protected virtual void OnFutureSiteLogEvent(string log)
		{
			if (LogEvent != null)
				LogEvent(this, new FutureSiteLogArgs(log));
		}
		protected virtual void OnFutureSiteNoticeEvent(Object obj)
		{
			if (NoticeEvent != null)
				NoticeEvent(this, new FutureSiteEventArgs(obj));
		}        
		protected virtual void OnFutureSiteLogicEvent(Object obj)
        {
            if (LogicEvent != null)
                LogicEvent(this, new FutureSiteEventArgs(obj));
        }
        public override bool Start()
		{
			if (String.IsNullOrEmpty(UserPassword))
				return false;
            // ItemSymbol = "";
            ItemPrecision = 0;
            return base.Start();
		}

		public virtual void Close()
		{
			Stop();
		}

		enum FUSTAGE
		{
			LOGIN = STAGE.LAST,
			PREPARE = LOGIN + 1,
			CHECK = PREPARE + 1,
		}
		protected override bool Run()
		{
			bool bAutoStop = false;
			switch(_stage)
			{
				case (int)STAGE.NONE:
					login_state = LOGINSTATE.NONE;
					SetStage((int)FUSTAGE.LOGIN);
					break;
					// login
				case (int)FUSTAGE.LOGIN:
					last_error_code = Login(user_id, user_pwd);					
					if (last_error_code == ERRORCODE.SUCCESS)
					{
						login_remain = 0;
						login_state = LOGINSTATE.OK;
                        if (!Settings.Default.SignalSiteOn || this.Type != SITETYPE.KIWOOM)
                            OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.LOGIN);
						if (ItemSymbol.Length < 1)
							OnFutureSiteLogEvent("로그인 성공!");
                        SetStageWait((int)FUSTAGE.PREPARE, 1000);
					}
					else if(last_error_code == ERRORCODE.LOGIN_NO_ID)
                    {
                        OnFutureSiteLogEvent("존재하지 않는 아이디입니다.");
                        bAutoStop = true;
                    }
                    else if (last_error_code == ERRORCODE.LOGIN_NO_PWD)
                    {
                        OnFutureSiteLogEvent("비번틀림!! (5회중 " + login_errors + "회)");
                        bAutoStop = true;
                    }
                    else if (last_error_code == ERRORCODE.ACCOUNT_STANDBY)
                    {
                        OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.NOLOGIN);
                        bAutoStop = true;
                    }
                    else 
					{
						login_state = LOGINSTATE.FAILED;
						if(login_remain > 0)
                        {
							login_remain--;
							OnFutureSiteLogEvent("로그인 실패!");
							SetStageWait((int)FUSTAGE.LOGIN, 8000);

						} else if(last_error_code == ERRORCODE.CANT_CONNECT || last_error_code == ERRORCODE.SERVER_BUSY )
                        {
							OnFutureSiteLogEvent("서버접속 실패!");
							bAutoStop = true;
						} else if(last_error_code == ERRORCODE.LOGIN_NO_CON)
                        {
                            OnFutureSiteLogEvent("로그인 실패! 아이디/비번을 확인해 주십시오.");
                            bAutoStop = true;
                        }
                        else 
                        {
                            OnFutureSiteLogEvent("로그인 실패! 아이디/비번을 확인해 주십시오.");
                            bAutoStop = true;
                        }
						
					}
					break;
					// prepare
				case (int)FUSTAGE.PREPARE:
					last_error_code = Prepare();
					if (last_error_code == ERRORCODE.SUCCESS)
					{
						OnPrepare();
						if(!Settings.Default.SignalSiteOn || this.Type != SITETYPE.KIWOOM)
                        {
                            OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.PREPARE);
                            OnFutureSiteLogEvent("실시간데이터접속 성공!");
                        }
                        SetStageWait((int)FUSTAGE.CHECK, 1000);
					}
					else if (last_error_code == ERRORCODE.PREPARE_FAILED)
					{
						// OnFutureSiteLogEvent("실시간데이터접속 실패!");
						SetStageWait((int)FUSTAGE.PREPARE, 500);
					}
                    else
                    {
						// OnFutureSiteLogEvent("실시간데이터접속 실패!!");
                        // bAutoStop = true;
                    }
                    break;
					// check (such as connection, ect)
				case (int)FUSTAGE.CHECK:
					last_error_code = Check();
                    if (last_error_code == ERRORCODE.ACCOUNT_STANDBY)
                        SetStageWait((int)FUSTAGE.LOGIN, 3000);
                    else if (last_error_code == ERRORCODE.PREPARE_FAILED)	//종목변경
                        SetStageWait((int)FUSTAGE.PREPARE, 1000);
                    else if (last_error_code == ERRORCODE.SUCCESS)
						SetStageWait((int)FUSTAGE.CHECK, 500);
					else
					{
						login_remain = 5;
						if(!Settings.Default.SignalSiteOn || this.Type != SITETYPE.KIWOOM)
                            OnFutureSiteLogEvent("연결끊김 !!!");
                        OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.DISCONNECT);
						SetStageWait((int)FUSTAGE.LOGIN, 5000);
						//bAutoStop = true;
					}
					break;
				default:
					bAutoStop = !base.Run();
					break;
			}

			return !bAutoStop;
		}

		protected override void OnStopped(bool bAutoStop)
		{
			user_id = "";
			user_pwd = "";
			user_money = 0;

			if (LoginState == LOGINSTATE.OK)
				LogOut();

			OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.LOGOUT);

			if (LogEvent != null)
			{
				foreach (Delegate d in LogEvent.GetInvocationList())
					LogEvent -= (EventHandler<FutureSiteLogArgs>)d;
			}

			if(NoticeEvent != null)
			{
				foreach (Delegate d1 in NoticeEvent.GetInvocationList())
					NoticeEvent -= (EventHandler<FutureSiteEventArgs>)d1;
			}

            if (LogicEvent != null)
            {
                foreach (Delegate d2 in LogicEvent.GetInvocationList())
                    LogicEvent -= (EventHandler<FutureSiteEventArgs>)d2;
            }
            base.OnStopped(bAutoStop);
		}

		protected virtual void OnLogin()
		{
			this.ValuationList = this.CreateValuationInfo();
			this.ItemPriceList = this.CreateItemPriceInfo();
			this.TotalQuoteList = this.CreateTotalQuoteInfo();
			this.OrderList = new List<OrderInfo>();
			this.LiquidOrder = null;
			this.ItemList = new List<ItemSymbolInfo>();
			this.CurrentList = new List<CurrentInfo>();
			this.StartPriceRow = new QuoteInfo();
			this.HighPriceRow = new QuoteInfo();
			this.LowPriceRow = new QuoteInfo();
			this.CurrentPriceRow = new QuoteInfo();
			this.BeforeClosePriceRow = new QuoteInfo();
			this._oldAskInfo = new List<QuoteInfo>();
			this._oldBidInfo = new List<QuoteInfo>();
			this._oldHiLowInfo = new List<QuoteInfo>();
			this._oldOrderPositions = new List<QuoteInfo>();
		}

		protected virtual void OnPrepare()
		{
// 			this._itemCurrents[this._view.Item.ItemId] = loadItemResult.Current;
// 			this._view.Orders = loadItemResult.Orders;
// 			this._view.DayProfitLoss = loadItemResult.DayProfitLoss;
			
			this.Ask1Row = new QuoteInfo(); //  askList[0];
            this.Bid1Row = new QuoteInfo(); // bidList[0];
        }

		public abstract bool CancelOrder(OrderInfo orderInfo);
		public abstract bool LiquidateOrder(OrderInfo orderInfo);
		public abstract bool DoSellOrder(QuoteInfo quoteInfo, int nQuantity = 1, bool bMarketPrice = false);
		public abstract bool DoBuyOrder(QuoteInfo quoteInfo, int nQuantity = 1, bool bMarketPrice = false);
		public abstract bool ChangeItem(string sSymbol);
        public abstract bool RequestRChart();
        public abstract bool RequestDChart(bool bDChart = true);
        protected abstract List<QuoteInfo> CreateQuoteInfo();
		protected virtual List<TotalQuoteInfo> CreateTotalQuoteInfo()
		{
			return new List<TotalQuoteInfo> { new TotalQuoteInfo() };
		}
		protected virtual List<ValuationInfo> CreateValuationInfo()
		{
			return new List<ValuationInfo>
			{
				new ValuationInfo()
			};
		}
		protected virtual List<ItemPriceInfo> CreateItemPriceInfo()
		{
			return new List<ItemPriceInfo>
			{
				new ItemPriceInfo
				{
					Title1 = "현/비/％",
					Title2 = "시/고/저"
				}
			};
		}

		string user_id = "";
		string user_pwd = "";
        string user_acc = "";
        UInt64 user_money = 0;
		int login_remain = 0;
		public int login_errors = 0;

		ERRORCODE last_error_code = (int) ERRORCODE.SUCCESS;

		LOGINSTATE login_state = LOGINSTATE.NONE;
		public LOGINSTATE LoginState { get => login_state; set => login_state = value; }
		
		protected bool Prepared { get; set; }
        protected List<QuoteInfo> _oldAskInfo;
		protected List<QuoteInfo> _oldBidInfo;
		protected List<QuoteInfo> _oldHiLowInfo;
		protected List<QuoteInfo> _oldOrderPositions;

		public UserInfo User { get; set; }
		public List<UserAccountInfo> UserAccounts { get; set; }
		public UserAccountInfo CurrentUserAccount { get; set; }
		public long ItemId { get; set; }
        public string ItemSymbol { get; set; }
		public ItemSymbolInfo CurItemSymbol { get; set; }
		public int ItemPrecision { get; set; }
        public long MarketId { get; set; }
		public List<ValuationInfo> ValuationList { get; set; }
		public List<ItemPriceInfo> ItemPriceList { get; set; }
		public List<OrderInfo> OrderList { get; set; }
        public OrderVal LiquidOrder { get; set; }

        public List<ItemSymbolInfo> ItemList { get; set; }
		public CurrentInfo Current { get; set; }
		public List<CurrentInfo> CurrentList { get; set; }
		public List<QuoteInfo> QuoteList { get; set; }
		public List<TotalQuoteInfo> TotalQuoteList { get; set; }
		public QuoteInfo Ask1Row { get; set; }
		public QuoteInfo Bid1Row { get; set; }
		public QuoteInfo CurrentPriceRow { get; set; }
		public QuoteInfo StartPriceRow { get; set; }
		public QuoteInfo BeforeClosePriceRow { get; set; }
		public QuoteInfo HighPriceRow { get; set; }
		public QuoteInfo LowPriceRow { get; set; }
		public QuoteInfo PositionRow { get; set; }
		public TRADETYPE? PositionTradeType { get; set; }
		public DayProfitLossInfo DayProfitLoss { get; set; }
		public int SellAcceptable { get; set; }
		public int BuyAcceptable { get; set; }		
		public double StartPrice { get; set; }

	}
}
