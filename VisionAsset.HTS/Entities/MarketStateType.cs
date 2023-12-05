using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x0200006D RID: 109
	[DataContract]
	public enum MarketStateType
	{
		// Token: 0x04000306 RID: 774
		[EnumMember]
		[Description("개장전")]
		BeforeOpen,
		// Token: 0x04000307 RID: 775
		[EnumMember]
		[Description("장전동시호가")]
		OpenSynchronized,
		// Token: 0x04000308 RID: 776
		[EnumMember]
		[Description("장중")]
		Open,
		// Token: 0x04000309 RID: 777
		[EnumMember]
		[Description("일시정지")]
		Pause,
		// Token: 0x0400030A RID: 778
		[EnumMember]
		[Description("주문종료")]
		EndOrder,
		// Token: 0x0400030B RID: 779
		[EnumMember]
		[Description("장마감동시호가")]
		CloseSynchronized,
		// Token: 0x0400030C RID: 780
		[EnumMember]
		[Description("장마감")]
		Close,
		// Token: 0x0400030D RID: 781
		[EnumMember]
		[Description("거래정지")]
		Suspension
	}
}
