using System;
using System.Data.Entity;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Repositories.Concrete
{
	// Token: 0x02000029 RID: 41
	public class EfMarketRepository : IMarketRepository, IDisposable
	{
		// Token: 0x0600028C RID: 652 RVA: 0x0000EF7F File Offset: 0x0000D17F
		public IQueryable<Market> Get()
		{
			return this._context.Markets;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000EF8C File Offset: 0x0000D18C
		public Market GetById(long id)
		{
			return this._context.Markets.Find(new object[]
			{
				id
			});
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000EFAD File Offset: 0x0000D1AD
		public void Insert(Market market)
		{
			this._context.Markets.Add(market);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000EFC4 File Offset: 0x0000D1C4
		public void Delete(long id)
		{
			Market entity = this._context.Markets.Find(new object[]
			{
				id
			});
			this._context.Markets.Remove(entity);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000F003 File Offset: 0x0000D203
		public void Update(Market market)
		{
			this._context.Entry<Market>(market).State = EntityState.Modified;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000F018 File Offset: 0x0000D218
		public void Save()
		{
			this._context.SaveChanges();
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000F026 File Offset: 0x0000D226
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000F035 File Offset: 0x0000D235
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

		// Token: 0x06000294 RID: 660 RVA: 0x0000F058 File Offset: 0x0000D258
		~EfMarketRepository()
		{
			this.Dispose(false);
		}

		// Token: 0x040001C4 RID: 452
		private readonly EfDbContext _context = new EfDbContext();

		// Token: 0x040001C5 RID: 453
		private bool _disposed;
	}
}
