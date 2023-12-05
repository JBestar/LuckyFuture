using System;
using System.Data.Entity;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Repositories.Concrete
{
	// Token: 0x02000022 RID: 34
	public class EfCompanyRepository : ICompanyRepository, IDisposable
	{
		// Token: 0x0600022A RID: 554 RVA: 0x0000E78B File Offset: 0x0000C98B
		public IQueryable<Company> Get()
		{
			return this._context.Companies;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000E798 File Offset: 0x0000C998
		public Company GetById(long id)
		{
			return this._context.Companies.Find(new object[]
			{
				id
			});
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000E7B9 File Offset: 0x0000C9B9
		public void Insert(Company company)
		{
			this._context.Companies.Add(company);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000E7D0 File Offset: 0x0000C9D0
		public void Delete(long id)
		{
			Company entity = this._context.Companies.Find(new object[]
			{
				id
			});
			this._context.Companies.Remove(entity);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000E80F File Offset: 0x0000CA0F
		public void Update(Company company)
		{
			this._context.Entry<Company>(company).State = EntityState.Modified;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000E824 File Offset: 0x0000CA24
		public void Save()
		{
			this._context.SaveChanges();
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000E832 File Offset: 0x0000CA32
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000E841 File Offset: 0x0000CA41
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

		// Token: 0x06000232 RID: 562 RVA: 0x0000E864 File Offset: 0x0000CA64
		~EfCompanyRepository()
		{
			this.Dispose(false);
		}

		// Token: 0x040001A6 RID: 422
		private readonly EfDbContext _context = new EfDbContext();

		// Token: 0x040001A7 RID: 423
		private bool _disposed;
	}
}
