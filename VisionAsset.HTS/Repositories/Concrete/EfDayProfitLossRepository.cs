using System;
using System.Data.Entity;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Repositories.Concrete
{
	// Token: 0x02000025 RID: 37
	public class EfDayProfitLossRepository : IDayProfitLossRepository, IDisposable
	{
		// Token: 0x06000248 RID: 584 RVA: 0x0000EADF File Offset: 0x0000CCDF
		public IQueryable<DayProfitLoss> Get()
		{
			return this._context.DayProfitLosses;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000EAEC File Offset: 0x0000CCEC
		public DayProfitLoss GetById(long id)
		{
			return this._context.DayProfitLosses.Find(new object[]
			{
				id
			});
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000EB0D File Offset: 0x0000CD0D
		public void Insert(DayProfitLoss dayProfitLoss)
		{
			this._context.DayProfitLosses.Add(dayProfitLoss);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000EB24 File Offset: 0x0000CD24
		public void Delete(long id)
		{
			DayProfitLoss entity = this._context.DayProfitLosses.Find(new object[]
			{
				id
			});
			this._context.DayProfitLosses.Remove(entity);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000EB63 File Offset: 0x0000CD63
		public void Update(DayProfitLoss dayProfitLoss)
		{
			this._context.Entry<DayProfitLoss>(dayProfitLoss).State = EntityState.Modified;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000EB78 File Offset: 0x0000CD78
		public void Save()
		{
			this._context.SaveChanges();
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000EB86 File Offset: 0x0000CD86
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000EB95 File Offset: 0x0000CD95
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

		// Token: 0x06000250 RID: 592 RVA: 0x0000EBB8 File Offset: 0x0000CDB8
		~EfDayProfitLossRepository()
		{
			this.Dispose(false);
		}

		// Token: 0x040001AC RID: 428
		private readonly EfDbContext _context = new EfDbContext();

		// Token: 0x040001AD RID: 429
		private bool _disposed;
	}
}
