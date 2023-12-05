using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x02000069 RID: 105
	[DataContract]
	public class Item
	{
		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x00016EF6 File Offset: 0x000150F6
		// (set) Token: 0x060005AB RID: 1451 RVA: 0x00016EFE File Offset: 0x000150FE
		[DataMember]
		public long ItemId { get; set; }

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x00016F07 File Offset: 0x00015107
		// (set) Token: 0x060005AD RID: 1453 RVA: 0x00016F0F File Offset: 0x0001510F
		[DataMember]
		public string Symbol { get; set; }

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x00016F18 File Offset: 0x00015118
		// (set) Token: 0x060005AF RID: 1455 RVA: 0x00016F20 File Offset: 0x00015120
		[DataMember]
		public string ItemName { get; set; }

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x00016F29 File Offset: 0x00015129
		// (set) Token: 0x060005B1 RID: 1457 RVA: 0x00016F31 File Offset: 0x00015131
		[DataMember]
		public ItemType ItemType { get; set; }

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x060005B2 RID: 1458 RVA: 0x00016F3A File Offset: 0x0001513A
		// (set) Token: 0x060005B3 RID: 1459 RVA: 0x00016F42 File Offset: 0x00015142
		[DataMember]
		public CurrencyType CurrencyType { get; set; }

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x00016F4B File Offset: 0x0001514B
		// (set) Token: 0x060005B5 RID: 1461 RVA: 0x00016F53 File Offset: 0x00015153
		[DataMember]
		public double Tax { get; set; }

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x060005B6 RID: 1462 RVA: 0x00016F5C File Offset: 0x0001515C
		// (set) Token: 0x060005B7 RID: 1463 RVA: 0x00016F64 File Offset: 0x00015164
		[DataMember]
		public double ParentCommission { get; set; }

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x00016F6D File Offset: 0x0001516D
		// (set) Token: 0x060005B9 RID: 1465 RVA: 0x00016F75 File Offset: 0x00015175
		[DataMember]
		public int Leverage { get; set; }

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x00016F7E File Offset: 0x0001517E
		// (set) Token: 0x060005BB RID: 1467 RVA: 0x00016F86 File Offset: 0x00015186
		[DataMember]
		public int PricePrecision { get; set; }

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x00016F8F File Offset: 0x0001518F
		// (set) Token: 0x060005BD RID: 1469 RVA: 0x00016F97 File Offset: 0x00015197
		[DataMember]
		public int AveragePricePrecision { get; set; }

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x00016FA0 File Offset: 0x000151A0
		// (set) Token: 0x060005BF RID: 1471 RVA: 0x00016FA8 File Offset: 0x000151A8
		[DataMember]
		public double ReferencePoint { get; set; }

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x00016FB1 File Offset: 0x000151B1
		// (set) Token: 0x060005C1 RID: 1473 RVA: 0x00016FB9 File Offset: 0x000151B9
		[DataMember]
		public double OverTick { get; set; }

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x00016FC2 File Offset: 0x000151C2
		// (set) Token: 0x060005C3 RID: 1475 RVA: 0x00016FCA File Offset: 0x000151CA
		[DataMember]
		public double UnderTick { get; set; }

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x00016FD3 File Offset: 0x000151D3
		// (set) Token: 0x060005C5 RID: 1477 RVA: 0x00016FDB File Offset: 0x000151DB
		[DataMember]
		public double OverTickValue { get; set; }

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x00016FE4 File Offset: 0x000151E4
		// (set) Token: 0x060005C7 RID: 1479 RVA: 0x00016FEC File Offset: 0x000151EC
		[DataMember]
		public double UnderTickValue { get; set; }

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x00016FF5 File Offset: 0x000151F5
		// (set) Token: 0x060005C9 RID: 1481 RVA: 0x00016FFD File Offset: 0x000151FD
		[DataMember]
		public DateTime StartTradingDay { get; set; }

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x060005CA RID: 1482 RVA: 0x00017006 File Offset: 0x00015206
		// (set) Token: 0x060005CB RID: 1483 RVA: 0x0001700E File Offset: 0x0001520E
		[DataMember]
		public DateTime LastTradingDay { get; set; }

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x060005CC RID: 1484 RVA: 0x00017017 File Offset: 0x00015217
		// (set) Token: 0x060005CD RID: 1485 RVA: 0x0001701F File Offset: 0x0001521F
		[DataMember]
		public long SellMargin { get; set; }

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x00017028 File Offset: 0x00015228
		// (set) Token: 0x060005CF RID: 1487 RVA: 0x00017030 File Offset: 0x00015230
		[DataMember]
		public long BuyMargin { get; set; }

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x00017039 File Offset: 0x00015239
		// (set) Token: 0x060005D1 RID: 1489 RVA: 0x00017041 File Offset: 0x00015241
		[DataMember]
		public long SellMinimumMargin { get; set; }

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x0001704A File Offset: 0x0001524A
		// (set) Token: 0x060005D3 RID: 1491 RVA: 0x00017052 File Offset: 0x00015252
		[DataMember]
		public long BuyMinimumMargin { get; set; }

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x060005D4 RID: 1492 RVA: 0x0001705B File Offset: 0x0001525B
		// (set) Token: 0x060005D5 RID: 1493 RVA: 0x00017063 File Offset: 0x00015263
		[DataMember]
		public long SellLossCutMargin { get; set; }

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x060005D6 RID: 1494 RVA: 0x0001706C File Offset: 0x0001526C
		// (set) Token: 0x060005D7 RID: 1495 RVA: 0x00017074 File Offset: 0x00015274
		[DataMember]
		public long BuyLossCutMargin { get; set; }

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0001707D File Offset: 0x0001527D
		// (set) Token: 0x060005D9 RID: 1497 RVA: 0x00017085 File Offset: 0x00015285
		[DataMember]
		public long SellOvernightMinimumMargin { get; set; }

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x0001708E File Offset: 0x0001528E
		// (set) Token: 0x060005DB RID: 1499 RVA: 0x00017096 File Offset: 0x00015296
		[DataMember]
		public long BuyOvernightMinimumMargin { get; set; }

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x0001709F File Offset: 0x0001529F
		// (set) Token: 0x060005DD RID: 1501 RVA: 0x000170A7 File Offset: 0x000152A7
		[DataMember]
		public double OrderUpLimit { get; set; }

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x060005DE RID: 1502 RVA: 0x000170B0 File Offset: 0x000152B0
		// (set) Token: 0x060005DF RID: 1503 RVA: 0x000170B8 File Offset: 0x000152B8
		[DataMember]
		public double OrderDownLimit { get; set; }

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x060005E0 RID: 1504 RVA: 0x000170C1 File Offset: 0x000152C1
		// (set) Token: 0x060005E1 RID: 1505 RVA: 0x000170C9 File Offset: 0x000152C9
		[DataMember]
		public int ConclusionSpeed { get; set; }

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x000170D2 File Offset: 0x000152D2
		// (set) Token: 0x060005E3 RID: 1507 RVA: 0x000170DA File Offset: 0x000152DA
		[DataMember]
		public TimeSpan CancelOrderTime { get; set; }

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x000170E3 File Offset: 0x000152E3
		// (set) Token: 0x060005E5 RID: 1509 RVA: 0x000170EB File Offset: 0x000152EB
		[DataMember]
		public int CancelOrderTick { get; set; }

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x000170F4 File Offset: 0x000152F4
		// (set) Token: 0x060005E7 RID: 1511 RVA: 0x000170FC File Offset: 0x000152FC
		[DataMember]
		public bool IsBlankQuoteOrder { get; set; }

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x060005E8 RID: 1512 RVA: 0x00017105 File Offset: 0x00015305
		// (set) Token: 0x060005E9 RID: 1513 RVA: 0x0001710D File Offset: 0x0001530D
		[DataMember]
		public bool IsBlankQuoteConslusion { get; set; }

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x060005EA RID: 1514 RVA: 0x00017116 File Offset: 0x00015316
		// (set) Token: 0x060005EB RID: 1515 RVA: 0x0001711E File Offset: 0x0001531E
		[DataMember]
		public bool IsOnlyLimitPrice { get; set; }

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x060005EC RID: 1516 RVA: 0x00017127 File Offset: 0x00015327
		// (set) Token: 0x060005ED RID: 1517 RVA: 0x0001712F File Offset: 0x0001532F
		[DataMember]
		public int SortIndex { get; set; }

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x060005EE RID: 1518 RVA: 0x00017138 File Offset: 0x00015338
		// (set) Token: 0x060005EF RID: 1519 RVA: 0x00017140 File Offset: 0x00015340
		[DataMember]
		public bool IsUse { get; set; }

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x060005F0 RID: 1520 RVA: 0x00017149 File Offset: 0x00015349
		// (set) Token: 0x060005F1 RID: 1521 RVA: 0x00017151 File Offset: 0x00015351
		[DataMember]
		public virtual ICollection<Market> Markets { get; set; }

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x060005F2 RID: 1522 RVA: 0x0001715A File Offset: 0x0001535A
		// (set) Token: 0x060005F3 RID: 1523 RVA: 0x00017162 File Offset: 0x00015362
		[DataMember]
		public virtual ICollection<Quote> Quotes { get; set; }

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x060005F4 RID: 1524 RVA: 0x0001716B File Offset: 0x0001536B
		// (set) Token: 0x060005F5 RID: 1525 RVA: 0x00017173 File Offset: 0x00015373
		[DataMember]
		public virtual ICollection<Current> Currents { get; set; }
	}
}
