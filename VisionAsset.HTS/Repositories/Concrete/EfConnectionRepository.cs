using System;
using System.Data.Entity;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Repositories.Concrete
{
	// Token: 0x02000023 RID: 35
	public class EfConnectionRepository : IConnectionRepository, IDisposable
	{
		// Token: 0x06000234 RID: 564 RVA: 0x0000E8A7 File Offset: 0x0000CAA7
		public IQueryable<Connection> Get()
		{
			return this._context.Connections;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000E8B4 File Offset: 0x0000CAB4
		public Connection GetById(long id)
		{
			return this._context.Connections.Find(new object[]
			{
				id
			});
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000E8D5 File Offset: 0x0000CAD5
		public void Insert(Connection connection)
		{
			this._context.Connections.Add(connection);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000E8EC File Offset: 0x0000CAEC
		public void Delete(long id)
		{
			Connection entity = this._context.Connections.Find(new object[]
			{
				id
			});
			this._context.Connections.Remove(entity);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000E92B File Offset: 0x0000CB2B
		public void Update(Connection connection)
		{
			this._context.Entry<Connection>(connection).State = EntityState.Modified;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000E940 File Offset: 0x0000CB40
		public void Save()
		{
			this._context.SaveChanges();
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000E94E File Offset: 0x0000CB4E
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000E95D File Offset: 0x0000CB5D
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

		// Token: 0x0600023C RID: 572 RVA: 0x0000E980 File Offset: 0x0000CB80
		~EfConnectionRepository()
		{
			this.Dispose(false);
		}

		// Token: 0x040001A8 RID: 424
		private readonly EfDbContext _context = new EfDbContext();

		// Token: 0x040001A9 RID: 425
		private bool _disposed;
	}
}
