using System;
using System.Data.Entity.Migrations;
using Goodbyte.TradingSystem.Domain.Repositories.Concrete;

namespace Goodbyte.TradingSystem.Domain.Migrations
{
	// Token: 0x02000059 RID: 89
	internal sealed class Configuration : DbMigrationsConfiguration<EfDbContext>
	{
		// Token: 0x06000406 RID: 1030 RVA: 0x0001615B File Offset: 0x0001435B
		public Configuration()
		{
			base.AutomaticMigrationsEnabled = false;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000ED46 File Offset: 0x0000CF46
		protected override void Seed(EfDbContext context)
		{
		}
	}
}
