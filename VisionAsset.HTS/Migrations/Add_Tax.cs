using System;
using System.CodeDom.Compiler;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Builders;
using System.Data.Entity.Migrations.Infrastructure;
using System.Resources;

namespace Goodbyte.TradingSystem.Domain.Migrations
{
	// Token: 0x02000050 RID: 80
	[GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
	public sealed class Add_Tax : DbMigration, IMigrationMetadata
	{
		// Token: 0x060003D0 RID: 976 RVA: 0x000158C4 File Offset: 0x00013AC4
		public override void Up()
		{
			base.AddColumn("dbo.DayProfitLosses", "KospiTax", (ColumnBuilder c) => c.Long(new bool?(false), false, null, null, null, null, null), null);
			base.AddColumn("dbo.DayProfitLosses", "KosdaqTax", (ColumnBuilder c) => c.Long(new bool?(false), false, null, null, null, null, null), null);
			base.AddColumn("dbo.DayProfitLosses", "KospiVirtualTax", (ColumnBuilder c) => c.Long(new bool?(false), false, null, null, null, null, null), null);
			base.AddColumn("dbo.DayProfitLosses", "KosdaqVirtualTax", (ColumnBuilder c) => c.Long(new bool?(false), false, null, null, null, null, null), null);
			base.AddColumn("dbo.DayProfitLosses", "TotalTax", (ColumnBuilder c) => c.Long(new bool?(false), false, null, null, null, null, null), null);
			base.AddColumn("dbo.DayProfitLosses", "TotalVirtualTax", (ColumnBuilder c) => c.Long(new bool?(false), false, null, null, null, null, null), null);
			base.AddColumn("dbo.Items", "Tax", (ColumnBuilder c) => c.Double(new bool?(false), null, null, null, null, null), null);
			base.AddColumn("dbo.Orders", "Tax", (ColumnBuilder c) => c.Long(new bool?(false), false, null, null, null, null, null), null);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00015A54 File Offset: 0x00013C54
		public override void Down()
		{
			base.DropColumn("dbo.Orders", "Tax", null);
			base.DropColumn("dbo.Items", "Tax", null);
			base.DropColumn("dbo.DayProfitLosses", "TotalVirtualTax", null);
			base.DropColumn("dbo.DayProfitLosses", "TotalTax", null);
			base.DropColumn("dbo.DayProfitLosses", "KosdaqVirtualTax", null);
			base.DropColumn("dbo.DayProfitLosses", "KospiVirtualTax", null);
			base.DropColumn("dbo.DayProfitLosses", "KosdaqTax", null);
			base.DropColumn("dbo.DayProfitLosses", "KospiTax", null);
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x00015AE9 File Offset: 0x00013CE9
		string IMigrationMetadata.Id
		{
			get
			{
				return "201611190935039_Add_Tax";
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x000155BA File Offset: 0x000137BA
		string IMigrationMetadata.Source
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x00015AF0 File Offset: 0x00013CF0
		string IMigrationMetadata.Target
		{
			get
			{
				return this.Resources.GetString("Target");
			}
		}

		// Token: 0x040001DB RID: 475
		private readonly ResourceManager Resources = new ResourceManager(typeof(Add_Tax));
	}
}
