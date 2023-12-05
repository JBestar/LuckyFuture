using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x02000063 RID: 99
	[DataContract]
	public enum TradeType
	{
		// Token: 0x0400026A RID: 618
		[EnumMember]
		[Description("매도")]
		Sell,
		// Token: 0x0400026B RID: 619
		[EnumMember]
		[Description("매수")]
		Buy
	}
}
