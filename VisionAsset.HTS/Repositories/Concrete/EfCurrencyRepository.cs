using System;
using System.Data.Entity;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Repositories.Concrete
{
	// Token: 0x02000024 RID: 36
	public class EfCurrencyRepository : ICurrencyRepository, IDisposable
	{
		// Token: 0x0600023E RID: 574 RVA: 0x0000E9C3 File Offset: 0x0000CBC3
		public IQueryable<Currency> Get()
		{
			return this._context.Currencies;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000E9D0 File Offset: 0x0000CBD0
		public Currency GetById(CurrencyType id)
		{
			return this._context.Currencies.Find(new object[]
			{
				id
			});
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000E9F1 File Offset: 0x0000CBF1
		public void Insert(Currency currency)
		{
			this._context.Currencies.Add(currency);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000EA08 File Offset: 0x0000CC08
		public void Delete(CurrencyType id)
		{
			Currency entity = this._context.Currencies.Find(new object[]
			{
				id
			});
			this._context.Currencies.Remove(entity);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000EA47 File Offset: 0x0000CC47
		public void Update(Currency currency)
		{
			this._context.Entry<Currency>(currency).State = EntityState.Modified;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000EA5C File Offset: 0x0000CC5C
		public void Save()
		{
			this._context.SaveChanges();
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000EA6A File Offset: 0x0000CC6A
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000EA79 File Offset: 0x0000CC79
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

		// Token: 0x06000246 RID: 582 RVA: 0x0000EA9C File Offset: 0x0000CC9C
		~EfCurrencyRepository()
		{
			this.Dispose(false);
		}

		// Token: 0x040001AA RID: 426
		private readonly EfDbContext _context = new EfDbContext();

		// Token: 0x040001AB RID: 427
		private bool _disposed;
	}
}
