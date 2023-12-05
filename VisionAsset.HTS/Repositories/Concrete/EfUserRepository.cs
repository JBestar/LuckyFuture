using System;
using System.Data.Entity;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Repositories.Concrete
{
	// Token: 0x02000032 RID: 50
	public class EfUserRepository : IUserRepository, IDisposable
	{
		// Token: 0x060002E6 RID: 742 RVA: 0x0000F97B File Offset: 0x0000DB7B
		public IQueryable<User> Get()
		{
			return this._context.Users;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000F988 File Offset: 0x0000DB88
		public User GetById(long id)
		{
			return this._context.Users.Find(new object[]
			{
				id
			});
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000F9A9 File Offset: 0x0000DBA9
		public void Insert(User user)
		{
			this._context.Users.Add(user);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000F9C0 File Offset: 0x0000DBC0
		public void Delete(long id)
		{
			User entity = this._context.Users.Find(new object[]
			{
				id
			});
			this._context.Users.Remove(entity);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000F9FF File Offset: 0x0000DBFF
		public void Update(User user)
		{
			this._context.Entry<User>(user).State = EntityState.Modified;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000FA14 File Offset: 0x0000DC14
		public void Save()
		{
			this._context.SaveChanges();
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000FA22 File Offset: 0x0000DC22
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000FA31 File Offset: 0x0000DC31
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

		// Token: 0x060002EE RID: 750 RVA: 0x0000FA54 File Offset: 0x0000DC54
		~EfUserRepository()
		{
			this.Dispose(false);
		}

		// Token: 0x040001D6 RID: 470
		private readonly EfDbContext _context = new EfDbContext();

		// Token: 0x040001D7 RID: 471
		private bool _disposed;
	}
}
