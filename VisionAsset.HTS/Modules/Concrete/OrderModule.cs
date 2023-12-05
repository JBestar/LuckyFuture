using System;
using System.Collections.Generic;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Common;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Modules.Abstract;

namespace Goodbyte.TradingSystem.Domain.Modules.Concrete
{
	// Token: 0x02000047 RID: 71
	public class OrderModule : IOrderModule
	{
		// Token: 0x0600037E RID: 894 RVA: 0x0001209C File Offset: 0x0001029C
		public bool IsOrderAcceptable(Order order, Market market)
		{
			bool result = false;
			if (market.MarketStateType == MarketStateType.OpenSynchronized || market.MarketStateType == MarketStateType.Open || market.MarketStateType == MarketStateType.CloseSynchronized)
			{
				result = true;
			}
			else if (market.MarketStateType == MarketStateType.Suspension && (order.OrderType == OrderType.Cancel || order.OrderRouteType == OrderRouteType.ClearAllButton || order.OrderRouteType == OrderRouteType.ClearCurrentButton || order.OrderRouteType == OrderRouteType.OrderInfoGrid))
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x000120FC File Offset: 0x000102FC
		public bool IsClearOrder(Order order)
		{
			OrderRouteType orderRouteType = order.OrderRouteType;
			if (orderRouteType != OrderRouteType.OrderInfoGrid)
			{
				switch (orderRouteType)
				{
				case OrderRouteType.ClearCurrentButton:
					return true;
				case OrderRouteType.ClearAllButton:
					return true;
				case OrderRouteType.StopLoss:
					return true;
				case OrderRouteType.LossCut:
					return true;
				}
				return false;
			}
			return true;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00012144 File Offset: 0x00010344
		public string CheckOrderAcceptable(UserAccount userAccount, Market market, Item item)
		{
			string result = string.Empty;
			if (market.MarketStateType == MarketStateType.Suspension)
			{
				result = "거래정지 종목입니다. 청산 및 취소 주문만 가능합니다.";
			}
			else if (market.MarketStateType != MarketStateType.Open && market.MarketStateType != MarketStateType.OpenSynchronized)
			{
				result = "지금은 주문 가능 시간이 아닙니다.";
			}
			else if (item.ItemType == ItemType.Futures && market.MarketStateType == MarketStateType.CloseSynchronized && !userAccount.IsFuturesOvernight)
			{
				result = "지금은 주문 가능 시간이 아닙니다.";
			}
			else if (item.ItemType == ItemType.Options && market.MarketStateType == MarketStateType.CloseSynchronized && !userAccount.IsOptionOvernight)
			{
				result = "지금은 주문 가능 시간이 아닙니다.";
			}
			else if (item.ItemType == ItemType.Cme && market.MarketStateType == MarketStateType.CloseSynchronized && !userAccount.IsCmeOvernight)
			{
				result = "지금은 주문 가능 시간이 아닙니다.";
			}
			else if (item.ItemType == ItemType.Eurex && market.MarketStateType == MarketStateType.CloseSynchronized && !userAccount.IsEurexOvernight)
			{
				result = "지금은 주문 가능 시간이 아닙니다.";
			}
			else if (item.ItemType == ItemType.Foreign && market.MarketStateType == MarketStateType.CloseSynchronized && !userAccount.IsForeignOvernight)
			{
				result = "지금은 주문 가능 시간이 아닙니다.";
			}
			else if (item.ItemType == ItemType.Kospi && market.MarketStateType == MarketStateType.CloseSynchronized && !userAccount.IsKospiOvernight)
			{
				result = "지금은 주문 가능 시간이 아닙니다.";
			}
			else if (item.ItemType == ItemType.Kosdaq && market.MarketStateType == MarketStateType.CloseSynchronized && !userAccount.IsKosdaqOvernight)
			{
				result = "지금은 주문 가능 시간이 아닙니다.";
			}
			else if (!item.IsUse)
			{
				result = "거래 가능한 종목이 아닙니다.";
			}
			else if (item.ItemType == ItemType.Futures && !userAccount.IsFuturesTrade)
			{
				result = "현재 선물 주문이 불가능한 상태입니다. 관리자에게 문의하세요.";
			}
			else if (item.ItemType == ItemType.Options && !userAccount.IsOptionTrade)
			{
				result = "현재 옵션 주문이 불가능한 상태입니다. 관리자에게 문의하세요.";
			}
			else if (item.ItemType == ItemType.Cme && !userAccount.IsCmeTrade)
			{
				result = "현재 야간선물 주문이 불가능한 상태입니다. 관리자에게 문의하세요.";
			}
			else if (item.ItemType == ItemType.Eurex && !userAccount.IsEurexTrade)
			{
				result = "현재 야간옵션 주문이 불가능한 상태입니다. 관리자에게 문의하세요.";
			}
			else if (item.ItemType == ItemType.Foreign && !userAccount.IsForeignTrade)
			{
				result = "현재 해외선물 주문이 불가능한 상태입니다. 관리자에게 문의하세요.";
			}
			else if (item.ItemType == ItemType.Kospi && !userAccount.IsKospiTrade)
			{
				result = "현재 코스피 주식 주문이 불가능한 상태입니다. 관리자에게 문의하세요.";
			}
			else if (item.ItemType == ItemType.Kosdaq && !userAccount.IsKosdaqTrade)
			{
				result = "현재 코스닥 주식 주문이 불가능한 상태입니다. 관리자에게 문의하세요.";
			}
			else if (userAccount.UserAccountStateType == UserAccountStateType.Suspension)
			{
				result = "거래정지 계좌입니다.";
			}
			else if (userAccount.UserAccountStateType == UserAccountStateType.Losscut && (userAccount.CmeOrderSignalType == OrderSignalType.Mini || userAccount.EurexOrderSignalType == OrderSignalType.Mini || userAccount.ForeignOrderSignalType == OrderSignalType.Mini || userAccount.FuturesOrderSignalType == OrderSignalType.Mini || userAccount.OptionOrderSignalType == OrderSignalType.Mini || userAccount.KospiOrderSignalType == OrderSignalType.Mini || userAccount.KosdaqOrderSignalType == OrderSignalType.Mini))
			{
				result = "로스컷 계좌입니다. 레버리지 변경이나 추가 입금하셔야 거래할 수 있습니다.";
			}
			else if (userAccount.UserAccountStateType == UserAccountStateType.Losscut)
			{
				result = "로스컷 계좌입니다. 추가 입금하셔야 거래할 수 있습니다.";
			}
			return result;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x000123C8 File Offset: 0x000105C8
		public long CalculateLossCut(Order order, Item item)
		{
			long result;
			if (item.ItemType == ItemType.Kospi || item.ItemType == ItemType.Kosdaq)
			{
				result = ((order.TradeType == TradeType.Sell) ? Convert.ToInt64(order.Price * (double)order.UnliquidationQty * ((double)item.SellLossCutMargin / 100.0) / (double)order.ApplyLeverage / (double)item.Leverage) : Convert.ToInt64(order.Price * (double)order.UnliquidationQty * ((double)item.BuyLossCutMargin / 100.0) / (double)order.ApplyLeverage / (double)item.Leverage));
			}
			else
			{
				result = ((order.TradeType == TradeType.Sell) ? ((long)order.UnliquidationQty * item.SellLossCutMargin / (long)order.ApplyLeverage / (long)item.Leverage) : ((long)order.UnliquidationQty * item.BuyLossCutMargin / (long)order.ApplyLeverage / (long)item.Leverage));
			}
			return result;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x000124AC File Offset: 0x000106AC
		public long CalculateValuation(Order order, Item item, double currentPrice, List<Currency> currencies)
		{
			Currency currency = currencies.FirstOrDefault((Currency c) => c.CurrencyId == item.CurrencyType);
			if (currency != null)
			{
				int num = (order.TradeType == TradeType.Sell) ? (order.UnliquidationQty * -1) : order.UnliquidationQty;
				bool flag = order.Price >= item.ReferencePoint;
				bool flag2 = currentPrice >= item.ReferencePoint;
				if (flag2 && flag)
				{
					return Convert.ToInt64((currentPrice - order.Price) / item.OverTick * item.OverTickValue * (double)num / (double)order.ApplyLeverage * currency.Exchange);
				}
				if (!flag2 && !flag)
				{
					return Convert.ToInt64((currentPrice - order.Price) / item.UnderTick * item.UnderTickValue * (double)num / (double)order.ApplyLeverage * currency.Exchange);
				}
				if (flag2 && !flag)
				{
					long num2 = Convert.ToInt64((currentPrice - item.ReferencePoint) / item.OverTick * item.OverTickValue * (double)num / (double)order.ApplyLeverage * currency.Exchange);
					long num3 = Convert.ToInt64((item.ReferencePoint - order.Price) / item.UnderTick * item.UnderTickValue * (double)num / (double)order.ApplyLeverage * currency.Exchange);
					return num2 + num3;
				}
				if (!flag2 && flag)
				{
					long num4 = Convert.ToInt64((currentPrice - item.ReferencePoint) / item.UnderTick * item.UnderTickValue * (double)num / (double)order.ApplyLeverage * currency.Exchange);
					long num5 = Convert.ToInt64((item.ReferencePoint - order.Price) / item.OverTick * item.OverTickValue * (double)num / (double)order.ApplyLeverage * currency.Exchange);
					return num4 + num5;
				}
			}
			return 0L;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x000126C4 File Offset: 0x000108C4
		public long CalculateProfit(Order openOrder, Item item, double liquidaionPrice, int liquidaionQty, List<Currency> currencies)
		{
			Currency currency = currencies.FirstOrDefault((Currency c) => c.CurrencyId == item.CurrencyType);
			if (currency != null)
			{
				int num = (openOrder.TradeType == TradeType.Sell) ? (liquidaionQty * -1) : liquidaionQty;
				bool flag = openOrder.MovingAveragePrice >= item.ReferencePoint;
				bool flag2 = liquidaionPrice >= item.ReferencePoint;
				if (flag2 && flag)
				{
					return Convert.ToInt64((liquidaionPrice - openOrder.MovingAveragePrice) / item.OverTick * item.OverTickValue * (double)num / (double)openOrder.ApplyLeverage * currency.Exchange);
				}
				if (!flag2 && !flag)
				{
					return Convert.ToInt64((liquidaionPrice - openOrder.MovingAveragePrice) / item.UnderTick * item.UnderTickValue * (double)num / (double)openOrder.ApplyLeverage * currency.Exchange);
				}
				if (flag2 && !flag)
				{
					long num2 = Convert.ToInt64((liquidaionPrice - item.ReferencePoint) / item.OverTick * item.OverTickValue * (double)num / (double)openOrder.ApplyLeverage * currency.Exchange);
					long num3 = Convert.ToInt64((item.ReferencePoint - openOrder.MovingAveragePrice) / item.UnderTick * item.UnderTickValue * (double)num / (double)openOrder.ApplyLeverage * currency.Exchange);
					return num2 + num3;
				}
				if (!flag2 && flag)
				{
					long num4 = Convert.ToInt64((liquidaionPrice - item.ReferencePoint) / item.UnderTick * item.UnderTickValue * (double)num / (double)openOrder.ApplyLeverage * currency.Exchange);
					long num5 = Convert.ToInt64((item.ReferencePoint - openOrder.MovingAveragePrice) / item.OverTick * item.OverTickValue * (double)num / (double)openOrder.ApplyLeverage * currency.Exchange);
					return num4 + num5;
				}
			}
			return 0L;
		}

		// Token: 0x06000384 RID: 900 RVA: 0x000128D4 File Offset: 0x00010AD4
		public long CalculateCommission(UserAccount userAccount, List<UserAccountSpecific> userAccountSpecifics, Order order, Item item, int conclusionQty, double conclutionPrice, List<Currency> currencies)
		{
			long result = 0L;
			Currency currency = currencies.FirstOrDefault((Currency c) => c.CurrencyId == item.CurrencyType);
			if (currency != null)
			{
				switch (item.ItemType)
				{
				case ItemType.Futures:
					result = Convert.ToInt64(Math.Ceiling(userAccount.FuturesCommission * conclutionPrice * (item.OverTickValue / item.OverTick) * currency.Exchange * (double)conclusionQty / 100.0 / (double)order.ApplyLeverage));
					break;
				case ItemType.Options:
					result = Convert.ToInt64(Math.Ceiling(userAccount.OptionCommission * conclutionPrice * (item.OverTickValue / item.OverTick) * currency.Exchange * (double)conclusionQty / 100.0 / (double)order.ApplyLeverage));
					break;
				case ItemType.Cme:
					result = Convert.ToInt64(Math.Ceiling(userAccount.CmeCommission * conclutionPrice * (item.OverTickValue / item.OverTick) * currency.Exchange * (double)conclusionQty / 100.0 / (double)order.ApplyLeverage));
					break;
				case ItemType.Eurex:
					result = Convert.ToInt64(Math.Ceiling(userAccount.EurexCommission * conclutionPrice * (item.OverTickValue / item.OverTick) * currency.Exchange * (double)conclusionQty / 100.0 / (double)order.ApplyLeverage));
					break;
				case ItemType.Foreign:
					result = Convert.ToInt64(Math.Ceiling(userAccount.ForeignCommission * currency.Exchange * (double)conclusionQty / (double)order.ApplyLeverage));
					break;
				case ItemType.Kospi:
					result = ((order.TradeType == TradeType.Buy) ? Convert.ToInt64(Math.Ceiling(userAccount.KospiCommission * conclutionPrice * currency.Exchange * (double)conclusionQty / 100.0 / (double)order.ApplyLeverage)) : Convert.ToInt64(Math.Ceiling((userAccount.KospiCommission + 0.3) * conclutionPrice * currency.Exchange * (double)conclusionQty / 100.0 / (double)order.ApplyLeverage)));
					break;
				case ItemType.Kosdaq:
					result = ((order.TradeType == TradeType.Buy) ? Convert.ToInt64(Math.Ceiling(userAccount.KosdaqCommission * conclutionPrice * currency.Exchange * (double)conclusionQty / 100.0 / (double)order.ApplyLeverage)) : Convert.ToInt64(Math.Ceiling((userAccount.KosdaqCommission + 0.3) * conclutionPrice * currency.Exchange * (double)conclusionQty / 100.0 / (double)order.ApplyLeverage)));
					break;
				}
				if (userAccountSpecifics != null && userAccountSpecifics.Any<UserAccountSpecific>())
				{
					UserAccountSpecific userAccountSpecific = (from u in userAccountSpecifics
					where u.UserAccountId == userAccount.UserAccountId
					select u).FirstOrDefault((UserAccountSpecific u) => u.SymbolCode == Utility.GetSymbolCode(item));
					if (userAccountSpecific != null)
					{
						switch (item.ItemType)
						{
						case ItemType.Futures:
							result = Convert.ToInt64(Math.Ceiling(userAccountSpecific.ItemCommission * conclutionPrice * (item.OverTickValue / item.OverTick) * currency.Exchange * (double)conclusionQty / 100.0 / (double)order.ApplyLeverage));
							break;
						case ItemType.Options:
							result = Convert.ToInt64(Math.Ceiling(userAccountSpecific.ItemCommission * conclutionPrice * (item.OverTickValue / item.OverTick) * currency.Exchange * (double)conclusionQty / 100.0 / (double)order.ApplyLeverage));
							break;
						case ItemType.Cme:
							result = Convert.ToInt64(Math.Ceiling(userAccountSpecific.ItemCommission * conclutionPrice * (item.OverTickValue / item.OverTick) * currency.Exchange * (double)conclusionQty / 100.0 / (double)order.ApplyLeverage));
							break;
						case ItemType.Eurex:
							result = Convert.ToInt64(Math.Ceiling(userAccountSpecific.ItemCommission * conclutionPrice * (item.OverTickValue / item.OverTick) * currency.Exchange * (double)conclusionQty / 100.0 / (double)order.ApplyLeverage));
							break;
						case ItemType.Foreign:
							result = Convert.ToInt64(Math.Ceiling(userAccountSpecific.ItemCommission * currency.Exchange * (double)conclusionQty / (double)order.ApplyLeverage));
							break;
						case ItemType.Kospi:
							result = ((order.TradeType == TradeType.Buy) ? Convert.ToInt64(Math.Ceiling(userAccountSpecific.ItemCommission * conclutionPrice * currency.Exchange * (double)conclusionQty / 100.0 / (double)order.ApplyLeverage)) : Convert.ToInt64(Math.Ceiling((userAccountSpecific.ItemCommission + 0.3) * conclutionPrice * currency.Exchange * (double)conclusionQty / 100.0 / (double)order.ApplyLeverage)));
							break;
						case ItemType.Kosdaq:
							result = ((order.TradeType == TradeType.Buy) ? Convert.ToInt64(Math.Ceiling(userAccountSpecific.ItemCommission * conclutionPrice * currency.Exchange * (double)conclusionQty / 100.0 / (double)order.ApplyLeverage)) : Convert.ToInt64(Math.Ceiling((userAccountSpecific.ItemCommission + 0.3) * conclutionPrice * currency.Exchange * (double)conclusionQty / 100.0 / (double)order.ApplyLeverage)));
							break;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00012E90 File Offset: 0x00011090
		public long CalculateTax(Order order, Item item, int conclusionQty, double conclutionPrice, List<Currency> currencies)
		{
			long result = 0L;
			Currency currency = currencies.FirstOrDefault((Currency c) => c.CurrencyId == item.CurrencyType);
			if (currency != null)
			{
				result = ((order.TradeType == TradeType.Sell && (item.ItemType == ItemType.Kospi || item.ItemType == ItemType.Kosdaq)) ? Convert.ToInt64(Math.Ceiling(0.3 * conclutionPrice * currency.Exchange * (double)conclusionQty / 100.0 / (double)order.ApplyLeverage)) : 0L);
			}
			return result;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00012F20 File Offset: 0x00011120
		public long CalculateParentCommission(UserAccount userAccount, Order order, Item item, int conclusionQty, double conclutionPrice, List<Currency> currencies)
		{
			long result = 0L;
			Currency currency = currencies.FirstOrDefault((Currency c) => c.CurrencyId == item.CurrencyType);
			if (currency != null)
			{
				switch (item.ItemType)
				{
				case ItemType.Futures:
					result = Convert.ToInt64(Math.Ceiling(item.ParentCommission * conclutionPrice * (item.OverTickValue / item.OverTick) * currency.Exchange * (double)conclusionQty / 100.0 / (double)order.ApplyLeverage));
					break;
				case ItemType.Options:
					result = Convert.ToInt64(Math.Ceiling(item.ParentCommission * conclutionPrice * (item.OverTickValue / item.OverTick) * currency.Exchange * (double)conclusionQty / 100.0 / (double)order.ApplyLeverage));
					break;
				case ItemType.Cme:
					result = Convert.ToInt64(Math.Ceiling(item.ParentCommission * conclutionPrice * (item.OverTickValue / item.OverTick) * currency.Exchange * (double)conclusionQty / 100.0 / (double)order.ApplyLeverage));
					break;
				case ItemType.Eurex:
					result = Convert.ToInt64(Math.Ceiling(item.ParentCommission * conclutionPrice * (item.OverTickValue / item.OverTick) * currency.Exchange * (double)conclusionQty / 100.0 / (double)order.ApplyLeverage));
					break;
				case ItemType.Foreign:
					result = Convert.ToInt64(Math.Ceiling(item.ParentCommission * currency.Exchange * (double)conclusionQty / (double)order.ApplyLeverage));
					break;
				case ItemType.Kospi:
				case ItemType.Kosdaq:
					result = Convert.ToInt64(Math.Ceiling(item.ParentCommission * conclutionPrice * currency.Exchange * (double)conclusionQty / 100.0 / (double)order.ApplyLeverage));
					break;
				}
			}
			return result;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00013138 File Offset: 0x00011338
		public long CalculateOvernightMargin(UserAccount userAccount, Order order, Item item)
		{
			long result = 0L;
			switch (item.ItemType)
			{
			case ItemType.Futures:
				if (order.TradeType == TradeType.Sell)
				{
					result = item.SellOvernightMinimumMargin * (long)order.UnliquidationQty / (long)userAccount.Leverage;
				}
				else
				{
					result = item.BuyOvernightMinimumMargin * (long)order.UnliquidationQty / (long)userAccount.Leverage;
				}
				break;
			case ItemType.Options:
				if (order.TradeType == TradeType.Sell)
				{
					result = item.SellOvernightMinimumMargin * (long)order.UnliquidationQty / (long)userAccount.Leverage;
				}
				else
				{
					result = ((item.BuyOvernightMinimumMargin > 0L) ? (item.BuyOvernightMinimumMargin * (long)order.UnliquidationQty / (long)userAccount.Leverage) : Convert.ToInt64(order.Price * (item.OverTickValue / item.OverTick) * (double)order.UnliquidationQty / (double)userAccount.Leverage));
				}
				break;
			case ItemType.Cme:
				if (order.TradeType == TradeType.Sell)
				{
					result = item.SellOvernightMinimumMargin * (long)order.UnliquidationQty / (long)userAccount.Leverage;
				}
				else
				{
					result = item.BuyOvernightMinimumMargin * (long)order.UnliquidationQty / (long)userAccount.Leverage;
				}
				break;
			case ItemType.Eurex:
				if (order.TradeType == TradeType.Sell)
				{
					result = item.SellOvernightMinimumMargin * (long)order.UnliquidationQty / (long)userAccount.Leverage;
				}
				else
				{
					result = ((item.BuyOvernightMinimumMargin > 0L) ? (item.BuyOvernightMinimumMargin * (long)order.UnliquidationQty / (long)userAccount.Leverage) : Convert.ToInt64(order.Price * (item.OverTickValue / item.OverTick) * (double)order.UnliquidationQty / (double)userAccount.Leverage));
				}
				break;
			case ItemType.Foreign:
				if (order.TradeType == TradeType.Sell)
				{
					result = item.SellOvernightMinimumMargin * (long)order.UnliquidationQty / (long)userAccount.Leverage;
				}
				else
				{
					result = item.BuyOvernightMinimumMargin * (long)order.UnliquidationQty / (long)userAccount.Leverage;
				}
				break;
			case ItemType.Kospi:
				if (order.TradeType == TradeType.Sell)
				{
					result = Convert.ToInt64(order.Price * (double)order.UnliquidationQty * ((double)item.SellOvernightMinimumMargin / 100.0) / (double)userAccount.Leverage);
				}
				else
				{
					result = Convert.ToInt64(order.Price * (double)order.UnliquidationQty * ((double)item.BuyOvernightMinimumMargin / 100.0) / (double)userAccount.Leverage);
				}
				break;
			case ItemType.Kosdaq:
				if (order.TradeType == TradeType.Sell)
				{
					result = Convert.ToInt64(order.Price * (double)order.UnliquidationQty * ((double)item.SellOvernightMinimumMargin / 100.0) / (double)userAccount.Leverage);
				}
				else
				{
					result = Convert.ToInt64(order.Price * (double)order.UnliquidationQty * ((double)item.BuyOvernightMinimumMargin / 100.0) / (double)userAccount.Leverage);
				}
				break;
			}
			return result;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x000133F2 File Offset: 0x000115F2
		public bool IsAwaitOvernight(Order order, Market market)
		{
			return order.OrderRouteType == OrderRouteType.AwaitOvernight || (order.OrderRouteType == OrderRouteType.Overnight && (market.MarketStateType == MarketStateType.BeforeOpen || market.MarketStateType == MarketStateType.OpenSynchronized));
		}
	}
}
