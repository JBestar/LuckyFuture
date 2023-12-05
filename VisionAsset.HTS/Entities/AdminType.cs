using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x02000066 RID: 102
	[DataContract]
	public enum AdminType
	{
		// Token: 0x040002B9 RID: 697
		[EnumMember]
		[Description("마스터")]
		Master,
		// Token: 0x040002BA RID: 698
		[EnumMember]
		[Description("관리자")]
		Administrator
	}
}
