using System;
using System.Data.Entity;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Repositories.Concrete
{
	// Token: 0x02000031 RID: 49
	public class EfUserAccountRepository : IUserAccountRepository, IDisposable
	{
		// Token: 0x060002DC RID: 732 RVA: 0x0000F85F File Offset: 0x0000DA5F
		public IQueryable<UserAccount> Get()
		{
			return this._context.UserAccounts;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000F86C File Offset: 0x0000DA6C
		public UserAccount GetById(long id)
		{
			return this._context.UserAccounts.Find(new object[]
			{
				id
			});
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000F88D File Offset: 0x0000DA8D
		public void Insert(UserAccount userAccount)
		{
			this._context.UserAccounts.Add(userAccount);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000F8A4 File Offset: 0x0000DAA4
		public void Delete(long id)
		{
			UserAccount entity = this._context.UserAccounts.Find(new object[]
			{
				id
			});
			this._context.UserAccounts.Remove(entity);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000F8E3 File Offset: 0x0000DAE3
		public void Update(UserAccount userAccount)
		{
			this._context.Entry<UserAccount>(userAccount).State = EntityState.Modified;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000F8F8 File Offset: 0x0000DAF8
		public void Save()
		{
			this._context.SaveChanges();
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000F906 File Offset: 0x0000DB06
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000F915 File Offset: 0x0000DB15
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

		// Token: 0x060002E4 RID: 740 RVA: 0x0000F938 File Offset: 0x0000DB38
		~EfUserAccountRepository()
		{
			this.Dispose(false);
		}

		// Token: 0x040001D4 RID: 468
		private readonly EfDbContext _context = new EfDbContext();

		// Token: 0x040001D5 RID: 469
		private bool _disposed;
	}
}
