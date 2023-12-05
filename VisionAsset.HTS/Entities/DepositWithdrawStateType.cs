using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x02000067 RID: 103
	[DataContract]
	public enum DepositWithdrawStateType
	{
		// Token: 0x040002BC RID: 700
		[EnumMember]
		[Description("요청")]
		Request,
		// Token: 0x040002BD RID: 701
		[EnumMember]
		[Description("승인")]
		Authorization,
		// Token: 0x040002BE RID: 702
		[EnumMember]
		[Description("취소")]
		Cancel
	}
}
