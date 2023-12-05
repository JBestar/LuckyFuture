using System;
using System.CodeDom.Compiler;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Builders;
using System.Data.Entity.Migrations.Infrastructure;
using System.Resources;

namespace Goodbyte.TradingSystem.Domain.Migrations
{
	// Token: 0x02000053 RID: 83
	[GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
	public sealed class AddDepositWithdrawApplyId : DbMigration, IMigrationMetadata
	{
		// Token: 0x060003E2 RID: 994 RVA: 0x00015CB9 File Offset: 0x00013EB9
		public override void Up()
		{
			base.AddColumn("dbo.DepositWithdraws", "ApplyAdminId", (ColumnBuilder c) => c.String(null, null, null, null, null, null, null, null, null), null);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00015CEB File Offset: 0x00013EEB
		public override void Down()
		{
			base.DropColumn("dbo.DepositWithdraws", "ApplyAdminId", null);
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x00015CFE File Offset: 0x00013EFE
		string IMigrationMetadata.Id
		{
			get
			{
				return "201706150153180_AddDepositWithdrawApplyId";
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x000155BA File Offset: 0x000137BA
		string IMigrationMetadata.Source
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x00015D05 File Offset: 0x00013F05
		string IMigrationMetadata.Target
		{
			get
			{
				return this.Resources.GetString("Target");
			}
		}

		// Token: 0x040001DE RID: 478
		private readonly ResourceManager Resources = new ResourceManager(typeof(AddDepositWithdrawApplyId));
	}
}
