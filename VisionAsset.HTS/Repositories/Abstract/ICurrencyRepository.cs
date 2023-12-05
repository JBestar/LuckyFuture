using System;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.Repositories.Abstract
{
	// Token: 0x02000037 RID: 55
	public interface ICurrencyRepository : IDisposable
	{
		// Token: 0x06000308 RID: 776
		IQueryable<Currency> Get();

		// Token: 0x06000309 RID: 777
		Currency GetById(CurrencyType id);

		// Token: 0x0600030A RID: 778
		void Insert(Currency currency);

		// Token: 0x0600030B RID: 779
		void Delete(CurrencyType id);

		// Token: 0x0600030C RID: 780
		void Update(Currency currency);

		// Token: 0x0600030D RID: 781
		void Save();
	}
}
