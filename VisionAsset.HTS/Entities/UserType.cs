using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x02000081 RID: 129
	[DataContract]
	public enum UserType
	{
		// Token: 0x04000409 RID: 1033
		[EnumMember]
		[Description("승인대기")]
		Standby,
		// Token: 0x0400040A RID: 1034
		[EnumMember]
		[Description("정회원")]
		Member,
		// Token: 0x0400040B RID: 1035
		[EnumMember]
		[Description("전문가")]
		Expert,
		// Token: 0x0400040C RID: 1036
		[EnumMember]
		[Description("테스트")]
		Test,
		// Token: 0x0400040D RID: 1037
		[EnumMember]
		[Description("매니저")]
		Manager,
		// Token: 0x0400040E RID: 1038
		[EnumMember]
		[Description("총매니저")]
		ChiefManager
	}
}
