using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x02000078 RID: 120
	[DataContract]
	public enum PriceType
	{
		// Token: 0x0400037E RID: 894
		[EnumMember]
		[Description("지정가")]
		Limit,
		// Token: 0x0400037F RID: 895
		[EnumMember]
		[Description("시장가")]
		Market,
		// Token: 0x04000380 RID: 896
		[EnumMember]
		[Description("동시호가")]
		Synchronized
	}
}
