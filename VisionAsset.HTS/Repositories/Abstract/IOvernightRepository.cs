using System;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.Repositories.Abstract
{
	// Token: 0x0200003F RID: 63
	public interface IOvernightRepository : IDisposable
	{
		// Token: 0x06000338 RID: 824
		IQueryable<Overnight> Get();

		// Token: 0x06000339 RID: 825
		Overnight GetById(long id);

		// Token: 0x0600033A RID: 826
		void Insert(Overnight overnight);

		// Token: 0x0600033B RID: 827
		void Delete(long id);

		// Token: 0x0600033C RID: 828
		void Update(Overnight overnight);

		// Token: 0x0600033D RID: 829
		void Save();
	}
}
