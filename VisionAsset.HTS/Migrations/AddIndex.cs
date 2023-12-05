using System;
using System.CodeDom.Compiler;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Builders;
using System.Data.Entity.Migrations.Infrastructure;
using System.Resources;

namespace Goodbyte.TradingSystem.Domain.Migrations
{
	// Token: 0x02000051 RID: 81
	[GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
	public sealed class AddIndex : DbMigration, IMigrationMetadata
	{
		// Token: 0x060003D6 RID: 982 RVA: 0x00015B20 File Offset: 0x00013D20
		public override void Up()
		{
			base.AddColumn("dbo.Users", "LatestLoginDate", (ColumnBuilder c) => c.DateTime(new bool?(false), null, null, null, null, null, null), null);
			base.CreateIndex("dbo.DayProfitLosses", "MarketDate", false, null, false, null);
			base.CreateIndex("dbo.DepositWithdraws", "MarketDate", false, null, false, null);
			base.CreateIndex("dbo.Orders", "IsNotCompleted", false, null, false, null);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00015B9C File Offset: 0x00013D9C
		public override void Down()
		{
			base.DropIndex("dbo.Orders", new string[]
			{
				"IsNotCompleted"
			}, null);
			base.DropIndex("dbo.DepositWithdraws", new string[]
			{
				"MarketDate"
			}, null);
			base.DropIndex("dbo.DayProfitLosses", new string[]
			{
				"MarketDate"
			}, null);
			base.DropColumn("dbo.Users", "LatestLoginDate", null);
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x00015C08 File Offset: 0x00013E08
		string IMigrationMetadata.Id
		{
			get
			{
				return "201704010605147_AddIndex";
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x000155BA File Offset: 0x000137BA
		string IMigrationMetadata.Source
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060003DA RID: 986 RVA: 0x00015C0F File Offset: 0x00013E0F
		string IMigrationMetadata.Target
		{
			get
			{
				return this.Resources.GetString("Target");
			}
		}

		// Token: 0x040001DC RID: 476
		private readonly ResourceManager Resources = new ResourceManager(typeof(AddIndex));
	}
}
