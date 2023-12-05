using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x02000073 RID: 115
	[DataContract]
	public enum OrderSignalType
	{
		// Token: 0x04000358 RID: 856
		[EnumMember]
		[Description("미니")]
		Mini,
		// Token: 0x04000359 RID: 857
		[EnumMember]
		[Description("가상")]
		Virtual,
		// Token: 0x0400035A RID: 858
		[EnumMember]
		[Description("하이")]
		Hybrid,
		// Token: 0x0400035B RID: 859
		[EnumMember]
		[Description("리얼")]
		Real,
		// Token: 0x0400035C RID: 860
		[EnumMember]
		[Description("시스템")]
		System,
		// Token: 0x0400035D RID: 861
		[EnumMember]
		[Description("기타1")]
		Other1,
		// Token: 0x0400035E RID: 862
		[EnumMember]
		[Description("기타2")]
		Other2,
		// Token: 0x0400035F RID: 863
		[EnumMember]
		[Description("기타3")]
		Other3,
		// Token: 0x04000360 RID: 864
		[EnumMember]
		[Description("기타4")]
		Other4
	}
}
