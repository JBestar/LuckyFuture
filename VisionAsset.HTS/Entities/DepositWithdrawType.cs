using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x02000068 RID: 104
	[DataContract]
	public enum DepositWithdrawType
	{
		// Token: 0x040002C0 RID: 704
		[EnumMember]
		[Description("입금")]
		Deposit,
		// Token: 0x040002C1 RID: 705
		[EnumMember]
		[Description("출금")]
		Withdraw,
		// Token: 0x040002C2 RID: 706
		[EnumMember]
		[Description("이체")]
		Transfer
	}
}
