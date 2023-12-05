using System;
using System.CodeDom.Compiler;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Builders;
using System.Data.Entity.Migrations.Infrastructure;
using System.Resources;

namespace Goodbyte.TradingSystem.Domain.Migrations
{
	// Token: 0x02000056 RID: 86
	[GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
	public sealed class AddAveragePriceAndPhoneNumber : DbMigration, IMigrationMetadata
	{
		// Token: 0x060003F4 RID: 1012 RVA: 0x00015E2C File Offset: 0x0001402C
		public override void Up()
		{
			base.AddColumn("dbo.CompanyAccounts", "Phone", (ColumnBuilder c) => c.String(null, null, null, null, null, null, null, null, null), null);
			base.AddColumn("dbo.Orders", "AveragePrice", (ColumnBuilder c) => c.Double(new bool?(false), null, null, null, null, null), null);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00015E99 File Offset: 0x00014099
		public override void Down()
		{
			base.DropColumn("dbo.Orders", "AveragePrice", null);
			base.DropColumn("dbo.CompanyAccounts", "Phone", null);
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x00015EBD File Offset: 0x000140BD
		string IMigrationMetadata.Id
		{
			get
			{
				return "201707260249496_AddAveragePriceAndPhoneNumber";
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x000155BA File Offset: 0x000137BA
		string IMigrationMetadata.Source
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x00015EC4 File Offset: 0x000140C4
		string IMigrationMetadata.Target
		{
			get
			{
				return this.Resources.GetString("Target");
			}
		}

		// Token: 0x040001E1 RID: 481
		private readonly ResourceManager Resources = new ResourceManager(typeof(AddAveragePriceAndPhoneNumber));
	}
}
