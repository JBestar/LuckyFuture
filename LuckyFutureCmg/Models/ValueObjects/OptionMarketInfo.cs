using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyFuture.Models.ValueObjects
{
	public class OptionMarketInfo
	{
		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000D7B RID: 3451 RVA: 0x0004C9B2 File Offset: 0x0004ABB2
		// (set) Token: 0x06000D7C RID: 3452 RVA: 0x0004C9BA File Offset: 0x0004ABBA
		public int OptionMarketInfoId { get; set; }

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000D7D RID: 3453 RVA: 0x0004C9C3 File Offset: 0x0004ABC3
		// (set) Token: 0x06000D7E RID: 3454 RVA: 0x0004C9CB File Offset: 0x0004ABCB
		public long CallOptionItemId { get; set; }

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000D7F RID: 3455 RVA: 0x0004C9D4 File Offset: 0x0004ABD4
		// (set) Token: 0x06000D80 RID: 3456 RVA: 0x0004C9DC File Offset: 0x0004ABDC
		public string IsOpenCallOption { get; set; }

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000D81 RID: 3457 RVA: 0x0004C9E5 File Offset: 0x0004ABE5
		// (set) Token: 0x06000D82 RID: 3458 RVA: 0x0004C9ED File Offset: 0x0004ABED
		public double? Price { get; set; }

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000D83 RID: 3459 RVA: 0x0004C9F6 File Offset: 0x0004ABF6
		// (set) Token: 0x06000D84 RID: 3460 RVA: 0x0004C9FE File Offset: 0x0004ABFE
		public string IsOpenPutOption { get; set; }

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000D85 RID: 3461 RVA: 0x0004CA07 File Offset: 0x0004AC07
		// (set) Token: 0x06000D86 RID: 3462 RVA: 0x0004CA0F File Offset: 0x0004AC0F
		public long PutOptionItemId { get; set; }
	}
}
