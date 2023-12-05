using System;
using System.Collections.Generic;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Common;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Modules.Abstract;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Modules.Concrete
{
	// Token: 0x02000048 RID: 72
	public class UserAccountModule : IUserAccountModule
	{
		// Token: 0x0600038A RID: 906 RVA: 0x00013420 File Offset: 0x00011620
		public OrderSignalType GetOrderSignalType(IUserAccountRepository userAccountRepo, IUserAccountSpecificRepository userAccountSpecificRepo, long userAccountId, Item item)
		{
			OrderSignalType result = OrderSignalType.Mini;
			UserAccount userAccount = userAccountRepo.GetById(userAccountId);
			List<UserAccountSpecific> source = (from u in userAccountSpecificRepo.Get()
			where u.UserAccountId == userAccount.UserAccountId
			select u).ToList<UserAccountSpecific>();
			if (userAccount != null)
			{
				switch (item.ItemType)
				{
				case ItemType.Futures:
					result = userAccount.FuturesOrderSignalType;
					break;
				case ItemType.Options:
					result = userAccount.OptionOrderSignalType;
					break;
				case ItemType.Cme:
					result = userAccount.CmeOrderSignalType;
					break;
				case ItemType.Eurex:
					result = userAccount.EurexOrderSignalType;
					break;
				case ItemType.Foreign:
					result = userAccount.ForeignOrderSignalType;
					break;
				case ItemType.Kospi:
					result = userAccount.KospiOrderSignalType;
					break;
				case ItemType.Kosdaq:
					result = userAccount.KosdaqOrderSignalType;
					break;
				}
				if (source.Any<UserAccountSpecific>())
				{
					UserAccountSpecific userAccountSpecific = source.FirstOrDefault((UserAccountSpecific u) => u.SymbolCode == Utility.GetSymbolCode(item));
					if (userAccountSpecific != null)
					{
						result = userAccountSpecific.ItemOrderSignalType;
					}
				}
			}
			return result;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00013594 File Offset: 0x00011794
		public OrderSignalType GetOrderSignalType(UserAccount userAccount, List<UserAccountSpecific> userAccountSpecifics, Item item)
		{
			OrderSignalType result = OrderSignalType.Mini;
			if (userAccount != null)
			{
				switch (item.ItemType)
				{
				case ItemType.Futures:
					result = userAccount.FuturesOrderSignalType;
					break;
				case ItemType.Options:
					result = userAccount.OptionOrderSignalType;
					break;
				case ItemType.Cme:
					result = userAccount.CmeOrderSignalType;
					break;
				case ItemType.Eurex:
					result = userAccount.EurexOrderSignalType;
					break;
				case ItemType.Foreign:
					result = userAccount.ForeignOrderSignalType;
					break;
				case ItemType.Kospi:
					result = userAccount.KospiOrderSignalType;
					break;
				case ItemType.Kosdaq:
					result = userAccount.KosdaqOrderSignalType;
					break;
				}
				if (userAccountSpecifics != null && userAccountSpecifics.Any<UserAccountSpecific>())
				{
					UserAccountSpecific userAccountSpecific = (from u in userAccountSpecifics
					where u.UserAccountId == userAccount.UserAccountId
					select u).FirstOrDefault((UserAccountSpecific u) => u.SymbolCode == Utility.GetSymbolCode(item));
					if (userAccountSpecific != null)
					{
						result = userAccountSpecific.ItemOrderSignalType;
					}
				}
			}
			return result;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0001368C File Offset: 0x0001188C
		public void GetMaxSellBuyQty(IUserAccountRepository userAccountRepo, IUserAccountSpecificRepository userAccountSpecificRepo, long userAccountId, Item item, out int maxSellQty, out int maxBuyQty)
		{
			maxSellQty = 0;
			maxBuyQty = 0;
			UserAccount userAccount = userAccountRepo.GetById(userAccountId);
			List<UserAccountSpecific> source = (from u in userAccountSpecificRepo.Get()
			where u.UserAccountId == userAccount.UserAccountId
			select u).ToList<UserAccountSpecific>();
			if (userAccount != null)
			{
				switch (item.ItemType)
				{
				case ItemType.Futures:
					maxSellQty = userAccount.FuturesMaxSellQty;
					maxBuyQty = userAccount.FuturesMaxBuyQty;
					break;
				case ItemType.Options:
					maxSellQty = userAccount.OptionMaxSellQty;
					maxBuyQty = userAccount.OptionMaxBuyQty;
					break;
				case ItemType.Cme:
					maxSellQty = userAccount.CmeMaxSellQty;
					maxBuyQty = userAccount.CmeMaxBuyQty;
					break;
				case ItemType.Eurex:
					maxSellQty = userAccount.EurexMaxSellQty;
					maxBuyQty = userAccount.EurexMaxBuyQty;
					break;
				case ItemType.Foreign:
					maxSellQty = userAccount.ForeignMaxSellQty;
					maxBuyQty = userAccount.ForeignMaxBuyQty;
					break;
				}
				if (source.Any<UserAccountSpecific>())
				{
					UserAccountSpecific userAccountSpecific = source.FirstOrDefault((UserAccountSpecific u) => u.SymbolCode == Utility.GetSymbolCode(item));
					if (userAccountSpecific != null)
					{
						maxSellQty = userAccountSpecific.ItemMaxSellQty;
						maxBuyQty = userAccountSpecific.ItemMaxBuyQty;
					}
				}
			}
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0001383C File Offset: 0x00011A3C
		public void GetMaxSellBuyQty(UserAccount userAccount, List<UserAccountSpecific> userAccountSpecifics, Item item, out int maxSellQty, out int maxBuyQty)
		{
			maxSellQty = 0;
			maxBuyQty = 0;
			if (userAccount != null)
			{
				switch (item.ItemType)
				{
				case ItemType.Futures:
					maxSellQty = userAccount.FuturesMaxSellQty;
					maxBuyQty = userAccount.FuturesMaxBuyQty;
					break;
				case ItemType.Options:
					maxSellQty = userAccount.OptionMaxSellQty;
					maxBuyQty = userAccount.OptionMaxBuyQty;
					break;
				case ItemType.Cme:
					maxSellQty = userAccount.CmeMaxSellQty;
					maxBuyQty = userAccount.CmeMaxBuyQty;
					break;
				case ItemType.Eurex:
					maxSellQty = userAccount.EurexMaxSellQty;
					maxBuyQty = userAccount.EurexMaxBuyQty;
					break;
				case ItemType.Foreign:
					maxSellQty = userAccount.ForeignMaxSellQty;
					maxBuyQty = userAccount.ForeignMaxBuyQty;
					break;
				}
				if (userAccountSpecifics != null && userAccountSpecifics.Any<UserAccountSpecific>())
				{
					UserAccountSpecific userAccountSpecific = (from u in userAccountSpecifics
					where u.UserAccountId == userAccount.UserAccountId
					select u).FirstOrDefault((UserAccountSpecific u) => u.SymbolCode == Utility.GetSymbolCode(item));
					if (userAccountSpecific != null)
					{
						maxSellQty = userAccountSpecific.ItemMaxSellQty;
						maxBuyQty = userAccountSpecific.ItemMaxBuyQty;
					}
				}
			}
		}
	}
}
