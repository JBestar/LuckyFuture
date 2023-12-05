using System;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.Repositories.Abstract
{
	// Token: 0x02000040 RID: 64
	public interface IParentAccountRepository : IDisposable
	{
		// Token: 0x0600033E RID: 830
		IQueryable<ParentAccount> Get();

		// Token: 0x0600033F RID: 831
		ParentAccount GetById(long id);

		// Token: 0x06000340 RID: 832
		void Insert(ParentAccount parentAccount);

		// Token: 0x06000341 RID: 833
		void Delete(long id);

		// Token: 0x06000342 RID: 834
		void Update(ParentAccount parentAccount);

		// Token: 0x06000343 RID: 835
		void Save();
	}
}
