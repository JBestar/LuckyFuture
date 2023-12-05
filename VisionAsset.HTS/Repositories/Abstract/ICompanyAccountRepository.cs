using System;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.Repositories.Abstract
{
	// Token: 0x02000034 RID: 52
	public interface ICompanyAccountRepository : IDisposable
	{
		// Token: 0x060002F6 RID: 758
		IQueryable<CompanyAccount> Get();

		// Token: 0x060002F7 RID: 759
		CompanyAccount GetById(long id);

		// Token: 0x060002F8 RID: 760
		void Insert(CompanyAccount companyAccount);

		// Token: 0x060002F9 RID: 761
		void Delete(long id);

		// Token: 0x060002FA RID: 762
		void Update(CompanyAccount companyAccount);

		// Token: 0x060002FB RID: 763
		void Save();
	}
}
