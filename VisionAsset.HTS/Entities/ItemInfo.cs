using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
    // Token: 0x0200006B RID: 107
    [DataContract]
    public class ItemInfo
    {
        public string ItemName { get; set; }
        public long ItemId { get; set; }
        public string Symbol { get; set; }
        public int Precision { get; set; }
    }
}
