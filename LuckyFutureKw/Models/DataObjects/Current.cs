using System;
using System.Runtime.Serialization;

namespace LuckyFuture.Models.DataObjects
{
    public enum TradeType
    {
        // Token: 0x0400026A RID: 618
        Sell,
        // Token: 0x0400026B RID: 619
        Buy
    }
    // Token: 0x02000062 RID: 98
    class Current
    {
        // Token: 0x1700023F RID: 575
        // (get) Token: 0x060004EF RID: 1263 RVA: 0x000168DA File Offset: 0x00014ADA
        // (set) Token: 0x060004F0 RID: 1264 RVA: 0x000168E2 File Offset: 0x00014AE2
        public long CurrentId { get; set; }

        // Token: 0x17000240 RID: 576
        // (get) Token: 0x060004F1 RID: 1265 RVA: 0x000168EB File Offset: 0x00014AEB
        // (set) Token: 0x060004F2 RID: 1266 RVA: 0x000168F3 File Offset: 0x00014AF3
        public string Symbol { get; set; }

        // Token: 0x17000241 RID: 577
        // (get) Token: 0x060004F3 RID: 1267 RVA: 0x000168FC File Offset: 0x00014AFC
        // (set) Token: 0x060004F4 RID: 1268 RVA: 0x00016904 File Offset: 0x00014B04
        public double CurrentPrice { get; set; }

        // Token: 0x17000242 RID: 578
        // (get) Token: 0x060004F5 RID: 1269 RVA: 0x0001690D File Offset: 0x00014B0D
        // (set) Token: 0x060004F6 RID: 1270 RVA: 0x00016915 File Offset: 0x00014B15
        public double StartPrice { get; set; }

        // Token: 0x17000243 RID: 579
        // (get) Token: 0x060004F7 RID: 1271 RVA: 0x0001691E File Offset: 0x00014B1E
        // (set) Token: 0x060004F8 RID: 1272 RVA: 0x00016926 File Offset: 0x00014B26
        public double HighPrice { get; set; }

        // Token: 0x17000244 RID: 580
        // (get) Token: 0x060004F9 RID: 1273 RVA: 0x0001692F File Offset: 0x00014B2F
        // (set) Token: 0x060004FA RID: 1274 RVA: 0x00016937 File Offset: 0x00014B37
        public double LowPrice { get; set; }

        // Token: 0x17000245 RID: 581
        // (get) Token: 0x060004FB RID: 1275 RVA: 0x00016940 File Offset: 0x00014B40
        // (set) Token: 0x060004FC RID: 1276 RVA: 0x00016948 File Offset: 0x00014B48
        public double BeforeClosePrice { get; set; }

        // Token: 0x17000246 RID: 582
        // (get) Token: 0x060004FD RID: 1277 RVA: 0x00016951 File Offset: 0x00014B51
        // (set) Token: 0x060004FE RID: 1278 RVA: 0x00016959 File Offset: 0x00014B59
        public double UpLimitPrice { get; set; }

        // Token: 0x17000247 RID: 583
        // (get) Token: 0x060004FF RID: 1279 RVA: 0x00016962 File Offset: 0x00014B62
        // (set) Token: 0x06000500 RID: 1280 RVA: 0x0001696A File Offset: 0x00014B6A
        public double DownLimitPrice { get; set; }

        // Token: 0x17000248 RID: 584
        // (get) Token: 0x06000501 RID: 1281 RVA: 0x00016973 File Offset: 0x00014B73
        // (set) Token: 0x06000502 RID: 1282 RVA: 0x0001697B File Offset: 0x00014B7B
        public int ConclusionVolume { get; set; }

        // Token: 0x17000249 RID: 585
        // (get) Token: 0x06000503 RID: 1283 RVA: 0x00016984 File Offset: 0x00014B84
        // (set) Token: 0x06000504 RID: 1284 RVA: 0x0001698C File Offset: 0x00014B8C
        public int Volume { get; set; }

        // Token: 0x1700024A RID: 586
        // (get) Token: 0x06000505 RID: 1285 RVA: 0x00016995 File Offset: 0x00014B95
        // (set) Token: 0x06000506 RID: 1286 RVA: 0x0001699D File Offset: 0x00014B9D
        public TradeType TradeType { get; set; }

        // Token: 0x1700024B RID: 587
        // (get) Token: 0x06000507 RID: 1287 RVA: 0x000169A6 File Offset: 0x00014BA6
        // (set) Token: 0x06000508 RID: 1288 RVA: 0x000169AE File Offset: 0x00014BAE
        public string CurrentTime { get; set; }

        // Token: 0x1700024C RID: 588
        // (get) Token: 0x06000509 RID: 1289 RVA: 0x000169B7 File Offset: 0x00014BB7
        // (set) Token: 0x0600050A RID: 1290 RVA: 0x000169BF File Offset: 0x00014BBF
        public DateTime ReceivedDate { get; set; }

        // Token: 0x1700024D RID: 589
        // (get) Token: 0x0600050B RID: 1291 RVA: 0x000169C8 File Offset: 0x00014BC8
        // (set) Token: 0x0600050C RID: 1292 RVA: 0x000169D0 File Offset: 0x00014BD0
        public long ItemId { get; set; }

        public double Contrast { get; set; }

        public double ContrastPer { get; set; }

    }
}
