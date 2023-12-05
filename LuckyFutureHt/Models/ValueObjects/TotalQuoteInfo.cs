using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyFuture.Models.ValueObjects
{
	public class TotalQuoteInfo
	{
		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000DD9 RID: 3545 RVA: 0x0004CCAF File Offset: 0x0004AEAF
		// (set) Token: 0x06000DDA RID: 3546 RVA: 0x0004CCB7 File Offset: 0x0004AEB7
		public int TotalSellMit { get; set; }

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000DDB RID: 3547 RVA: 0x0004CCC0 File Offset: 0x0004AEC0
		// (set) Token: 0x06000DDC RID: 3548 RVA: 0x0004CCC8 File Offset: 0x0004AEC8
		public int TotalSellOrder { get; set; }

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000DDD RID: 3549 RVA: 0x0004CCD1 File Offset: 0x0004AED1
		// (set) Token: 0x06000DDE RID: 3550 RVA: 0x0004CCD9 File Offset: 0x0004AED9
		public int TotalAskCount { get; set; }

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000DDF RID: 3551 RVA: 0x0004CCE2 File Offset: 0x0004AEE2
		// (set) Token: 0x06000DE0 RID: 3552 RVA: 0x0004CCEA File Offset: 0x0004AEEA
		public int TotalAskQty { get; set; }

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000DE1 RID: 3553 RVA: 0x0004CCF3 File Offset: 0x0004AEF3
		// (set) Token: 0x06000DE2 RID: 3554 RVA: 0x0004CCFB File Offset: 0x0004AEFB
		public int Difference { get; set; }

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000DE3 RID: 3555 RVA: 0x0004CD04 File Offset: 0x0004AF04
		// (set) Token: 0x06000DE4 RID: 3556 RVA: 0x0004CD0C File Offset: 0x0004AF0C
		public int TotalBidQty { get; set; }

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000DE5 RID: 3557 RVA: 0x0004CD15 File Offset: 0x0004AF15
		// (set) Token: 0x06000DE6 RID: 3558 RVA: 0x0004CD1D File Offset: 0x0004AF1D
		public int TotalBidCount { get; set; }

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000DE7 RID: 3559 RVA: 0x0004CD26 File Offset: 0x0004AF26
		// (set) Token: 0x06000DE8 RID: 3560 RVA: 0x0004CD2E File Offset: 0x0004AF2E
		public int TotalBuyOrder { get; set; }

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000DE9 RID: 3561 RVA: 0x0004CD37 File Offset: 0x0004AF37
		// (set) Token: 0x06000DEA RID: 3562 RVA: 0x0004CD3F File Offset: 0x0004AF3F
		public int TotalBuyMit { get; set; }
	}
}
