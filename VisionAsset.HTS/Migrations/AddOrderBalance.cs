using System;
using System.CodeDom.Compiler;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Builders;
using System.Data.Entity.Migrations.Infrastructure;
using System.Resources;

namespace Goodbyte.TradingSystem.Domain.Migrations
{
	// Token: 0x02000055 RID: 85
	[GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
	public sealed class AddOrderBalance : DbMigration, IMigrationMetadata
	{
		// Token: 0x060003EE RID: 1006 RVA: 0x00015DAF File Offset: 0x00013FAF
		public override void Up()
		{
			base.AddColumn("dbo.Orders", "Balance", (ColumnBuilder c) => c.Long(new bool?(false), false, null, null, null, null, null), null);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00015DE1 File Offset: 0x00013FE1
		public override void Down()
		{
			base.DropColumn("dbo.Orders", "Balance", null);
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x00015DF4 File Offset: 0x00013FF4
		string IMigrationMetadata.Id
		{
			get
			{
				return "201707030816060_AddOrderBalance";
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x000155BA File Offset: 0x000137BA
		string IMigrationMetadata.Source
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x00015DFB File Offset: 0x00013FFB
		string IMigrationMetadata.Target
		{
			get
			{
				return this.Resources.GetString("Target");
			}
		}

		// Token: 0x040001E0 RID: 480
		private readonly ResourceManager Resources = new ResourceManager(typeof(AddOrderBalance));
	}
}
