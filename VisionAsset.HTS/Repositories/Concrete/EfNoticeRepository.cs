using System;
using System.Data.Entity;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Repositories.Concrete
{
	// Token: 0x0200002B RID: 43
	public class EfNoticeRepository : INoticeRepository, IDisposable
	{
		// Token: 0x060002A0 RID: 672 RVA: 0x0000F1B7 File Offset: 0x0000D3B7
		public IQueryable<Notice> Get()
		{
			return this._context.Notices;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000F1C4 File Offset: 0x0000D3C4
		public Notice GetById(long id)
		{
			return this._context.Notices.Find(new object[]
			{
				id
			});
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000F1E5 File Offset: 0x0000D3E5
		public void Insert(Notice notice)
		{
			this._context.Notices.Add(notice);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000F1FC File Offset: 0x0000D3FC
		public void Delete(long id)
		{
			Notice entity = this._context.Notices.Find(new object[]
			{
				id
			});
			this._context.Notices.Remove(entity);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000F23B File Offset: 0x0000D43B
		public void Update(Notice notice)
		{
			this._context.Entry<Notice>(notice).State = EntityState.Modified;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000F250 File Offset: 0x0000D450
		public void Save()
		{
			this._context.SaveChanges();
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000F25E File Offset: 0x0000D45E
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000F26D File Offset: 0x0000D46D
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

		// Token: 0x060002A8 RID: 680 RVA: 0x0000F290 File Offset: 0x0000D490
		~EfNoticeRepository()
		{
			this.Dispose(false);
		}

		// Token: 0x040001C8 RID: 456
		private readonly EfDbContext _context = new EfDbContext();

		// Token: 0x040001C9 RID: 457
		private bool _disposed;
	}
}
