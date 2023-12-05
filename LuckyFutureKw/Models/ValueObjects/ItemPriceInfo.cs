using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyFuture.Models.ValueObjects
{
	public class ItemPriceInfo
	{
		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000D42 RID: 3394 RVA: 0x0004C7F8 File Offset: 0x0004A9F8
		// (set) Token: 0x06000D43 RID: 3395 RVA: 0x0004C800 File Offset: 0x0004AA00
		public string Title1 { get; set; }

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000D44 RID: 3396 RVA: 0x0004C809 File Offset: 0x0004AA09
		// (set) Token: 0x06000D45 RID: 3397 RVA: 0x0004C811 File Offset: 0x0004AA11
		public double CurrentPrice { get; set; }

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000D46 RID: 3398 RVA: 0x0004C81A File Offset: 0x0004AA1A
		// (set) Token: 0x06000D47 RID: 3399 RVA: 0x0004C822 File Offset: 0x0004AA22
		public double Contrast { get; set; }

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000D48 RID: 3400 RVA: 0x0004C82B File Offset: 0x0004AA2B
		// (set) Token: 0x06000D49 RID: 3401 RVA: 0x0004C833 File Offset: 0x0004AA33
		public double ContrastPer { get; set; }

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000D4A RID: 3402 RVA: 0x0004C83C File Offset: 0x0004AA3C
		// (set) Token: 0x06000D4B RID: 3403 RVA: 0x0004C844 File Offset: 0x0004AA44
		public string Title2 { get; set; }

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000D4C RID: 3404 RVA: 0x0004C84D File Offset: 0x0004AA4D
		// (set) Token: 0x06000D4D RID: 3405 RVA: 0x0004C855 File Offset: 0x0004AA55
		public double StartPrice { get; set; }

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000D4E RID: 3406 RVA: 0x0004C85E File Offset: 0x0004AA5E
		// (set) Token: 0x06000D4F RID: 3407 RVA: 0x0004C866 File Offset: 0x0004AA66
		public double HighPrice { get; set; }

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000D50 RID: 3408 RVA: 0x0004C86F File Offset: 0x0004AA6F
		// (set) Token: 0x06000D51 RID: 3409 RVA: 0x0004C877 File Offset: 0x0004AA77
		public double LowPrice { get; set; }
	}
}
