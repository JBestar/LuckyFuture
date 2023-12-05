using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x02000076 RID: 118
	[DataContract]
	public enum OvernightRouteType
	{
		// Token: 0x04000376 RID: 886
		[EnumMember]
		[Description("사용자")]
		User,
		// Token: 0x04000377 RID: 887
		[EnumMember]
		[Description("관리자")]
		Administrator,
		// Token: 0x04000378 RID: 888
		[EnumMember]
		[Description("시스템")]
		System
	}
}
