using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x0200006C RID: 108
	[DataContract]
	public class Market
	{
		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x0001717C File Offset: 0x0001537C
		// (set) Token: 0x060005F8 RID: 1528 RVA: 0x00017184 File Offset: 0x00015384
		[DataMember]
		public long MarketId { get; set; }

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x060005F9 RID: 1529 RVA: 0x0001718D File Offset: 0x0001538D
		// (set) Token: 0x060005FA RID: 1530 RVA: 0x00017195 File Offset: 0x00015395
		[Index]
		[DataMember]
		public DateTime MarketDate { get; set; }

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x060005FB RID: 1531 RVA: 0x0001719E File Offset: 0x0001539E
		// (set) Token: 0x060005FC RID: 1532 RVA: 0x000171A6 File Offset: 0x000153A6
		[DataMember]
		public DateTime OpenSynchronizedTime { get; set; }

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x060005FD RID: 1533 RVA: 0x000171AF File Offset: 0x000153AF
		// (set) Token: 0x060005FE RID: 1534 RVA: 0x000171B7 File Offset: 0x000153B7
		[DataMember]
		public DateTime OpenTime { get; set; }

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x060005FF RID: 1535 RVA: 0x000171C0 File Offset: 0x000153C0
		// (set) Token: 0x06000600 RID: 1536 RVA: 0x000171C8 File Offset: 0x000153C8
		[DataMember]
		public DateTime EndOrderTime { get; set; }

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x000171D1 File Offset: 0x000153D1
		// (set) Token: 0x06000602 RID: 1538 RVA: 0x000171D9 File Offset: 0x000153D9
		[DataMember]
		public DateTime CloseTime { get; set; }

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x000171E2 File Offset: 0x000153E2
		// (set) Token: 0x06000604 RID: 1540 RVA: 0x000171EA File Offset: 0x000153EA
		[DataMember]
		public DateTime PauseTime1 { get; set; }

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x000171F3 File Offset: 0x000153F3
		// (set) Token: 0x06000606 RID: 1542 RVA: 0x000171FB File Offset: 0x000153FB
		[DataMember]
		public DateTime ReopenTime1 { get; set; }

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x00017204 File Offset: 0x00015404
		// (set) Token: 0x06000608 RID: 1544 RVA: 0x0001720C File Offset: 0x0001540C
		[DataMember]
		public DateTime PauseTime2 { get; set; }

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000609 RID: 1545 RVA: 0x00017215 File Offset: 0x00015415
		// (set) Token: 0x0600060A RID: 1546 RVA: 0x0001721D File Offset: 0x0001541D
		[DataMember]
		public DateTime ReopenTime2 { get; set; }

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x00017226 File Offset: 0x00015426
		// (set) Token: 0x0600060C RID: 1548 RVA: 0x0001722E File Offset: 0x0001542E
		[DataMember]
		public MarketStateType MarketStateType { get; set; }

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x0600060D RID: 1549 RVA: 0x00017237 File Offset: 0x00015437
		// (set) Token: 0x0600060E RID: 1550 RVA: 0x0001723F File Offset: 0x0001543F
		[DataMember]
		public long ItemId { get; set; }

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x0600060F RID: 1551 RVA: 0x00017248 File Offset: 0x00015448
		// (set) Token: 0x06000610 RID: 1552 RVA: 0x00017250 File Offset: 0x00015450
		[DataMember]
		public virtual Item Item { get; set; }

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000611 RID: 1553 RVA: 0x00017259 File Offset: 0x00015459
		// (set) Token: 0x06000612 RID: 1554 RVA: 0x00017261 File Offset: 0x00015461
		[DataMember]
		public virtual ICollection<Order> Orders { get; set; }

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000613 RID: 1555 RVA: 0x0001726A File Offset: 0x0001546A
		// (set) Token: 0x06000614 RID: 1556 RVA: 0x00017272 File Offset: 0x00015472
		[DataMember]
		public virtual ICollection<MitOrder> MitOrders { get; set; }

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000615 RID: 1557 RVA: 0x0001727B File Offset: 0x0001547B
		// (set) Token: 0x06000616 RID: 1558 RVA: 0x00017283 File Offset: 0x00015483
		[DataMember]
		public virtual ICollection<StopLoss> StopLosses { get; set; }
	}
}
