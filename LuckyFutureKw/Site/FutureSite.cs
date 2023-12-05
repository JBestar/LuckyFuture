using LuckyFutureLib.Include;
using LuckyFuture.Models.ValueObjects;
using LuckyFuture.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

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
	}

	public enum SITETYPE
	{
		NONE = -1,
		KF_OPEN,
		VISION_ASSET,
		RELEASE = 100,
	}

	public enum SITE_NOTICEEVENTTYPE
	{
		LOGIN,
		NOLOGIN,
		PREPARE,
		CURRENT,
		INITCURRENT,
		QUOTE,
		ORDER,
		VALUATION,
		LOGOUT,
		STOP,
		REDRAW,
	}

	public enum LOGINSTATE
	{
		NONE,
		OK,
		FAILED,
		UPDATE,
	}

    public enum CONSTATE
    {
        NONE = 0,
		SUCCESSS = 1,
        NO_LOGIN = -1,      //미접속
		ERR_LOGIN = -100,   //로그인시 접속실패
		ERR_CONNECT = -101, //서버 접속 실패
		ERR_VERSION = -102,	//버전처리 실패
		ERR_TRCODE = -103,	//TrCode 존재안함
		ERR_REGAPI = -104,	//해외 OpenAPI 미신청
		ERR_ACCPWD = -301,  //계좌비밀번호입력			
		ERR_ACCNO = -302,	//타인계좌사용			
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
		public string ProductCode { get => product_code; set => product_code = value; }
		public UInt64 UserMoney { get => user_money; set => user_money = value; }
		public ERRORCODE LastErrorCode { get => last_error_code; }
		public abstract SITETYPE Type { get; }

		protected AxKFOpenAPILib.AxKFOpenAPI axKFOpenAPI;
		
		public abstract ERRORCODE Login();
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
					//login_state = LOGINSTATE.NONE;
					SetStage((int)FUSTAGE.LOGIN);
					break;
				
					// login
				case (int)FUSTAGE.LOGIN:
					last_error_code = Login();
					if (last_error_code == ERRORCODE.SUCCESS)
                    {
                        OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.LOGIN);
                        SetStage((int)FUSTAGE.PREPARE);

					}  
					else if(last_error_code == ERRORCODE.ACCOUNT_STANDBY)
                    {
                        OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.NOLOGIN);
                        bAutoStop = true;
                    } else
                    {
						bAutoStop = true;
					}
					break;
				
					// prepare
				case (int)FUSTAGE.PREPARE:
					last_error_code = Prepare();
					if (last_error_code == ERRORCODE.SUCCESS)
					{
						OnPrepare();
						OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.PREPARE);
						OnFutureSiteLogEvent("실시간데이터접속 성공!");
						SetStageWait((int)FUSTAGE.CHECK, 1000);
					}
                    else
                    {
                        OnFutureSiteLogEvent("연결끊김 !!!");
                        bAutoStop = true;
                    }
                    break;
					// check (such as connection, ect)
				case (int)FUSTAGE.CHECK:
					last_error_code = Check();
					if (last_error_code == ERRORCODE.SUCCESS)
						SetStageWait((int)FUSTAGE.CHECK, 5000);
					else
					{
						OnFutureSiteLogEvent("연결끊김 !!!");
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

			//if (LoginState == LOGINSTATE.OK)
			//	LogOut();
			//axKFOpenAPI.DisconnectRealData("9000");
			//Thread.Sleep(1000);

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
			this.CurrentList = new List<CurrentInfo>();
			this.CurrentBufList = new List<CurrentInfo>();
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
			this.QuoteList = this.CreateQuoteInfo();
			this.Ask1Row = new QuoteInfo(); //  askList[0];
            this.Bid1Row = new QuoteInfo(); // bidList[0];
			
		}

		public abstract bool CancelOrder(OrderInfo orderInfo);
		public abstract bool LiquidateOrder(OrderInfo orderInfo);
		public abstract bool DoSellOrder(QuoteInfo quoteInfo, int nQuantity = 1, bool bMarketPrice = false);
		public abstract bool DoBuyOrder(QuoteInfo quoteInfo, int nQuantity = 1, bool bMarketPrice = false);

		public abstract bool RequestRChart();
		public abstract bool RequestDChart();

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
		string product_code = "";
		UInt64 user_money = 0;
		ERRORCODE last_error_code = (int) ERRORCODE.SUCCESS;

		LOGINSTATE login_state = LOGINSTATE.NONE;
		public LOGINSTATE LoginState { get => login_state; set => login_state = value; }
		
		protected List<QuoteInfo> _oldAskInfo;
		protected List<QuoteInfo> _oldBidInfo;
		protected List<QuoteInfo> _oldHiLowInfo;
		protected List<QuoteInfo> _oldOrderPositions;

		public UserInfo User { get; set; }
		public List<UserAccountInfo> UserAccounts { get; set; }
		public UserAccountInfo CurrentUserAccount { get; set; }
		public long ItemId { get; set; }
		public long MarketId { get; set; }
		public List<ValuationInfo> ValuationList { get; set; }
		public List<ItemPriceInfo> ItemPriceList { get; set; }
		public List<OrderInfo> OrderList { get; set; }
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
		public List<CurrentInfo> CurrentBufList { get; set; }
	}
}
