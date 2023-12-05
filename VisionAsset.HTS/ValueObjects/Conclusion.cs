using System;

namespace Goodbyte.TradingSystem.Domain.ValueObjects
{
	// Token: 0x02000017 RID: 23
	public class Conclusion
	{
		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060001CB RID: 459 RVA: 0x0000E307 File Offset: 0x0000C507
		// (set) Token: 0x060001CC RID: 460 RVA: 0x0000E30F File Offset: 0x0000C50F
		public string AccountNumber { get; set; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060001CD RID: 461 RVA: 0x0000E318 File Offset: 0x0000C518
		// (set) Token: 0x060001CE RID: 462 RVA: 0x0000E320 File Offset: 0x0000C520
		public ConclusionItemType ConclusionItemType { get; set; }

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060001CF RID: 463 RVA: 0x0000E329 File Offset: 0x0000C529
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x0000E331 File Offset: 0x0000C531
		public string Symbol { get; set; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000E33A File Offset: 0x0000C53A
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x0000E342 File Offset: 0x0000C542
		public long OrderNumber { get; set; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x0000E34B File Offset: 0x0000C54B
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x0000E353 File Offset: 0x0000C553
		public int Qty { get; set; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x0000E35C File Offset: 0x0000C55C
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x0000E364 File Offset: 0x0000C564
		public double Price { get; set; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000E36D File Offset: 0x0000C56D
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x0000E375 File Offset: 0x0000C575
		public DateTime ReceivedDate { get; set; }
	}
}
