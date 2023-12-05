using System;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x0200007B RID: 123
	[DataContract]
	public class StopLoss
	{
		// Token: 0x17000318 RID: 792
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x00017743 File Offset: 0x00015943
		// (set) Token: 0x060006AC RID: 1708 RVA: 0x0001774B File Offset: 0x0001594B
		[DataMember]
		public long StopLossId { get; set; }

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x00017754 File Offset: 0x00015954
		// (set) Token: 0x060006AE RID: 1710 RVA: 0x0001775C File Offset: 0x0001595C
		[DataMember]
		public Guid SpeedOrderViewGuid { get; set; }

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x00017765 File Offset: 0x00015965
		// (set) Token: 0x060006B0 RID: 1712 RVA: 0x0001776D File Offset: 0x0001596D
		[DataMember]
		public StopLossType StopLossType { get; set; }

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x00017776 File Offset: 0x00015976
		// (set) Token: 0x060006B2 RID: 1714 RVA: 0x0001777E File Offset: 0x0001597E
		[DataMember]
		public bool IsLoss { get; set; }

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x00017787 File Offset: 0x00015987
		// (set) Token: 0x060006B4 RID: 1716 RVA: 0x0001778F File Offset: 0x0001598F
		[DataMember]
		public int LossTick { get; set; }

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x00017798 File Offset: 0x00015998
		// (set) Token: 0x060006B6 RID: 1718 RVA: 0x000177A0 File Offset: 0x000159A0
		[DataMember]
		public bool IsProfit { get; set; }

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x060006B7 RID: 1719 RVA: 0x000177A9 File Offset: 0x000159A9
		// (set) Token: 0x060006B8 RID: 1720 RVA: 0x000177B1 File Offset: 0x000159B1
		[DataMember]
		public int ProfitTick { get; set; }

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x000177BA File Offset: 0x000159BA
		// (set) Token: 0x060006BA RID: 1722 RVA: 0x000177C2 File Offset: 0x000159C2
		[DataMember]
		public double CurrentPrice { get; set; }

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x000177CB File Offset: 0x000159CB
		// (set) Token: 0x060006BC RID: 1724 RVA: 0x000177D3 File Offset: 0x000159D3
		[DataMember]
		public TradeType? PositionTradeType { get; set; }

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x060006BD RID: 1725 RVA: 0x000177DC File Offset: 0x000159DC
		// (set) Token: 0x060006BE RID: 1726 RVA: 0x000177E4 File Offset: 0x000159E4
		[DataMember]
		public double PositionAveragePrice { get; set; }

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x060006BF RID: 1727 RVA: 0x000177ED File Offset: 0x000159ED
		// (set) Token: 0x060006C0 RID: 1728 RVA: 0x000177F5 File Offset: 0x000159F5
		[DataMember]
		public DateTime ProcessDate { get; set; }

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x000177FE File Offset: 0x000159FE
		// (set) Token: 0x060006C2 RID: 1730 RVA: 0x00017806 File Offset: 0x00015A06
		[DataMember]
		public string Symbol { get; set; }

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x0001780F File Offset: 0x00015A0F
		// (set) Token: 0x060006C4 RID: 1732 RVA: 0x00017817 File Offset: 0x00015A17
		[DataMember]
		public long MarketId { get; set; }

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x00017820 File Offset: 0x00015A20
		// (set) Token: 0x060006C6 RID: 1734 RVA: 0x00017828 File Offset: 0x00015A28
		[DataMember]
		public virtual Market Market { get; set; }

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x00017831 File Offset: 0x00015A31
		// (set) Token: 0x060006C8 RID: 1736 RVA: 0x00017839 File Offset: 0x00015A39
		[DataMember]
		public long UserAccountId { get; set; }

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x00017842 File Offset: 0x00015A42
		// (set) Token: 0x060006CA RID: 1738 RVA: 0x0001784A File Offset: 0x00015A4A
		[DataMember]
		public virtual UserAccount UserAccount { get; set; }
	}
}
