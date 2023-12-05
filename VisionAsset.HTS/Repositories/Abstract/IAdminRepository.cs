using System;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.Repositories.Abstract
{
	// Token: 0x02000033 RID: 51
	public interface IAdminRepository : IDisposable
	{
		// Token: 0x060002F0 RID: 752
		IQueryable<Admin> Get();

		// Token: 0x060002F1 RID: 753
		Admin GetById(string id);

		// Token: 0x060002F2 RID: 754
		void Insert(Admin admin);

		// Token: 0x060002F3 RID: 755
		void Delete(string id);

		// Token: 0x060002F4 RID: 756
		void Update(Admin admin);

		// Token: 0x060002F5 RID: 757
		void Save();
	}
}
