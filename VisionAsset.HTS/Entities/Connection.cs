using System;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x0200005F RID: 95
	[DataContract]
	public class Connection
	{
		// Token: 0x17000232 RID: 562
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x000167FD File Offset: 0x000149FD
		// (set) Token: 0x060004D4 RID: 1236 RVA: 0x00016805 File Offset: 0x00014A05
		[DataMember]
		public long ConnectionId { get; set; }

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x0001680E File Offset: 0x00014A0E
		// (set) Token: 0x060004D6 RID: 1238 RVA: 0x00016816 File Offset: 0x00014A16
		[DataMember]
		public string IpAddress { get; set; }

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0001681F File Offset: 0x00014A1F
		// (set) Token: 0x060004D8 RID: 1240 RVA: 0x00016827 File Offset: 0x00014A27
		[DataMember]
		public string MacAddress { get; set; }

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x00016830 File Offset: 0x00014A30
		// (set) Token: 0x060004DA RID: 1242 RVA: 0x00016838 File Offset: 0x00014A38
		[DataMember]
		public string UserDomainName { get; set; }

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x00016841 File Offset: 0x00014A41
		// (set) Token: 0x060004DC RID: 1244 RVA: 0x00016849 File Offset: 0x00014A49
		[DataMember]
		public string OsVersion { get; set; }

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x00016852 File Offset: 0x00014A52
		// (set) Token: 0x060004DE RID: 1246 RVA: 0x0001685A File Offset: 0x00014A5A
		[DataMember]
		public string OsUserName { get; set; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x00016863 File Offset: 0x00014A63
		// (set) Token: 0x060004E0 RID: 1248 RVA: 0x0001686B File Offset: 0x00014A6B
		[DataMember]
		public ConnectionType ConnectionType { get; set; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x00016874 File Offset: 0x00014A74
		// (set) Token: 0x060004E2 RID: 1250 RVA: 0x0001687C File Offset: 0x00014A7C
		[DataMember]
		public DateTime ConnectionDate { get; set; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x00016885 File Offset: 0x00014A85
		// (set) Token: 0x060004E4 RID: 1252 RVA: 0x0001688D File Offset: 0x00014A8D
		[DataMember]
		public long UserId { get; set; }

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x00016896 File Offset: 0x00014A96
		// (set) Token: 0x060004E6 RID: 1254 RVA: 0x0001689E File Offset: 0x00014A9E
		[DataMember]
		public virtual User User { get; set; }
	}
}
