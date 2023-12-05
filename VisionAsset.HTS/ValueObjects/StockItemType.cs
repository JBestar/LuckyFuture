using System;
using System.ComponentModel;

namespace Goodbyte.TradingSystem.Domain.ValueObjects
{
	// Token: 0x0200001F RID: 31
	public enum StockItemType
	{
		// Token: 0x040001A0 RID: 416
		[Description("코스피")]
		Kospi,
		// Token: 0x040001A1 RID: 417
		[Description("코스닥")]
		Kosdaq
	}
}
