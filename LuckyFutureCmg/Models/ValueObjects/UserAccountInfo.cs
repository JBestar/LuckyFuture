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
		/// <summary>Prime(MtApi) 계정명 표시용</summary>
		public string AccountName { get; set; }
		public double Balance { get; set; }
		public int Leverage { get; set; }

		public long TotalProfit { get; set; }
	}
}
