using System;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.ValueObjects;

namespace Goodbyte.TradingSystem.Domain.Common
{
	// Token: 0x02000083 RID: 131
	public static class DefaultEntityFactory
	{
		// Token: 0x06000789 RID: 1929 RVA: 0x00017E80 File Offset: 0x00016080
		public static Item GetDefaultOptionItem(string symbol, ItemType itemType, DateTime startTradingDay, DateTime lastTradingDay)
		{
			Item item = new Item
			{
				ItemId = 0L,
				Symbol = symbol,
				ItemType = itemType,
				SortIndex = 0,
				CurrencyType = CurrencyType.Krw,
				Tax = 0.0,
				ParentCommission = 0.0,
				Leverage = 1,
				PricePrecision = 2,
				AveragePricePrecision = 3,
				ReferencePoint = 10.0,
				OverTick = 0.05,
				UnderTick = 0.01,
				OverTickValue = 12500.0,
				UnderTickValue = 2500.0,
				StartTradingDay = startTradingDay,
				LastTradingDay = lastTradingDay,
				SellMargin = 1000000L,
				BuyMargin = 200000L,
				SellMinimumMargin = 200000L,
				BuyMinimumMargin = 40000L,
				SellLossCutMargin = 200000L,
				BuyLossCutMargin = 40000L,
				SellOvernightMinimumMargin = 3000000L,
				BuyOvernightMinimumMargin = 0L,
				ConclusionSpeed = 70,
				CancelOrderTime = new TimeSpan(0, 0, 0, 30),
				CancelOrderTick = 0,
				OrderUpLimit = 3.0,
				OrderDownLimit = 0.1,
				IsBlankQuoteOrder = true,
				IsBlankQuoteConslusion = false,
				IsOnlyLimitPrice = false,
				IsUse = true
			};
			item.ItemName = string.Format("{0}월 {1} {2}.{3}", new object[]
			{
				(int)(('A' <= symbol[4]) ? (symbol[4] - 'A' + '\n') : (symbol[4] - '0')),
				('2' == symbol[0]) ? "콜" : "풋",
				symbol.Substring(5),
				('0' == symbol[7] || '5' == symbol[7]) ? '0' : '5'
			});
			return item;
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00018084 File Offset: 0x00016284
		public static Item GetDefaultEurexOptionItem(string symbol, ItemType itemType, DateTime startTradingDay, DateTime lastTradingDay)
		{
			Item item = new Item
			{
				ItemId = 0L,
				Symbol = symbol,
				ItemType = itemType,
				SortIndex = 0,
				CurrencyType = CurrencyType.Krw,
				Tax = 0.0,
				ParentCommission = 0.0,
				Leverage = 1,
				PricePrecision = 2,
				AveragePricePrecision = 3,
				ReferencePoint = 10.0,
				OverTick = 0.05,
				UnderTick = 0.01,
				OverTickValue = 12500.0,
				UnderTickValue = 2500.0,
				StartTradingDay = startTradingDay,
				LastTradingDay = lastTradingDay,
				SellMargin = 1000000L,
				BuyMargin = 200000L,
				SellMinimumMargin = 200000L,
				BuyMinimumMargin = 40000L,
				SellLossCutMargin = 200000L,
				BuyLossCutMargin = 40000L,
				SellOvernightMinimumMargin = 3000000L,
				BuyOvernightMinimumMargin = 0L,
				ConclusionSpeed = 70,
				CancelOrderTime = new TimeSpan(0, 0, 0, 30),
				CancelOrderTick = 0,
				OrderUpLimit = 2.0,
				OrderDownLimit = 0.1,
				IsBlankQuoteOrder = true,
				IsBlankQuoteConslusion = false,
				IsOnlyLimitPrice = false,
				IsUse = true
			};
			item.ItemName = string.Format("야간 {0}월 {1} {2}.{3}", new object[]
			{
				(int)(('A' <= symbol[4]) ? (symbol[4] - 'A' + '\n') : (symbol[4] - '0')),
				('2' == symbol[0]) ? "콜" : "풋",
				symbol.Substring(5),
				('0' == symbol[7] || '5' == symbol[7]) ? '0' : '5'
			});
			return item;
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x00018288 File Offset: 0x00016488
		public static Item GetDefaultStockItem(string symbol, StockItemType stockItemType, string itemName, double referencePoint, double overTick, double underTick)
		{
			return new Item
			{
				ItemId = 0L,
				Symbol = symbol,
				ItemType = ((stockItemType == StockItemType.Kospi) ? ItemType.Kospi : ItemType.Kosdaq),
				ItemName = itemName,
				SortIndex = 0,
				CurrencyType = CurrencyType.Krw,
				Tax = 0.3,
				ParentCommission = 0.015,
				Leverage = 1,
				PricePrecision = 0,
				AveragePricePrecision = 1,
				ReferencePoint = referencePoint,
				OverTick = overTick,
				UnderTick = underTick,
				OverTickValue = overTick,
				UnderTickValue = underTick,
				StartTradingDay = new DateTime(1900, 1, 1),
				LastTradingDay = new DateTime(2100, 1, 1),
				SellMargin = 0L,
				BuyMargin = 0L,
				SellMinimumMargin = 0L,
				BuyMinimumMargin = 0L,
				SellLossCutMargin = 20L,
				BuyLossCutMargin = 20L,
				SellOvernightMinimumMargin = 30L,
				BuyOvernightMinimumMargin = 30L,
				ConclusionSpeed = 70,
				CancelOrderTime = new TimeSpan(0, 0, 0, 30),
				CancelOrderTick = 0,
				OrderUpLimit = 0.0,
				OrderDownLimit = 0.0,
				IsBlankQuoteOrder = true,
				IsBlankQuoteConslusion = false,
				IsOnlyLimitPrice = false,
				IsUse = true
			};
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x000183E8 File Offset: 0x000165E8
		public static UserAccount GetDefaultUserAccount(long userId)
		{
			return new UserAccount
			{
				UserAccountId = 0L,
				Balance = 0L,
				Leverage = 1,
				CreateDate = DateTime.Now,
				UserAccountStateType = UserAccountStateType.Normal,
				IsFuturesTrade = true,
				FuturesOrderSignalType = OrderSignalType.Virtual,
				FuturesCommission = 0.002,
				FuturesMaxSellQty = 10,
				FuturesMaxBuyQty = 10,
				IsFuturesOvernight = false,
				IsFuturesOvernightSettingPermission = false,
				IsOptionTrade = false,
				OptionOrderSignalType = OrderSignalType.Virtual,
				OptionCommission = 0.12,
				OptionMaxSellQty = 5,
				OptionMaxBuyQty = 25,
				IsOptionOvernight = false,
				IsOptionOvernightSettingPermission = false,
				IsCmeTrade = false,
				CmeOrderSignalType = OrderSignalType.Virtual,
				CmeCommission = 0.002,
				CmeMaxSellQty = 10,
				CmeMaxBuyQty = 10,
				IsCmeOvernight = false,
				IsCmeOvernightSettingPermission = true,
				IsEurexTrade = false,
				EurexOrderSignalType = OrderSignalType.Virtual,
				EurexCommission = 0.12,
				EurexMaxSellQty = 5,
				EurexMaxBuyQty = 25,
				IsEurexOvernight = false,
				IsEurexOvernightSettingPermission = false,
				IsForeignTrade = true,
				ForeignOrderSignalType = OrderSignalType.Virtual,
				ForeignCommission = 7.0,
				ForeignMaxSellQty = 5,
				ForeignMaxBuyQty = 5,
				IsForeignOvernight = false,
				IsForeignOvernightSettingPermission = false,
				IsKospiTrade = false,
				KospiOrderSignalType = OrderSignalType.Virtual,
				KospiCommission = 0.15,
				KospiMaxSellMargin = 0L,
				KospiMaxBuyMargin = 100000000L,
				IsKospiOvernight = false,
				IsKospiOvernightSettingPermission = false,
				IsKosdaqTrade = false,
				KosdaqOrderSignalType = OrderSignalType.Virtual,
				KosdaqCommission = 0.15,
				KosdaqMaxSellMargin = 0L,
				KosdaqMaxBuyMargin = 100000000L,
				IsKosdaqOvernight = false,
				IsKosdaqOvernightSettingPermission = false,
				UserId = userId
			};
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x000185CC File Offset: 0x000167CC
		public static DateTime GetDefaultMarketOpenSynchronizedTime(DateTime marketDate, Item item, bool isSummerTime)
		{
			if (Utility.GetSymbolCode(item) == "SCN")
			{
				return marketDate.Add(new TimeSpan(10, 0, 0));
			}
			if (Utility.GetSymbolCode(item) == "HSI")
			{
				return marketDate.Add(new TimeSpan(10, 16, 0));
			}
			if (Utility.GetSymbolCode(item) == "HMH")
			{
				return marketDate.Add(new TimeSpan(10, 16, 0));
			}
			switch (item.ItemType)
			{
			case ItemType.Futures:
				return marketDate.Add(new TimeSpan(8, 35, 0));
			case ItemType.Options:
				return marketDate.Add(new TimeSpan(8, 35, 0));
			case ItemType.Cme:
				return marketDate.Add(new TimeSpan(18, 0, 0));
			case ItemType.Eurex:
				return marketDate.Add(new TimeSpan(18, 0, 0));
			case ItemType.Foreign:
				return marketDate.Add(new TimeSpan(8, 1, 0));
			case ItemType.Kospi:
				return marketDate.Add(new TimeSpan(8, 35, 0));
			case ItemType.Kosdaq:
				return marketDate.Add(new TimeSpan(8, 35, 0));
			default:
				return marketDate;
			}
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x000186E8 File Offset: 0x000168E8
		public static DateTime GetDefaultMarketOpenTime(DateTime marketDate, Item item, bool isSummerTime)
		{
			if (Utility.GetSymbolCode(item) == "SCN")
			{
				return marketDate.Add(new TimeSpan(10, 0, 0));
			}
			if (Utility.GetSymbolCode(item) == "HSI")
			{
				return marketDate.Add(new TimeSpan(10, 16, 0));
			}
			if (Utility.GetSymbolCode(item) == "HMH")
			{
				return marketDate.Add(new TimeSpan(10, 16, 0));
			}
			switch (item.ItemType)
			{
			case ItemType.Futures:
				return marketDate.Add(new TimeSpan(9, 0, 0));
			case ItemType.Options:
				return marketDate.Add(new TimeSpan(9, 0, 0));
			case ItemType.Cme:
				return marketDate.Add(new TimeSpan(18, 0, 0));
			case ItemType.Eurex:
				return marketDate.Add(new TimeSpan(18, 0, 0));
			case ItemType.Foreign:
				return marketDate.Add(new TimeSpan(8, 1, 0));
			case ItemType.Kospi:
				return marketDate.Add(new TimeSpan(9, 0, 0));
			case ItemType.Kosdaq:
				return marketDate.Add(new TimeSpan(9, 0, 0));
			default:
				return marketDate;
			}
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00018804 File Offset: 0x00016A04
		public static DateTime GetDefaultMarketEndOrderTime(DateTime marketDate, Item item, bool isSummerTime)
		{
			if (Utility.GetSymbolCode(item) == "SCN")
			{
				return marketDate.Add(new TimeSpan(26, 55, 0));
			}
			if (Utility.GetSymbolCode(item) == "HSI")
			{
				return marketDate.Add(new TimeSpan(17, 25, 0));
			}
			if (Utility.GetSymbolCode(item) == "HMH")
			{
				return marketDate.Add(new TimeSpan(17, 25, 0));
			}
			switch (item.ItemType)
			{
			case ItemType.Futures:
				if (!(marketDate.Date == item.LastTradingDay.Date))
				{
					return marketDate.Add(new TimeSpan(15, 30, 0));
				}
				return marketDate.Add(new TimeSpan(15, 15, 0));
			case ItemType.Options:
				if (!(marketDate.Date == item.LastTradingDay.Date))
				{
					return marketDate.Add(new TimeSpan(15, 30, 0));
				}
				return marketDate.Add(new TimeSpan(15, 15, 0));
			case ItemType.Cme:
				return marketDate.Add(new TimeSpan(28, 55, 0));
			case ItemType.Eurex:
				if (!isSummerTime)
				{
					return marketDate.Add(new TimeSpan(28, 55, 0));
				}
				return marketDate.Add(new TimeSpan(27, 55, 0));
			case ItemType.Foreign:
				return marketDate.Add(new TimeSpan(29, 0, 0));
			case ItemType.Kospi:
				return marketDate.Add(new TimeSpan(15, 15, 0));
			case ItemType.Kosdaq:
				return marketDate.Add(new TimeSpan(15, 15, 0));
			default:
				return marketDate;
			}
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0001899C File Offset: 0x00016B9C
		public static DateTime GetDefaultMarketCloseTime(DateTime marketDate, Item item, bool isSummerTime)
		{
			if (Utility.GetSymbolCode(item) == "SCN")
			{
				return marketDate.Add(new TimeSpan(27, 0, 0));
			}
			if (Utility.GetSymbolCode(item) == "HSI")
			{
				return marketDate.Add(new TimeSpan(17, 26, 0));
			}
			if (Utility.GetSymbolCode(item) == "HMH")
			{
				return marketDate.Add(new TimeSpan(17, 26, 0));
			}
			switch (item.ItemType)
			{
			case ItemType.Futures:
				if (!(marketDate.Date == item.LastTradingDay.Date))
				{
					return marketDate.Add(new TimeSpan(15, 45, 0));
				}
				return marketDate.Add(new TimeSpan(15, 20, 0));
			case ItemType.Options:
				if (!(marketDate.Date == item.LastTradingDay.Date))
				{
					return marketDate.Add(new TimeSpan(15, 45, 0));
				}
				return marketDate.Add(new TimeSpan(15, 20, 0));
			case ItemType.Cme:
				return marketDate.Add(new TimeSpan(29, 0, 0));
			case ItemType.Eurex:
				if (!isSummerTime)
				{
					return marketDate.Add(new TimeSpan(29, 0, 0));
				}
				return marketDate.Add(new TimeSpan(28, 0, 0));
			case ItemType.Foreign:
				return marketDate.Add(new TimeSpan(30, 0, 0));
			case ItemType.Kospi:
				return marketDate.Add(new TimeSpan(15, 30, 0));
			case ItemType.Kosdaq:
				return marketDate.Add(new TimeSpan(15, 30, 0));
			default:
				return marketDate;
			}
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00018B30 File Offset: 0x00016D30
		public static DateTime GetDefaultMarketPauseTime1(DateTime marketDate, Item item, bool isSummerTime)
		{
			if (Utility.GetSymbolCode(item) == "SCN")
			{
				return marketDate.Add(new TimeSpan(17, 35, 0));
			}
			if (Utility.GetSymbolCode(item) == "HSI")
			{
				return marketDate.Add(new TimeSpan(13, 0, 0));
			}
			if (Utility.GetSymbolCode(item) == "HMH")
			{
				return marketDate.Add(new TimeSpan(13, 0, 0));
			}
			switch (item.ItemType)
			{
			case ItemType.Futures:
				if (!(marketDate.Date == item.LastTradingDay.Date))
				{
					return marketDate.Add(new TimeSpan(15, 45, 0));
				}
				return marketDate.Add(new TimeSpan(15, 20, 0));
			case ItemType.Options:
				if (!(marketDate.Date == item.LastTradingDay.Date))
				{
					return marketDate.Add(new TimeSpan(15, 45, 0));
				}
				return marketDate.Add(new TimeSpan(15, 20, 0));
			case ItemType.Cme:
				return marketDate.Add(new TimeSpan(29, 0, 0));
			case ItemType.Eurex:
				if (!isSummerTime)
				{
					return marketDate.Add(new TimeSpan(29, 0, 0));
				}
				return marketDate.Add(new TimeSpan(28, 0, 0));
			case ItemType.Foreign:
				return marketDate.Add(new TimeSpan(30, 0, 0));
			case ItemType.Kospi:
				return marketDate.Add(new TimeSpan(15, 30, 0));
			case ItemType.Kosdaq:
				return marketDate.Add(new TimeSpan(15, 30, 0));
			default:
				return marketDate;
			}
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x00018CC4 File Offset: 0x00016EC4
		public static DateTime GetDefaultMarketReopenTime1(DateTime marketDate, Item item, bool isSummerTime)
		{
			if (Utility.GetSymbolCode(item) == "SCN")
			{
				return marketDate.Add(new TimeSpan(18, 15, 0));
			}
			if (Utility.GetSymbolCode(item) == "HSI")
			{
				return marketDate.Add(new TimeSpan(14, 0, 0));
			}
			if (Utility.GetSymbolCode(item) == "HMH")
			{
				return marketDate.Add(new TimeSpan(14, 0, 0));
			}
			switch (item.ItemType)
			{
			case ItemType.Futures:
				if (!(marketDate.Date == item.LastTradingDay.Date))
				{
					return marketDate.Add(new TimeSpan(15, 45, 0));
				}
				return marketDate.Add(new TimeSpan(15, 20, 0));
			case ItemType.Options:
				if (!(marketDate.Date == item.LastTradingDay.Date))
				{
					return marketDate.Add(new TimeSpan(15, 45, 0));
				}
				return marketDate.Add(new TimeSpan(15, 20, 0));
			case ItemType.Cme:
				return marketDate.Add(new TimeSpan(29, 0, 0));
			case ItemType.Eurex:
				if (!isSummerTime)
				{
					return marketDate.Add(new TimeSpan(29, 0, 0));
				}
				return marketDate.Add(new TimeSpan(28, 0, 0));
			case ItemType.Foreign:
				return marketDate.Add(new TimeSpan(30, 0, 0));
			case ItemType.Kospi:
				return marketDate.Add(new TimeSpan(15, 30, 0));
			case ItemType.Kosdaq:
				return marketDate.Add(new TimeSpan(15, 30, 0));
			default:
				return marketDate;
			}
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x00018E58 File Offset: 0x00017058
		public static DateTime GetDefaultMarketPauseTime2(DateTime marketDate, Item item, bool isSummerTime)
		{
			if (Utility.GetSymbolCode(item) == "SCN")
			{
				return marketDate.Add(new TimeSpan(27, 0, 0));
			}
			if (Utility.GetSymbolCode(item) == "HSI")
			{
				return marketDate.Add(new TimeSpan(17, 30, 0));
			}
			if (Utility.GetSymbolCode(item) == "HMH")
			{
				return marketDate.Add(new TimeSpan(17, 30, 0));
			}
			switch (item.ItemType)
			{
			case ItemType.Futures:
				if (!(marketDate.Date == item.LastTradingDay.Date))
				{
					return marketDate.Add(new TimeSpan(15, 45, 0));
				}
				return marketDate.Add(new TimeSpan(15, 20, 0));
			case ItemType.Options:
				if (!(marketDate.Date == item.LastTradingDay.Date))
				{
					return marketDate.Add(new TimeSpan(15, 45, 0));
				}
				return marketDate.Add(new TimeSpan(15, 20, 0));
			case ItemType.Cme:
				return marketDate.Add(new TimeSpan(29, 0, 0));
			case ItemType.Eurex:
				if (!isSummerTime)
				{
					return marketDate.Add(new TimeSpan(29, 0, 0));
				}
				return marketDate.Add(new TimeSpan(28, 0, 0));
			case ItemType.Foreign:
				return marketDate.Add(new TimeSpan(30, 0, 0));
			case ItemType.Kospi:
				return marketDate.Add(new TimeSpan(15, 30, 0));
			case ItemType.Kosdaq:
				return marketDate.Add(new TimeSpan(15, 30, 0));
			default:
				return marketDate;
			}
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x00018FEC File Offset: 0x000171EC
		public static DateTime GetDefaultMarketReopenTime2(DateTime marketDate, Item item, bool isSummerTime)
		{
			if (Utility.GetSymbolCode(item) == "SCN")
			{
				return marketDate.Add(new TimeSpan(27, 0, 0));
			}
			if (Utility.GetSymbolCode(item) == "HSI")
			{
				return marketDate.Add(new TimeSpan(18, 15, 0));
			}
			if (Utility.GetSymbolCode(item) == "HMH")
			{
				return marketDate.Add(new TimeSpan(18, 15, 0));
			}
			switch (item.ItemType)
			{
			case ItemType.Futures:
				if (!(marketDate.Date == item.LastTradingDay.Date))
				{
					return marketDate.Add(new TimeSpan(15, 50, 0));
				}
				return marketDate.Add(new TimeSpan(15, 20, 0));
			case ItemType.Options:
				if (!(marketDate.Date == item.LastTradingDay.Date))
				{
					return marketDate.Add(new TimeSpan(17, 50, 0));
				}
				return marketDate.Add(new TimeSpan(15, 20, 0));
			case ItemType.Cme:
				return marketDate.Add(new TimeSpan(29, 0, 0));
			case ItemType.Eurex:
				if (!isSummerTime)
				{
					return marketDate.Add(new TimeSpan(29, 0, 0));
				}
				return marketDate.Add(new TimeSpan(28, 0, 0));
			case ItemType.Foreign:
				return marketDate.Add(new TimeSpan(30, 0, 0));
			case ItemType.Kospi:
				return marketDate.Add(new TimeSpan(15, 30, 0));
			case ItemType.Kosdaq:
				return marketDate.Add(new TimeSpan(15, 30, 0));
			default:
				return marketDate;
			}
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x00019180 File Offset: 0x00017380
		public static MarketStateType GetDefaultMarketState(DateTime marketDate, Item item)
		{
			switch (item.ItemType)
			{
			case ItemType.Futures:
				return MarketStateType.BeforeOpen;
			case ItemType.Options:
				return MarketStateType.BeforeOpen;
			case ItemType.Cme:
				if (!(marketDate.Date == item.LastTradingDay.Date))
				{
					return MarketStateType.BeforeOpen;
				}
				return MarketStateType.Close;
			case ItemType.Eurex:
				if (!(marketDate.Date == item.LastTradingDay.Date))
				{
					return MarketStateType.BeforeOpen;
				}
				return MarketStateType.Close;
			case ItemType.Foreign:
				return MarketStateType.BeforeOpen;
			case ItemType.Kospi:
				return MarketStateType.BeforeOpen;
			case ItemType.Kosdaq:
				return MarketStateType.BeforeOpen;
			default:
				return MarketStateType.BeforeOpen;
			}
		}
	}
}
