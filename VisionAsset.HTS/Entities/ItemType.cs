using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x0200006B RID: 107
	[DataContract]
	public enum ItemType
	{
		// Token: 0x040002EE RID: 750
		[EnumMember]
		[Description("선물")]
		Futures,
		// Token: 0x040002EF RID: 751
		[EnumMember]
		[Description("옵션")]
		Options,
		// Token: 0x040002F0 RID: 752
		[EnumMember]
		[Description("야간선물")]
		Cme,
		// Token: 0x040002F1 RID: 753
		[EnumMember]
		[Description("야간옵션")]
		Eurex,
		// Token: 0x040002F2 RID: 754
		[EnumMember]
		[Description("해외선물")]
		Foreign,
		// Token: 0x040002F3 RID: 755
		[EnumMember]
		[Description("코스피")]
		Kospi,
		// Token: 0x040002F4 RID: 756
		[EnumMember]
		[Description("코스닥")]
		Kosdaq
	}
}
