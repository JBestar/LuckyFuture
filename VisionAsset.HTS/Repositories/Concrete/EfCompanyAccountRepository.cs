using System;
using System.Data.Entity;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Repositories.Concrete
{
	// Token: 0x02000021 RID: 33
	public class EfCompanyAccountRepository : ICompanyAccountRepository, IDisposable
	{
		// Token: 0x06000220 RID: 544 RVA: 0x0000E66F File Offset: 0x0000C86F
		public IQueryable<CompanyAccount> Get()
		{
			return this._context.CompanyAccounts;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000E67C File Offset: 0x0000C87C
		public CompanyAccount GetById(long id)
		{
			return this._context.CompanyAccounts.Find(new object[]
			{
				id
			});
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000E69D File Offset: 0x0000C89D
		public void Insert(CompanyAccount companyAccount)
		{
			this._context.CompanyAccounts.Add(companyAccount);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000E6B4 File Offset: 0x0000C8B4
		public void Delete(long id)
		{
			CompanyAccount entity = this._context.CompanyAccounts.Find(new object[]
			{
				id
			});
			this._context.CompanyAccounts.Remove(entity);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000E6F3 File Offset: 0x0000C8F3
		public void Update(CompanyAccount companyAccount)
		{
			this._context.Entry<CompanyAccount>(companyAccount).State = EntityState.Modified;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000E708 File Offset: 0x0000C908
		public void Save()
		{
			this._context.SaveChanges();
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000E716 File Offset: 0x0000C916
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000E725 File Offset: 0x0000C925
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

		// Token: 0x06000228 RID: 552 RVA: 0x0000E748 File Offset: 0x0000C948
		~EfCompanyAccountRepository()
		{
			this.Dispose(false);
		}

		// Token: 0x040001A4 RID: 420
		private readonly EfDbContext _context = new EfDbContext();

		// Token: 0x040001A5 RID: 421
		private bool _disposed;
	}
}
