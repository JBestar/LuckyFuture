using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x0200006F RID: 111
	[DataContract]
	public enum MitOrderType
	{
		// Token: 0x0400031E RID: 798
		[EnumMember]
		[Description("접수")]
		New,
		// Token: 0x0400031F RID: 799
		[EnumMember]
		[Description("정정")]
		Change,
		// Token: 0x04000320 RID: 800
		[EnumMember]
		[Description("취소")]
		Cancel,
		// Token: 0x04000321 RID: 801
		[EnumMember]
		[Description("실행")]
		Execute,
		// Token: 0x04000322 RID: 802
		[EnumMember]
		[Description("거부")]
		Refuse
	}
}
