using System;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x02000079 RID: 121
	[DataContract]
	public class ParentAccount
	{
		// Token: 0x17000312 RID: 786
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x000176DD File Offset: 0x000158DD
		// (set) Token: 0x0600069F RID: 1695 RVA: 0x000176E5 File Offset: 0x000158E5
		[DataMember]
		public long ParentAccountId { get; set; }

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x000176EE File Offset: 0x000158EE
		// (set) Token: 0x060006A1 RID: 1697 RVA: 0x000176F6 File Offset: 0x000158F6
		[DataMember]
		public ParentAccountType ParentAccountType { get; set; }

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x060006A2 RID: 1698 RVA: 0x000176FF File Offset: 0x000158FF
		// (set) Token: 0x060006A3 RID: 1699 RVA: 0x00017707 File Offset: 0x00015907
		[DataMember]
		public string ParentAccountNumber { get; set; }

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x060006A4 RID: 1700 RVA: 0x00017710 File Offset: 0x00015910
		// (set) Token: 0x060006A5 RID: 1701 RVA: 0x00017718 File Offset: 0x00015918
		[DataMember]
		public string ParentAccountPassword { get; set; }

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x060006A6 RID: 1702 RVA: 0x00017721 File Offset: 0x00015921
		// (set) Token: 0x060006A7 RID: 1703 RVA: 0x00017729 File Offset: 0x00015929
		[DataMember]
		public long CompanyId { get; set; }

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x060006A8 RID: 1704 RVA: 0x00017732 File Offset: 0x00015932
		// (set) Token: 0x060006A9 RID: 1705 RVA: 0x0001773A File Offset: 0x0001593A
		[DataMember]
		public virtual Company Company { get; set; }
	}
}
