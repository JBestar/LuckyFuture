using System;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.Repositories.Abstract
{
	// Token: 0x02000044 RID: 68
	public interface IUserRepository : IDisposable
	{
		// Token: 0x06000356 RID: 854
		IQueryable<User> Get();

		// Token: 0x06000357 RID: 855
		User GetById(long id);

		// Token: 0x06000358 RID: 856
		void Insert(User user);

		// Token: 0x06000359 RID: 857
		void Delete(long id);

		// Token: 0x0600035A RID: 858
		void Update(User user);

		// Token: 0x0600035B RID: 859
		void Save();
	}
}
