using System;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.Repositories.Abstract
{
	// Token: 0x02000042 RID: 66
	public interface IUserAccountSpecificRepository : IDisposable
	{
		// Token: 0x0600034A RID: 842
		IQueryable<UserAccountSpecific> Get();

		// Token: 0x0600034B RID: 843
		UserAccountSpecific GetById(long id);

		// Token: 0x0600034C RID: 844
		void Insert(UserAccountSpecific userAccountSpecifics);

		// Token: 0x0600034D RID: 845
		void Delete(long id);

		// Token: 0x0600034E RID: 846
		void Update(UserAccountSpecific userAccountSpecific);

		// Token: 0x0600034F RID: 847
		void Save();
	}
}
