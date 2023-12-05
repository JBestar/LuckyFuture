using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x02000080 RID: 128
	[DataContract]
	public enum UserAccountStateType
	{
		// Token: 0x04000405 RID: 1029
		[EnumMember]
		[Description("정상")]
		Normal,
		// Token: 0x04000406 RID: 1030
		[EnumMember]
		[Description("로스컷")]
		Losscut,
		// Token: 0x04000407 RID: 1031
		[EnumMember]
		[Description("거래정지")]
		Suspension
	}
}
