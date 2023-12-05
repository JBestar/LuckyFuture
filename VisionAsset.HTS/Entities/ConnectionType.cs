using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x02000060 RID: 96
	[DataContract]
	public enum ConnectionType
	{
		// Token: 0x04000252 RID: 594
		[EnumMember]
		[Description("로그인")]
		Login,
		// Token: 0x04000253 RID: 595
		[EnumMember]
		[Description("로그아웃")]
		Logout,
		// Token: 0x04000254 RID: 596
		[EnumMember]
		[Description("종료신호")]
		SignOff,
		// Token: 0x04000255 RID: 597
		[EnumMember]
		[Description("NULL")]
		Null
	}
}
