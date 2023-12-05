using System;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x02000075 RID: 117
	[DataContract]
	public class Overnight
	{
		// Token: 0x17000303 RID: 771
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x000175DE File Offset: 0x000157DE
		// (set) Token: 0x06000680 RID: 1664 RVA: 0x000175E6 File Offset: 0x000157E6
		[DataMember]
		public long OvernightId { get; set; }

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x000175EF File Offset: 0x000157EF
		// (set) Token: 0x06000682 RID: 1666 RVA: 0x000175F7 File Offset: 0x000157F7
		[DataMember]
		public OvernightType OvernightType { get; set; }

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x00017600 File Offset: 0x00015800
		// (set) Token: 0x06000684 RID: 1668 RVA: 0x00017608 File Offset: 0x00015808
		[DataMember]
		public bool IsFuturesOvernight { get; set; }

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000685 RID: 1669 RVA: 0x00017611 File Offset: 0x00015811
		// (set) Token: 0x06000686 RID: 1670 RVA: 0x00017619 File Offset: 0x00015819
		[DataMember]
		public bool IsCmeFuturesOvernight { get; set; }

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x00017622 File Offset: 0x00015822
		// (set) Token: 0x06000688 RID: 1672 RVA: 0x0001762A File Offset: 0x0001582A
		[DataMember]
		public bool IsOptionOvernight { get; set; }

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x00017633 File Offset: 0x00015833
		// (set) Token: 0x0600068A RID: 1674 RVA: 0x0001763B File Offset: 0x0001583B
		[DataMember]
		public bool IsEurexOptionOvernight { get; set; }

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x00017644 File Offset: 0x00015844
		// (set) Token: 0x0600068C RID: 1676 RVA: 0x0001764C File Offset: 0x0001584C
		[DataMember]
		public bool IsForeignOvernight { get; set; }

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x00017655 File Offset: 0x00015855
		// (set) Token: 0x0600068E RID: 1678 RVA: 0x0001765D File Offset: 0x0001585D
		[DataMember]
		public bool IsKospiOvernight { get; set; }

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x00017666 File Offset: 0x00015866
		// (set) Token: 0x06000690 RID: 1680 RVA: 0x0001766E File Offset: 0x0001586E
		[DataMember]
		public bool IsKosdaqOvernight { get; set; }

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000691 RID: 1681 RVA: 0x00017677 File Offset: 0x00015877
		// (set) Token: 0x06000692 RID: 1682 RVA: 0x0001767F File Offset: 0x0001587F
		[DataMember]
		public long NeedBalance { get; set; }

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x00017688 File Offset: 0x00015888
		// (set) Token: 0x06000694 RID: 1684 RVA: 0x00017690 File Offset: 0x00015890
		[DataMember]
		public long Balance { get; set; }

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x00017699 File Offset: 0x00015899
		// (set) Token: 0x06000696 RID: 1686 RVA: 0x000176A1 File Offset: 0x000158A1
		[DataMember]
		public OvernightRouteType OvernightRouteType { get; set; }

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000697 RID: 1687 RVA: 0x000176AA File Offset: 0x000158AA
		// (set) Token: 0x06000698 RID: 1688 RVA: 0x000176B2 File Offset: 0x000158B2
		[DataMember]
		public DateTime ProcessDate { get; set; }

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x000176BB File Offset: 0x000158BB
		// (set) Token: 0x0600069A RID: 1690 RVA: 0x000176C3 File Offset: 0x000158C3
		[DataMember]
		public long UserAccountId { get; set; }

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x000176CC File Offset: 0x000158CC
		// (set) Token: 0x0600069C RID: 1692 RVA: 0x000176D4 File Offset: 0x000158D4
		[DataMember]
		public virtual UserAccount UserAccount { get; set; }
	}
}
