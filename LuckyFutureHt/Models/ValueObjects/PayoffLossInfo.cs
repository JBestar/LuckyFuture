using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyFuture.Models.ValueObjects
{
	public class PayoffLossInfo
	{
        public int Stage { get; set; }
        public string StageName { get; set; }
        public long Amount { get; set; }
        public int Rate { get; set; }
        public string Param { get; set; }
        public string AmountUnit { get; set; }
        public string RateUnit { get; set; }
        public int Enabled { get; set; }
        public string ActionDelete { get; set; }
    }
}
