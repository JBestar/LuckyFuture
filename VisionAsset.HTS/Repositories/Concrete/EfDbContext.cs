using System;
using System.Data.Entity;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.Repositories.Concrete
{
	// Token: 0x02000026 RID: 38
	public class EfDbContext : DbContext
	{
		// Token: 0x06000252 RID: 594 RVA: 0x0000EBFB File Offset: 0x0000CDFB
		public EfDbContext() : base("name=DefaultConnection")
		{
			base.Configuration.ProxyCreationEnabled = false;
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0000EC14 File Offset: 0x0000CE14
		// (set) Token: 0x06000254 RID: 596 RVA: 0x0000EC1C File Offset: 0x0000CE1C
		public DbSet<Admin> Admins { get; set; }

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0000EC25 File Offset: 0x0000CE25
		// (set) Token: 0x06000256 RID: 598 RVA: 0x0000EC2D File Offset: 0x0000CE2D
		public DbSet<Company> Companies { get; set; }

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000EC36 File Offset: 0x0000CE36
		// (set) Token: 0x06000258 RID: 600 RVA: 0x0000EC3E File Offset: 0x0000CE3E
		public DbSet<CompanyAccount> CompanyAccounts { get; set; }

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000EC47 File Offset: 0x0000CE47
		// (set) Token: 0x0600025A RID: 602 RVA: 0x0000EC4F File Offset: 0x0000CE4F
		public DbSet<Connection> Connections { get; set; }

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x0600025B RID: 603 RVA: 0x0000EC58 File Offset: 0x0000CE58
		// (set) Token: 0x0600025C RID: 604 RVA: 0x0000EC60 File Offset: 0x0000CE60
		public DbSet<Currency> Currencies { get; set; }

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x0600025D RID: 605 RVA: 0x0000EC69 File Offset: 0x0000CE69
		// (set) Token: 0x0600025E RID: 606 RVA: 0x0000EC71 File Offset: 0x0000CE71
		public DbSet<DayProfitLoss> DayProfitLosses { get; set; }

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000EC7A File Offset: 0x0000CE7A
		// (set) Token: 0x06000260 RID: 608 RVA: 0x0000EC82 File Offset: 0x0000CE82
		public DbSet<DepositWithdraw> DepositWithdraws { get; set; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000261 RID: 609 RVA: 0x0000EC8B File Offset: 0x0000CE8B
		// (set) Token: 0x06000262 RID: 610 RVA: 0x0000EC93 File Offset: 0x0000CE93
		public DbSet<Item> Items { get; set; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000263 RID: 611 RVA: 0x0000EC9C File Offset: 0x0000CE9C
		// (set) Token: 0x06000264 RID: 612 RVA: 0x0000ECA4 File Offset: 0x0000CEA4
		public DbSet<Market> Markets { get; set; }

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000265 RID: 613 RVA: 0x0000ECAD File Offset: 0x0000CEAD
		// (set) Token: 0x06000266 RID: 614 RVA: 0x0000ECB5 File Offset: 0x0000CEB5
		public DbSet<MitOrder> MitOrders { get; set; }

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0000ECBE File Offset: 0x0000CEBE
		// (set) Token: 0x06000268 RID: 616 RVA: 0x0000ECC6 File Offset: 0x0000CEC6
		public DbSet<Notice> Notices { get; set; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000269 RID: 617 RVA: 0x0000ECCF File Offset: 0x0000CECF
		// (set) Token: 0x0600026A RID: 618 RVA: 0x0000ECD7 File Offset: 0x0000CED7
		public DbSet<Order> Orders { get; set; }

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0000ECE0 File Offset: 0x0000CEE0
		// (set) Token: 0x0600026C RID: 620 RVA: 0x0000ECE8 File Offset: 0x0000CEE8
		public DbSet<Overnight> Overnights { get; set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0000ECF1 File Offset: 0x0000CEF1
		// (set) Token: 0x0600026E RID: 622 RVA: 0x0000ECF9 File Offset: 0x0000CEF9
		public DbSet<ParentAccount> StockAccounts { get; set; }

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0000ED02 File Offset: 0x0000CF02
		// (set) Token: 0x06000270 RID: 624 RVA: 0x0000ED0A File Offset: 0x0000CF0A
		public DbSet<StopLoss> StopLosses { get; set; }

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000ED13 File Offset: 0x0000CF13
		// (set) Token: 0x06000272 RID: 626 RVA: 0x0000ED1B File Offset: 0x0000CF1B
		public DbSet<User> Users { get; set; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000ED24 File Offset: 0x0000CF24
		// (set) Token: 0x06000274 RID: 628 RVA: 0x0000ED2C File Offset: 0x0000CF2C
		public DbSet<UserAccount> UserAccounts { get; set; }

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000ED35 File Offset: 0x0000CF35
		// (set) Token: 0x06000276 RID: 630 RVA: 0x0000ED3D File Offset: 0x0000CF3D
		public DbSet<UserAccountSpecific> UserAccountSpecifics { get; set; }

		// Token: 0x06000277 RID: 631 RVA: 0x0000ED46 File Offset: 0x0000CF46
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
		}
	}
}
