using System;
using System.Data.Entity;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Repositories.Concrete
{
	// Token: 0x0200002F RID: 47
	public class EfStopLossRepository : IStopLossRepository, IDisposable
	{
		// Token: 0x060002C8 RID: 712 RVA: 0x0000F627 File Offset: 0x0000D827
		public IQueryable<StopLoss> Get()
		{
			return this._context.StopLosses;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000F634 File Offset: 0x0000D834
		public StopLoss GetById(long id)
		{
			return this._context.StopLosses.Find(new object[]
			{
				id
			});
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000F655 File Offset: 0x0000D855
		public void Insert(StopLoss stopLoss)
		{
			this._context.StopLosses.Add(stopLoss);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000F66C File Offset: 0x0000D86C
		public void Delete(long id)
		{
			StopLoss entity = this._context.StopLosses.Find(new object[]
			{
				id
			});
			this._context.StopLosses.Remove(entity);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000F6AB File Offset: 0x0000D8AB
		public void Update(StopLoss stopLoss)
		{
			this._context.Entry<StopLoss>(stopLoss).State = EntityState.Modified;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000F6C0 File Offset: 0x0000D8C0
		public void Save()
		{
			this._context.SaveChanges();
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000F6CE File Offset: 0x0000D8CE
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000F6DD File Offset: 0x0000D8DD
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

		// Token: 0x060002D0 RID: 720 RVA: 0x0000F700 File Offset: 0x0000D900
		~EfStopLossRepository()
		{
			this.Dispose(false);
		}

		// Token: 0x040001D0 RID: 464
		private readonly EfDbContext _context = new EfDbContext();

		// Token: 0x040001D1 RID: 465
		private bool _disposed;
	}
}
