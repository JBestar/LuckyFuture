using System;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.Repositories.Abstract
{
	// Token: 0x02000035 RID: 53
	public interface ICompanyRepository : IDisposable
	{
		// Token: 0x060002FC RID: 764
		IQueryable<Company> Get();

		// Token: 0x060002FD RID: 765
		Company GetById(long id);

		// Token: 0x060002FE RID: 766
		void Insert(Company company);

		// Token: 0x060002FF RID: 767
		void Delete(long id);

		// Token: 0x06000300 RID: 768
		void Update(Company company);

		// Token: 0x06000301 RID: 769
		void Save();
	}
}
