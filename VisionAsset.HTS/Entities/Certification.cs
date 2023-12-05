using System;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x0200005C RID: 92
	[DataContract]
	public class Certification
	{
		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x00016698 File Offset: 0x00014898
		// (set) Token: 0x060004A7 RID: 1191 RVA: 0x000166A0 File Offset: 0x000148A0
		public long CertificationId { get; set; }

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x000166A9 File Offset: 0x000148A9
		// (set) Token: 0x060004A9 RID: 1193 RVA: 0x000166B1 File Offset: 0x000148B1
		[DataMember]
		public string Id { get; set; }

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x000166BA File Offset: 0x000148BA
		// (set) Token: 0x060004AB RID: 1195 RVA: 0x000166C2 File Offset: 0x000148C2
		[DataMember]
		public string Password { get; set; }
        // Token: 0x17000214 RID: 532
        // (get) Token: 0x06000487 RID: 1159 RVA: 0x00014B81 File Offset: 0x00012D81
        // (set) Token: 0x06000488 RID: 1160 RVA: 0x00014B89 File Offset: 0x00012D89
        [DataMember]
        public long CompanyId { get; set; }

        // Token: 0x17000215 RID: 533
        // (get) Token: 0x06000489 RID: 1161 RVA: 0x00014B92 File Offset: 0x00012D92
        // (set) Token: 0x0600048A RID: 1162 RVA: 0x00014B9A File Offset: 0x00012D9A
        [DataMember]
        public virtual Company Company { get; set; }
    }
}

