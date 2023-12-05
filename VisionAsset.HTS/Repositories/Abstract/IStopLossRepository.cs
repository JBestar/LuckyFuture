using System;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.Repositories.Abstract
{
	// Token: 0x02000041 RID: 65
	public interface IStopLossRepository : IDisposable
	{
		// Token: 0x06000344 RID: 836
		IQueryable<StopLoss> Get();

		// Token: 0x06000345 RID: 837
		StopLoss GetById(long id);

		// Token: 0x06000346 RID: 838
		void Insert(StopLoss stopLoss);

		// Token: 0x06000347 RID: 839
		void Delete(long id);

		// Token: 0x06000348 RID: 840
		void Update(StopLoss stopLoss);

		// Token: 0x06000349 RID: 841
		void Save();
	}
}
