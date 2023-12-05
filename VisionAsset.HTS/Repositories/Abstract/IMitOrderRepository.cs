using System;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.Repositories.Abstract
{
	// Token: 0x0200003C RID: 60
	public interface IMitOrderRepository : IDisposable
	{
		// Token: 0x06000326 RID: 806
		IQueryable<MitOrder> Get();

		// Token: 0x06000327 RID: 807
		MitOrder GetById(long id);

		// Token: 0x06000328 RID: 808
		void Insert(MitOrder mitOrder);

		// Token: 0x06000329 RID: 809
		void Delete(long id);

		// Token: 0x0600032A RID: 810
		void Update(MitOrder mitOrder);

		// Token: 0x0600032B RID: 811
		void Save();
	}
}
