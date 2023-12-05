using System;
using System.Data.Entity;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Repositories.Concrete
{
	// Token: 0x02000030 RID: 48
	public class EfUserAccountSpecificRepository : IUserAccountSpecificRepository, IDisposable
	{
		// Token: 0x060002D2 RID: 722 RVA: 0x0000F743 File Offset: 0x0000D943
		public IQueryable<UserAccountSpecific> Get()
		{
			return this._context.UserAccountSpecifics;
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000F750 File Offset: 0x0000D950
		public UserAccountSpecific GetById(long id)
		{
			return this._context.UserAccountSpecifics.Find(new object[]
			{
				id
			});
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000F771 File Offset: 0x0000D971
		public void Insert(UserAccountSpecific userAccountSpecifics)
		{
			this._context.UserAccountSpecifics.Add(userAccountSpecifics);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000F788 File Offset: 0x0000D988
		public void Delete(long id)
		{
			UserAccountSpecific entity = this._context.UserAccountSpecifics.Find(new object[]
			{
				id
			});
			this._context.UserAccountSpecifics.Remove(entity);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000F7C7 File Offset: 0x0000D9C7
		public void Update(UserAccountSpecific userAccountSpecific)
		{
			this._context.Entry<UserAccountSpecific>(userAccountSpecific).State = EntityState.Modified;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000F7DC File Offset: 0x0000D9DC
		public void Save()
		{
			this._context.SaveChanges();
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000F7EA File Offset: 0x0000D9EA
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000F7F9 File Offset: 0x0000D9F9
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

		// Token: 0x060002DA RID: 730 RVA: 0x0000F81C File Offset: 0x0000DA1C
		~EfUserAccountSpecificRepository()
		{
			this.Dispose(false);
		}

		// Token: 0x040001D2 RID: 466
		private readonly EfDbContext _context = new EfDbContext();

		// Token: 0x040001D3 RID: 467
		private bool _disposed;
	}
}
