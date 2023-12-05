using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x02000072 RID: 114
	[DataContract]
	public enum OrderRouteType
	{
		// Token: 0x04000347 RID: 839
		[EnumMember]
		[Description("호가창")]
		QuoteInfoGrid,
		// Token: 0x04000348 RID: 840
		[EnumMember]
		[Description("청산창")]
		OrderInfoGrid,
		// Token: 0x04000349 RID: 841
		[EnumMember]
		[Description("시장가버튼")]
		MarketPriceButton,
		// Token: 0x0400034A RID: 842
		[EnumMember]
		[Description("취소버튼")]
		CancelButton,
		// Token: 0x0400034B RID: 843
		[EnumMember]
		[Description("현종목취소")]
		CancelCurrentButton,
		// Token: 0x0400034C RID: 844
		[EnumMember]
		[Description("현종목청산")]
		ClearCurrentButton,
		// Token: 0x0400034D RID: 845
		[EnumMember]
		[Description("전종목취소")]
		CancelAllButton,
		// Token: 0x0400034E RID: 846
		[EnumMember]
		[Description("전종목청산")]
		ClearAllButton,
		// Token: 0x0400034F RID: 847
		[EnumMember]
		[Description("MIT")]
		Mit,
		// Token: 0x04000350 RID: 848
		[EnumMember]
		[Description("스탑로스")]
		StopLoss,
		// Token: 0x04000351 RID: 849
		[EnumMember]
		[Description("로스컷")]
		LossCut,
		// Token: 0x04000352 RID: 850
		[EnumMember]
		[Description("장마감")]
		EndOrder,
		// Token: 0x04000353 RID: 851
		[EnumMember]
		[Description("이월")]
		Overnight,
		// Token: 0x04000354 RID: 852
		[EnumMember]
		[Description("관리자")]
		Administrator,
		// Token: 0x04000355 RID: 853
		[EnumMember]
		[Description("시스템")]
		System,
		// Token: 0x04000356 RID: 854
		[EnumMember]
		[Description("이월대기")]
		AwaitOvernight
	}
}
