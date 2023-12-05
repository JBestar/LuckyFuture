using System;
using System.Collections.Generic;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.ValueObjects
{
	// Token: 0x0200001B RID: 27
	public class MitOrderResult
	{
		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000E474 File Offset: 0x0000C674
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x0000E47C File Offset: 0x0000C67C
		public MitOrder MitOrder { get; set; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000E485 File Offset: 0x0000C685
		// (set) Token: 0x060001FB RID: 507 RVA: 0x0000E48D File Offset: 0x0000C68D
		public List<UserAccount> UserAccounts { get; set; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000E496 File Offset: 0x0000C696
		// (set) Token: 0x060001FD RID: 509 RVA: 0x0000E49E File Offset: 0x0000C69E
		public List<MitOrder> NotProcessedMitOrders { get; set; }
	}
}
