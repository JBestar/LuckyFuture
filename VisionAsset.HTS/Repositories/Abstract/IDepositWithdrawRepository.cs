using System;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.Repositories.Abstract
{
	// Token: 0x02000039 RID: 57
	public interface IDepositWithdrawRepository : IDisposable
	{
		// Token: 0x06000314 RID: 788
		IQueryable<DepositWithdraw> Get();

		// Token: 0x06000315 RID: 789
		DepositWithdraw GetById(long id);

		// Token: 0x06000316 RID: 790
		void Insert(DepositWithdraw depositWithdraw);

		// Token: 0x06000317 RID: 791
		void Delete(long id);

		// Token: 0x06000318 RID: 792
		void Update(DepositWithdraw depositWithdraw);

		// Token: 0x06000319 RID: 793
		void Save();
	}
}
