using System;
using System.CodeDom.Compiler;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Builders;
using System.Data.Entity.Migrations.Infrastructure;
using System.Resources;

namespace Goodbyte.TradingSystem.Domain.Migrations
{
	// Token: 0x02000057 RID: 87
	public sealed class ModifyCertification : DbMigration, IMigrationMetadata
	{
		// Token: 0x060003FA RID: 1018 RVA: 0x00015EF4 File Offset: 0x000140F4
		public override void Up()
		{
			base.DropForeignKey("dbo.Certifications", "CompanyId", "dbo.Companies");
			base.DropIndex("dbo.Certifications", new string[]
			{
				"CompanyId"
			}, null);
			base.RenameColumn("dbo.Certifications", "CompanyId", "Company_CompanyId", null);
			base.AlterColumn("dbo.Certifications", "Company_CompanyId", (ColumnBuilder c) => c.Long(null, false, null, null, null, null, null), null);
			base.CreateIndex("dbo.Certifications", "Company_CompanyId", false, null, false, null);
			base.AddForeignKey("dbo.Certifications", "Company_CompanyId", "dbo.Companies", "CompanyId", false, null, null);
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00015FA8 File Offset: 0x000141A8
		public override void Down()
		{
			base.DropForeignKey("dbo.Certifications", "Company_CompanyId", "dbo.Companies");
			base.DropIndex("dbo.Certifications", new string[]
			{
				"Company_CompanyId"
			}, null);
			base.AlterColumn("dbo.Certifications", "Company_CompanyId", (ColumnBuilder c) => c.Long(new bool?(false), false, null, null, null, null, null), null);
			base.RenameColumn("dbo.Certifications", "Company_CompanyId", "CompanyId", null);
			base.CreateIndex("dbo.Certifications", "CompanyId", false, null, false, null);
			base.AddForeignKey("dbo.Certifications", "CompanyId", "dbo.Companies", "CompanyId", true, null, null);
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x0001605C File Offset: 0x0001425C
		string IMigrationMetadata.Id
		{
			get
			{
				return "201712060203081_ModifyCertification";
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x000155BA File Offset: 0x000137BA
		string IMigrationMetadata.Source
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x00016063 File Offset: 0x00014263
		string IMigrationMetadata.Target
		{
			get
			{
				return this.Resources.GetString("Target");
			}
		}

		// Token: 0x040001E2 RID: 482
		private readonly ResourceManager Resources = new ResourceManager(typeof(ModifyCertification));
	}
}
