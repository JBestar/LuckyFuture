using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyFuture.Models.ValueObjects
{
	public enum TRADETYPE
	{
		NONE,
		SELL,
		BUY,
		BOTH,
	}
    public class SignalInfo
    {
        public TRADETYPE TradeType { get; set; }
        public long UpdatedTick { get; set; }
        public bool Enabled { get; set; }
    }

}
