using System;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.Repositories.Abstract
{
	// Token: 0x0200003B RID: 59
	public interface IMarketRepository : IDisposable
	{
		// Token: 0x06000320 RID: 800
		IQueryable<Market> Get();

		// Token: 0x06000321 RID: 801
		Market GetById(long id);

		// Token: 0x06000322 RID: 802
		void Insert(Market market);

		// Token: 0x06000323 RID: 803
		void Delete(long id);

		// Token: 0x06000324 RID: 804
		void Update(Market market);

		// Token: 0x06000325 RID: 805
		void Save();
	}
}
