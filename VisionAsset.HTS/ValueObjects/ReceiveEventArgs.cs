using System;

namespace Goodbyte.TradingSystem.Domain.ValueObjects
{
	// Token: 0x0200001D RID: 29
	public class ReceiveEventArgs : EventArgs
	{
		// Token: 0x06000208 RID: 520 RVA: 0x0000E4EB File Offset: 0x0000C6EB
		public ReceiveEventArgs(object data)
		{
			this.Data = data;
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000209 RID: 521 RVA: 0x0000E4FA File Offset: 0x0000C6FA
		// (set) Token: 0x0600020A RID: 522 RVA: 0x0000E502 File Offset: 0x0000C702
		public object Data { get; set; }
	}
}
