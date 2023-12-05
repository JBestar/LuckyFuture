using System;
using System.Data.Entity;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Repositories.Concrete
{
	// Token: 0x0200002D RID: 45
	public class EfOvernightRepository : IOvernightRepository, IDisposable
	{
		// Token: 0x060002B4 RID: 692 RVA: 0x0000F3EF File Offset: 0x0000D5EF
		public IQueryable<Overnight> Get()
		{
			return this._context.Overnights;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000F3FC File Offset: 0x0000D5FC
		public Overnight GetById(long id)
		{
			return this._context.Overnights.Find(new object[]
			{
				id
			});
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000F41D File Offset: 0x0000D61D
		public void Insert(Overnight overnight)
		{
			this._context.Overnights.Add(overnight);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000F434 File Offset: 0x0000D634
		public void Delete(long id)
		{
			Overnight entity = this._context.Overnights.Find(new object[]
			{
				id
			});
			this._context.Overnights.Remove(entity);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000F473 File Offset: 0x0000D673
		public void Update(Overnight overnight)
		{
			this._context.Entry<Overnight>(overnight).State = EntityState.Modified;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000F488 File Offset: 0x0000D688
		public void Save()
		{
			this._context.SaveChanges();
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000F496 File Offset: 0x0000D696
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000F4A5 File Offset: 0x0000D6A5
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

		// Token: 0x060002BC RID: 700 RVA: 0x0000F4C8 File Offset: 0x0000D6C8
		~EfOvernightRepository()
		{
			this.Dispose(false);
		}

		// Token: 0x040001CC RID: 460
		private readonly EfDbContext _context = new EfDbContext();

		// Token: 0x040001CD RID: 461
		private bool _disposed;
	}
}
