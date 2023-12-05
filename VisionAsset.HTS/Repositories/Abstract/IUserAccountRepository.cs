using System;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.Repositories.Abstract
{
	// Token: 0x02000043 RID: 67
	public interface IUserAccountRepository : IDisposable
	{
		// Token: 0x06000350 RID: 848
		IQueryable<UserAccount> Get();

		// Token: 0x06000351 RID: 849
		UserAccount GetById(long id);

		// Token: 0x06000352 RID: 850
		void Insert(UserAccount userAccount);

		// Token: 0x06000353 RID: 851
		void Delete(long id);

		// Token: 0x06000354 RID: 852
		void Update(UserAccount userAccount);

		// Token: 0x06000355 RID: 853
		void Save();
	}
}
