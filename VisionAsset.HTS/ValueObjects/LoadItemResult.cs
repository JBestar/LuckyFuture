using System;
using System.Collections.Generic;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.ValueObjects
{
	// Token: 0x02000019 RID: 25
	public class LoadItemResult
	{
		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060001DA RID: 474 RVA: 0x0000E386 File Offset: 0x0000C586
		// (set) Token: 0x060001DB RID: 475 RVA: 0x0000E38E File Offset: 0x0000C58E
		public Quote Quote { get; set; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060001DC RID: 476 RVA: 0x0000E397 File Offset: 0x0000C597
		// (set) Token: 0x060001DD RID: 477 RVA: 0x0000E39F File Offset: 0x0000C59F
		public Current Current { get; set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060001DE RID: 478 RVA: 0x0000E3A8 File Offset: 0x0000C5A8
		// (set) Token: 0x060001DF RID: 479 RVA: 0x0000E3B0 File Offset: 0x0000C5B0
		public DayProfitLoss DayProfitLoss { get; set; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x0000E3B9 File Offset: 0x0000C5B9
		// (set) Token: 0x060001E1 RID: 481 RVA: 0x0000E3C1 File Offset: 0x0000C5C1
		public List<Order> Orders { get; set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x0000E3CA File Offset: 0x0000C5CA
		// (set) Token: 0x060001E3 RID: 483 RVA: 0x0000E3D2 File Offset: 0x0000C5D2
		public List<MitOrder> MitOrders { get; set; }
	}
}
