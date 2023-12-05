using System;
using System.Collections.Generic;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.Modules.Abstract
{
	// Token: 0x0200004B RID: 75
	public interface IOrderModule
	{
		// Token: 0x060003AF RID: 943
		bool IsOrderAcceptable(Order order, Market market);

		// Token: 0x060003B0 RID: 944
		bool IsClearOrder(Order order);

		// Token: 0x060003B1 RID: 945
		string CheckOrderAcceptable(UserAccount userAccount, Market market, Item item);

		// Token: 0x060003B2 RID: 946
		long CalculateLossCut(Order order, Item item);

		// Token: 0x060003B3 RID: 947
		long CalculateValuation(Order order, Item item, double currentPrice, List<Currency> currencies);

		// Token: 0x060003B4 RID: 948
		long CalculateProfit(Order openOrder, Item item, double liquidaionPrice, int liquidaionQty, List<Currency> currencies);

		// Token: 0x060003B5 RID: 949
		long CalculateCommission(UserAccount userAccount, List<UserAccountSpecific> userAccountSpecifics, Order order, Item item, int conclusionQty, double conclutionPrice, List<Currency> currencies);

		// Token: 0x060003B6 RID: 950
		long CalculateTax(Order order, Item item, int conclusionQty, double conclutionPrice, List<Currency> currencies);

		// Token: 0x060003B7 RID: 951
		long CalculateParentCommission(UserAccount userAccount, Order order, Item item, int conclusionQty, double conclutionPrice, List<Currency> currencies);

		// Token: 0x060003B8 RID: 952
		long CalculateOvernightMargin(UserAccount userAccount, Order order, Item item);

		// Token: 0x060003B9 RID: 953
		bool IsAwaitOvernight(Order order, Market market);
	}
}
