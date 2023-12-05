using System;
using System.Data.Entity;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Repositories.Concrete
{
	// Token: 0x02000028 RID: 40
	public class EfItemRepository : IItemRepository, IDisposable
	{
		// Token: 0x06000282 RID: 642 RVA: 0x0000EE63 File Offset: 0x0000D063
		public IQueryable<Item> Get()
		{
			return this._context.Items;
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000EE70 File Offset: 0x0000D070
		public Item GetById(long id)
		{
			return this._context.Items.Find(new object[]
			{
				id
			});
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000EE91 File Offset: 0x0000D091
		public void Insert(Item item)
		{
			this._context.Items.Add(item);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000EEA8 File Offset: 0x0000D0A8
		public void Delete(long id)
		{
			Item entity = this._context.Items.Find(new object[]
			{
				id
			});
			this._context.Items.Remove(entity);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000EEE7 File Offset: 0x0000D0E7
		public void Update(Item item)
		{
			this._context.Entry<Item>(item).State = EntityState.Modified;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000EEFC File Offset: 0x0000D0FC
		public void Save()
		{
			this._context.SaveChanges();
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000EF0A File Offset: 0x0000D10A
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000EF19 File Offset: 0x0000D119
		protected virtual void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			if (disposing)
			{
				this._context.Dispose();
			}
			this._disposed = true;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000EF3C File Offset: 0x0000D13C
		~EfItemRepository()
		{
			this.Dispose(false);
		}

		// Token: 0x040001C2 RID: 450
		private readonly EfDbContext _context = new EfDbContext();

		// Token: 0x040001C3 RID: 451
		private bool _disposed;
	}
}
