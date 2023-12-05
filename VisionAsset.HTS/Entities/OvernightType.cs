using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x02000077 RID: 119
	[DataContract]
	public enum OvernightType
	{
		// Token: 0x0400037A RID: 890
		[EnumMember]
		[Description("변경")]
		Change,
		// Token: 0x0400037B RID: 891
		[EnumMember]
		[Description("실행")]
		Execute,
		// Token: 0x0400037C RID: 892
		[EnumMember]
		[Description("거부")]
		Refuse
	}
}
