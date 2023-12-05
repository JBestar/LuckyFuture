using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x0200006A RID: 106
	[DataContract]
	public enum CurrencyType
	{
		// Token: 0x040002EA RID: 746
		[EnumMember]
		[Description("KRW")]
		Krw,
		// Token: 0x040002EB RID: 747
		[EnumMember]
		[Description("USD")]
		Usd,
		// Token: 0x040002EC RID: 748
		[EnumMember]
		[Description("HKD")]
		Hkd
	}
}
