using System;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Modules.Abstract
{
	// Token: 0x0200004A RID: 74
	public interface IMarketModule
	{
		// Token: 0x060003AD RID: 941
		DateTime GetLatestMarketDate(IMarketRepository marketRepo);

		// Token: 0x060003AE RID: 942
		bool IsTPlus1Market(Item item, Market market);
	}
}
