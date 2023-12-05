using System;
using System.Collections.Generic;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.ValueObjects
{
	// Token: 0x0200001A RID: 26
	public class LoadSpeedOrderViewResult
	{
		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000E3DB File Offset: 0x0000C5DB
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x0000E3E3 File Offset: 0x0000C5E3
		public List<Currency> Currencies { get; set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000E3EC File Offset: 0x0000C5EC
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x0000E3F4 File Offset: 0x0000C5F4
		public Company Company { get; set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000E3FD File Offset: 0x0000C5FD
		// (set) Token: 0x060001EA RID: 490 RVA: 0x0000E405 File Offset: 0x0000C605
		public List<Item> Items { get; set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000E40E File Offset: 0x0000C60E
		// (set) Token: 0x060001EC RID: 492 RVA: 0x0000E416 File Offset: 0x0000C616
		public double OptionAtm { get; set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000E41F File Offset: 0x0000C61F
		// (set) Token: 0x060001EE RID: 494 RVA: 0x0000E427 File Offset: 0x0000C627
		public List<Market> Markets { get; set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060001EF RID: 495 RVA: 0x0000E430 File Offset: 0x0000C630
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x0000E438 File Offset: 0x0000C638
		public Dictionary<long, Current> Currents { get; set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000E441 File Offset: 0x0000C641
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x0000E449 File Offset: 0x0000C649
		public User User { get; set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x0000E452 File Offset: 0x0000C652
		// (set) Token: 0x060001F4 RID: 500 RVA: 0x0000E45A File Offset: 0x0000C65A
		public List<UserAccount> UserAccounts { get; set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000E463 File Offset: 0x0000C663
		// (set) Token: 0x060001F6 RID: 502 RVA: 0x0000E46B File Offset: 0x0000C66B
		public List<UserAccountSpecific> UserAccountSpecifics { get; set; }
	}
}
