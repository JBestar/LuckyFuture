using System;
using System.CodeDom.Compiler;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Builders;
using System.Data.Entity.Migrations.Infrastructure;
using System.Resources;

namespace Goodbyte.TradingSystem.Domain.Migrations
{
	// Token: 0x0200004F RID: 79
	[GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
	public sealed class VirtualParentCommission : DbMigration, IMigrationMetadata
	{
		// Token: 0x060003CA RID: 970 RVA: 0x00015668 File Offset: 0x00013868
		public override void Up()
		{
			base.AddColumn("dbo.DayProfitLosses", "FuturesVirtualParentCommission", (ColumnBuilder c) => c.Long(new bool?(false), false, null, null, null, null, null), null);
			base.AddColumn("dbo.DayProfitLosses", "OptionVirtualParentCommission", (ColumnBuilder c) => c.Long(new bool?(false), false, null, null, null, null, null), null);
			base.AddColumn("dbo.DayProfitLosses", "CmeVirtualParentCommission", (ColumnBuilder c) => c.Long(new bool?(false), false, null, null, null, null, null), null);
			base.AddColumn("dbo.DayProfitLosses", "EurexVirtualParentCommission", (ColumnBuilder c) => c.Long(new bool?(false), false, null, null, null, null, null), null);
			base.AddColumn("dbo.DayProfitLosses", "ForeignVirtualParentCommission", (ColumnBuilder c) => c.Long(new bool?(false), false, null, null, null, null, null), null);
			base.AddColumn("dbo.DayProfitLosses", "KospiVirtualParentCommission", (ColumnBuilder c) => c.Long(new bool?(false), false, null, null, null, null, null), null);
			base.AddColumn("dbo.DayProfitLosses", "KosdaqVirtualParentCommission", (ColumnBuilder c) => c.Long(new bool?(false), false, null, null, null, null, null), null);
			base.AddColumn("dbo.DayProfitLosses", "TotalVirtualParentCommission", (ColumnBuilder c) => c.Long(new bool?(false), false, null, null, null, null, null), null);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x000157F8 File Offset: 0x000139F8
		public override void Down()
		{
			base.DropColumn("dbo.DayProfitLosses", "TotalVirtualParentCommission", null);
			base.DropColumn("dbo.DayProfitLosses", "KosdaqVirtualParentCommission", null);
			base.DropColumn("dbo.DayProfitLosses", "KospiVirtualParentCommission", null);
			base.DropColumn("dbo.DayProfitLosses", "ForeignVirtualParentCommission", null);
			base.DropColumn("dbo.DayProfitLosses", "EurexVirtualParentCommission", null);
			base.DropColumn("dbo.DayProfitLosses", "CmeVirtualParentCommission", null);
			base.DropColumn("dbo.DayProfitLosses", "OptionVirtualParentCommission", null);
			base.DropColumn("dbo.DayProfitLosses", "FuturesVirtualParentCommission", null);
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060003CC RID: 972 RVA: 0x0001588D File Offset: 0x00013A8D
		string IMigrationMetadata.Id
		{
			get
			{
				return "201609052217109_VirtualParentCommission";
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060003CD RID: 973 RVA: 0x000155BA File Offset: 0x000137BA
		string IMigrationMetadata.Source
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060003CE RID: 974 RVA: 0x00015894 File Offset: 0x00013A94
		string IMigrationMetadata.Target
		{
			get
			{
				return this.Resources.GetString("Target");
			}
		}

		// Token: 0x040001DA RID: 474
		private readonly ResourceManager Resources = new ResourceManager(typeof(VirtualParentCommission));
	}
}
