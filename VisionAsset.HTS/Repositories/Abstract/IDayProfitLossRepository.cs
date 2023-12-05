using System;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.Repositories.Abstract
{
	// Token: 0x02000038 RID: 56
	public interface IDayProfitLossRepository : IDisposable
	{
		// Token: 0x0600030E RID: 782
		IQueryable<DayProfitLoss> Get();

		// Token: 0x0600030F RID: 783
		DayProfitLoss GetById(long id);

		// Token: 0x06000310 RID: 784
		void Insert(DayProfitLoss dayProfitLoss);

		// Token: 0x06000311 RID: 785
		void Delete(long id);

		// Token: 0x06000312 RID: 786
		void Update(DayProfitLoss dayProfitLoss);

		// Token: 0x06000313 RID: 787
		void Save();
	}
}
