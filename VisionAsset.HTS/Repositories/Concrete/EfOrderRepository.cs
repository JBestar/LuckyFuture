using System;
using System.Data.Entity;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Repositories.Concrete
{
	// Token: 0x0200002C RID: 44
	public class EfOrderRepository : IOrderRepository, IDisposable
	{
		// Token: 0x060002AA RID: 682 RVA: 0x0000F2D3 File Offset: 0x0000D4D3
		public IQueryable<Order> Get()
		{
			return this._context.Orders;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000F2E0 File Offset: 0x0000D4E0
		public Order GetById(long id)
		{
			return this._context.Orders.Find(new object[]
			{
				id
			});
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000F301 File Offset: 0x0000D501
		public void Insert(Order order)
		{
			this._context.Orders.Add(order);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000F318 File Offset: 0x0000D518
		public void Delete(long id)
		{
			Order entity = this._context.Orders.Find(new object[]
			{
				id
			});
			this._context.Orders.Remove(entity);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000F357 File Offset: 0x0000D557
		public void Update(Order order)
		{
			this._context.Entry<Order>(order).State = EntityState.Modified;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000F36C File Offset: 0x0000D56C
		public void Save()
		{
			this._context.SaveChanges();
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000F37A File Offset: 0x0000D57A
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000F389 File Offset: 0x0000D589
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

		// Token: 0x060002B2 RID: 690 RVA: 0x0000F3AC File Offset: 0x0000D5AC
		~EfOrderRepository()
		{
			this.Dispose(false);
		}

		// Token: 0x040001CA RID: 458
		private readonly EfDbContext _context = new EfDbContext();

		// Token: 0x040001CB RID: 459
		private bool _disposed;
	}
}
