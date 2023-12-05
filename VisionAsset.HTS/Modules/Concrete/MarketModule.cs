using System;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Modules.Abstract;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Modules.Concrete
{
	// Token: 0x02000046 RID: 70
	public class MarketModule : IMarketModule
	{
		// Token: 0x0600037B RID: 891 RVA: 0x00011B08 File Offset: 0x0000FD08
		public DateTime GetLatestMarketDate(IMarketRepository marketRepo)
		{
			Market market = (from m in marketRepo.Get()
			where (int)m.MarketStateType == 2
			orderby m.MarketDate descending
			select m).FirstOrDefault<Market>();
			if (market != null)
			{
				return market.MarketDate;
			}
			Market market2 = (from m in marketRepo.Get()
			where (int)m.MarketStateType == 1
			orderby m.MarketDate descending
			select m).FirstOrDefault<Market>();
			if (market2 != null)
			{
				return market2.MarketDate;
			}
			Market market3 = (from m in marketRepo.Get()
			where (int)m.MarketStateType == 4
			orderby m.MarketDate descending
			select m).FirstOrDefault<Market>();
			if (market3 != null)
			{
				return market3.MarketDate;
			}
			Market market4 = (from m in marketRepo.Get()
			where (int)m.MarketStateType == 5
			orderby m.MarketDate descending
			select m).FirstOrDefault<Market>();
			if (market4 != null)
			{
				return market4.MarketDate;
			}
			Market market5 = (from m in marketRepo.Get()
			where (int)m.MarketStateType == 0
			orderby m.MarketDate descending
			select m).FirstOrDefault<Market>();
			if (market5 != null)
			{
				return market5.MarketDate;
			}
			Market market6 = (from m in marketRepo.Get()
			where (int)m.MarketStateType == 7
			orderby m.MarketDate descending
			select m).FirstOrDefault<Market>();
			if (market6 != null)
			{
				return market6.MarketDate;
			}
			Market market7 = (from m in marketRepo.Get()
			where (int)m.MarketStateType == 6
			orderby m.MarketDate descending
			select m).FirstOrDefault<Market>();
			if (market7 != null)
			{
				return market7.MarketDate;
			}
			return DateTime.Now.Date;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00012073 File Offset: 0x00010273
		public bool IsTPlus1Market(Item item, Market market)
		{
			return (item.ItemType == ItemType.Cme || item.ItemType == ItemType.Eurex) && (market.MarketStateType == MarketStateType.BeforeOpen || market.MarketStateType == MarketStateType.OpenSynchronized);
		}
	}
}
