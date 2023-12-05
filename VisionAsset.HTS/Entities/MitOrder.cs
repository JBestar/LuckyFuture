using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x0200006E RID: 110
	[DataContract]
	public class MitOrder
	{
		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000618 RID: 1560 RVA: 0x0001728C File Offset: 0x0001548C
		// (set) Token: 0x06000619 RID: 1561 RVA: 0x00017294 File Offset: 0x00015494
		[DataMember]
		public long MitOrderId { get; set; }

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x0001729D File Offset: 0x0001549D
		// (set) Token: 0x0600061B RID: 1563 RVA: 0x000172A5 File Offset: 0x000154A5
		[Index]
		[DataMember]
		public long RootMitOrderId { get; set; }

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x000172AE File Offset: 0x000154AE
		// (set) Token: 0x0600061D RID: 1565 RVA: 0x000172B6 File Offset: 0x000154B6
		[DataMember]
		public MitOrderType MitOrderType { get; set; }

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x000172BF File Offset: 0x000154BF
		// (set) Token: 0x0600061F RID: 1567 RVA: 0x000172C7 File Offset: 0x000154C7
		[DataMember]
		public TradeType TradeType { get; set; }

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x000172D0 File Offset: 0x000154D0
		// (set) Token: 0x06000621 RID: 1569 RVA: 0x000172D8 File Offset: 0x000154D8
		[DataMember]
		public int Qty { get; set; }

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x000172E1 File Offset: 0x000154E1
		// (set) Token: 0x06000623 RID: 1571 RVA: 0x000172E9 File Offset: 0x000154E9
		[DataMember]
		public double Price { get; set; }

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x000172F2 File Offset: 0x000154F2
		// (set) Token: 0x06000625 RID: 1573 RVA: 0x000172FA File Offset: 0x000154FA
		[DataMember]
		public bool IsReverse { get; set; }

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x00017303 File Offset: 0x00015503
		// (set) Token: 0x06000627 RID: 1575 RVA: 0x0001730B File Offset: 0x0001550B
		[DataMember]
		public DateTime ProcessDate { get; set; }

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x00017314 File Offset: 0x00015514
		// (set) Token: 0x06000629 RID: 1577 RVA: 0x0001731C File Offset: 0x0001551C
		[Index]
		[DataMember]
		public int NotExecuteQty { get; set; }

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x00017325 File Offset: 0x00015525
		// (set) Token: 0x0600062B RID: 1579 RVA: 0x0001732D File Offset: 0x0001552D
		[DataMember]
		public double Current { get; set; }

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x00017336 File Offset: 0x00015536
		// (set) Token: 0x0600062D RID: 1581 RVA: 0x0001733E File Offset: 0x0001553E
		[DataMember]
		public string Symbol { get; set; }

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x00017347 File Offset: 0x00015547
		// (set) Token: 0x0600062F RID: 1583 RVA: 0x0001734F File Offset: 0x0001554F
		[DataMember]
		public long MarketId { get; set; }

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x00017358 File Offset: 0x00015558
		// (set) Token: 0x06000631 RID: 1585 RVA: 0x00017360 File Offset: 0x00015560
		[DataMember]
		public virtual Market Market { get; set; }

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x00017369 File Offset: 0x00015569
		// (set) Token: 0x06000633 RID: 1587 RVA: 0x00017371 File Offset: 0x00015571
		[DataMember]
		public long UserAccountId { get; set; }

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x0001737A File Offset: 0x0001557A
		// (set) Token: 0x06000635 RID: 1589 RVA: 0x00017382 File Offset: 0x00015582
		[DataMember]
		public virtual UserAccount UserAccount { get; set; }
	}
}
