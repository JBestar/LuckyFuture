using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x02000071 RID: 113
	[DataContract]
	public class Order
	{
		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x000173F1 File Offset: 0x000155F1
		// (set) Token: 0x06000645 RID: 1605 RVA: 0x000173F9 File Offset: 0x000155F9
		[DataMember]
		public long OrderId { get; set; }

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x00017402 File Offset: 0x00015602
		// (set) Token: 0x06000647 RID: 1607 RVA: 0x0001740A File Offset: 0x0001560A
		[Index]
		[DataMember]
		public long RootOrderId { get; set; }

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x00017413 File Offset: 0x00015613
		// (set) Token: 0x06000649 RID: 1609 RVA: 0x0001741B File Offset: 0x0001561B
		[Index]
		[DataMember]
		public long OrderNumber { get; set; }

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x00017424 File Offset: 0x00015624
		// (set) Token: 0x0600064B RID: 1611 RVA: 0x0001742C File Offset: 0x0001562C
		[DataMember]
		public OrderType OrderType { get; set; }

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x00017435 File Offset: 0x00015635
		// (set) Token: 0x0600064D RID: 1613 RVA: 0x0001743D File Offset: 0x0001563D
		[DataMember]
		public TradeType TradeType { get; set; }

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x00017446 File Offset: 0x00015646
		// (set) Token: 0x0600064F RID: 1615 RVA: 0x0001744E File Offset: 0x0001564E
		[DataMember]
		public PriceType PriceType { get; set; }

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x00017457 File Offset: 0x00015657
		// (set) Token: 0x06000651 RID: 1617 RVA: 0x0001745F File Offset: 0x0001565F
		[DataMember]
		public int Qty { get; set; }

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x00017468 File Offset: 0x00015668
		// (set) Token: 0x06000653 RID: 1619 RVA: 0x00017470 File Offset: 0x00015670
		[DataMember]
		public double Price { get; set; }

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x00017479 File Offset: 0x00015679
		// (set) Token: 0x06000655 RID: 1621 RVA: 0x00017481 File Offset: 0x00015681
		[DataMember]
		public int ApplyLeverage { get; set; }

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x0001748A File Offset: 0x0001568A
		// (set) Token: 0x06000657 RID: 1623 RVA: 0x00017492 File Offset: 0x00015692
		[DataMember]
		public long Profit { get; set; }

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x0001749B File Offset: 0x0001569B
		// (set) Token: 0x06000659 RID: 1625 RVA: 0x000174A3 File Offset: 0x000156A3
		[DataMember]
		public long Commission { get; set; }

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x000174AC File Offset: 0x000156AC
		// (set) Token: 0x0600065B RID: 1627 RVA: 0x000174B4 File Offset: 0x000156B4
		[DataMember]
		public long Tax { get; set; }

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x000174BD File Offset: 0x000156BD
		// (set) Token: 0x0600065D RID: 1629 RVA: 0x000174C5 File Offset: 0x000156C5
		[DataMember]
		public DateTime ProcessDate { get; set; }

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x000174CE File Offset: 0x000156CE
		// (set) Token: 0x0600065F RID: 1631 RVA: 0x000174D6 File Offset: 0x000156D6
		[DataMember]
		public OrderSignalType OrderSignalType { get; set; }

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x000174DF File Offset: 0x000156DF
		// (set) Token: 0x06000661 RID: 1633 RVA: 0x000174E7 File Offset: 0x000156E7
		[DataMember]
		public OrderRouteType OrderRouteType { get; set; }

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x000174F0 File Offset: 0x000156F0
		// (set) Token: 0x06000663 RID: 1635 RVA: 0x000174F8 File Offset: 0x000156F8
		[Index]
		[DataMember]
		public int NotConclusionQty { get; set; }

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x00017501 File Offset: 0x00015701
		// (set) Token: 0x06000665 RID: 1637 RVA: 0x00017509 File Offset: 0x00015709
		[Index]
		[DataMember]
		public int UnliquidationQty { get; set; }

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x00017512 File Offset: 0x00015712
		// (set) Token: 0x06000667 RID: 1639 RVA: 0x0001751A File Offset: 0x0001571A
		[DataMember]
		public double MovingAveragePrice { get; set; }

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x00017523 File Offset: 0x00015723
		// (set) Token: 0x06000669 RID: 1641 RVA: 0x0001752B File Offset: 0x0001572B
		[DataMember]
		public int ProcessCount { get; set; }

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x0600066A RID: 1642 RVA: 0x00017534 File Offset: 0x00015734
		// (set) Token: 0x0600066B RID: 1643 RVA: 0x0001753C File Offset: 0x0001573C
		[DataMember]
		public bool IsSatisfaction { get; set; }

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x00017545 File Offset: 0x00015745
		// (set) Token: 0x0600066D RID: 1645 RVA: 0x0001754D File Offset: 0x0001574D
		[DataMember]
		public bool IsNotCanceled { get; set; }

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x00017556 File Offset: 0x00015756
		// (set) Token: 0x0600066F RID: 1647 RVA: 0x0001755E File Offset: 0x0001575E
		[Index]
		[DataMember]
		public bool IsNotCompleted { get; set; }

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000670 RID: 1648 RVA: 0x00017567 File Offset: 0x00015767
		// (set) Token: 0x06000671 RID: 1649 RVA: 0x0001756F File Offset: 0x0001576F
		[DataMember]
		public long Balance { get; set; }

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x00017578 File Offset: 0x00015778
		// (set) Token: 0x06000673 RID: 1651 RVA: 0x00017580 File Offset: 0x00015780
		[DataMember]
		public string ApplyAdminId { get; set; }

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000674 RID: 1652 RVA: 0x00017589 File Offset: 0x00015789
		// (set) Token: 0x06000675 RID: 1653 RVA: 0x00017591 File Offset: 0x00015791
		[DataMember]
		public string Symbol { get; set; }

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000676 RID: 1654 RVA: 0x0001759A File Offset: 0x0001579A
		// (set) Token: 0x06000677 RID: 1655 RVA: 0x000175A2 File Offset: 0x000157A2
		[DataMember]
		public long MarketId { get; set; }

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000678 RID: 1656 RVA: 0x000175AB File Offset: 0x000157AB
		// (set) Token: 0x06000679 RID: 1657 RVA: 0x000175B3 File Offset: 0x000157B3
		[DataMember]
		public virtual Market Market { get; set; }

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x0600067A RID: 1658 RVA: 0x000175BC File Offset: 0x000157BC
		// (set) Token: 0x0600067B RID: 1659 RVA: 0x000175C4 File Offset: 0x000157C4
		[DataMember]
		public long UserAccountId { get; set; }

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x0600067C RID: 1660 RVA: 0x000175CD File Offset: 0x000157CD
		// (set) Token: 0x0600067D RID: 1661 RVA: 0x000175D5 File Offset: 0x000157D5
		[DataMember]
		public virtual UserAccount UserAccount { get; set; }
	}
}
