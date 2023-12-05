using System;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x0200007F RID: 127
	[DataContract]
	public class UserAccountSpecific
	{
		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000778 RID: 1912 RVA: 0x00017DF8 File Offset: 0x00015FF8
		// (set) Token: 0x06000779 RID: 1913 RVA: 0x00017E00 File Offset: 0x00016000
		[DataMember]
		public long UserAccountSpecificId { get; set; }

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x00017E09 File Offset: 0x00016009
		// (set) Token: 0x0600077B RID: 1915 RVA: 0x00017E11 File Offset: 0x00016011
		[DataMember]
		public string SymbolCode { get; set; }

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x0600077C RID: 1916 RVA: 0x00017E1A File Offset: 0x0001601A
		// (set) Token: 0x0600077D RID: 1917 RVA: 0x00017E22 File Offset: 0x00016022
		[DataMember]
		public double ItemCommission { get; set; }

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x0600077E RID: 1918 RVA: 0x00017E2B File Offset: 0x0001602B
		// (set) Token: 0x0600077F RID: 1919 RVA: 0x00017E33 File Offset: 0x00016033
		[DataMember]
		public int ItemMaxSellQty { get; set; }

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000780 RID: 1920 RVA: 0x00017E3C File Offset: 0x0001603C
		// (set) Token: 0x06000781 RID: 1921 RVA: 0x00017E44 File Offset: 0x00016044
		[DataMember]
		public int ItemMaxBuyQty { get; set; }

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000782 RID: 1922 RVA: 0x00017E4D File Offset: 0x0001604D
		// (set) Token: 0x06000783 RID: 1923 RVA: 0x00017E55 File Offset: 0x00016055
		[DataMember]
		public OrderSignalType ItemOrderSignalType { get; set; }

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000784 RID: 1924 RVA: 0x00017E5E File Offset: 0x0001605E
		// (set) Token: 0x06000785 RID: 1925 RVA: 0x00017E66 File Offset: 0x00016066
		[DataMember]
		public long UserAccountId { get; set; }

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000786 RID: 1926 RVA: 0x00017E6F File Offset: 0x0001606F
		// (set) Token: 0x06000787 RID: 1927 RVA: 0x00017E77 File Offset: 0x00016077
		[DataMember]
		public virtual UserAccount UserAccount { get; set; }
	}
}
