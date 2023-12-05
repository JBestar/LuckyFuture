using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x02000065 RID: 101
	[DataContract]
	public class DepositWithdraw
	{
		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x00016E2A File Offset: 0x0001502A
		// (set) Token: 0x06000592 RID: 1426 RVA: 0x00016E32 File Offset: 0x00015032
		[DataMember]
		public long DepositWithdrawId { get; set; }

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x00016E3B File Offset: 0x0001503B
		// (set) Token: 0x06000594 RID: 1428 RVA: 0x00016E43 File Offset: 0x00015043
		[Index]
		[DataMember]
		public DateTime MarketDate { get; set; }

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x00016E4C File Offset: 0x0001504C
		// (set) Token: 0x06000596 RID: 1430 RVA: 0x00016E54 File Offset: 0x00015054
		[DataMember]
		public long RequestAmount { get; set; }

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x00016E5D File Offset: 0x0001505D
		// (set) Token: 0x06000598 RID: 1432 RVA: 0x00016E65 File Offset: 0x00015065
		[DataMember]
		public DateTime RequestDate { get; set; }

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x00016E6E File Offset: 0x0001506E
		// (set) Token: 0x0600059A RID: 1434 RVA: 0x00016E76 File Offset: 0x00015076
		[DataMember]
		public long ApplyAmount { get; set; }

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x00016E7F File Offset: 0x0001507F
		// (set) Token: 0x0600059C RID: 1436 RVA: 0x00016E87 File Offset: 0x00015087
		[DataMember]
		public DateTime? ApplyDate { get; set; }

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x00016E90 File Offset: 0x00015090
		// (set) Token: 0x0600059E RID: 1438 RVA: 0x00016E98 File Offset: 0x00015098
		[DataMember]
		public DepositWithdrawType DepositWithdrawType { get; set; }

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x00016EA1 File Offset: 0x000150A1
		// (set) Token: 0x060005A0 RID: 1440 RVA: 0x00016EA9 File Offset: 0x000150A9
		[DataMember]
		public DepositWithdrawStateType DepositWithdrawStateType { get; set; }

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x00016EB2 File Offset: 0x000150B2
		// (set) Token: 0x060005A2 RID: 1442 RVA: 0x00016EBA File Offset: 0x000150BA
		[DataMember]
		public string ApplyAdminId { get; set; }

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x00016EC3 File Offset: 0x000150C3
		// (set) Token: 0x060005A4 RID: 1444 RVA: 0x00016ECB File Offset: 0x000150CB
		[DataMember]
		public string Memo { get; set; }

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x00016ED4 File Offset: 0x000150D4
		// (set) Token: 0x060005A6 RID: 1446 RVA: 0x00016EDC File Offset: 0x000150DC
		[DataMember]
		public long UserAccountId { get; set; }

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x00016EE5 File Offset: 0x000150E5
		// (set) Token: 0x060005A8 RID: 1448 RVA: 0x00016EED File Offset: 0x000150ED
		[DataMember]
		public virtual UserAccount UserAccount { get; set; }
	}
}
