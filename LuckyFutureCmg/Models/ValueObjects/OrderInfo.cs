using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyFuture.Models.ValueObjects
{
	public class OrderInfo
	{
		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000DAD RID: 3501 RVA: 0x0004CB4A File Offset: 0x0004AD4A
		// (set) Token: 0x06000DAE RID: 3502 RVA: 0x0004CB52 File Offset: 0x0004AD52
		public string OrderType { get; set; }

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000DAF RID: 3503 RVA: 0x0004CB5B File Offset: 0x0004AD5B
		// (set) Token: 0x06000DB0 RID: 3504 RVA: 0x0004CB63 File Offset: 0x0004AD63
		public long ItemId { get; set; }

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000DB1 RID: 3505 RVA: 0x0004CB6C File Offset: 0x0004AD6C
		// (set) Token: 0x06000DB2 RID: 3506 RVA: 0x0004CB74 File Offset: 0x0004AD74
		public string Symbol { get; set; }

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000DB3 RID: 3507 RVA: 0x0004CB7D File Offset: 0x0004AD7D
		// (set) Token: 0x06000DB4 RID: 3508 RVA: 0x0004CB85 File Offset: 0x0004AD85
		public string CurrentPrice { get; set; }

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x0004CB8E File Offset: 0x0004AD8E
		// (set) Token: 0x06000DB6 RID: 3510 RVA: 0x0004CB96 File Offset: 0x0004AD96
		public string AveragePrice { get; set; }

		public double MaxAveragePrice { get; set; }
        public int LossPayoffTick { get; set; }
		public int EarnPayoffTick { get; set; }
		public double StartCciPrice { get; set; }
        public double MaxCciPrice { get; set; }
		public double CrossAveragePrice { get; set; }
        // Token: 0x170002A7 RID: 679
        // (get) Token: 0x06000DB7 RID: 3511 RVA: 0x0004CB9F File Offset: 0x0004AD9F
        // (set) Token: 0x06000DB8 RID: 3512 RVA: 0x0004CBA7 File Offset: 0x0004ADA7
        public string Qty { get; set; }

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x0004CBB0 File Offset: 0x0004ADB0
		// (set) Token: 0x06000DBA RID: 3514 RVA: 0x0004CBB8 File Offset: 0x0004ADB8
		public double Valuation { get; set; }

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000DBB RID: 3515 RVA: 0x0004CBC1 File Offset: 0x0004ADC1
		// (set) Token: 0x06000DBC RID: 3516 RVA: 0x0004CBC9 File Offset: 0x0004ADC9
		public string Action { get; set; }

		public int OrderTime { get; set; }
        public string OrderDate { get; set; }

        public string OrderNo { get; set; }
		public double OrderQty { get; set; }
		public TRADETYPE TradeType { get; set; }
        public string SymbolName { get; set; }
        public string TradeTypeNo { get; set; }
        public string OrderPrice { get; set; }
        public string StopPrice { get; set; }
        public string ErrorCode { get; set; }
        public string OrderMsg { get; set; }
        public string FCMCode { get; set; }
    }
}
