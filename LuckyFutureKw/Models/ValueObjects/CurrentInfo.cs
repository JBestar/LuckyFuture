using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyFuture.Models.ValueObjects
{
	// Token: 0x020000E2 RID: 226
	public class CurrentInfo
	{
		public DateTime Time { get; set; }
		public double CurrentPrice { get; set; }
		public int ConclusionQty { get; set; }
		public TRADETYPE TradeType { get; set; }
	}
}
