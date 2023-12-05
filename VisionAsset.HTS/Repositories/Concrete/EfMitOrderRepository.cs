using System;
using System.Data.Entity;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Repositories.Concrete
{
	// Token: 0x0200002A RID: 42
	public class EfMitOrderRepository : IMitOrderRepository, IDisposable
	{
		// Token: 0x06000296 RID: 662 RVA: 0x0000F09B File Offset: 0x0000D29B
		public IQueryable<MitOrder> Get()
		{
			return this._context.MitOrders;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000F0A8 File Offset: 0x0000D2A8
		public MitOrder GetById(long id)
		{
			return this._context.MitOrders.Find(new object[]
			{
				id
			});
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000F0C9 File Offset: 0x0000D2C9
		public void Insert(MitOrder mitOrder)
		{
			this._context.MitOrders.Add(mitOrder);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000F0E0 File Offset: 0x0000D2E0
		public void Delete(long id)
		{
			MitOrder entity = this._context.MitOrders.Find(new object[]
			{
				id
			});
			this._context.MitOrders.Remove(entity);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000F11F File Offset: 0x0000D31F
		public void Update(MitOrder mitOrder)
		{
			this._context.Entry<MitOrder>(mitOrder).State = EntityState.Modified;
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000F134 File Offset: 0x0000D334
		public void Save()
		{
			this._context.SaveChanges();
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000F142 File Offset: 0x0000D342
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000F151 File Offset: 0x0000D351
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

		// Token: 0x0600029E RID: 670 RVA: 0x0000F174 File Offset: 0x0000D374
		~EfMitOrderRepository()
		{
			this.Dispose(false);
		}

		// Token: 0x040001C6 RID: 454
		private readonly EfDbContext _context = new EfDbContext();

		// Token: 0x040001C7 RID: 455
		private bool _disposed;
	}
}
