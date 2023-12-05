using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x02000064 RID: 100
	[DataContract]
	public class DayProfitLoss
	{
		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x000169EA File Offset: 0x00014BEA
		// (set) Token: 0x06000511 RID: 1297 RVA: 0x000169F2 File Offset: 0x00014BF2
		[DataMember]
		public long DayProfitLossId { get; set; }

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x000169FB File Offset: 0x00014BFB
		// (set) Token: 0x06000513 RID: 1299 RVA: 0x00016A03 File Offset: 0x00014C03
		[Index]
		[DataMember]
		public DateTime MarketDate { get; set; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x00016A0C File Offset: 0x00014C0C
		// (set) Token: 0x06000515 RID: 1301 RVA: 0x00016A14 File Offset: 0x00014C14
		[DataMember]
		public long OpenBalance { get; set; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x00016A1D File Offset: 0x00014C1D
		// (set) Token: 0x06000517 RID: 1303 RVA: 0x00016A25 File Offset: 0x00014C25
		[DataMember]
		public long TotalDeposit { get; set; }

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x00016A2E File Offset: 0x00014C2E
		// (set) Token: 0x06000519 RID: 1305 RVA: 0x00016A36 File Offset: 0x00014C36
		[DataMember]
		public long TotalWithdraw { get; set; }

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x00016A3F File Offset: 0x00014C3F
		// (set) Token: 0x0600051B RID: 1307 RVA: 0x00016A47 File Offset: 0x00014C47
		[DataMember]
		public long FuturesRealProfit { get; set; }

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x00016A50 File Offset: 0x00014C50
		// (set) Token: 0x0600051D RID: 1309 RVA: 0x00016A58 File Offset: 0x00014C58
		[DataMember]
		public long FuturesRealCommission { get; set; }

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x00016A61 File Offset: 0x00014C61
		// (set) Token: 0x0600051F RID: 1311 RVA: 0x00016A69 File Offset: 0x00014C69
		[DataMember]
		public long FuturesParentCommission { get; set; }

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x00016A72 File Offset: 0x00014C72
		// (set) Token: 0x06000521 RID: 1313 RVA: 0x00016A7A File Offset: 0x00014C7A
		[DataMember]
		public long OptionRealProfit { get; set; }

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x00016A83 File Offset: 0x00014C83
		// (set) Token: 0x06000523 RID: 1315 RVA: 0x00016A8B File Offset: 0x00014C8B
		[DataMember]
		public long OptionRealCommission { get; set; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x00016A94 File Offset: 0x00014C94
		// (set) Token: 0x06000525 RID: 1317 RVA: 0x00016A9C File Offset: 0x00014C9C
		[DataMember]
		public long OptionParentCommission { get; set; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x00016AA5 File Offset: 0x00014CA5
		// (set) Token: 0x06000527 RID: 1319 RVA: 0x00016AAD File Offset: 0x00014CAD
		[DataMember]
		public long CmeRealProfit { get; set; }

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x00016AB6 File Offset: 0x00014CB6
		// (set) Token: 0x06000529 RID: 1321 RVA: 0x00016ABE File Offset: 0x00014CBE
		[DataMember]
		public long CmeRealCommission { get; set; }

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x00016AC7 File Offset: 0x00014CC7
		// (set) Token: 0x0600052B RID: 1323 RVA: 0x00016ACF File Offset: 0x00014CCF
		[DataMember]
		public long CmeParentCommission { get; set; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x00016AD8 File Offset: 0x00014CD8
		// (set) Token: 0x0600052D RID: 1325 RVA: 0x00016AE0 File Offset: 0x00014CE0
		[DataMember]
		public long EurexRealProfit { get; set; }

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x00016AE9 File Offset: 0x00014CE9
		// (set) Token: 0x0600052F RID: 1327 RVA: 0x00016AF1 File Offset: 0x00014CF1
		[DataMember]
		public long EurexRealCommission { get; set; }

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x00016AFA File Offset: 0x00014CFA
		// (set) Token: 0x06000531 RID: 1329 RVA: 0x00016B02 File Offset: 0x00014D02
		[DataMember]
		public long EurexParentCommission { get; set; }

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x00016B0B File Offset: 0x00014D0B
		// (set) Token: 0x06000533 RID: 1331 RVA: 0x00016B13 File Offset: 0x00014D13
		[DataMember]
		public long ForeignRealProfit { get; set; }

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x00016B1C File Offset: 0x00014D1C
		// (set) Token: 0x06000535 RID: 1333 RVA: 0x00016B24 File Offset: 0x00014D24
		[DataMember]
		public long ForeignRealCommission { get; set; }

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x00016B2D File Offset: 0x00014D2D
		// (set) Token: 0x06000537 RID: 1335 RVA: 0x00016B35 File Offset: 0x00014D35
		[DataMember]
		public long ForeignParentCommission { get; set; }

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x00016B3E File Offset: 0x00014D3E
		// (set) Token: 0x06000539 RID: 1337 RVA: 0x00016B46 File Offset: 0x00014D46
		[DataMember]
		public long KospiRealProfit { get; set; }

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x00016B4F File Offset: 0x00014D4F
		// (set) Token: 0x0600053B RID: 1339 RVA: 0x00016B57 File Offset: 0x00014D57
		[DataMember]
		public long KospiRealCommission { get; set; }

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x00016B60 File Offset: 0x00014D60
		// (set) Token: 0x0600053D RID: 1341 RVA: 0x00016B68 File Offset: 0x00014D68
		[DataMember]
		public long KospiTax { get; set; }

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x00016B71 File Offset: 0x00014D71
		// (set) Token: 0x0600053F RID: 1343 RVA: 0x00016B79 File Offset: 0x00014D79
		[DataMember]
		public long KospiParentCommission { get; set; }

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x00016B82 File Offset: 0x00014D82
		// (set) Token: 0x06000541 RID: 1345 RVA: 0x00016B8A File Offset: 0x00014D8A
		[DataMember]
		public long KosdaqRealProfit { get; set; }

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x00016B93 File Offset: 0x00014D93
		// (set) Token: 0x06000543 RID: 1347 RVA: 0x00016B9B File Offset: 0x00014D9B
		[DataMember]
		public long KosdaqRealCommission { get; set; }

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x00016BA4 File Offset: 0x00014DA4
		// (set) Token: 0x06000545 RID: 1349 RVA: 0x00016BAC File Offset: 0x00014DAC
		[DataMember]
		public long KosdaqTax { get; set; }

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x00016BB5 File Offset: 0x00014DB5
		// (set) Token: 0x06000547 RID: 1351 RVA: 0x00016BBD File Offset: 0x00014DBD
		[DataMember]
		public long KosdaqParentCommission { get; set; }

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x00016BC6 File Offset: 0x00014DC6
		// (set) Token: 0x06000549 RID: 1353 RVA: 0x00016BCE File Offset: 0x00014DCE
		[DataMember]
		public long FuturesVirtualProfit { get; set; }

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x00016BD7 File Offset: 0x00014DD7
		// (set) Token: 0x0600054B RID: 1355 RVA: 0x00016BDF File Offset: 0x00014DDF
		[DataMember]
		public long FuturesVirtualCommission { get; set; }

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x00016BE8 File Offset: 0x00014DE8
		// (set) Token: 0x0600054D RID: 1357 RVA: 0x00016BF0 File Offset: 0x00014DF0
		[DataMember]
		public long FuturesVirtualParentCommission { get; set; }

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x00016BF9 File Offset: 0x00014DF9
		// (set) Token: 0x0600054F RID: 1359 RVA: 0x00016C01 File Offset: 0x00014E01
		[DataMember]
		public long OptionVirtualProfit { get; set; }

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x00016C0A File Offset: 0x00014E0A
		// (set) Token: 0x06000551 RID: 1361 RVA: 0x00016C12 File Offset: 0x00014E12
		[DataMember]
		public long OptionVirtualCommission { get; set; }

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x00016C1B File Offset: 0x00014E1B
		// (set) Token: 0x06000553 RID: 1363 RVA: 0x00016C23 File Offset: 0x00014E23
		[DataMember]
		public long OptionVirtualParentCommission { get; set; }

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x00016C2C File Offset: 0x00014E2C
		// (set) Token: 0x06000555 RID: 1365 RVA: 0x00016C34 File Offset: 0x00014E34
		[DataMember]
		public long CmeVirtualProfit { get; set; }

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x00016C3D File Offset: 0x00014E3D
		// (set) Token: 0x06000557 RID: 1367 RVA: 0x00016C45 File Offset: 0x00014E45
		[DataMember]
		public long CmeVirtualCommission { get; set; }

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x00016C4E File Offset: 0x00014E4E
		// (set) Token: 0x06000559 RID: 1369 RVA: 0x00016C56 File Offset: 0x00014E56
		[DataMember]
		public long CmeVirtualParentCommission { get; set; }

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x00016C5F File Offset: 0x00014E5F
		// (set) Token: 0x0600055B RID: 1371 RVA: 0x00016C67 File Offset: 0x00014E67
		[DataMember]
		public long EurexVirtualProfit { get; set; }

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x00016C70 File Offset: 0x00014E70
		// (set) Token: 0x0600055D RID: 1373 RVA: 0x00016C78 File Offset: 0x00014E78
		[DataMember]
		public long EurexVirtualCommission { get; set; }

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x00016C81 File Offset: 0x00014E81
		// (set) Token: 0x0600055F RID: 1375 RVA: 0x00016C89 File Offset: 0x00014E89
		[DataMember]
		public long EurexVirtualParentCommission { get; set; }

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x00016C92 File Offset: 0x00014E92
		// (set) Token: 0x06000561 RID: 1377 RVA: 0x00016C9A File Offset: 0x00014E9A
		[DataMember]
		public long ForeignVirtualProfit { get; set; }

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x00016CA3 File Offset: 0x00014EA3
		// (set) Token: 0x06000563 RID: 1379 RVA: 0x00016CAB File Offset: 0x00014EAB
		[DataMember]
		public long ForeignVirtualCommission { get; set; }

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x00016CB4 File Offset: 0x00014EB4
		// (set) Token: 0x06000565 RID: 1381 RVA: 0x00016CBC File Offset: 0x00014EBC
		[DataMember]
		public long ForeignVirtualParentCommission { get; set; }

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x00016CC5 File Offset: 0x00014EC5
		// (set) Token: 0x06000567 RID: 1383 RVA: 0x00016CCD File Offset: 0x00014ECD
		[DataMember]
		public long KospiVirtualProfit { get; set; }

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x00016CD6 File Offset: 0x00014ED6
		// (set) Token: 0x06000569 RID: 1385 RVA: 0x00016CDE File Offset: 0x00014EDE
		[DataMember]
		public long KospiVirtualCommission { get; set; }

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x00016CE7 File Offset: 0x00014EE7
		// (set) Token: 0x0600056B RID: 1387 RVA: 0x00016CEF File Offset: 0x00014EEF
		[DataMember]
		public long KospiVirtualTax { get; set; }

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x00016CF8 File Offset: 0x00014EF8
		// (set) Token: 0x0600056D RID: 1389 RVA: 0x00016D00 File Offset: 0x00014F00
		[DataMember]
		public long KospiVirtualParentCommission { get; set; }

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x00016D09 File Offset: 0x00014F09
		// (set) Token: 0x0600056F RID: 1391 RVA: 0x00016D11 File Offset: 0x00014F11
		[DataMember]
		public long KosdaqVirtualProfit { get; set; }

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x00016D1A File Offset: 0x00014F1A
		// (set) Token: 0x06000571 RID: 1393 RVA: 0x00016D22 File Offset: 0x00014F22
		[DataMember]
		public long KosdaqVirtualCommission { get; set; }

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x00016D2B File Offset: 0x00014F2B
		// (set) Token: 0x06000573 RID: 1395 RVA: 0x00016D33 File Offset: 0x00014F33
		[DataMember]
		public long KosdaqVirtualTax { get; set; }

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x00016D3C File Offset: 0x00014F3C
		// (set) Token: 0x06000575 RID: 1397 RVA: 0x00016D44 File Offset: 0x00014F44
		[DataMember]
		public long KosdaqVirtualParentCommission { get; set; }

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x00016D4D File Offset: 0x00014F4D
		// (set) Token: 0x06000577 RID: 1399 RVA: 0x00016D55 File Offset: 0x00014F55
		[DataMember]
		public long TotalRealProfit { get; set; }

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x00016D5E File Offset: 0x00014F5E
		// (set) Token: 0x06000579 RID: 1401 RVA: 0x00016D66 File Offset: 0x00014F66
		[DataMember]
		public long TotalRealCommission { get; set; }

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x00016D6F File Offset: 0x00014F6F
		// (set) Token: 0x0600057B RID: 1403 RVA: 0x00016D77 File Offset: 0x00014F77
		[DataMember]
		public long TotalTax { get; set; }

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x00016D80 File Offset: 0x00014F80
		// (set) Token: 0x0600057D RID: 1405 RVA: 0x00016D88 File Offset: 0x00014F88
		[DataMember]
		public long TotalParentCommission { get; set; }

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x00016D91 File Offset: 0x00014F91
		// (set) Token: 0x0600057F RID: 1407 RVA: 0x00016D99 File Offset: 0x00014F99
		[DataMember]
		public long TotalVirtualProfit { get; set; }

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x00016DA2 File Offset: 0x00014FA2
		// (set) Token: 0x06000581 RID: 1409 RVA: 0x00016DAA File Offset: 0x00014FAA
		[DataMember]
		public long TotalVirtualCommission { get; set; }

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x00016DB3 File Offset: 0x00014FB3
		// (set) Token: 0x06000583 RID: 1411 RVA: 0x00016DBB File Offset: 0x00014FBB
		[DataMember]
		public long TotalVirtualTax { get; set; }

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000584 RID: 1412 RVA: 0x00016DC4 File Offset: 0x00014FC4
		// (set) Token: 0x06000585 RID: 1413 RVA: 0x00016DCC File Offset: 0x00014FCC
		[DataMember]
		public long TotalVirtualParentCommission { get; set; }

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x00016DD5 File Offset: 0x00014FD5
		// (set) Token: 0x06000587 RID: 1415 RVA: 0x00016DDD File Offset: 0x00014FDD
		[DataMember]
		public long TotalProfit { get; set; }

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x00016DE6 File Offset: 0x00014FE6
		// (set) Token: 0x06000589 RID: 1417 RVA: 0x00016DEE File Offset: 0x00014FEE
		[DataMember]
		public long TotalCommission { get; set; }

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x00016DF7 File Offset: 0x00014FF7
		// (set) Token: 0x0600058B RID: 1419 RVA: 0x00016DFF File Offset: 0x00014FFF
		[DataMember]
		public long CloseBalance { get; set; }

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x00016E08 File Offset: 0x00015008
		// (set) Token: 0x0600058D RID: 1421 RVA: 0x00016E10 File Offset: 0x00015010
		[DataMember]
		public long UserAccountId { get; set; }

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x00016E19 File Offset: 0x00015019
		// (set) Token: 0x0600058F RID: 1423 RVA: 0x00016E21 File Offset: 0x00015021
		[DataMember]
		public virtual UserAccount UserAccount { get; set; }
	}
}
