using System;
using System.ComponentModel;

namespace Goodbyte.TradingSystem.Domain.ValueObjects
{
	// Token: 0x02000018 RID: 24
	public enum ConclusionItemType
	{
		// Token: 0x0400017F RID: 383
		[Description("선물옵션")]
		FuturesOptions,
		// Token: 0x04000180 RID: 384
		[Description("야간선물")]
		Cme,
		// Token: 0x04000181 RID: 385
		[Description("야간옵션")]
		Eurex,
		// Token: 0x04000182 RID: 386
		[Description("해외선물")]
		Foreign,
		// Token: 0x04000183 RID: 387
		[Description("주식")]
		Stock
	}
}
