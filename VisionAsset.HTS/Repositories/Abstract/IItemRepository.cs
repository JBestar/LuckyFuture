using System;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.Repositories.Abstract
{
	// Token: 0x0200003A RID: 58
	public interface IItemRepository : IDisposable
	{
		// Token: 0x0600031A RID: 794
		IQueryable<Item> Get();

		// Token: 0x0600031B RID: 795
		Item GetById(long id);

		// Token: 0x0600031C RID: 796
		void Insert(Item item);

		// Token: 0x0600031D RID: 797
		void Delete(long id);

		// Token: 0x0600031E RID: 798
		void Update(Item item);

		// Token: 0x0600031F RID: 799
		void Save();
	}
}
