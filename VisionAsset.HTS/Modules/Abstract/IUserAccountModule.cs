using System;
using System.Collections.Generic;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Modules.Abstract
{
	// Token: 0x0200004C RID: 76
	public interface IUserAccountModule
	{
		// Token: 0x060003BA RID: 954
		OrderSignalType GetOrderSignalType(IUserAccountRepository userAccountRepo, IUserAccountSpecificRepository userAccountSpecificRepo, long userAccountId, Item item);

		// Token: 0x060003BB RID: 955
		OrderSignalType GetOrderSignalType(UserAccount userAccount, List<UserAccountSpecific> userAccountSpecifics, Item item);

		// Token: 0x060003BC RID: 956
		void GetMaxSellBuyQty(IUserAccountRepository userAccountRepo, IUserAccountSpecificRepository userAccountSpecificRepo, long userAccountId, Item item, out int maxSellQty, out int maxBuyQty);

		// Token: 0x060003BD RID: 957
		void GetMaxSellBuyQty(UserAccount userAccount, List<UserAccountSpecific> userAccountSpecifics, Item item, out int maxSellQty, out int maxBuyQty);
	}
}
