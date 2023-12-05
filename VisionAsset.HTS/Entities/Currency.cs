using System;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x02000061 RID: 97
	[DataContract]
	public class Currency
	{
		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x000168A7 File Offset: 0x00014AA7
		// (set) Token: 0x060004E9 RID: 1257 RVA: 0x000168AF File Offset: 0x00014AAF
		[DataMember]
		public CurrencyType CurrencyId { get; set; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x000168B8 File Offset: 0x00014AB8
		// (set) Token: 0x060004EB RID: 1259 RVA: 0x000168C0 File Offset: 0x00014AC0
		[DataMember]
		public double Exchange { get; set; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x000168C9 File Offset: 0x00014AC9
		// (set) Token: 0x060004ED RID: 1261 RVA: 0x000168D1 File Offset: 0x00014AD1
		[DataMember]
		public bool IsAutoUpdateExchange { get; set; }
	}
}
