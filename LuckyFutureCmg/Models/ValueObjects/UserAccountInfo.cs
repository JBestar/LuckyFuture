using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyFuture.Models.ValueObjects
{
	public class UserAccountInfo
	{  
		public string UserAccountId { get; set; }
		public string UserAccountStr { get; set; }
		public double Balance { get; set; }
		public int Leverage { get; set; }

		public long TotalProfit { get; set; }
	}
}
