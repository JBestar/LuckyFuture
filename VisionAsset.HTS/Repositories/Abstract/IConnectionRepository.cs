using System;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.Repositories.Abstract
{
	// Token: 0x02000036 RID: 54
	public interface IConnectionRepository : IDisposable
	{
		// Token: 0x06000302 RID: 770
		IQueryable<Connection> Get();

		// Token: 0x06000303 RID: 771
		Connection GetById(long id);

		// Token: 0x06000304 RID: 772
		void Insert(Connection connection);

		// Token: 0x06000305 RID: 773
		void Delete(long id);

		// Token: 0x06000306 RID: 774
		void Update(Connection connection);

		// Token: 0x06000307 RID: 775
		void Save();
	}
}
