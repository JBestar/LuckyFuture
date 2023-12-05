using System;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x02000070 RID: 112
	[DataContract]
	public class Notice
	{
		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x0001738B File Offset: 0x0001558B
		// (set) Token: 0x06000638 RID: 1592 RVA: 0x00017393 File Offset: 0x00015593
		[DataMember]
		public long NoticeId { get; set; }

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x0001739C File Offset: 0x0001559C
		// (set) Token: 0x0600063A RID: 1594 RVA: 0x000173A4 File Offset: 0x000155A4
		[DataMember]
		public string Subject { get; set; }

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x0600063B RID: 1595 RVA: 0x000173AD File Offset: 0x000155AD
		// (set) Token: 0x0600063C RID: 1596 RVA: 0x000173B5 File Offset: 0x000155B5
		[DataMember]
		public string Content { get; set; }

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x000173BE File Offset: 0x000155BE
		// (set) Token: 0x0600063E RID: 1598 RVA: 0x000173C6 File Offset: 0x000155C6
		[DataMember]
		public DateTime WriteDate { get; set; }

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x000173CF File Offset: 0x000155CF
		// (set) Token: 0x06000640 RID: 1600 RVA: 0x000173D7 File Offset: 0x000155D7
		[DataMember]
		public long CompanyId { get; set; }

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x000173E0 File Offset: 0x000155E0
		// (set) Token: 0x06000642 RID: 1602 RVA: 0x000173E8 File Offset: 0x000155E8
		[DataMember]
		public virtual Company Company { get; set; }
	}
}
