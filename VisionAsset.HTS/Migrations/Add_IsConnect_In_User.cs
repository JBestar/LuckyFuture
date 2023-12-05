using System;
using System.CodeDom.Compiler;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Builders;
using System.Data.Entity.Migrations.Infrastructure;
using System.Resources;

namespace Goodbyte.TradingSystem.Domain.Migrations
{
	// Token: 0x0200004E RID: 78
	[GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
	public sealed class Add_IsConnect_In_User : DbMigration, IMigrationMetadata
	{
		// Token: 0x060003C4 RID: 964 RVA: 0x000155EC File Offset: 0x000137EC
		public override void Up()
		{
			base.AddColumn("dbo.Users", "IsConnect", (ColumnBuilder c) => c.Boolean(new bool?(false), null, null, null, null, null), null);
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0001561E File Offset: 0x0001381E
		public override void Down()
		{
			base.DropColumn("dbo.Users", "IsConnect", null);
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x00015631 File Offset: 0x00013831
		string IMigrationMetadata.Id
		{
			get
			{
				return "201609010732444_Add_IsConnect_In_User";
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x000155BA File Offset: 0x000137BA
		string IMigrationMetadata.Source
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x00015638 File Offset: 0x00013838
		string IMigrationMetadata.Target
		{
			get
			{
				return this.Resources.GetString("Target");
			}
		}

		// Token: 0x040001D9 RID: 473
		private readonly ResourceManager Resources = new ResourceManager(typeof(Add_IsConnect_In_User));
	}
}
