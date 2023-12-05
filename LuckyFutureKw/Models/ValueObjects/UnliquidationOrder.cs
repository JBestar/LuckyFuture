using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyFuture.Models.ValueObjects
{
	public class UnliquidationOrder
	{
		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000DEC RID: 3564 RVA: 0x0004CD48 File Offset: 0x0004AF48
		// (set) Token: 0x06000DED RID: 3565 RVA: 0x0004CD50 File Offset: 0x0004AF50
		public long ItemId { get; set; }

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000DEE RID: 3566 RVA: 0x0004CD59 File Offset: 0x0004AF59
		// (set) Token: 0x06000DEF RID: 3567 RVA: 0x0004CD61 File Offset: 0x0004AF61
		public long MarketId { get; set; }

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000DF0 RID: 3568 RVA: 0x0004CD6A File Offset: 0x0004AF6A
		// (set) Token: 0x06000DF1 RID: 3569 RVA: 0x0004CD72 File Offset: 0x0004AF72
		public string ItemType { get; set; }

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000DF2 RID: 3570 RVA: 0x0004CD7B File Offset: 0x0004AF7B
		// (set) Token: 0x06000DF3 RID: 3571 RVA: 0x0004CD83 File Offset: 0x0004AF83
		public string TradeType { get; set; }

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000DF4 RID: 3572 RVA: 0x0004CD8C File Offset: 0x0004AF8C
		// (set) Token: 0x06000DF5 RID: 3573 RVA: 0x0004CD94 File Offset: 0x0004AF94
		public double Price { get; set; }

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000DF6 RID: 3574 RVA: 0x0004CD9D File Offset: 0x0004AF9D
		// (set) Token: 0x06000DF7 RID: 3575 RVA: 0x0004CDA5 File Offset: 0x0004AFA5
		public double AveragePrice { get; set; }

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000DF8 RID: 3576 RVA: 0x0004CDAE File Offset: 0x0004AFAE
		// (set) Token: 0x06000DF9 RID: 3577 RVA: 0x0004CDB6 File Offset: 0x0004AFB6
		public int Qty { get; set; }

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000DFA RID: 3578 RVA: 0x0004CDBF File Offset: 0x0004AFBF
		// (set) Token: 0x06000DFB RID: 3579 RVA: 0x0004CDC7 File Offset: 0x0004AFC7
		public long Valuation { get; set; }

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000DFC RID: 3580 RVA: 0x0004CDD0 File Offset: 0x0004AFD0
		// (set) Token: 0x06000DFD RID: 3581 RVA: 0x0004CDD8 File Offset: 0x0004AFD8
		public long LossCut { get; set; }
	}
}
