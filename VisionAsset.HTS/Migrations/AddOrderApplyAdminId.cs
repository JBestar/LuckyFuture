using System;
using System.CodeDom.Compiler;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Builders;
using System.Data.Entity.Migrations.Infrastructure;
using System.Resources;

namespace Goodbyte.TradingSystem.Domain.Migrations
{
	// Token: 0x02000054 RID: 84
	[GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
	public sealed class AddOrderApplyAdminId : DbMigration, IMigrationMetadata
	{
		// Token: 0x060003E8 RID: 1000 RVA: 0x00015D34 File Offset: 0x00013F34
		public override void Up()
		{
			base.AddColumn("dbo.Orders", "ApplyAdminId", (ColumnBuilder c) => c.String(null, null, null, null, null, null, null, null, null), null);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00015D66 File Offset: 0x00013F66
		public override void Down()
		{
			base.DropColumn("dbo.Orders", "ApplyAdminId", null);
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x00015D79 File Offset: 0x00013F79
		string IMigrationMetadata.Id
		{
			get
			{
				return "201706260647507_AddOrderApplyAdminId";
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x000155BA File Offset: 0x000137BA
		string IMigrationMetadata.Source
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x00015D80 File Offset: 0x00013F80
		string IMigrationMetadata.Target
		{
			get
			{
				return this.Resources.GetString("Target");
			}
		}

		// Token: 0x040001DF RID: 479
		private readonly ResourceManager Resources = new ResourceManager(typeof(AddOrderApplyAdminId));
	}
}
