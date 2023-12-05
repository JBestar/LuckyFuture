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
using System.Windows.Forms;

namespace LuckyFuture.Logic
{
    
    internal class LogicAuto: StageThreadEx
	{
		public static LogicAuto Default = new LogicAuto();
		private const float TICK_SIZE = 0.25f;

		private FutureSite _currentSite = null;
		private EventHandler<FutureSiteLogArgs> LogEvent;
		private EventHandler<FutureSiteEventArgs> NoticeEvent;
		private FrmMain frmMain = null;
		private TRADETYPE _tradeTypeToOrder = TRADETYPE.NONE;
		private OrderInfo _orderToCancel = null;

		private readonly object _objLock = new object();

		private int m_tickOrder = 0;
        private int m_tickCancel = 0;

        public FutureSite CurrentSite { get => _currentSite; }
		public bool Start(
			string accId,
			string accPwd,
			EventHandler<FutureSiteLogArgs> logEvent,
			EventHandler<FutureSiteEventArgs> noticeEvent,
			FrmMain frmMain, AxKFOpenAPILib.AxKFOpenAPI axKFOpenAPI)
		{
			if (IsRunning)
				return false;

            if (!CreateSiteObject(accId, accPwd, logEvent, noticeEvent, axKFOpenAPI))
                return false;

            LogEvent += logEvent;
			NoticeEvent += noticeEvent;
			this.frmMain = frmMain;

			return Start();
		}
		enum LASTAGE
		{
			LOGIN = STAGE.LAST,
			LOGIN_CHECK = LOGIN + 1,
			CHECK = LOGIN_CHECK + 1,
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
				// login popup
				case (int)LASTAGE.LOGIN:
					if (!_currentSite.Start())
					{
						bAutoStop = true;						
					} 
					else { 
						SetStageWait((int)LASTAGE.CHECK, 1000);
					}
						
					break;
                // check
                case (int)LASTAGE.CHECK:
					if (!_currentSite.IsRunning)
						bAutoStop = true;
// 					else if (IsAutoTradeEnable())
// 						SetStage((int)LASTAGE.ORDER);
// 					else if (NeedToCancel())
// 						SetStage((int)LASTAGE.CANCEL);
					else if (NeedToStop())
						bAutoStop = true;
					else
						SetStageWait((int)LASTAGE.CHECK, 1000);
					break;
				// order
// 				case (int)LASTAGE.ORDER:
// 					DoOrder();
// 					SetStageWait((int)LASTAGE.CHECK, 3000);
// 					break;
// 				// cancel
// 				case (int)LASTAGE.CANCEL:
// 					DoCancel();
// 					SetStageWait((int)LASTAGE.CHECK, 2000);
// 					break;
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

		public bool CreateSiteObject(
			string accId,
			string accPwd,
			EventHandler<FutureSiteLogArgs> logEvent,
			EventHandler<FutureSiteEventArgs> noticeEvent,
			AxKFOpenAPILib.AxKFOpenAPI axKFOpenAPI
		)
		{
			if (_currentSite != null)
				return false;

			_currentSite = new KFOpen(axKFOpenAPI);

			if (_currentSite != null)
			{
				_currentSite.UserId = accId;
				_currentSite.UserPassword = accPwd;
				_currentSite.LogEvent += logEvent;
				_currentSite.NoticeEvent += noticeEvent;
				_currentSite.LogicEvent += OnLogicNoticeReceive;
			}

			return _currentSite != null;
		}

		private void ReleaseSiteObject()
		{
			if (_currentSite == null)
				return;
			try
			{
				_currentSite.Close();
			}
			catch(Exception e)
			{
				OnLogEvent(e.Message);
			}
			_currentSite = null;
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

		private void OnLogicNoticeReceive(object sender, FutureSiteEventArgs e)
		{
            try {
				if (NeedToStop())
					return;
				else if (IsAutoTradeEnable())
				{
                    lock (_objLock)
                    {
						if (Environment.TickCount - m_tickOrder >= 5000)
						{
							m_tickOrder = Environment.TickCount;
						}
						else return;
                    }
					DoOrder();
				}
				else if (NeedToCancel())
				{
					lock (_objLock)
					{
						if (Environment.TickCount - m_tickCancel >= 3000)
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
			if (_currentSite == null || (_currentSite.OrderList != null && _currentSite.OrderList.Count > 0))
				return false;
			lock (_objLock)
			{
				if (Environment.TickCount - m_tickOrder <= 5000)
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

			if ((!Settings.Default.EarnPayoff || Settings.Default.EarnPayoffMoney == 0)
				&& (!Settings.Default.LossPayoff || Settings.Default.LossPayoffMoney == 0)
				&& (!Settings.Default.CandlePayoff || Settings.Default.CandlePayoffCount < 1))
				return false;

			// has order list
			if (_currentSite == null || _currentSite.OrderList == null || _currentSite.OrderList.Count <= 0)
				return false;
			lock (_objLock)
			{
				if (Environment.TickCount - m_tickCancel <= 3000)
					return false;
			}
            List<DItem> lastCandlelist = null;
			CH_AVGTYPE avgType = CH_AVGTYPE.LINE_1;
			float fTickDiff = 0.0f, fTickConf = 0.0f;
			int iFirstIdx = 0;
			if (Settings.Default.BettingType == (int)BETTYPE.UPDOWN)
            {
                lastCandlelist = frmMain.GetCandleList((BETTYPE)Settings.Default.BettingType, Settings.Default.CandlePayoffCount);
                
				if (lastCandlelist.Count < Settings.Default.CandlePayoffCount)
                    return false;
                
				avgType = (CH_AVGTYPE)Settings.Default.AvgType;
                fTickDiff = Math.Abs(lastCandlelist.First().GetAvgVal(avgType) - lastCandlelist.Last().GetAvgVal(avgType));
                fTickConf = TICK_SIZE * Settings.Default.TickPayoffCount;
                iFirstIdx = lastCandlelist.First().Index;
            }
			DateTime dtLimit = DateTime.Now.AddSeconds(-Settings.Default.OrderStopDelay);
			
			double dDeltaTick = 0, dAvgPrice = 0, dCurPrice;
			lock (_currentSite.OrderList)
            {
                _orderToCancel = _currentSite.OrderList.FirstOrDefault<OrderInfo>(
					delegate (OrderInfo o)
					{
						if (o.Valuation == null)
							return false;
						if (o.OrderType == "미체결")
						{
							if (Settings.Default.OrderStop && Settings.Default.OrderStopDelay > 0)
							{
								if (o.OrderTime <= dtLimit)
									return true;
							}

						}
						else if (Settings.Default.BettingType == (int)BETTYPE.EQUIVALENT || Settings.Default.BettingType == (int)BETTYPE.CROSS)
						{
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

                            if (o.Qty.StartsWith("매수"))
                            {
                                if (Settings.Default.EarnPayoff && Settings.Default.EarnPayoffMoney != 0
                                    && dDeltaTick >= Settings.Default.EarnPayoffMoney * TICK_SIZE)
                                    return true;
                                if (Settings.Default.LossPayoff && Settings.Default.LossPayoffMoney != 0
                                    && dDeltaTick <= -Settings.Default.LossPayoffMoney * TICK_SIZE)
                                    return true;
                            }
                            else if (o.Qty.StartsWith("매도"))
                            {
                                if (Settings.Default.EarnPayoff && Settings.Default.EarnPayoffMoney != 0
                                    && dDeltaTick <= -Settings.Default.EarnPayoffMoney * TICK_SIZE)
                                    return true;
                                if (Settings.Default.LossPayoff && Settings.Default.LossPayoffMoney != 0
                                    && dDeltaTick >= Settings.Default.LossPayoffMoney * TICK_SIZE)
                                    return true;
                            }
                        }
						else if (Settings.Default.BettingType == (int)BETTYPE.UPDOWN)
						{
							if (o.Qty.StartsWith("매수"))
							{
								if (fTickDiff >= fTickConf && lastCandlelist.Count<DItem>(d => d.GetTrendDown(avgType, iFirstIdx) == CH_TRENDTYPE.DOWN) >= Settings.Default.CandlePayoffCount)
									return true;
							}
							else if (o.Qty.StartsWith("매도"))
							{
								if (fTickDiff >= fTickConf && lastCandlelist.Count<DItem>(d => d.GetTrendUp(avgType, iFirstIdx) == CH_TRENDTYPE.UP) >= Settings.Default.CandlePayoffCount)
									return true;
							}
						}

						return false;
					}
				);
            }
			

			return _orderToCancel != null;
		}

		private bool NeedToStop()
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

            long valuation = 0;
            lock (_currentSite.ValuationList)
            {
				valuation = _currentSite.ValuationList[0].TotalProfit;
			}

            if (Settings.Default.EarnStop && Settings.Default.EarnStopMoney > 0)
			{
				if (valuation >= Settings.Default.EarnStopMoney * 10000)
					return true;
			}

			if (Settings.Default.LossStop && Settings.Default.LossStopMoney > 0)
			{
				if (valuation <= -Settings.Default.LossStopMoney * 10000)
					return true;
			}

			return false;
		}

		private bool DoOrder()
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
					return _currentSite.DoBuyOrder(quoteInfo, Settings.Default.OrderCount, Settings.Default.OrderType == 0);
				case TRADETYPE.SELL:
					return _currentSite.DoSellOrder(quoteInfo, Settings.Default.OrderCount, Settings.Default.OrderType == 0);
			}

			return false;
		}

		private bool DoCancel()
		{
			if (_orderToCancel == null)
				return false;
			if (_orderToCancel.OrderType == "미체결")
				return _currentSite.CancelOrder(_orderToCancel);
			return _currentSite.LiquidateOrder(_orderToCancel);
		}

		private TRADETYPE SelectTradeType()
		{
			int nCandleCnt = Settings.Default.BettingCandleCount;
			
			if (Settings.Default.BettingType == (int)BETTYPE.CROSS)
				nCandleCnt = 2;

			if (nCandleCnt < 1)
				return TRADETYPE.NONE;

			List<DItem> lastCandlelist = frmMain.GetCandleList((BETTYPE)Settings.Default.BettingType, nCandleCnt);
			
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
				float fTickConf = TICK_SIZE * Settings.Default.BettingTickCount;

				int iFirstIdx = lastCandlelist.First().Index;

				if (fTickDiff >= fTickConf && lastCandlelist.Count<DItem>(d => d.GetTrendUp(avgType, iFirstIdx) == CH_TRENDTYPE.UP) >= Settings.Default.BettingCandleCount)
					trade_type = TRADETYPE.BUY;
                else if(fTickDiff >= fTickConf && lastCandlelist.Count<DItem>(d => d.GetTrendDown(avgType, iFirstIdx) == CH_TRENDTYPE.DOWN) >= Settings.Default.BettingCandleCount)
                    trade_type = TRADETYPE.SELL;
				else trade_type = TRADETYPE.NONE;
            } 
			else if(Settings.Default.BettingType == (int)BETTYPE.CROSS)
            {
				CH_TRENDTYPE trend_type = lastCandlelist.Last().GetCrossTrend(CH_AVGTYPE.LINE_1, CH_AVGTYPE.LINE_2);
				if (trend_type == CH_TRENDTYPE.UP)
					trade_type = TRADETYPE.BUY;
				else if (trend_type == CH_TRENDTYPE.DOWN)
					trade_type = TRADETYPE.SELL;
				else trade_type = TRADETYPE.NONE;
			}


            return trade_type;
		}
	}
}
