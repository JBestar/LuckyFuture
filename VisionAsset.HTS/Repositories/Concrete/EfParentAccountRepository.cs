using System;
using System.Data.Entity;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Repositories.Concrete
{
	// Token: 0x0200002E RID: 46
	public class EfParentAccountRepository : IParentAccountRepository, IDisposable
	{
		// Token: 0x060002BE RID: 702 RVA: 0x0000F50B File Offset: 0x0000D70B
		public IQueryable<ParentAccount> Get()
		{
			return this._context.StockAccounts;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000F518 File Offset: 0x0000D718
		public ParentAccount GetById(long id)
		{
			return this._context.StockAccounts.Find(new object[]
			{
				id
			});
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000F539 File Offset: 0x0000D739
		public void Insert(ParentAccount parentAccount)
		{
			this._context.StockAccounts.Add(parentAccount);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000F550 File Offset: 0x0000D750
		public void Delete(long id)
		{
			ParentAccount entity = this._context.StockAccounts.Find(new object[]
			{
				id
			});
			this._context.StockAccounts.Remove(entity);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000F58F File Offset: 0x0000D78F
		public void Update(ParentAccount parentAccount)
		{
			this._context.Entry<ParentAccount>(parentAccount).State = EntityState.Modified;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000F5A4 File Offset: 0x0000D7A4
		public void Save()
		{
			this._context.SaveChanges();
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000F5B2 File Offset: 0x0000D7B2
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000F5C1 File Offset: 0x0000D7C1
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

		// Token: 0x060002C6 RID: 710 RVA: 0x0000F5E4 File Offset: 0x0000D7E4
		~EfParentAccountRepository()
		{
			this.Dispose(false);
		}

		// Token: 0x040001CE RID: 462
		private readonly EfDbContext _context = new EfDbContext();

		// Token: 0x040001CF RID: 463
		private bool _disposed;
	}
}
