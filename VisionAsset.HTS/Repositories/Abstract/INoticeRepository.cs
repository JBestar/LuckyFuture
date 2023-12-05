using System;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.Repositories.Abstract
{
	// Token: 0x0200003D RID: 61
	public interface INoticeRepository : IDisposable
	{
		// Token: 0x0600032C RID: 812
		IQueryable<Notice> Get();

		// Token: 0x0600032D RID: 813
		Notice GetById(long id);

		// Token: 0x0600032E RID: 814
		void Insert(Notice notice);

		// Token: 0x0600032F RID: 815
		void Delete(long id);

		// Token: 0x06000330 RID: 816
		void Update(Notice notice);

		// Token: 0x06000331 RID: 817
		void Save();
	}
}
