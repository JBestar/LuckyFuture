using System;
using System.CodeDom.Compiler;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Builders;
using System.Data.Entity.Migrations.Infrastructure;
using System.Resources;

namespace Goodbyte.TradingSystem.Domain.Migrations
{
	// Token: 0x02000058 RID: 88
	[GeneratedCode("EntityFramework.Migrations", "6.2.0-61023")]
	public sealed class AddUserAccountSpecificMaxSellBuyQty : DbMigration, IMigrationMetadata
	{
		// Token: 0x06000400 RID: 1024 RVA: 0x00016094 File Offset: 0x00014294
		public override void Up()
		{
			base.AddColumn("dbo.UserAccountSpecifics", "ItemMaxSellQty", (ColumnBuilder c) => c.Int(new bool?(false), false, null, null, null, null, null), null);
			base.AddColumn("dbo.UserAccountSpecifics", "ItemMaxBuyQty", (ColumnBuilder c) => c.Int(new bool?(false), false, null, null, null, null, null), null);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00016101 File Offset: 0x00014301
		public override void Down()
		{
			base.DropColumn("dbo.UserAccountSpecifics", "ItemMaxBuyQty", null);
			base.DropColumn("dbo.UserAccountSpecifics", "ItemMaxSellQty", null);
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x00016125 File Offset: 0x00014325
		string IMigrationMetadata.Id
		{
			get
			{
				return "201807241218055_AddUserAccountSpecificMaxSellBuyQty";
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x000155BA File Offset: 0x000137BA
		string IMigrationMetadata.Source
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x0001612C File Offset: 0x0001432C
		string IMigrationMetadata.Target
		{
			get
			{
				return this.Resources.GetString("Target");
			}
		}

		// Token: 0x040001E3 RID: 483
		private readonly ResourceManager Resources = new ResourceManager(typeof(AddUserAccountSpecificMaxSellBuyQty));
	}
}
