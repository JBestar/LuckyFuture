using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyFuture.Models.ValueObjects
{
	public class ValuationInfo
	{
		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000DFF RID: 3583 RVA: 0x0004CDE1 File Offset: 0x0004AFE1
		// (set) Token: 0x06000E00 RID: 3584 RVA: 0x0004CDE9 File Offset: 0x0004AFE9
		public string Balance { get; set; }

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000E01 RID: 3585 RVA: 0x0004CDF2 File Offset: 0x0004AFF2
		// (set) Token: 0x06000E02 RID: 3586 RVA: 0x0004CDFA File Offset: 0x0004AFFA
		public double AverageUnitPrice { get; set; }

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000E03 RID: 3587 RVA: 0x0004CE03 File Offset: 0x0004B003
		// (set) Token: 0x06000E04 RID: 3588 RVA: 0x0004CE0B File Offset: 0x0004B00B
		public long Valuation { get; set; }

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000E05 RID: 3589 RVA: 0x0004CE14 File Offset: 0x0004B014
		// (set) Token: 0x06000E06 RID: 3590 RVA: 0x0004CE1C File Offset: 0x0004B01C
		public long TotalValuation { get; set; }

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000E07 RID: 3591 RVA: 0x0004CE25 File Offset: 0x0004B025
		// (set) Token: 0x06000E08 RID: 3592 RVA: 0x0004CE2D File Offset: 0x0004B02D
		public long TotalProfit { get; set; }

		public long CurrentProfit { get; set; }
		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000E09 RID: 3593 RVA: 0x0004CE36 File Offset: 0x0004B036
		// (set) Token: 0x06000E0A RID: 3594 RVA: 0x0004CE3E File Offset: 0x0004B03E
		public long LossCut { get; set; }
	}
}
