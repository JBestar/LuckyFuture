using System;
using System.Data.Entity;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Repositories.Concrete
{
	// Token: 0x02000020 RID: 32
	public class EfAdminRepository : IAdminRepository, IDisposable
	{
		// Token: 0x06000216 RID: 534 RVA: 0x0000E560 File Offset: 0x0000C760
		public IQueryable<Admin> Get()
		{
			return this._context.Admins;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000E56D File Offset: 0x0000C76D
		public Admin GetById(string id)
		{
			return this._context.Admins.Find(new object[]
			{
				id
			});
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000E589 File Offset: 0x0000C789
		public void Insert(Admin admin)
		{
			this._context.Admins.Add(admin);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000E5A0 File Offset: 0x0000C7A0
		public void Delete(string id)
		{
			Admin entity = this._context.Admins.Find(new object[]
			{
				id
			});
			this._context.Admins.Remove(entity);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000E5DA File Offset: 0x0000C7DA
		public void Update(Admin admin)
		{
			this._context.Entry<Admin>(admin).State = EntityState.Modified;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000E5EF File Offset: 0x0000C7EF
		public void Save()
		{
			this._context.SaveChanges();
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000E5FD File Offset: 0x0000C7FD
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000E60C File Offset: 0x0000C80C
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

		// Token: 0x0600021E RID: 542 RVA: 0x0000E62C File Offset: 0x0000C82C
		~EfAdminRepository()
		{
			this.Dispose(false);
		}

		// Token: 0x040001A2 RID: 418
		private readonly EfDbContext _context = new EfDbContext();

		// Token: 0x040001A3 RID: 419
		private bool _disposed;
	}
}
