using System;
using System.Diagnostics;

namespace Goodbyte.TradingSystem.Domain.Common
{
	// Token: 0x02000085 RID: 133
	public static class TraceEx
	{
		// Token: 0x06000799 RID: 1945 RVA: 0x00019348 File Offset: 0x00017548
		public static void TraceException(Exception ex)
		{
			Trace.TraceError("\n<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<\n{0}\n{1}\n>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>", new object[]
			{
				ex.Message,
				ex.StackTrace
			});
		}
	}
}
