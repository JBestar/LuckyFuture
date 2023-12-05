using System;

namespace Goodbyte.TradingSystem.Domain.ValueObjects
{
	// Token: 0x0200001E RID: 30
	public class StockItem
	{
		// Token: 0x17000194 RID: 404
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0000E50B File Offset: 0x0000C70B
		// (set) Token: 0x0600020C RID: 524 RVA: 0x0000E513 File Offset: 0x0000C713
		public long StockItemId { get; set; }

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000E51C File Offset: 0x0000C71C
		// (set) Token: 0x0600020E RID: 526 RVA: 0x0000E524 File Offset: 0x0000C724
		public string Symbol { get; set; }

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x0600020F RID: 527 RVA: 0x0000E52D File Offset: 0x0000C72D
		// (set) Token: 0x06000210 RID: 528 RVA: 0x0000E535 File Offset: 0x0000C735
		public string ItemName { get; set; }

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000E53E File Offset: 0x0000C73E
		// (set) Token: 0x06000212 RID: 530 RVA: 0x0000E546 File Offset: 0x0000C746
		public StockItemType StockItemType { get; set; }

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0000E54F File Offset: 0x0000C74F
		// (set) Token: 0x06000214 RID: 532 RVA: 0x0000E557 File Offset: 0x0000C757
		public double CurrentPrice { get; set; }
	}
}
