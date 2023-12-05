using System;
using System.Data.Entity;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Repositories.Concrete
{
	// Token: 0x02000027 RID: 39
	public class EfDepositWithdrawRepository : IDepositWithdrawRepository, IDisposable
	{
		// Token: 0x06000278 RID: 632 RVA: 0x0000ED48 File Offset: 0x0000CF48
		public IQueryable<DepositWithdraw> Get()
		{
			return this._context.DepositWithdraws;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000ED55 File Offset: 0x0000CF55
		public DepositWithdraw GetById(long id)
		{
			return this._context.DepositWithdraws.Find(new object[]
			{
				id
			});
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000ED76 File Offset: 0x0000CF76
		public void Insert(DepositWithdraw depositWithdraw)
		{
			this._context.DepositWithdraws.Add(depositWithdraw);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000ED8C File Offset: 0x0000CF8C
		public void Delete(long id)
		{
			DepositWithdraw entity = this._context.DepositWithdraws.Find(new object[]
			{
				id
			});
			this._context.DepositWithdraws.Remove(entity);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000EDCB File Offset: 0x0000CFCB
		public void Update(DepositWithdraw depositWithdraw)
		{
			this._context.Entry<DepositWithdraw>(depositWithdraw).State = EntityState.Modified;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000EDE0 File Offset: 0x0000CFE0
		public void Save()
		{
			this._context.SaveChanges();
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000EDEE File Offset: 0x0000CFEE
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000EDFD File Offset: 0x0000CFFD
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

		// Token: 0x06000280 RID: 640 RVA: 0x0000EE20 File Offset: 0x0000D020
		~EfDepositWithdrawRepository()
		{
			this.Dispose(false);
		}

		// Token: 0x040001C0 RID: 448
		private readonly EfDbContext _context = new EfDbContext();

		// Token: 0x040001C1 RID: 449
		private bool _disposed;
	}
}
