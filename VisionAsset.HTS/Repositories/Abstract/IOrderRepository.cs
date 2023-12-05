using System;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.Repositories.Abstract
{
	// Token: 0x0200003E RID: 62
	public interface IOrderRepository : IDisposable
	{
		// Token: 0x06000332 RID: 818
		IQueryable<Order> Get();

		// Token: 0x06000333 RID: 819
		Order GetById(long id);

		// Token: 0x06000334 RID: 820
		void Insert(Order order);

		// Token: 0x06000335 RID: 821
		void Delete(long id);

		// Token: 0x06000336 RID: 822
		void Update(Order order);

		// Token: 0x06000337 RID: 823
		void Save();
	}
}
