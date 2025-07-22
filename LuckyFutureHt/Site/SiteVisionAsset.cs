using Goodbyte.TradingSystem.Client.Common;
using Goodbyte.TradingSystem.Domain.Common;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Modules.Concrete;
using Goodbyte.TradingSystem.Domain.ValueObjects;
using LuckyFuture.Models.ValueObjects;
using LuckyFuture.Properties;
using LuckyFutureLib.Include;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using VisionAsset.Client.CompanyService;
using VisionAsset.Client.ConnectionService;
using VisionAsset.Client.CurrencyService;
using VisionAsset.Client.DayProfitLossService;
using VisionAsset.Client.MarketService;
using VisionAsset.Client.NoticeService;
using VisionAsset.Client.OrderService;
using VisionAsset.Client.UserAccountService;
using VisionAsset.Client.UserService;

namespace LuckyFuture.Site
{
	class SiteVisionAsset: FutureSite
	{
		public override SITETYPE Type { get; set; }

		OrderModule _orderModule;
		MarketModule _marketModule;
		UserAccountModule _userAccountModule;

		private readonly object _objLock = new object();

		public SiteVisionAsset(SITETYPE siteType)
		{
			Type = siteType;
			this._orderModule = new OrderModule();
			this._marketModule = new MarketModule();
			this._userAccountModule = new UserAccountModule();
		}

		private void CreateConnection()
		{
			ConnectionServiceClient connectionServiceClient = new ConnectionServiceClient();
			try
			{
				Connection connection = new Connection
				{
					ConnectionId = 0L,
					IpAddress = ClientState.RealIp,
					MacAddress = ClientState.MacAddress,
					UserDomainName = ClientState.UserDomainName,
					OsVersion = ClientState.OsVersion,
					OsUserName = ClientState.OsUserName,
					ConnectionType = ConnectionType.Login,
					ConnectionDate = DateTime.Now,
					UserId = ClientState.UserId
				};
				connectionServiceClient.CreateConnection(ClientState.Certification, connection);
				
			}
			catch (Exception e)
			{
				TraceEx.TraceException(e);
				return;
			}
			connectionServiceClient.Close();
		}

		protected override ERRORCODE Login(string id, string password)
		{
			Prepared = false;
			CurItemSymbol = null;

			UserServiceClient userServiceClient = new UserServiceClient();
			CurrencyServiceClient currencyServiceClient = new CurrencyServiceClient();
			ConnectionServiceClient connectionServiceClient = new ConnectionServiceClient();
			CompanyServiceClient companyServiceClient = new CompanyServiceClient();
			UserAccountServiceClient userAccountServiceClient = new UserAccountServiceClient();
			MarketServiceClient marketServiceClient = new MarketServiceClient();
			DayProfitLossServiceClient dayProfitLossServiceClient = new DayProfitLossServiceClient();
			ERRORCODE error_code = ERRORCODE.SUCCESS;
			try
			{
				User user = userServiceClient.LoginUser(1L, id, password);

				if (user == null)
					return ERRORCODE.INVALID_ACCOUNT;

				if (user.UserType == UserType.Standby)
					return ERRORCODE.ACCOUNT_STANDBY;

				ClientState.UserId = user.UserId;
				ClientState.LoginId = user.LoginId;
				ClientState.UserPassword = user.UserPassword;
				ClientState.UserType = user.UserType;
				ClientState.CompanyId = user.CompanyId;
				// ClientState.RealIp = Utility.GetRealIp();
				ClientState.MacAddress = Utility.GetMacAddress();
				ClientState.UserDomainName = Utility.GetUserDomainName();
				ClientState.OsVersion = Utility.GetOsVersion();
				ClientState.OsUserName = Utility.GetOsUserName();
				ClientState.Certification.Id = user.LoginId;
				ClientState.Certification.Password = user.UserPassword;
				ClientState.Currencies = currencyServiceClient.GetCurrencies(ClientState.Certification);
				ClientState.Company = companyServiceClient.GetCompany(ClientState.Certification, ClientState.CompanyId);
				ConnectionType loginState = connectionServiceClient.GetLoginState(ClientState.Certification, user.UserId);
				userServiceClient.UpdateLatestLoginDate(ClientState.Certification);
				if (loginState == ConnectionType.Null || loginState == ConnectionType.SignOff)
					return ERRORCODE.SERVER_BUSY;

				if (user.UserType != UserType.Expert && user.UserType != UserType.Manager && user.UserType != UserType.ChiefManager && connectionServiceClient.IsLogin(ClientState.Certification, user.UserId))
				{
//					this._isDisconnectSignalSender = true;
					Connection connection = new Connection
					{
						ConnectionId = 0L,
						IpAddress = ClientState.RealIp,
						MacAddress = ClientState.MacAddress,
						UserDomainName = ClientState.UserDomainName,
						OsVersion = ClientState.OsVersion,
						OsUserName = ClientState.OsUserName,
						ConnectionType = ConnectionType.SignOff,
						ConnectionDate = DateTime.Now,
						UserId = ClientState.UserId
					};
					connectionServiceClient.DisconnectUser(ClientState.Certification, connection);
				}

				this.CreateConnection();

				if (userAccountServiceClient.GetUserAccounts(ClientState.Certification, ClientState.UserId).Count == 0)
				{
					userAccountServiceClient.CreateUserAccount(ClientState.Certification, DefaultEntityFactory.GetDefaultUserAccount(ClientState.UserId));
				}
				DateTime latestMarketDate = marketServiceClient.GetLatestMarketDate(ClientState.Certification);
				foreach (UserAccount userAccount in userAccountServiceClient.GetUserAccounts(ClientState.Certification, ClientState.UserId))
				{
					if (dayProfitLossServiceClient.GetDayProfitLoss(ClientState.Certification, userAccount.UserAccountId, latestMarketDate) == null)
					{
						dayProfitLossServiceClient.CreateDayProfitLoss(ClientState.Certification, userAccount.UserAccountId, latestMarketDate);
					}
				}
			}
			catch(Exception ex)
			{
				TraceEx.TraceException(ex);
				error_code = ERRORCODE.CANT_CONNECT;
			}
			finally
			{
				try
				{
					currencyServiceClient.Close();
				}
				catch (Exception ex2)
				{
					TraceEx.TraceException(ex2);
					currencyServiceClient.Abort();
				}
				try
				{
					companyServiceClient.Close();
				}
				catch (Exception ex3)
				{
					TraceEx.TraceException(ex3);
					companyServiceClient.Abort();
				}
				try
				{
					marketServiceClient.Close();
				}
				catch (Exception ex4)
				{
					TraceEx.TraceException(ex4);
					marketServiceClient.Abort();
				}
				try
				{
					connectionServiceClient.Close();
				}
				catch (Exception ex5)
				{
					TraceEx.TraceException(ex5);
					connectionServiceClient.Abort();
				}
				try
				{
					userServiceClient.Close();
				}
				catch (Exception ex6)
				{
					TraceEx.TraceException(ex6);
					userServiceClient.Abort();
				}
				try
				{
					userAccountServiceClient.Close();
				}
				catch (Exception ex7)
				{
					TraceEx.TraceException(ex7);
					userAccountServiceClient.Abort();
				}
				try
				{
					dayProfitLossServiceClient.Close();
				}
				catch (Exception ex8)
				{
					TraceEx.TraceException(ex8);
					dayProfitLossServiceClient.Abort();
				}
			}
// 			currencyServiceClient.Close();
// 			companyServiceClient.Close();
// 			marketServiceClient.Close();
// 			connectionServiceClient.Close();
// 			userServiceClient.Close();
// 			userAccountServiceClient.Close();
// 			dayProfitLossServiceClient.Close();

			return error_code;
		}

		protected override ERRORCODE LogOut()
		{
			Prepared = false;
			if (ClientState.UserId != 0L)
			{
				ConnectionServiceClient connectionServiceClient = new ConnectionServiceClient();
				try
				{
					Connection connection = new Connection
					{
						ConnectionId = 0L,
						IpAddress = ClientState.RealIp,
						MacAddress = ClientState.MacAddress,
						UserDomainName = ClientState.UserDomainName,
						OsVersion = ClientState.OsVersion,
						OsUserName = ClientState.OsUserName,
						ConnectionType = ConnectionType.Logout,
						ConnectionDate = DateTime.Now,
						UserId = ClientState.UserId
					};
					connectionServiceClient.CreateConnection(ClientState.Certification, connection);
					connectionServiceClient.Close();
				}
				catch (Exception ex)
				{
					TraceEx.TraceException(ex);
					connectionServiceClient.Abort();
				}
			}
			return ERRORCODE.SUCCESS;
		}

		protected override ERRORCODE Prepare()
		{
			List<ItemInfo> itemInfos = new List<ItemInfo>();
			if (RealTimeData.GetInstance().Start(itemInfos, ItemSymbol))
			{
				//AddEvent();
				this.User = ConvertTo(ClientState.CurrentUser);
				this.UserAccounts = ConvertTo(ClientState.UserAccounts);
				if (this.UserAccounts.Any())
					this.CurrentUserAccount = this.UserAccounts[0];
				this.ItemId = ClientState.Item.ItemId;
				this.MarketId = ClientState.Market.MarketId;
                this.ItemSymbol = ClientState.Item.Symbol;
                this.ItemPrecision = ClientState.Item.PricePrecision;
				Settings.Default.PriceFormat = Common.GetPriceFormat(this.ItemPrecision);

				this.ItemList.Clear();
				ItemSymbolInfo itemSymbol;
				foreach (ItemInfo itemInfo in itemInfos)
				{
					itemSymbol = new ItemSymbolInfo()
					{
						ItemId = itemInfo.ItemId,
						Symbol = itemInfo.Symbol,
						ItemName = itemInfo.ItemName,
						Precision = itemInfo.Precision,
					};
					this.ItemList.Add(itemSymbol);
					if(ItemSymbol == itemInfo.Symbol)
                    {
						CurItemSymbol = itemSymbol;
						Prepared = true;
						OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.PREPAREITEM);
                    }
				}
                return ERRORCODE.SUCCESS;
			}
			return ERRORCODE.UNKNOWN_FAILED;
		}
		protected override ERRORCODE Check()
		{
            if (RealTimeData.GetInstance().CheckConnection())
            {
                if (this.ItemSymbol != CurItemSymbol.Symbol)
                {
                    //RealTimeData.GetInstance().Close();
                    LogOut();
                    return ERRORCODE.ACCOUNT_STANDBY;
                }
                else return ERRORCODE.SUCCESS;
			}
				
			return ERRORCODE.UNKNOWN_FAILED;
		}

        public override bool ChangeItem(string sSymbol)
        {
            if (ItemList == null || ItemList.Count < 0)
                return false;
            if (ItemSymbol != sSymbol)
            {
                ItemSymbol = sSymbol;
				Prepared = false;

				return true;
            }

            return false;
        }
        protected override void OnStarted()
		{
			base.OnStarted();
			AddEvent();
		}

		protected override void OnStopped(bool bAutoStop)
		{
			RealTimeData.GetInstance().Close();
			RemoveEvent();

			base.OnStopped(bAutoStop);
		}
        protected void AddEvent()
        {
            RealTimeData.GetInstance().ReceiveQuote += OnReceiveQuote;
            RealTimeData.GetInstance().ReceiveCurrent += OnReceiveCurrent;
            RealTimeData.GetInstance().ReceiveOrder += OnReceiveOrder;
            RealTimeData.GetInstance().ReceiveChangeStateSignal += OnReceiveChangeStateSignal;
        }
        protected void RemoveEvent()
        {
            RealTimeData.GetInstance().ReceiveQuote -= OnReceiveQuote;
            RealTimeData.GetInstance().ReceiveCurrent -= OnReceiveCurrent;
            RealTimeData.GetInstance().ReceiveOrder -= OnReceiveOrder;
            RealTimeData.GetInstance().ReceiveChangeStateSignal -= OnReceiveChangeStateSignal;
        }
        protected override void OnPrepare()
		{
			base.OnPrepare();
			this.QuoteList = this.CreateQuoteInfo();

			SetQuoteInfo(ClientState.Quote);
            TimeSpan tmSpan = ClientState.Current.ReceivedDate.Subtract(DateTime.Now);
			Settings.Default.ServerTimeDelay = (int)(tmSpan.TotalSeconds);
			SetCurrentInfo(ClientState.Current);
			SetHiLowPrice(ClientState.Current);
			SetItemPriceInfo(ClientState.Current);
			SetUnliquidationPosition((from o in ClientState.Orders
										   where o.UnliquidationQty > 0 && o.MarketId == this.MarketId && o.Symbol == CurItemSymbol.Symbol
									  select o).ToList<Order>());
			this.DayProfitLoss = ConvertTo(ClientState.DayProfitLoss);
			SetValuationInfo(
				(from o in ClientState.Orders where o.UnliquidationQty > 0 select o).ToList<Order>()
			);
			SetOrderInfo(ClientState.Orders);
			SetAcceptable();

			//OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.CURRENT);
		}

		void OnReceiveCurrent(Object sender, ReceiveEventArgs arg)
		{
			try
			{
				Current current = arg.Data as Current;
                
				if (!Prepared || CurItemSymbol ==null ||  CurItemSymbol.Symbol != ItemSymbol)
					return;
                if (current.Symbol != CurItemSymbol.Symbol)
                    return;
                SetCurrentInfo(current);
				SetItemPriceInfo(current);
				SetHiLowPrice(current);
				if (ClientState.OldItemCurrents[current.ItemId].CurrentPrice != current.CurrentPrice)
				{
					SetValuationInfo(
						(from o in ClientState.Orders where o.UnliquidationQty > 0 select o).ToList<Order>()
					);
                    lock (_objLock)
                    {
						SetOrderInfo(ClientState.Orders);

					}
				}
				SetAcceptable();
				
			}
			catch (Exception) { }

            OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.CURRENT);
            // OnFutureSiteLogicEvent(SITE_NOTICEEVENTTYPE.CURRENT);
        }

        void OnReceiveQuote(Object sender, ReceiveEventArgs arg)
		{
			try
			{
				Quote quote = arg.Data as Quote;
                if (!Prepared || CurItemSymbol == null || CurItemSymbol.Symbol != ItemSymbol)
                    return;
				if (quote.Symbol != CurItemSymbol.Symbol)
					return;
                if (ClientState.ItemCurrents.ContainsKey(quote.ItemId) )
				{
					SetQuoteInfo(quote);
					SetTotalQuoteInfo(quote);
					OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.QUOTE);
				}
			}
			catch (Exception) { }
		}

		void OnReceiveOrder(Object sender, ReceiveEventArgs arg)
		{
			try
			{
                if (!Prepared || CurItemSymbol == null || CurItemSymbol.Symbol != ItemSymbol)
                    return;

                OrderResult orderResult = arg.Data as OrderResult;
                
				if (orderResult.Order.Symbol != CurItemSymbol.Symbol)
                    return;

                UpdateDataByOrder(orderResult);
				SetAcceptable();
				OnFutureSiteNoticeEvent(SITE_NOTICEEVENTTYPE.ORDER);
			}
			catch (Exception) { }
		}

		void OnReceiveChangeStateSignal(Object sender, ReceiveEventArgs arg)
		{
//			OnFutureSiteNotice(arg.Data);
		}

		private void UpdateDataByOrder(OrderResult orderResult)
		{
			this.UserAccounts = ConvertTo(ClientState.UserAccounts);
			if (orderResult == null || this.CurrentUserAccount == null)
				return;
			if (orderResult.Order.UserAccountId.ToString() != CurrentUserAccount.UserAccountId)
				return;
			if (orderResult.Order.Symbol != CurItemSymbol.Symbol)
                return;
            // 일일손익
            this.DayProfitLoss = ConvertTo(orderResult.DayProfitLoss);
			if(orderResult.Order.OrderType == OrderType.Conclusion)
			{
				if(this.CurrentList != null)
				{
					OrderSignalType ost = GetOrderSignalType();
					if(ost == OrderSignalType.Virtual || ost == OrderSignalType.Hybrid)
					{
						switch (orderResult.Order.TradeType)
						{
							case TradeType.Sell:
								this.CurrentList.Insert(0, new CurrentInfo
								{
									Time = orderResult.Order.ProcessDate,
									CurrentPrice = orderResult.Order.Price,
									CurrentPriceStr = String.Format(Settings.Default.PriceFormat, Math.Round(orderResult.Order.Price, 6)),
									ConclusionQty = (this.CurrentList.Any<CurrentInfo>() ? orderResult.Order.Qty : 0),
									TradeType = TRADETYPE.SELL
								});
								break;
							case TradeType.Buy:
								this.CurrentList.Insert(0, new CurrentInfo
								{
									Time = orderResult.Order.ProcessDate,
									CurrentPrice = orderResult.Order.Price,
									CurrentPriceStr = String.Format(Settings.Default.PriceFormat, Math.Round(orderResult.Order.Price, 6)),
									ConclusionQty = (this.CurrentList.Any<CurrentInfo>() ? orderResult.Order.Qty : 0),
									TradeType = TRADETYPE.BUY
								});
								break;
						}
					}
					this.SetUnliquidationPosition((from o in ClientState.Orders
												   where o.UnliquidationQty > 0 && o.MarketId == this.MarketId
														&& o.Symbol == CurItemSymbol.Symbol
												   select o).ToList<Order>());
					this.SetValuationInfo((from o in ClientState.Orders
										   where o.UnliquidationQty > 0
										   select o).ToList<Order>());
				}
				
				string msg = "";
				if (orderResult.Order.UnliquidationQty > 0)
                {
					//msg += orderResult.Order.TradeType == TradeType.Sell ? "매도" : "매수";
                    //msg += "[" + orderResult.Order.UnliquidationQty.ToString() + "]체결:";
                    msg += "체결가:" + orderResult.Order.Price.ToString();
					
                }
                else
                {
                    //msg += orderResult.Order.TradeType == TradeType.Sell ? "매수" : "매도";
                    //msg += "[" + orderResult.Order.Qty.ToString() + "]청산:";
                    msg += "청산가:" + orderResult.Order.Price.ToString();
					
				}

				if(Settings.Default.IsAutoMode) {
					msg += "(자동-"+ Common.GetBetTypeStr((BETTYPE)Settings.Default.BettingType)+")";
				} else msg += "(수동)";

				OnFutureSiteLogEvent(msg);
				OnFutureSiteLogEvent("##");

			}
            lock (_objLock)
            {
				SetOrderInfo(ClientState.Orders);
			}
						

		}

		private void SetHiLowPrice(Current current)
		{
            if (current.Symbol != CurItemSymbol.Symbol)
                return;

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
		private void SetCurrentInfo(Current current)
		{
			if (this.CurrentList == null)
				return;
            
			if (current.Symbol != CurItemSymbol.Symbol)
                return;

			string strPrice = String.Format(Settings.Default.PriceFormat, Math.Round(current.CurrentPrice, 6));

			this.Current = new CurrentInfo
			{
				Time = current.ReceivedDate,
				CurrentPrice = current.CurrentPrice,
				CurrentPriceStr = strPrice,
				ConclusionQty = this.CurrentList.Any<CurrentInfo>() ? current.ConclusionVolume : 0,
				TradeType =  (current.TradeType == TradeType.Sell) ? TRADETYPE.SELL : TRADETYPE.BUY
			};
			this.CurrentList.Insert(0, this.Current);

			if (this.CurrentList.Count > 500)
				this.CurrentList.RemoveAt(500);
		}

		protected override List<QuoteInfo> CreateQuoteInfo()
		{
			return CreateQuoteInfo(ClientState.Item, ClientState.Current);
		}

		private void SetQuoteInfo(Quote quote)
		{
			if (this.QuoteList == null)
				return;
            
			if (quote.Symbol != CurItemSymbol.Symbol)
                return;

            if (this.QuoteList.Any<QuoteInfo>())
			{
				if(this._oldAskInfo != null)
				{
					foreach (QuoteInfo quoteInfo1 in this._oldAskInfo)
					{
						quoteInfo1.AskCount = null;
						quoteInfo1.AskQty = null;
						quoteInfo1.BidQty = null;
					}
				}
				if(this._oldBidInfo != null)
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
					for(int j = 0; j < priceList.GetLength(1); j ++)
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

				this.CurrentPriceRow = this.FindQuoteInfo(this.QuoteList, ClientState.Current.CurrentPrice);
			}
		}

		private List<QuoteInfo> CreateQuoteInfo(Item item, Current current)
		{
			List<QuoteInfo> list = new List<QuoteInfo>();
			if (item.ItemId != 0L && current.ItemId != 0L && current.Symbol == CurItemSymbol.Symbol)
			{
				Settings.Default.ItemOverTick = (float)item.OverTick;
				double upLimitPrice = current.CurrentPrice + item.OverTick * 1000.0;
				double downLimitPrice = current.CurrentPrice - item.OverTick * 1000.0;
				double tick = (upLimitPrice >= item.ReferencePoint) ? item.OverTick : item.UnderTick;
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
							Price = Math.Round(curPrice, item.PricePrecision),
							PriceStr = string.Format(Settings.Default.PriceFormat, curPrice)
						});
						if (Math.Abs(curPrice - item.ReferencePoint) < 1E-06)
						{
							tick = item.UnderTick;
						}
						index++;
						curPrice -= tick;
					}
				}
			}
			return list;
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

            if (quote.Symbol != CurItemSymbol.Symbol)
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
            
			if (current.Symbol != CurItemSymbol.Symbol)
                return;

			this.ItemPriceList[0].CurrentPrice = string.Format(Settings.Default.PriceFormat, current.CurrentPrice);
            this.ItemPriceList[0].Contrast = current.Contrast;
            this.ItemPriceList[0].ContrastPer = current.ContrastPer;
            this.ItemPriceList[0].StartPrice = string.Format(Settings.Default.PriceFormat, current.StartPrice);
            this.ItemPriceList[0].HighPrice = string.Format(Settings.Default.PriceFormat, current.HighPrice);
            this.ItemPriceList[0].LowPrice = string.Format(Settings.Default.PriceFormat, current.LowPrice);
        }
		private void SetUnliquidationPosition(List<Order> orders)
		{
			try
			{
				int num = (this.PositionRow == null) ? 0 : this.PositionRow.QuoteInfoId;
				List<QuoteInfo> list = new List<QuoteInfo>();
				foreach (Order order in orders)
				{
					for (int i = 0; i < order.UnliquidationQty; i++)
						list.Add(this.FindQuoteInfo(this.QuoteList, order.MovingAveragePrice));
				}
				if (list.Any<QuoteInfo>())
				{
					this.PositionRow = this.QuoteList[Convert.ToInt32(list.Average((QuoteInfo q) => q.QuoteInfoId))];
					this.PositionTradeType = new TRADETYPE?(
						(from o in orders select o.TradeType).FirstOrDefault<TradeType>() == TradeType.Sell ?
						 TRADETYPE.SELL : TRADETYPE.BUY
					);
				}
				else
				{
					this.PositionRow = null;
					this.PositionTradeType = null;
				}
            }
            catch (Exception)
            {
                this.PositionRow = null;
                this.PositionTradeType = null;
            }
		}

		private void SetValuationInfo(List<Order> orders)
		{
			if (this.DayProfitLoss == null)
				return;

			try
			{
				List<UnliquidationOrder> list = new List<UnliquidationOrder>();
				foreach (Order order in orders)
				{
					Item item = ClientState.Item;
					if (item != null && item.Symbol == order.Symbol)
					{
						double currentPrice = ClientState.ItemCurrents[ClientState.Item.ItemId].CurrentPrice;
						list.Add(new UnliquidationOrder
						{
							ItemId = item.ItemId,
							MarketId = this.MarketId,
							ItemType = EnumHelper.GetEnumDescription(item.ItemType),
							TradeType = EnumHelper.GetEnumDescription(order.TradeType),
							Price = order.Price,
							AveragePrice = order.MovingAveragePrice,
							Qty = order.UnliquidationQty,
							Valuation = this._orderModule.CalculateValuation(order, item, currentPrice, ClientState.Currencies),
							LossCut = this._orderModule.CalculateLossCut(order, item)
						});
					}
				}
				List<UnliquidationOrder> source = (from o in list
												   where o.MarketId == this.MarketId
												   select o).ToList<UnliquidationOrder>();
				if (this.ValuationList.Any<ValuationInfo>() && source.Any<UnliquidationOrder>())
				{
					this.ValuationList[0].Balance = string.Format(
						"{0}[{1}]",
						(from o in source select o.TradeType).FirstOrDefault<string>(), source.Sum((UnliquidationOrder o) => o.Qty)
					);
					this.ValuationList[0].AverageUnitPrice = source.Sum(
						(UnliquidationOrder o) => o.AveragePrice * (double)o.Qty) / (double)source.Sum((UnliquidationOrder o) => o.Qty
					);
					this.ValuationList[0].Valuation = source.Sum((UnliquidationOrder o) => o.Valuation);
					this.ValuationList[0].TotalValuation = list.Sum((UnliquidationOrder o) => o.Valuation);
					//this.ValuationList[0].LossCut = list.Sum((UnliquidationOrder o) => o.LossCut);
					this.ValuationList[0].TotalProfit = this.DayProfitLoss.TotalProfit;
					this.ValuationList[0].CurrentProfit = this.DayProfitLoss.TotalProfit - this.DayProfitLoss.TotalCommission + this.ValuationList[0].TotalValuation;
                    return;
                }
				if (this.ValuationList.Any<ValuationInfo>() && list.Any<UnliquidationOrder>())
				{
					this.ValuationList[0].Balance = "-";
					this.ValuationList[0].AverageUnitPrice = 0.0;
					this.ValuationList[0].Valuation = 0L;
					this.ValuationList[0].TotalValuation = list.Sum((UnliquidationOrder o) => o.Valuation);
					//this.ValuationList[0].LossCut = list.Sum((UnliquidationOrder o) => o.LossCut);
                    this.ValuationList[0].TotalProfit = this.DayProfitLoss.TotalProfit;
                    this.ValuationList[0].CurrentProfit = this.DayProfitLoss.TotalProfit - this.DayProfitLoss.TotalCommission + this.ValuationList[0].TotalValuation;
					return;
				}
				this.ValuationList[0].Balance = "-";
				this.ValuationList[0].AverageUnitPrice = 0.0;
				this.ValuationList[0].Valuation = 0L;
				this.ValuationList[0].TotalValuation = 0L;
                //this.ValuationList[0].LossCut = 0L;
                this.ValuationList[0].TotalProfit = this.DayProfitLoss.TotalProfit;
                this.ValuationList[0].CurrentProfit = this.DayProfitLoss.TotalProfit - this.DayProfitLoss.TotalCommission;
			}
			catch(Exception)
			{

			}
		}

		private void SetOrderInfo(List<Order> orders)
		{
            
			try {
				List<OrderInfo> orderInfoList = this.OrderList;
				List<OrderInfo> orderListToAdd = new List<OrderInfo>();
				List<OrderInfo> orderListToRemove = new List<OrderInfo>(orderInfoList);
				using (IEnumerator<OrderInfo> enumerator = (
					from order in orders
					where order.UnliquidationQty > 0 && order.Symbol == CurItemSymbol.Symbol
					join market in ClientState.Markets on order.MarketId equals market.MarketId
					join item in ClientState.Items on market.ItemId equals item.ItemId
					select new
					{
						order,
						item
					} into joinOrder
					group joinOrder by joinOrder.item.ItemId into g
					let last = g.Last()
					let currentPrice = ClientState.ItemCurrents[g.Key].CurrentPrice
					let priceStringFormat = Utility.GetPriceStringFormat(last.item.PricePrecision)
					let averageStringFormat = Utility.GetPriceStringFormat(last.item.AveragePricePrecision)
					let totalPrice = g.Sum(_ => _.order.MovingAveragePrice * (double)_.order.UnliquidationQty)
					let totalQty = g.Sum(_ => _.order.UnliquidationQty)
					select new OrderInfo
					{
						OrderType = "체결",
						ItemId = g.Key,
						Symbol = last.item.Symbol,
						CurrentPrice = currentPrice.ToString(priceStringFormat),
						AveragePrice = (totalPrice / (double)totalQty).ToString(averageStringFormat),
						Qty = string.Format("{0}[{1}]", EnumHelper.GetEnumDescription(last.order.TradeType), totalQty),
						Valuation = new long?(g.Sum(_ => this._orderModule.CalculateValuation(_.order, _.item, currentPrice, ClientState.Currencies))),
						Action = "청산"
					}).GetEnumerator()
				)
				{
					while (enumerator.MoveNext())
					{
						OrderInfo unliquidationOrder = enumerator.Current;
						OrderInfo orderInfo2 = orderInfoList.FirstOrDefault((OrderInfo o) => o.ItemId == unliquidationOrder.ItemId && o.OrderType == "체결");
						if (orderInfo2 != null)
						{
							orderInfo2.CurrentPrice = unliquidationOrder.CurrentPrice;
							orderInfo2.AveragePrice = unliquidationOrder.AveragePrice;
							orderInfo2.Qty = unliquidationOrder.Qty;
							orderInfo2.Valuation = unliquidationOrder.Valuation;
							orderListToRemove.Remove(orderInfo2);
						}
						else
						{
							orderListToAdd.Add(unliquidationOrder);
						}
					}

				}
				using (IEnumerator<OrderInfo> enumerator = (
					from order in orders
					where order.NotConclusionQty > 0 && !order.IsNotCompleted && order.Symbol == CurItemSymbol.Symbol
					join market in ClientState.Markets on order.MarketId equals market.MarketId
					join item in ClientState.Items on market.ItemId equals item.ItemId
					select new
					{
						order,
						item
					} into joinOrder
					group joinOrder by new
					{
						joinOrder.item.ItemId,
						joinOrder.order.Price
					} into g
					let last = g.Last()
					let currentPrice = ClientState.ItemCurrents[g.Key.ItemId].CurrentPrice
					select new
					{
						tmp = new { currentPrice, tmp1 = new { g, last } },
						priceStringFormat = Utility.GetPriceStringFormat(last.item.PricePrecision)
					}).Select((element) => new OrderInfo
					{
						OrderType = "미체결",
						ItemId = element.tmp.tmp1.g.Key.ItemId,
						Symbol = element.tmp.tmp1.last.item.Symbol,
						CurrentPrice = element.tmp.currentPrice.ToString(element.priceStringFormat),
						AveragePrice = element.tmp.tmp1.g.Key.Price.ToString(element.priceStringFormat),
						Qty = string.Format("{0}[{1}]", EnumHelper.GetEnumDescription(element.tmp.tmp1.last.order.TradeType),
							element.tmp.tmp1.g.Sum(_ => _.order.NotConclusionQty)),
						Valuation = new long?(0L),
						Action = "취소",
						OrderTime = Environment.TickCount
					}).GetEnumerator()
				)


				while (enumerator.MoveNext())
				{
					OrderInfo notConclusionOrder = enumerator.Current;
					OrderInfo orderInfo3 = orderInfoList.FirstOrDefault((OrderInfo o) => o.ItemId == notConclusionOrder.ItemId && o.OrderType == "미체결" && o.AveragePrice == notConclusionOrder.AveragePrice);
					if (orderInfo3 != null)
					{
						orderInfo3.CurrentPrice = notConclusionOrder.CurrentPrice;
						orderInfo3.Qty = notConclusionOrder.Qty;
						orderListToRemove.Remove(orderInfo3);
					}
					else
					{
						orderListToAdd.Add(notConclusionOrder);
					}
				}

				foreach (OrderInfo item3 in orderListToRemove)
					orderInfoList.Remove(item3);

				if (0 < orderListToAdd.Count)
				{
					foreach (OrderInfo item2 in orderListToAdd)
						orderInfoList.Add(item2);
					orderInfoList.Sort(delegate (OrderInfo a, OrderInfo b)
					{
						int num = b.OrderType.CompareTo(a.OrderType);
						if (num == 0)
						{
							num = a.Symbol.CompareTo(b.Symbol);
							if (num == 0)
								num = a.Qty.CompareTo(b.Qty);
						}

						return num;
					});
				}
			}
			catch (Exception) { }
			
		}

		private void SetAcceptable()
		{
			if (this.CurrentUserAccount == null)
				return;

			// 계좌정보 
			UserAccount ua = ClientState.UserAccounts.FirstOrDefault<UserAccount>(u => u.UserAccountId.ToString() == this.CurrentUserAccount.UserAccountId);
			long totalValuation = this.ValuationList[0].TotalValuation;

			long num = 0L;
			long num2 = 0L;
			long num3 = 0L;
			long num4 = 0L;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			int num10 = 0;
			int num11 = 0;
			int num12 = 0;
			long num13 = 0L;
			using (HashSet<long>.Enumerator enumerator = new HashSet<long>(from o in ClientState.Orders select o.MarketId).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					long marketId = enumerator.Current;
					Market market = ClientState.Markets.FirstOrDefault((Market m) => m.MarketId == marketId);
					Item item = ClientState.Items.FirstOrDefault(i => i.ItemId == market.ItemId);
					if (market != null && item != null)
					{
						int num14 = 0;
						int num15 = 0;
						int num16 = 0;
						int num17 = 0;
						long num18 = 0L;
						long num19 = 0L;
						long num20 = 0L;
						long num21 = 0L;
						IEnumerable<Order> orders = ClientState.Orders;

						foreach (Order order in orders.Where(o => o.MarketId == marketId).ToList<Order>())
						{
							if (!this._orderModule.IsAwaitOvernight(order, market))
							{
								if (order.TradeType == TradeType.Sell)
								{
									num += (long)order.UnliquidationQty * item.SellMargin / (long)item.Leverage / (long)ua.Leverage;
									num += (long)order.NotConclusionQty * item.SellMargin / (long)item.Leverage / (long)ua.Leverage;
								}
								else if (order.TradeType == TradeType.Buy)
								{
									num += (long)order.UnliquidationQty * item.BuyMargin / (long)item.Leverage / (long)ua.Leverage;
									num += (long)order.NotConclusionQty * item.BuyMargin / (long)item.Leverage / (long)ua.Leverage;
								}
								if (ClientState.Item.ItemType == item.ItemType)
								{
									if (order.TradeType == TradeType.Sell)
									{
										num3 += (long)order.UnliquidationQty * item.SellMargin / (long)item.Leverage / (long)ua.Leverage;
										num3 += (long)order.NotConclusionQty * item.SellMargin / (long)item.Leverage / (long)ua.Leverage;
									}
									else if (order.TradeType == TradeType.Buy)
									{
										num3 += (long)order.UnliquidationQty * item.BuyMargin / (long)item.Leverage / (long)ua.Leverage;
										num3 += (long)order.NotConclusionQty * item.BuyMargin / (long)item.Leverage / (long)ua.Leverage;
									}
									num5 += order.UnliquidationQty + order.NotConclusionQty;
								}
							}
							else
							{
								num13 += this._orderModule.CalculateOvernightMargin(ua, order, item);
							}
							if (order.TradeType == TradeType.Sell && order.UnliquidationQty > 0)
							{
								num14 += order.UnliquidationQty;
								num18 += (long)order.UnliquidationQty * Convert.ToInt64(order.Price);
								if (order.MarketId == ClientState.Market.MarketId)
								{
									num9 += order.UnliquidationQty;
								}
							}
							else if (order.TradeType == TradeType.Buy && order.UnliquidationQty > 0)
							{
								num15 += order.UnliquidationQty;
								num19 += (long)order.UnliquidationQty * Convert.ToInt64(order.Price);
								if (order.MarketId == ClientState.Market.MarketId)
								{
									num10 += order.UnliquidationQty;
								}
							}
							if (order.TradeType == TradeType.Sell && order.NotConclusionQty > 0)
							{
								num16 += order.NotConclusionQty;
								num20 += (long)order.NotConclusionQty * Convert.ToInt64(order.Price);
								if (order.MarketId == ClientState.Market.MarketId)
								{
									num11 += order.NotConclusionQty;
								}
							}
							else if (order.TradeType == TradeType.Buy && order.NotConclusionQty > 0)
							{
								num17 += order.NotConclusionQty;
								num21 += (long)order.NotConclusionQty * Convert.ToInt64(order.Price);
								if (order.MarketId == ClientState.Market.MarketId)
								{
									num12 += order.NotConclusionQty;
								}
							}
						}
						num2 += ((num14 - num17 >= 0) ? ((long)num17 * item.BuyMargin / (long)item.Leverage / (long)ua.Leverage) : ((long)num14 * item.SellMargin / (long)item.Leverage / (long)ua.Leverage));
						num2 += ((num15 - num16 >= 0) ? ((long)num16 * item.SellMargin / (long)item.Leverage / (long)ua.Leverage) : ((long)num15 * item.BuyMargin / (long)item.Leverage / (long)ua.Leverage));
						if (ClientState.Item.ItemType == item.ItemType)
						{
							num4 += ((num14 - num17 >= 0) ? ((long)num17 * item.BuyMargin / (long)item.Leverage / (long)ua.Leverage) : ((long)num14 * item.SellMargin / (long)item.Leverage / (long)ua.Leverage));
							num4 += ((num15 - num16 >= 0) ? ((long)num16 * item.SellMargin / (long)item.Leverage / (long)ua.Leverage) : ((long)num15 * item.BuyMargin / (long)item.Leverage / (long)ua.Leverage));
							num6 += ((num14 - num17 >= 0) ? num17 : num14);
							num6 += ((num15 - num16 >= 0) ? num16 : num15);
						}
					}
				}
			}
			long num22 = ClientState.Item.SellMargin / (long)ClientState.Item.Leverage / (long)ua.Leverage;
			long num23 = ClientState.Item.BuyMargin / (long)ClientState.Item.Leverage / (long)ua.Leverage;
			int num24 = (num22 != 0L) ? Convert.ToInt32(Math.Floor((double)(ua.Balance + totalValuation - num13 - (num - num2)) / (double)num22)) : 0;
			int num25 = (num23 != 0L) ? Convert.ToInt32(Math.Floor((double)(ua.Balance + totalValuation - num13 - (num - num2)) / (double)num23)) : 0;
			int num26 = (num22 != 0L) ? Convert.ToInt32(Math.Floor((double)(ua.Balance + totalValuation - num13 - (num - num2)) % (double)num22)) : 0;
			int num27 = (num23 != 0L) ? Convert.ToInt32(Math.Floor((double)(ua.Balance + totalValuation - num13 - (num - num2)) % (double)num23)) : 0;
			int num28 = (num9 - num12 >= 0) ? (num9 - num12) : 0;
			int num29 = (num10 - num11 >= 0) ? (num10 - num11) : 0;
			bool flag = ClientState.Orders.FirstOrDefault((Order _) => _.UnliquidationQty > 0 || _.NotConclusionQty > 0) != null;
			int num30 = 0;
			int num31 = 0;
			this._userAccountModule.GetMaxSellBuyQty(ua, ClientState.UserAccountSpecs, ClientState.Item, out num30, out num31);
			if (ClientState.Item.ItemType == ItemType.Foreign)
			{
				num7 = ((num22 != 0L) ? (num30 - (num5 - num6)) : 0);
				num8 = ((num23 != 0L) ? (num31 - (num5 - num6)) : 0);
			}
			long num32 = ClientState.Item.SellMinimumMargin / (long)ClientState.Item.Leverage / (long)ua.Leverage;
			long num33 = ClientState.Item.BuyMinimumMargin / (long)ClientState.Item.Leverage / (long)ua.Leverage;
			if (!flag)
			{
				if (num24 <= 0 && num32 != 0L && (long)num26 >= num32 && (long)num26 - this.ValuationList[0].CurrentProfit >= num22)
				{
					num24++;
				}
				if (num25 <= 0 && num33 != 0L && (long)num27 >= num33 && (long)num27 - this.ValuationList[0].CurrentProfit >= num23)
				{
					num25++;
				}
			}
			if (ua.Balance + totalValuation < num32)
			{
				this.SellAcceptable = num29;
			}
			else if (num24 > num7)
			{
				this.SellAcceptable = ((num7 <= 0) ? num29 : (num7 + num29));
			}
			else
			{
				this.SellAcceptable = ((num24 <= 0) ? num29 : (num24 + num29));
			}
			if (ua.Balance + totalValuation < num33)
			{
				this.BuyAcceptable = num28;
			}
			else if (num25 > num8)
			{
				this.BuyAcceptable = ((num8 <= 0) ? num28 : (num8 + num28));
			}
			else
			{
				this.BuyAcceptable = ((num25 <= 0) ? num28 : (num25 + num28));
			}
		}

		private static UserInfo ConvertTo(User user)
		{
			return new UserInfo
			{
				UserId = user.UserId,
				UserPassword = user.UserPassword,
				UserName = user.UserName,
				NickName = user.NickName,
				Phone = user.Phone,
				Email = user.Email,
				BankName = user.BankName,
				BankUserName = user.BankUserName,
				BankAccount = user.BankAccount,
				RegistrationDate = user.RegistrationDate,
				LatestLoginDate = user.LatestLoginDate,
				CompanyId = user.CompanyId
			};
		}

		private List<UserAccountInfo> ConvertTo(List<UserAccount> accounts)
		{
			var userAccountInfoList = new List<UserAccountInfo>();
			userAccountInfoList.AddRange((from account in accounts select new UserAccountInfo
			{
				UserAccountId = account.UserAccountId.ToString(),
				UserAccountStr = string.Format("{0}-{1:000000}", User.CompanyId, account.UserAccountId),
				Balance = account.Balance,
				Leverage = account.Leverage
			}));
			return userAccountInfoList;
		}

		private DayProfitLossInfo ConvertTo(DayProfitLoss dayProfitLoss)
		{
			return new DayProfitLossInfo
			{
				DayProfitLossId = dayProfitLoss.DayProfitLossId,
				MarketDate = dayProfitLoss.MarketDate,
				OpenBalance = dayProfitLoss.OpenBalance,
				TotalDeposit = dayProfitLoss.TotalDeposit,
				TotalWithdraw = dayProfitLoss.TotalWithdraw,
				CmeRealProfit = dayProfitLoss.CmeRealProfit,
				CmeRealCommission = dayProfitLoss.CmeRealCommission,
				CmeParentCommission = dayProfitLoss.CmeParentCommission,
				ForeignRealProfit = dayProfitLoss.ForeignRealProfit,
				ForeignRealCommission = dayProfitLoss.ForeignRealCommission,
				ForeignParentCommission = dayProfitLoss.ForeignParentCommission,
				ForeignVirtualProfit = dayProfitLoss.ForeignVirtualProfit,
				ForeignVirtualCommission = dayProfitLoss.ForeignVirtualCommission,
				ForeignVirtualParentCommission = dayProfitLoss.ForeignVirtualParentCommission,
				TotalRealProfit = dayProfitLoss.TotalRealProfit,
				TotalRealCommission = dayProfitLoss.TotalRealCommission,
				TotalTax = dayProfitLoss.TotalTax,
				TotalParentCommission = dayProfitLoss.TotalParentCommission,
				TotalVirtualProfit = dayProfitLoss.TotalVirtualProfit,
				TotalVirtualCommission = dayProfitLoss.TotalVirtualCommission,
				TotalVirtualTax = dayProfitLoss.TotalVirtualTax,
				TotalVirtualParentCommission = dayProfitLoss.TotalVirtualParentCommission,
				TotalProfit = dayProfitLoss.TotalProfit,
				TotalCommission = dayProfitLoss.TotalCommission,
				CloseBalance = dayProfitLoss.CloseBalance,
				UserAccountId = dayProfitLoss.UserAccountId
			};
		}

		private OrderSignalType GetOrderSignalType()
		{
			return this._userAccountModule.GetOrderSignalType(
				ClientState.UserAccounts.FirstOrDefault<UserAccount>(u => u.UserAccountId.ToString() == this.CurrentUserAccount.UserAccountId),
				ClientState.UserAccountSpecs,
				ClientState.Item
			);
		}

		public override bool CancelOrder(OrderInfo orderInfo)
		{
			if (this.CurrentUserAccount == null)
				return false;

			// 계좌정보 
			UserAccount ua = ClientState.UserAccounts.FirstOrDefault<UserAccount>(u => u.UserAccountId.ToString() == this.CurrentUserAccount.UserAccountId);
			// 주문정보에 해당되는 마켓아이디 
			long marketId = (from m in ClientState.Markets where m.ItemId == orderInfo.ItemId select m.MarketId).FirstOrDefault<long>();

			if (orderInfo.Qty.StartsWith("매도"))
			{
				List<Order> list = (
					from o in ClientState.Orders
					where Math.Abs(o.Price - Convert.ToDouble(orderInfo.AveragePrice)) < 1E-06
						&& o.NotConclusionQty > 0 && o.MarketId == marketId && o.TradeType == TradeType.Sell
					select new Order
					{
						RootOrderId = o.OrderId,
						OrderType = OrderType.Cancel,
						TradeType = o.TradeType,
						Qty = o.NotConclusionQty,
						Price = o.Price,
						PriceType = o.PriceType,
						ApplyLeverage = ua.Leverage,
						OrderSignalType = o.OrderSignalType,
						OrderRouteType = OrderRouteType.OrderInfoGrid,
						Symbol = o.Symbol,
						MarketId = o.MarketId,
						UserAccountId = o.UserAccountId
					}
				).ToList<Order>();
				if (!list.Any<Order>())
					return false;
				OrderServiceClient orderServiceClient2 = new OrderServiceClient();
				try
				{
					orderServiceClient2.CancelOrder(ClientState.Certification, list);
					orderServiceClient2.Close();
				}
				catch (FaultException ex3)
				{
					orderServiceClient2.Close();
					OnFutureSiteLogEvent(ex3.Message);
					return false;
				}
				catch (Exception ex4)
				{
					TraceEx.TraceException(ex4);
					orderServiceClient2.Abort();
					OnFutureSiteLogEvent("취소주문 오류!!!");
					return false;
				}
				OnFutureSiteLogEvent("취소주문이 접수되었습니다.");
				return true;
			}
			if (!orderInfo.Qty.StartsWith("매수"))
				return false;

			List<Order> list2 = (
				from o in ClientState.Orders
				where Math.Abs(o.Price - Convert.ToDouble(orderInfo.AveragePrice)) < 1E-06 
					&& o.NotConclusionQty > 0 && o.MarketId == marketId && o.TradeType == TradeType.Buy
				select new Order
				{
					RootOrderId = o.OrderId,
					OrderType = OrderType.Cancel,
					TradeType = o.TradeType,
					Qty = o.NotConclusionQty,
					Price = o.Price,
					PriceType = o.PriceType,
					ApplyLeverage = ua.Leverage,
					OrderSignalType = o.OrderSignalType,
					OrderRouteType = OrderRouteType.OrderInfoGrid,
					Symbol = o.Symbol,
					MarketId = o.MarketId,
					UserAccountId = o.UserAccountId
				}
			).ToList<Order>();
			if (!list2.Any<Order>())
				return false;
			OrderServiceClient orderServiceClient3 = new OrderServiceClient();
			try
			{
				orderServiceClient3.CancelOrder(ClientState.Certification, list2);
				orderServiceClient3.Close();
			}
			catch (FaultException ex5)
			{
				orderServiceClient3.Close();
				OnFutureSiteLogEvent(ex5.Message);
				return false;
			}
			catch (Exception ex6)
			{
				TraceEx.TraceException(ex6);
				orderServiceClient3.Abort();
				OnFutureSiteLogEvent("취소주문 오류!!!");
				return false;
			}
			OnFutureSiteLogEvent("취소주문이 접수되었습니다.");
			return true;
		}

		public override bool LiquidateOrder(OrderInfo orderInfo)
		{
			if (this.CurrentUserAccount == null)
				return false;

			// 계좌정보 
			UserAccount ua = ClientState.UserAccounts.FirstOrDefault<UserAccount>(u => u.UserAccountId.ToString() == this.CurrentUserAccount.UserAccountId);
			// 주문정보에 해당되는 마켓아이디 
			long marketId = (from m in ClientState.Markets where m.ItemId == orderInfo.ItemId select m.MarketId).FirstOrDefault<long>();
			// 첫 주문정보 얻기
			Order firstOrder = ClientState.Orders.FirstOrDefault((Order o) => o.UnliquidationQty > 0 && o.MarketId == marketId);
			if (firstOrder == null)
				return false;
			// 청산되지 않은 모든 주문
			List<Order> cancelOrders = (
				from o in ClientState.Orders where o.NotConclusionQty > 0 && o.MarketId == firstOrder.MarketId
				select new Order
				{
					RootOrderId = o.OrderId,
					OrderType = OrderType.Cancel,
					TradeType = o.TradeType,
					Qty = o.NotConclusionQty,
					Price = o.Price,
					PriceType = o.PriceType,
					ApplyLeverage = ua.Leverage,
					OrderSignalType = o.OrderSignalType,
					OrderRouteType = OrderRouteType.OrderInfoGrid,
					Symbol = o.Symbol,
					MarketId = o.MarketId,
					UserAccountId = o.UserAccountId
				}
			).ToList<Order>();
			List<Order> clearOrders = new List<Order>();
			switch(firstOrder.TradeType)
			{
				case TradeType.Sell:
					List<Order> clearOrders3 = clearOrders;
					Order order = new Order();
					order.OrderType = OrderType.New;
					order.TradeType = TradeType.Buy;
					order.Qty = (from o in ClientState.Orders
								 where o.UnliquidationQty > 0 && o.MarketId == marketId
								 select o).Sum((Order o) => o.UnliquidationQty);
					order.Price = 0.0;
					order.PriceType = PriceType.Market;
					order.ApplyLeverage = firstOrder.ApplyLeverage;
					order.OrderSignalType = this._userAccountModule.GetOrderSignalType(ua, ClientState.UserAccountSpecs, ClientState.Item);
					order.OrderRouteType = OrderRouteType.OrderInfoGrid;
					order.ProcessCount = 0;
					order.Symbol = firstOrder.Symbol;
					order.MarketId = firstOrder.MarketId;
					order.UserAccountId = firstOrder.UserAccountId;
					clearOrders3.Add(order);
					break;
				case TradeType.Buy:
					List<Order> clearOrders2 = clearOrders;
					Order order2 = new Order();
					order2.OrderType = OrderType.New;
					order2.TradeType = TradeType.Sell;
					order2.Qty = (from o in ClientState.Orders
								  where o.UnliquidationQty > 0 && o.MarketId == marketId
								  select o).Sum((Order o) => o.UnliquidationQty);
					order2.Price = 0.0;
					order2.PriceType = PriceType.Market;
					order2.ApplyLeverage = firstOrder.ApplyLeverage;
					order2.OrderSignalType = this._userAccountModule.GetOrderSignalType(ua, ClientState.UserAccountSpecs, ClientState.Item);
					order2.OrderRouteType = OrderRouteType.OrderInfoGrid;
					order2.ProcessCount = 0;
					order2.Symbol = firstOrder.Symbol;
					order2.MarketId = firstOrder.MarketId;
					order2.UserAccountId = firstOrder.UserAccountId;
					clearOrders2.Add(order2);
					break;
			}

			if (!clearOrders.Any<Order>())
				return false;

			Market orderMarket = ClientState.Markets.FirstOrDefault((Market m) => m.MarketId == clearOrders[0].MarketId);
			if (orderMarket == null)
				return false;

			Item item = ClientState.Items.FirstOrDefault((Item i) => i.ItemId == orderMarket.ItemId);
			if (item == null)
				return false;

			string text = this._orderModule.CheckOrderAcceptable(ua, orderMarket, item);
			if (text != string.Empty)
			{
				this.OnFutureSiteLogEvent(text);
				return false;
			}
			OrderServiceClient orderServiceClient = new OrderServiceClient();
			try
			{
				orderServiceClient.CancelOrder(ClientState.Certification, cancelOrders);
				orderServiceClient.NewOrder(ClientState.Certification, clearOrders);
				orderServiceClient.Close();
				if (firstOrder.TradeType == TradeType.Sell)
					OnFutureSiteLogEvent("매도주문이 청산되었습니다.");
				else
					OnFutureSiteLogEvent("매수주문이 청산되었습니다.");
				OnFutureSiteLogEvent("청산시가격:" + Current.CurrentPrice);
			}
			catch (FaultException ex)
			{
				OnFutureSiteLogEvent(ex.Message);
				orderServiceClient.Close();
				return false;
			}
			catch (Exception ex2)
			{
				TraceEx.TraceException(ex2);
				orderServiceClient.Abort();
				OnFutureSiteLogEvent("청산 오류!!!");
				return false;
			}
			return true;
		}

		public override bool DoSellOrder(QuoteInfo quoteInfo, int nQuantity = 1, bool bMarketPrice = false)
		{
			
            if (this.CurrentUserAccount == null)
				return false;
			
            if (nQuantity < 1 || nQuantity > 10)
            {
                OnFutureSiteLogEvent("주문수량 오류!");
                return false;
            }
			
			if (this.UserAccounts[0].Balance < ClientState.Item.SellMinimumMargin * nQuantity)
            {
                OnFutureSiteLogEvent("주문가능수량 초과!");
                return false;
            }
			
				// 계좌정보 
			UserAccount ua = ClientState.UserAccounts.FirstOrDefault<UserAccount>(u => u.UserAccountId.ToString() == this.CurrentUserAccount.UserAccountId);
			List<Order> list = new List<Order>
			{
				new Order
				{
					OrderType = OrderType.New,
					TradeType = TradeType.Sell,
					Qty = nQuantity,
					Price = quoteInfo.Price,
					PriceType = bMarketPrice?PriceType.Market:PriceType.Limit,
					ApplyLeverage = ua.Leverage,
					OrderSignalType = this._userAccountModule.GetOrderSignalType(ua, ClientState.UserAccountSpecs, ClientState.Item),
					OrderRouteType = OrderRouteType.QuoteInfoGrid,
					ProcessCount = Convert.ToInt32(quoteInfo.AskQty) + nQuantity,
					Symbol = CurItemSymbol.Symbol,
					MarketId = ClientState.Market.MarketId,
					UserAccountId = ua.UserAccountId
				}
			};
			string text = this._orderModule.CheckOrderAcceptable(ua, ClientState.Market, ClientState.Item);
			int num = (from o in ClientState.Orders
					   where o.MarketId == ClientState.Market.MarketId && o.UnliquidationQty > 0 && o.TradeType == TradeType.Buy
					   select o).Sum((Order o) => o.UnliquidationQty);
			int num2 = (from o in ClientState.Orders
						where o.MarketId == ClientState.Market.MarketId && o.NotConclusionQty > 0 && o.TradeType == TradeType.Sell
						select o).Sum((Order o) => o.NotConclusionQty);
			double num3 = Math.Max(
				(ClientState.Market.MarketStateType == MarketStateType.BeforeOpen 
				|| ClientState.Market.MarketStateType == MarketStateType.OpenSynchronized) ? 
				this.CurrentPriceRow.Price : this.Bid1Row.Price, 
				quoteInfo.Price);
			if (num - num2 < nQuantity){
				if (ClientState.Item.OrderUpLimit > 0.0 && num3 > ClientState.Item.OrderUpLimit)
				{
					OnFutureSiteLogEvent("주문 가격이 초과 됨");
					return false;
				}
				if (ClientState.Item.OrderDownLimit > 0.0 && num3 < ClientState.Item.OrderDownLimit)
				{
					OnFutureSiteLogEvent("주문 가격이 너무 작음");
					return false;
				}
			}
			if (!ClientState.Item.IsBlankQuoteOrder 
				&& quoteInfo.Price < this.Ask1Row.Price 
				&& quoteInfo.Price > this.Bid1Row.Price 
				&& ClientState.Market.MarketStateType != MarketStateType.OpenSynchronized)
			{
				OnFutureSiteLogEvent("빈 호가엔 주문할 수 없습니다.");
				return false;
			}
// 			if (ClientState.SellAcceptable < list.Sum((Order o) => o.Qty))
// 			{
// 				this._soundModule.PlayBlockedOrderSound();
// 				ClientState.ShowMessageBox("주문 가능 수량을 초과하였습니다.");
// 				return;
// 			}
			if (text != string.Empty)
			{
				OnFutureSiteLogEvent(text);
				return false;
			}
			OrderServiceClient orderServiceClient = new OrderServiceClient();
			try
			{
				orderServiceClient.NewOrder(ClientState.Certification, list);
				orderServiceClient.Close();				
			}
			catch (FaultException ex)
			{
				OnFutureSiteLogEvent(ex.Message);
				orderServiceClient.Close();
			}
			catch (Exception ex2)
			{
				TraceEx.TraceException(ex2);
				orderServiceClient.Abort();
				OnFutureSiteLogEvent("매도주문 실패!!");
			}
			OnFutureSiteLogEvent("매도주문이 접수되었습니다.");
			OnFutureSiteLogEvent("주문시가격:"+ Current.CurrentPrice);
			return true;
		}

		public override bool DoBuyOrder(QuoteInfo quoteInfo, int nQuantity = 1, bool bMarketPrice = false)
		{
            if (this.CurrentUserAccount == null)
				return false;
            if (nQuantity < 1 || nQuantity > 10)
            {
                OnFutureSiteLogEvent("주문수량 오류!");
                return false;
            }
			
            if (this.UserAccounts[0].Balance < ClientState.Item.BuyMinimumMargin * nQuantity)
            {
                OnFutureSiteLogEvent("주문가능수량 초과!");
                return false;
            }
            // 계좌정보 
            UserAccount ua = ClientState.UserAccounts.FirstOrDefault<UserAccount>(u => u.UserAccountId.ToString() == this.CurrentUserAccount.UserAccountId);

			List<Order> list = new List<Order>
			{
				new Order
				{
					OrderType = OrderType.New,
					TradeType = TradeType.Buy,
					Qty = nQuantity,
					Price = quoteInfo.Price,
					PriceType = bMarketPrice?PriceType.Market:PriceType.Limit,
					ApplyLeverage = this.CurrentUserAccount.Leverage,
					OrderSignalType = this._userAccountModule.GetOrderSignalType(
						ua,
						ClientState.UserAccountSpecs, 
						ClientState.Item
					),
					OrderRouteType = OrderRouteType.QuoteInfoGrid,
					ProcessCount = Convert.ToInt32(quoteInfo.BidQty) + nQuantity,
					Symbol = CurItemSymbol.Symbol,
					MarketId = ClientState.Market.MarketId,
					UserAccountId = ua.UserAccountId
				}
			};
			string text = this._orderModule.CheckOrderAcceptable(ua, ClientState.Market, ClientState.Item);
			int num = (from o in ClientState.Orders
					   where o.MarketId == ClientState.Market.MarketId && o.UnliquidationQty > 0 && o.TradeType == TradeType.Sell
					   select o.UnliquidationQty).Sum();
			int num2 = (from o in ClientState.Orders
						where o.MarketId == ClientState.Market.MarketId && o.NotConclusionQty > 0 && o.TradeType == TradeType.Buy
						select o.NotConclusionQty).Sum();
			double num3 = Math.Min((ClientState.Market.MarketStateType == MarketStateType.BeforeOpen 
				|| ClientState.Market.MarketStateType == MarketStateType.OpenSynchronized) ? 
				this.CurrentPriceRow.Price : this.Ask1Row.Price, quoteInfo.Price
			);
			if (num - num2 < nQuantity)
			{
				if(ClientState.Item.OrderUpLimit > 0.0 && num3 > ClientState.Item.OrderUpLimit)
				{
					OnFutureSiteLogEvent("주문 가격이 초과 됨");
					return false;
				}
				if (ClientState.Item.OrderDownLimit > 0.0 && num3 < ClientState.Item.OrderDownLimit)
				{
					OnFutureSiteLogEvent("주문 가격이 너무 작음");
					return false;
				}				
			}
			if (!ClientState.Item.IsBlankQuoteOrder 
				&& quoteInfo.Price < this.Ask1Row.Price 
				&& quoteInfo.Price > this.Bid1Row.Price 
				&& ClientState.Market.MarketStateType != MarketStateType.OpenSynchronized)
			{
				OnFutureSiteLogEvent("빈 호가엔 주문할 수 없습니다.");
				return false;
			}
// 			if (ClientState.BuyAcceptable < list.Sum((Order o) => o.Qty))
// 			{
// 				OnFutureSiteLogEvent("주문 가능 수량을 초과하였습니다.");
// 				return;
// 			}
			if (text != string.Empty)
			{
				OnFutureSiteLogEvent(text);
				return false;
			}
			OrderServiceClient orderServiceClient = new OrderServiceClient();
			try
			{
				orderServiceClient.NewOrder(ClientState.Certification, list);
				orderServiceClient.Close();
			}
			catch (FaultException ex)
			{
				OnFutureSiteLogEvent(ex.Message);
				orderServiceClient.Close();
			}
			catch (Exception ex2)
			{
				TraceEx.TraceException(ex2);
				orderServiceClient.Abort();
				OnFutureSiteLogEvent("매수주문 실패!!!");
				return false;
			}
			OnFutureSiteLogEvent("매수주문이 접수되었습니다.");
			OnFutureSiteLogEvent("주문시가격:" + Current.CurrentPrice);
			return true;
		}
	}
}
