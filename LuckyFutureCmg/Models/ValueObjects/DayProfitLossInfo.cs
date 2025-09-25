using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyFuture.Models.ValueObjects
{
	public class DayProfitLossInfo
	{
		public long DayProfitLossId { get; set; }
		public DateTime MarketDate { get; set; }
		public long OpenBalance { get; set; }
		public long TotalDeposit { get; set; }
		public long TotalWithdraw { get; set; }
		public long CmeRealProfit { get; set; }
		public long CmeRealCommission { get; set; }
		public long CmeParentCommission { get; set; }
		public long ForeignRealProfit { get; set; }
		public long ForeignRealCommission { get; set; }
		public long ForeignParentCommission { get; set; }
		public long ForeignVirtualProfit { get; set; }
		public long ForeignVirtualCommission { get; set; }
		public long ForeignVirtualParentCommission { get; set; }
		public long TotalRealProfit { get; set; }
		public long TotalRealCommission { get; set; }
		public long TotalTax { get; set; }
		public long TotalParentCommission { get; set; }
		public long TotalVirtualProfit { get; set; }
		public long TotalVirtualCommission { get; set; }
		public long TotalVirtualTax { get; set; }
		public long TotalVirtualParentCommission { get; set; }
		public long TotalProfit { get; set; }
		public long TotalCommission { get; set; }
		public long CloseBalance { get; set; }
		public long UserAccountId { get; set; }
	}
}
