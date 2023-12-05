using System;
using System.Collections.Generic;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.ValueObjects
{
	// Token: 0x0200001C RID: 28
	public class OrderResult
	{
		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060001FF RID: 511 RVA: 0x0000E4A7 File Offset: 0x0000C6A7
		// (set) Token: 0x06000200 RID: 512 RVA: 0x0000E4AF File Offset: 0x0000C6AF
		public Order Order { get; set; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000201 RID: 513 RVA: 0x0000E4B8 File Offset: 0x0000C6B8
		// (set) Token: 0x06000202 RID: 514 RVA: 0x0000E4C0 File Offset: 0x0000C6C0
		public DayProfitLoss DayProfitLoss { get; set; }

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000203 RID: 515 RVA: 0x0000E4C9 File Offset: 0x0000C6C9
		// (set) Token: 0x06000204 RID: 516 RVA: 0x0000E4D1 File Offset: 0x0000C6D1
		public List<UserAccount> UserAccounts { get; set; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000205 RID: 517 RVA: 0x0000E4DA File Offset: 0x0000C6DA
		// (set) Token: 0x06000206 RID: 518 RVA: 0x0000E4E2 File Offset: 0x0000C6E2
		public List<Order> UnclearOrders { get; set; }
	}
}
