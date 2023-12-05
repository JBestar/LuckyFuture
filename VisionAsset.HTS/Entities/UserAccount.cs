using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x0200007E RID: 126
	[DataContract]
	public class UserAccount
	{
		// Token: 0x1700033D RID: 829
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x000179B8 File Offset: 0x00015BB8
		// (set) Token: 0x060006F8 RID: 1784 RVA: 0x000179C0 File Offset: 0x00015BC0
		[DataMember]
		public long UserAccountId { get; set; }

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x000179C9 File Offset: 0x00015BC9
		// (set) Token: 0x060006FA RID: 1786 RVA: 0x000179D1 File Offset: 0x00015BD1
		[DataMember]
		public long Balance { get; set; }

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x060006FB RID: 1787 RVA: 0x000179DA File Offset: 0x00015BDA
		// (set) Token: 0x060006FC RID: 1788 RVA: 0x000179E2 File Offset: 0x00015BE2
		[DataMember]
		public int Leverage { get; set; }

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x000179EB File Offset: 0x00015BEB
		// (set) Token: 0x060006FE RID: 1790 RVA: 0x000179F3 File Offset: 0x00015BF3
		[DataMember]
		public DateTime CreateDate { get; set; }

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x000179FC File Offset: 0x00015BFC
		// (set) Token: 0x06000700 RID: 1792 RVA: 0x00017A04 File Offset: 0x00015C04
		[DataMember]
		public UserAccountStateType UserAccountStateType { get; set; }

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x00017A0D File Offset: 0x00015C0D
		// (set) Token: 0x06000702 RID: 1794 RVA: 0x00017A15 File Offset: 0x00015C15
		[DataMember]
		public DateTime? LatestLossCutDateTime { get; set; }

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x00017A1E File Offset: 0x00015C1E
		// (set) Token: 0x06000704 RID: 1796 RVA: 0x00017A26 File Offset: 0x00015C26
		[DataMember]
		public bool IsFuturesTrade { get; set; }

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x00017A2F File Offset: 0x00015C2F
		// (set) Token: 0x06000706 RID: 1798 RVA: 0x00017A37 File Offset: 0x00015C37
		[DataMember]
		public OrderSignalType FuturesOrderSignalType { get; set; }

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x00017A40 File Offset: 0x00015C40
		// (set) Token: 0x06000708 RID: 1800 RVA: 0x00017A48 File Offset: 0x00015C48
		[DataMember]
		public double FuturesCommission { get; set; }

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x00017A51 File Offset: 0x00015C51
		// (set) Token: 0x0600070A RID: 1802 RVA: 0x00017A59 File Offset: 0x00015C59
		[DataMember]
		public int FuturesMaxSellQty { get; set; }

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x00017A62 File Offset: 0x00015C62
		// (set) Token: 0x0600070C RID: 1804 RVA: 0x00017A6A File Offset: 0x00015C6A
		[DataMember]
		public int FuturesMaxBuyQty { get; set; }

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x00017A73 File Offset: 0x00015C73
		// (set) Token: 0x0600070E RID: 1806 RVA: 0x00017A7B File Offset: 0x00015C7B
		[DataMember]
		public bool IsFuturesOvernight { get; set; }

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x00017A84 File Offset: 0x00015C84
		// (set) Token: 0x06000710 RID: 1808 RVA: 0x00017A8C File Offset: 0x00015C8C
		[DataMember]
		public bool IsFuturesOvernightSettingPermission { get; set; }

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x00017A95 File Offset: 0x00015C95
		// (set) Token: 0x06000712 RID: 1810 RVA: 0x00017A9D File Offset: 0x00015C9D
		[DataMember]
		public bool IsOptionTrade { get; set; }

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x00017AA6 File Offset: 0x00015CA6
		// (set) Token: 0x06000714 RID: 1812 RVA: 0x00017AAE File Offset: 0x00015CAE
		[DataMember]
		public OrderSignalType OptionOrderSignalType { get; set; }

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x00017AB7 File Offset: 0x00015CB7
		// (set) Token: 0x06000716 RID: 1814 RVA: 0x00017ABF File Offset: 0x00015CBF
		[DataMember]
		public double OptionCommission { get; set; }

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x00017AC8 File Offset: 0x00015CC8
		// (set) Token: 0x06000718 RID: 1816 RVA: 0x00017AD0 File Offset: 0x00015CD0
		[DataMember]
		public int OptionMaxSellQty { get; set; }

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x00017AD9 File Offset: 0x00015CD9
		// (set) Token: 0x0600071A RID: 1818 RVA: 0x00017AE1 File Offset: 0x00015CE1
		[DataMember]
		public int OptionMaxBuyQty { get; set; }

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x0600071B RID: 1819 RVA: 0x00017AEA File Offset: 0x00015CEA
		// (set) Token: 0x0600071C RID: 1820 RVA: 0x00017AF2 File Offset: 0x00015CF2
		[DataMember]
		public bool IsOptionOvernight { get; set; }

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x0600071D RID: 1821 RVA: 0x00017AFB File Offset: 0x00015CFB
		// (set) Token: 0x0600071E RID: 1822 RVA: 0x00017B03 File Offset: 0x00015D03
		[DataMember]
		public bool IsOptionOvernightSettingPermission { get; set; }

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x0600071F RID: 1823 RVA: 0x00017B0C File Offset: 0x00015D0C
		// (set) Token: 0x06000720 RID: 1824 RVA: 0x00017B14 File Offset: 0x00015D14
		[DataMember]
		public bool IsCmeTrade { get; set; }

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000721 RID: 1825 RVA: 0x00017B1D File Offset: 0x00015D1D
		// (set) Token: 0x06000722 RID: 1826 RVA: 0x00017B25 File Offset: 0x00015D25
		[DataMember]
		public OrderSignalType CmeOrderSignalType { get; set; }

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x00017B2E File Offset: 0x00015D2E
		// (set) Token: 0x06000724 RID: 1828 RVA: 0x00017B36 File Offset: 0x00015D36
		[DataMember]
		public double CmeCommission { get; set; }

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x00017B3F File Offset: 0x00015D3F
		// (set) Token: 0x06000726 RID: 1830 RVA: 0x00017B47 File Offset: 0x00015D47
		[DataMember]
		public int CmeMaxSellQty { get; set; }

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x00017B50 File Offset: 0x00015D50
		// (set) Token: 0x06000728 RID: 1832 RVA: 0x00017B58 File Offset: 0x00015D58
		[DataMember]
		public int CmeMaxBuyQty { get; set; }

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x00017B61 File Offset: 0x00015D61
		// (set) Token: 0x0600072A RID: 1834 RVA: 0x00017B69 File Offset: 0x00015D69
		[DataMember]
		public bool IsCmeOvernight { get; set; }

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x00017B72 File Offset: 0x00015D72
		// (set) Token: 0x0600072C RID: 1836 RVA: 0x00017B7A File Offset: 0x00015D7A
		[DataMember]
		public bool IsCmeOvernightSettingPermission { get; set; }

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x00017B83 File Offset: 0x00015D83
		// (set) Token: 0x0600072E RID: 1838 RVA: 0x00017B8B File Offset: 0x00015D8B
		[DataMember]
		public bool IsEurexTrade { get; set; }

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x00017B94 File Offset: 0x00015D94
		// (set) Token: 0x06000730 RID: 1840 RVA: 0x00017B9C File Offset: 0x00015D9C
		[DataMember]
		public OrderSignalType EurexOrderSignalType { get; set; }

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x00017BA5 File Offset: 0x00015DA5
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x00017BAD File Offset: 0x00015DAD
		[DataMember]
		public double EurexCommission { get; set; }

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x00017BB6 File Offset: 0x00015DB6
		// (set) Token: 0x06000734 RID: 1844 RVA: 0x00017BBE File Offset: 0x00015DBE
		[DataMember]
		public int EurexMaxSellQty { get; set; }

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x00017BC7 File Offset: 0x00015DC7
		// (set) Token: 0x06000736 RID: 1846 RVA: 0x00017BCF File Offset: 0x00015DCF
		[DataMember]
		public int EurexMaxBuyQty { get; set; }

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x00017BD8 File Offset: 0x00015DD8
		// (set) Token: 0x06000738 RID: 1848 RVA: 0x00017BE0 File Offset: 0x00015DE0
		[DataMember]
		public bool IsEurexOvernight { get; set; }

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x00017BE9 File Offset: 0x00015DE9
		// (set) Token: 0x0600073A RID: 1850 RVA: 0x00017BF1 File Offset: 0x00015DF1
		[DataMember]
		public bool IsEurexOvernightSettingPermission { get; set; }

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x00017BFA File Offset: 0x00015DFA
		// (set) Token: 0x0600073C RID: 1852 RVA: 0x00017C02 File Offset: 0x00015E02
		[DataMember]
		public bool IsForeignTrade { get; set; }

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x0600073D RID: 1853 RVA: 0x00017C0B File Offset: 0x00015E0B
		// (set) Token: 0x0600073E RID: 1854 RVA: 0x00017C13 File Offset: 0x00015E13
		[DataMember]
		public OrderSignalType ForeignOrderSignalType { get; set; }

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x00017C1C File Offset: 0x00015E1C
		// (set) Token: 0x06000740 RID: 1856 RVA: 0x00017C24 File Offset: 0x00015E24
		[DataMember]
		public double ForeignCommission { get; set; }

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x00017C2D File Offset: 0x00015E2D
		// (set) Token: 0x06000742 RID: 1858 RVA: 0x00017C35 File Offset: 0x00015E35
		[DataMember]
		public int ForeignMaxSellQty { get; set; }

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000743 RID: 1859 RVA: 0x00017C3E File Offset: 0x00015E3E
		// (set) Token: 0x06000744 RID: 1860 RVA: 0x00017C46 File Offset: 0x00015E46
		[DataMember]
		public int ForeignMaxBuyQty { get; set; }

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000745 RID: 1861 RVA: 0x00017C4F File Offset: 0x00015E4F
		// (set) Token: 0x06000746 RID: 1862 RVA: 0x00017C57 File Offset: 0x00015E57
		[DataMember]
		public bool IsForeignOvernight { get; set; }

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000747 RID: 1863 RVA: 0x00017C60 File Offset: 0x00015E60
		// (set) Token: 0x06000748 RID: 1864 RVA: 0x00017C68 File Offset: 0x00015E68
		[DataMember]
		public bool IsForeignOvernightSettingPermission { get; set; }

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x00017C71 File Offset: 0x00015E71
		// (set) Token: 0x0600074A RID: 1866 RVA: 0x00017C79 File Offset: 0x00015E79
		[DataMember]
		public bool IsKospiTrade { get; set; }

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x0600074B RID: 1867 RVA: 0x00017C82 File Offset: 0x00015E82
		// (set) Token: 0x0600074C RID: 1868 RVA: 0x00017C8A File Offset: 0x00015E8A
		[DataMember]
		public OrderSignalType KospiOrderSignalType { get; set; }

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x0600074D RID: 1869 RVA: 0x00017C93 File Offset: 0x00015E93
		// (set) Token: 0x0600074E RID: 1870 RVA: 0x00017C9B File Offset: 0x00015E9B
		[DataMember]
		public double KospiCommission { get; set; }

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x00017CA4 File Offset: 0x00015EA4
		// (set) Token: 0x06000750 RID: 1872 RVA: 0x00017CAC File Offset: 0x00015EAC
		[DataMember]
		public long KospiMaxSellMargin { get; set; }

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000751 RID: 1873 RVA: 0x00017CB5 File Offset: 0x00015EB5
		// (set) Token: 0x06000752 RID: 1874 RVA: 0x00017CBD File Offset: 0x00015EBD
		[DataMember]
		public long KospiMaxBuyMargin { get; set; }

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000753 RID: 1875 RVA: 0x00017CC6 File Offset: 0x00015EC6
		// (set) Token: 0x06000754 RID: 1876 RVA: 0x00017CCE File Offset: 0x00015ECE
		[DataMember]
		public bool IsKospiOvernight { get; set; }

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x00017CD7 File Offset: 0x00015ED7
		// (set) Token: 0x06000756 RID: 1878 RVA: 0x00017CDF File Offset: 0x00015EDF
		[DataMember]
		public bool IsKospiOvernightSettingPermission { get; set; }

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000757 RID: 1879 RVA: 0x00017CE8 File Offset: 0x00015EE8
		// (set) Token: 0x06000758 RID: 1880 RVA: 0x00017CF0 File Offset: 0x00015EF0
		[DataMember]
		public bool IsKosdaqTrade { get; set; }

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000759 RID: 1881 RVA: 0x00017CF9 File Offset: 0x00015EF9
		// (set) Token: 0x0600075A RID: 1882 RVA: 0x00017D01 File Offset: 0x00015F01
		[DataMember]
		public OrderSignalType KosdaqOrderSignalType { get; set; }

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x0600075B RID: 1883 RVA: 0x00017D0A File Offset: 0x00015F0A
		// (set) Token: 0x0600075C RID: 1884 RVA: 0x00017D12 File Offset: 0x00015F12
		[DataMember]
		public double KosdaqCommission { get; set; }

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x00017D1B File Offset: 0x00015F1B
		// (set) Token: 0x0600075E RID: 1886 RVA: 0x00017D23 File Offset: 0x00015F23
		[DataMember]
		public long KosdaqMaxSellMargin { get; set; }

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x0600075F RID: 1887 RVA: 0x00017D2C File Offset: 0x00015F2C
		// (set) Token: 0x06000760 RID: 1888 RVA: 0x00017D34 File Offset: 0x00015F34
		[DataMember]
		public long KosdaqMaxBuyMargin { get; set; }

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000761 RID: 1889 RVA: 0x00017D3D File Offset: 0x00015F3D
		// (set) Token: 0x06000762 RID: 1890 RVA: 0x00017D45 File Offset: 0x00015F45
		[DataMember]
		public bool IsKosdaqOvernight { get; set; }

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000763 RID: 1891 RVA: 0x00017D4E File Offset: 0x00015F4E
		// (set) Token: 0x06000764 RID: 1892 RVA: 0x00017D56 File Offset: 0x00015F56
		[DataMember]
		public bool IsKosdaqOvernightSettingPermission { get; set; }

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000765 RID: 1893 RVA: 0x00017D5F File Offset: 0x00015F5F
		// (set) Token: 0x06000766 RID: 1894 RVA: 0x00017D67 File Offset: 0x00015F67
		[DataMember]
		public long UserId { get; set; }

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000767 RID: 1895 RVA: 0x00017D70 File Offset: 0x00015F70
		// (set) Token: 0x06000768 RID: 1896 RVA: 0x00017D78 File Offset: 0x00015F78
		[DataMember]
		public virtual User User { get; set; }

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000769 RID: 1897 RVA: 0x00017D81 File Offset: 0x00015F81
		// (set) Token: 0x0600076A RID: 1898 RVA: 0x00017D89 File Offset: 0x00015F89
		[DataMember]
		public virtual ICollection<UserAccountSpecific> UserAccountSpecifics { get; set; }

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x0600076B RID: 1899 RVA: 0x00017D92 File Offset: 0x00015F92
		// (set) Token: 0x0600076C RID: 1900 RVA: 0x00017D9A File Offset: 0x00015F9A
		[DataMember]
		public virtual ICollection<Order> Orders { get; set; }

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x0600076D RID: 1901 RVA: 0x00017DA3 File Offset: 0x00015FA3
		// (set) Token: 0x0600076E RID: 1902 RVA: 0x00017DAB File Offset: 0x00015FAB
		[DataMember]
		public virtual ICollection<MitOrder> MitOrders { get; set; }

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x0600076F RID: 1903 RVA: 0x00017DB4 File Offset: 0x00015FB4
		// (set) Token: 0x06000770 RID: 1904 RVA: 0x00017DBC File Offset: 0x00015FBC
		[DataMember]
		public virtual ICollection<StopLoss> StopLosses { get; set; }

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x00017DC5 File Offset: 0x00015FC5
		// (set) Token: 0x06000772 RID: 1906 RVA: 0x00017DCD File Offset: 0x00015FCD
		[DataMember]
		public virtual ICollection<Overnight> Overnights { get; set; }

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000773 RID: 1907 RVA: 0x00017DD6 File Offset: 0x00015FD6
		// (set) Token: 0x06000774 RID: 1908 RVA: 0x00017DDE File Offset: 0x00015FDE
		[DataMember]
		public virtual ICollection<DepositWithdraw> DepositAndWithdrawals { get; set; }

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000775 RID: 1909 RVA: 0x00017DE7 File Offset: 0x00015FE7
		// (set) Token: 0x06000776 RID: 1910 RVA: 0x00017DEF File Offset: 0x00015FEF
		[DataMember]
		public virtual ICollection<DayProfitLoss> DayProfitLosses { get; set; }
	}
}
