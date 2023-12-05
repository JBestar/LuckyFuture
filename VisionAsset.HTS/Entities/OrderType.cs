using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x02000074 RID: 116
	[DataContract]
	public enum OrderType
	{
		// Token: 0x04000362 RID: 866
		[EnumMember]
		[Description("접수")]
		New,
		// Token: 0x04000363 RID: 867
		[EnumMember]
		[Description("정정")]
		Change,
		// Token: 0x04000364 RID: 868
		[EnumMember]
		[Description("취소")]
		Cancel,
		// Token: 0x04000365 RID: 869
		[EnumMember]
		[Description("체결")]
		Conclusion
	}
}
