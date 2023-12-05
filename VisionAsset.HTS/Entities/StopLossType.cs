using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x0200007C RID: 124
	[DataContract]
	public enum StopLossType
	{
		// Token: 0x0400039D RID: 925
		[EnumMember]
		[Description("손절등록")]
		RegisterStopLoss,
		// Token: 0x0400039E RID: 926
		[EnumMember]
		[Description("익절등록")]
		RegisterTakeProfit,
		// Token: 0x0400039F RID: 927
		[EnumMember]
		[Description("손절해제")]
		CancelStopLoss,
		// Token: 0x040003A0 RID: 928
		[EnumMember]
		[Description("익절해제")]
		CancelTakeProfit,
		// Token: 0x040003A1 RID: 929
		[EnumMember]
		[Description("손절실행")]
		ExecuteStopLoss,
		// Token: 0x040003A2 RID: 930
		[EnumMember]
		[Description("익절실행")]
		ExecuteTakeProfit,
		// Token: 0x040003A3 RID: 931
		[EnumMember]
		[Description("평단변경")]
		ChangeAveragePrice,
		// Token: 0x040003A4 RID: 932
		[EnumMember]
		[Description("종목변경")]
		ChangeItem,
		// Token: 0x040003A5 RID: 933
		[EnumMember]
		[Description("계좌변경")]
		ChangeUserAccount,
		// Token: 0x040003A6 RID: 934
		[EnumMember]
		[Description("주문창종료")]
		CloseOrderView
	}
}
