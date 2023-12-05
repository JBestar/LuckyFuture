using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x0200007A RID: 122
	[DataContract]
	public enum ParentAccountType
	{
		// Token: 0x04000388 RID: 904
		[EnumMember]
		[Description("국내선물")]
		FuturesOptions,
		// Token: 0x04000389 RID: 905
		[EnumMember]
		[Description("해외선물")]
		Foreign,
		// Token: 0x0400038A RID: 906
		[EnumMember]
		[Description("국내주식")]
		Stock,
		// Token: 0x0400038B RID: 907
		[EnumMember]
		[Description("국내주식위임")]
		Delegate
	}
}
