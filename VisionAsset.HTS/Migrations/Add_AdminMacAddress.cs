using System;
using System.CodeDom.Compiler;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Builders;
using System.Data.Entity.Migrations.Infrastructure;
using System.Resources;

namespace Goodbyte.TradingSystem.Domain.Migrations
{
	// Token: 0x02000052 RID: 82
	[GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
	public sealed class Add_AdminMacAddress : DbMigration, IMigrationMetadata
	{
		// Token: 0x060003DC RID: 988 RVA: 0x00015C3E File Offset: 0x00013E3E
		public override void Up()
		{
			base.AddColumn("dbo.Admins", "MacAddress", (ColumnBuilder c) => c.String(null, null, null, null, null, null, null, null, null), null);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00015C70 File Offset: 0x00013E70
		public override void Down()
		{
			base.DropColumn("dbo.Admins", "MacAddress", null);
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060003DE RID: 990 RVA: 0x00015C83 File Offset: 0x00013E83
		string IMigrationMetadata.Id
		{
			get
			{
				return "201706081357179_Add_AdminMacAddress";
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060003DF RID: 991 RVA: 0x000155BA File Offset: 0x000137BA
		string IMigrationMetadata.Source
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x00015C8A File Offset: 0x00013E8A
		string IMigrationMetadata.Target
		{
			get
			{
				return this.Resources.GetString("Target");
			}
		}

		// Token: 0x040001DD RID: 477
		private readonly ResourceManager Resources = new ResourceManager(typeof(Add_AdminMacAddress));
	}
}
