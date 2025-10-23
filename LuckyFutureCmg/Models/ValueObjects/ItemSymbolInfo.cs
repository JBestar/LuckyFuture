using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyFuture.Models.ValueObjects
{
    // Token: 0x020000E2 RID: 226
    public class ItemSymbolInfo
    {
        public string ItemName { get; set; }
        public long ItemId { get; set; }
        public string Symbol { get; set; }
        public string PrdName { get; set; }
        public double OverTick { get; set; }
        public double ValueTick { get; set; }
        public double Exchange { get; set; }
        public int Precision { get; set; }
        public double MidPrice { get; set; }
        public int index { get; set; }
        public double MinVolume { get; set; }
        public double MaxVolume { get; set; }
        public double VolumeStep { get; set; }
    }
}
