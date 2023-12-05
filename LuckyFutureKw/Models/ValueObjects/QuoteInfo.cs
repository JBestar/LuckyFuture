using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyFuture.Models.ValueObjects
{
	public class QuoteInfo
	{
		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x0004C462 File Offset: 0x0004A662
		// (set) Token: 0x06000CD1 RID: 3281 RVA: 0x0004C46A File Offset: 0x0004A66A
		public int QuoteInfoId { get; set; }

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000CD2 RID: 3282 RVA: 0x0004C473 File Offset: 0x0004A673
		// (set) Token: 0x06000CD3 RID: 3283 RVA: 0x0004C47B File Offset: 0x0004A67B
		public int? SellMit { get; set; }

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000CD4 RID: 3284 RVA: 0x0004C484 File Offset: 0x0004A684
		// (set) Token: 0x06000CD5 RID: 3285 RVA: 0x0004C48C File Offset: 0x0004A68C
		public int? SellOrder { get; set; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x0004C495 File Offset: 0x0004A695
		// (set) Token: 0x06000CD7 RID: 3287 RVA: 0x0004C49D File Offset: 0x0004A69D
		public int? AskCount { get; set; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x0004C4A6 File Offset: 0x0004A6A6
		// (set) Token: 0x06000CD9 RID: 3289 RVA: 0x0004C4AE File Offset: 0x0004A6AE
		public int? AskQty { get; set; }

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x0004C4B7 File Offset: 0x0004A6B7
		// (set) Token: 0x06000CDB RID: 3291 RVA: 0x0004C4BF File Offset: 0x0004A6BF
		public string PriceSymbol { get; set; }

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x0004C4C8 File Offset: 0x0004A6C8
		// (set) Token: 0x06000CDD RID: 3293 RVA: 0x0004C4D0 File Offset: 0x0004A6D0
		public double Price { get; set; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000CDE RID: 3294 RVA: 0x0004C4D9 File Offset: 0x0004A6D9
		// (set) Token: 0x06000CDF RID: 3295 RVA: 0x0004C4E1 File Offset: 0x0004A6E1
		public int? BidQty { get; set; }

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x0004C4EA File Offset: 0x0004A6EA
		// (set) Token: 0x06000CE1 RID: 3297 RVA: 0x0004C4F2 File Offset: 0x0004A6F2
		public int? BidCount { get; set; }

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x0004C4FB File Offset: 0x0004A6FB
		// (set) Token: 0x06000CE3 RID: 3299 RVA: 0x0004C503 File Offset: 0x0004A703
		public int? BuyOrder { get; set; }

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x0004C50C File Offset: 0x0004A70C
		// (set) Token: 0x06000CE5 RID: 3301 RVA: 0x0004C514 File Offset: 0x0004A714
		public int? BuyMit { get; set; }

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x0004C51D File Offset: 0x0004A71D
		// (set) Token: 0x06000CE7 RID: 3303 RVA: 0x0004C525 File Offset: 0x0004A725
		public int UserAskCount { get; set; }

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x0004C52E File Offset: 0x0004A72E
		// (set) Token: 0x06000CE9 RID: 3305 RVA: 0x0004C536 File Offset: 0x0004A736
		public int UserAskQty { get; set; }

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000CEA RID: 3306 RVA: 0x0004C53F File Offset: 0x0004A73F
		// (set) Token: 0x06000CEB RID: 3307 RVA: 0x0004C547 File Offset: 0x0004A747
		public int UserBidQty { get; set; }

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000CEC RID: 3308 RVA: 0x0004C550 File Offset: 0x0004A750
		// (set) Token: 0x06000CED RID: 3309 RVA: 0x0004C558 File Offset: 0x0004A758
		public int UserBidCount { get; set; }
	}
}
