using System;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x0200005B RID: 91
	[DataContract]
	public class Quote
	{
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x000161F2 File Offset: 0x000143F2
		// (set) Token: 0x0600041A RID: 1050 RVA: 0x000161FA File Offset: 0x000143FA
		[DataMember]
		public long QuoteId { get; set; }

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x00016203 File Offset: 0x00014403
		// (set) Token: 0x0600041C RID: 1052 RVA: 0x0001620B File Offset: 0x0001440B
		[DataMember]
		public string Symbol { get; set; }

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x00016214 File Offset: 0x00014414
		// (set) Token: 0x0600041E RID: 1054 RVA: 0x0001621C File Offset: 0x0001441C
		[DataMember]
		public double Ask1 { get; set; }

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x00016225 File Offset: 0x00014425
		// (set) Token: 0x06000420 RID: 1056 RVA: 0x0001622D File Offset: 0x0001442D
		[DataMember]
		public double Ask2 { get; set; }

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x00016236 File Offset: 0x00014436
		// (set) Token: 0x06000422 RID: 1058 RVA: 0x0001623E File Offset: 0x0001443E
		[DataMember]
		public double Ask3 { get; set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x00016247 File Offset: 0x00014447
		// (set) Token: 0x06000424 RID: 1060 RVA: 0x0001624F File Offset: 0x0001444F
		[DataMember]
		public double Ask4 { get; set; }

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x00016258 File Offset: 0x00014458
		// (set) Token: 0x06000426 RID: 1062 RVA: 0x00016260 File Offset: 0x00014460
		[DataMember]
		public double Ask5 { get; set; }

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x00016269 File Offset: 0x00014469
		// (set) Token: 0x06000428 RID: 1064 RVA: 0x00016271 File Offset: 0x00014471
		[DataMember]
		public double Ask6 { get; set; }

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x0001627A File Offset: 0x0001447A
		// (set) Token: 0x0600042A RID: 1066 RVA: 0x00016282 File Offset: 0x00014482
		[DataMember]
		public double Ask7 { get; set; }

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x0001628B File Offset: 0x0001448B
		// (set) Token: 0x0600042C RID: 1068 RVA: 0x00016293 File Offset: 0x00014493
		[DataMember]
		public double Ask8 { get; set; }

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x0001629C File Offset: 0x0001449C
		// (set) Token: 0x0600042E RID: 1070 RVA: 0x000162A4 File Offset: 0x000144A4
		[DataMember]
		public double Ask9 { get; set; }

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x000162AD File Offset: 0x000144AD
		// (set) Token: 0x06000430 RID: 1072 RVA: 0x000162B5 File Offset: 0x000144B5
		[DataMember]
		public double Ask10 { get; set; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x000162BE File Offset: 0x000144BE
		// (set) Token: 0x06000432 RID: 1074 RVA: 0x000162C6 File Offset: 0x000144C6
		[DataMember]
		public int AskQty1 { get; set; }

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x000162CF File Offset: 0x000144CF
		// (set) Token: 0x06000434 RID: 1076 RVA: 0x000162D7 File Offset: 0x000144D7
		[DataMember]
		public int AskQty2 { get; set; }

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x000162E0 File Offset: 0x000144E0
		// (set) Token: 0x06000436 RID: 1078 RVA: 0x000162E8 File Offset: 0x000144E8
		[DataMember]
		public int AskQty3 { get; set; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x000162F1 File Offset: 0x000144F1
		// (set) Token: 0x06000438 RID: 1080 RVA: 0x000162F9 File Offset: 0x000144F9
		[DataMember]
		public int AskQty4 { get; set; }

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x00016302 File Offset: 0x00014502
		// (set) Token: 0x0600043A RID: 1082 RVA: 0x0001630A File Offset: 0x0001450A
		[DataMember]
		public int AskQty5 { get; set; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x00016313 File Offset: 0x00014513
		// (set) Token: 0x0600043C RID: 1084 RVA: 0x0001631B File Offset: 0x0001451B
		[DataMember]
		public int AskQty6 { get; set; }

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x00016324 File Offset: 0x00014524
		// (set) Token: 0x0600043E RID: 1086 RVA: 0x0001632C File Offset: 0x0001452C
		[DataMember]
		public int AskQty7 { get; set; }

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x00016335 File Offset: 0x00014535
		// (set) Token: 0x06000440 RID: 1088 RVA: 0x0001633D File Offset: 0x0001453D
		[DataMember]
		public int AskQty8 { get; set; }

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x00016346 File Offset: 0x00014546
		// (set) Token: 0x06000442 RID: 1090 RVA: 0x0001634E File Offset: 0x0001454E
		[DataMember]
		public int AskQty9 { get; set; }

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x00016357 File Offset: 0x00014557
		// (set) Token: 0x06000444 RID: 1092 RVA: 0x0001635F File Offset: 0x0001455F
		[DataMember]
		public int AskQty10 { get; set; }

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x00016368 File Offset: 0x00014568
		// (set) Token: 0x06000446 RID: 1094 RVA: 0x00016370 File Offset: 0x00014570
		[DataMember]
		public int TotalAskQty { get; set; }

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000447 RID: 1095 RVA: 0x00016379 File Offset: 0x00014579
		// (set) Token: 0x06000448 RID: 1096 RVA: 0x00016381 File Offset: 0x00014581
		[DataMember]
		public int AskCount1 { get; set; }

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x0001638A File Offset: 0x0001458A
		// (set) Token: 0x0600044A RID: 1098 RVA: 0x00016392 File Offset: 0x00014592
		[DataMember]
		public int AskCount2 { get; set; }

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x0600044B RID: 1099 RVA: 0x0001639B File Offset: 0x0001459B
		// (set) Token: 0x0600044C RID: 1100 RVA: 0x000163A3 File Offset: 0x000145A3
		[DataMember]
		public int AskCount3 { get; set; }

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x000163AC File Offset: 0x000145AC
		// (set) Token: 0x0600044E RID: 1102 RVA: 0x000163B4 File Offset: 0x000145B4
		[DataMember]
		public int AskCount4 { get; set; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x000163BD File Offset: 0x000145BD
		// (set) Token: 0x06000450 RID: 1104 RVA: 0x000163C5 File Offset: 0x000145C5
		[DataMember]
		public int AskCount5 { get; set; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x000163CE File Offset: 0x000145CE
		// (set) Token: 0x06000452 RID: 1106 RVA: 0x000163D6 File Offset: 0x000145D6
		[DataMember]
		public int AskCount6 { get; set; }

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x000163DF File Offset: 0x000145DF
		// (set) Token: 0x06000454 RID: 1108 RVA: 0x000163E7 File Offset: 0x000145E7
		[DataMember]
		public int AskCount7 { get; set; }

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x000163F0 File Offset: 0x000145F0
		// (set) Token: 0x06000456 RID: 1110 RVA: 0x000163F8 File Offset: 0x000145F8
		[DataMember]
		public int AskCount8 { get; set; }

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x00016401 File Offset: 0x00014601
		// (set) Token: 0x06000458 RID: 1112 RVA: 0x00016409 File Offset: 0x00014609
		[DataMember]
		public int AskCount9 { get; set; }

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x00016412 File Offset: 0x00014612
		// (set) Token: 0x0600045A RID: 1114 RVA: 0x0001641A File Offset: 0x0001461A
		[DataMember]
		public int AskCount10 { get; set; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x00016423 File Offset: 0x00014623
		// (set) Token: 0x0600045C RID: 1116 RVA: 0x0001642B File Offset: 0x0001462B
		[DataMember]
		public int TotalAskCount { get; set; }

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x00016434 File Offset: 0x00014634
		// (set) Token: 0x0600045E RID: 1118 RVA: 0x0001643C File Offset: 0x0001463C
		[DataMember]
		public double Bid1 { get; set; }

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x00016445 File Offset: 0x00014645
		// (set) Token: 0x06000460 RID: 1120 RVA: 0x0001644D File Offset: 0x0001464D
		[DataMember]
		public double Bid2 { get; set; }

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x00016456 File Offset: 0x00014656
		// (set) Token: 0x06000462 RID: 1122 RVA: 0x0001645E File Offset: 0x0001465E
		[DataMember]
		public double Bid3 { get; set; }

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x00016467 File Offset: 0x00014667
		// (set) Token: 0x06000464 RID: 1124 RVA: 0x0001646F File Offset: 0x0001466F
		[DataMember]
		public double Bid4 { get; set; }

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x00016478 File Offset: 0x00014678
		// (set) Token: 0x06000466 RID: 1126 RVA: 0x00016480 File Offset: 0x00014680
		[DataMember]
		public double Bid5 { get; set; }

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x00016489 File Offset: 0x00014689
		// (set) Token: 0x06000468 RID: 1128 RVA: 0x00016491 File Offset: 0x00014691
		[DataMember]
		public double Bid6 { get; set; }

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x0001649A File Offset: 0x0001469A
		// (set) Token: 0x0600046A RID: 1130 RVA: 0x000164A2 File Offset: 0x000146A2
		[DataMember]
		public double Bid7 { get; set; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x000164AB File Offset: 0x000146AB
		// (set) Token: 0x0600046C RID: 1132 RVA: 0x000164B3 File Offset: 0x000146B3
		[DataMember]
		public double Bid8 { get; set; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600046D RID: 1133 RVA: 0x000164BC File Offset: 0x000146BC
		// (set) Token: 0x0600046E RID: 1134 RVA: 0x000164C4 File Offset: 0x000146C4
		[DataMember]
		public double Bid9 { get; set; }

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600046F RID: 1135 RVA: 0x000164CD File Offset: 0x000146CD
		// (set) Token: 0x06000470 RID: 1136 RVA: 0x000164D5 File Offset: 0x000146D5
		[DataMember]
		public double Bid10 { get; set; }

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000471 RID: 1137 RVA: 0x000164DE File Offset: 0x000146DE
		// (set) Token: 0x06000472 RID: 1138 RVA: 0x000164E6 File Offset: 0x000146E6
		[DataMember]
		public int BidQty1 { get; set; }

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000473 RID: 1139 RVA: 0x000164EF File Offset: 0x000146EF
		// (set) Token: 0x06000474 RID: 1140 RVA: 0x000164F7 File Offset: 0x000146F7
		[DataMember]
		public int BidQty2 { get; set; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x00016500 File Offset: 0x00014700
		// (set) Token: 0x06000476 RID: 1142 RVA: 0x00016508 File Offset: 0x00014708
		[DataMember]
		public int BidQty3 { get; set; }

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x00016511 File Offset: 0x00014711
		// (set) Token: 0x06000478 RID: 1144 RVA: 0x00016519 File Offset: 0x00014719
		[DataMember]
		public int BidQty4 { get; set; }

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x00016522 File Offset: 0x00014722
		// (set) Token: 0x0600047A RID: 1146 RVA: 0x0001652A File Offset: 0x0001472A
		[DataMember]
		public int BidQty5 { get; set; }

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x00016533 File Offset: 0x00014733
		// (set) Token: 0x0600047C RID: 1148 RVA: 0x0001653B File Offset: 0x0001473B
		[DataMember]
		public int BidQty6 { get; set; }

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x00016544 File Offset: 0x00014744
		// (set) Token: 0x0600047E RID: 1150 RVA: 0x0001654C File Offset: 0x0001474C
		[DataMember]
		public int BidQty7 { get; set; }

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x00016555 File Offset: 0x00014755
		// (set) Token: 0x06000480 RID: 1152 RVA: 0x0001655D File Offset: 0x0001475D
		[DataMember]
		public int BidQty8 { get; set; }

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x00016566 File Offset: 0x00014766
		// (set) Token: 0x06000482 RID: 1154 RVA: 0x0001656E File Offset: 0x0001476E
		[DataMember]
		public int BidQty9 { get; set; }

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x00016577 File Offset: 0x00014777
		// (set) Token: 0x06000484 RID: 1156 RVA: 0x0001657F File Offset: 0x0001477F
		[DataMember]
		public int BidQty10 { get; set; }

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x00016588 File Offset: 0x00014788
		// (set) Token: 0x06000486 RID: 1158 RVA: 0x00016590 File Offset: 0x00014790
		[DataMember]
		public int TotalBidQty { get; set; }

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000487 RID: 1159 RVA: 0x00016599 File Offset: 0x00014799
		// (set) Token: 0x06000488 RID: 1160 RVA: 0x000165A1 File Offset: 0x000147A1
		[DataMember]
		public int BidCount1 { get; set; }

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x000165AA File Offset: 0x000147AA
		// (set) Token: 0x0600048A RID: 1162 RVA: 0x000165B2 File Offset: 0x000147B2
		[DataMember]
		public int BidCount2 { get; set; }

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x000165BB File Offset: 0x000147BB
		// (set) Token: 0x0600048C RID: 1164 RVA: 0x000165C3 File Offset: 0x000147C3
		[DataMember]
		public int BidCount3 { get; set; }

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x000165CC File Offset: 0x000147CC
		// (set) Token: 0x0600048E RID: 1166 RVA: 0x000165D4 File Offset: 0x000147D4
		[DataMember]
		public int BidCount4 { get; set; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x000165DD File Offset: 0x000147DD
		// (set) Token: 0x06000490 RID: 1168 RVA: 0x000165E5 File Offset: 0x000147E5
		[DataMember]
		public int BidCount5 { get; set; }

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x000165EE File Offset: 0x000147EE
		// (set) Token: 0x06000492 RID: 1170 RVA: 0x000165F6 File Offset: 0x000147F6
		[DataMember]
		public int BidCount6 { get; set; }

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x000165FF File Offset: 0x000147FF
		// (set) Token: 0x06000494 RID: 1172 RVA: 0x00016607 File Offset: 0x00014807
		[DataMember]
		public int BidCount7 { get; set; }

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x00016610 File Offset: 0x00014810
		// (set) Token: 0x06000496 RID: 1174 RVA: 0x00016618 File Offset: 0x00014818
		[DataMember]
		public int BidCount8 { get; set; }

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x00016621 File Offset: 0x00014821
		// (set) Token: 0x06000498 RID: 1176 RVA: 0x00016629 File Offset: 0x00014829
		[DataMember]
		public int BidCount9 { get; set; }

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x00016632 File Offset: 0x00014832
		// (set) Token: 0x0600049A RID: 1178 RVA: 0x0001663A File Offset: 0x0001483A
		[DataMember]
		public int BidCount10 { get; set; }

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x00016643 File Offset: 0x00014843
		// (set) Token: 0x0600049C RID: 1180 RVA: 0x0001664B File Offset: 0x0001484B
		[DataMember]
		public int TotalBidCount { get; set; }

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x00016654 File Offset: 0x00014854
		// (set) Token: 0x0600049E RID: 1182 RVA: 0x0001665C File Offset: 0x0001485C
		[DataMember]
		public string QuoteTime { get; set; }

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x00016665 File Offset: 0x00014865
		// (set) Token: 0x060004A0 RID: 1184 RVA: 0x0001666D File Offset: 0x0001486D
		[DataMember]
		public DateTime ReceivedDate { get; set; }

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x00016676 File Offset: 0x00014876
		// (set) Token: 0x060004A2 RID: 1186 RVA: 0x0001667E File Offset: 0x0001487E
		[DataMember]
		public long ItemId { get; set; }

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x00016687 File Offset: 0x00014887
		// (set) Token: 0x060004A4 RID: 1188 RVA: 0x0001668F File Offset: 0x0001488F
		[DataMember]
		public virtual Item Item { get; set; }
	}
}
