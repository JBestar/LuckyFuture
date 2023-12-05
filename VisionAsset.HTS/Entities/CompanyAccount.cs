using System;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x0200005E RID: 94
	[DataContract]
	public class CompanyAccount
	{
		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x00016786 File Offset: 0x00014986
		// (set) Token: 0x060004C5 RID: 1221 RVA: 0x0001678E File Offset: 0x0001498E
		[DataMember]
		public long CompanyAccountId { get; set; }

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00016797 File Offset: 0x00014997
		// (set) Token: 0x060004C7 RID: 1223 RVA: 0x0001679F File Offset: 0x0001499F
		[DataMember]
		public string BankName { get; set; }

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x000167A8 File Offset: 0x000149A8
		// (set) Token: 0x060004C9 RID: 1225 RVA: 0x000167B0 File Offset: 0x000149B0
		[DataMember]
		public string BankUserName { get; set; }

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x000167B9 File Offset: 0x000149B9
		// (set) Token: 0x060004CB RID: 1227 RVA: 0x000167C1 File Offset: 0x000149C1
		[DataMember]
		public string BankAccount { get; set; }

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x000167CA File Offset: 0x000149CA
		// (set) Token: 0x060004CD RID: 1229 RVA: 0x000167D2 File Offset: 0x000149D2
		[DataMember]
		public string Phone { get; set; }

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x060004CE RID: 1230 RVA: 0x000167DB File Offset: 0x000149DB
		// (set) Token: 0x060004CF RID: 1231 RVA: 0x000167E3 File Offset: 0x000149E3
		[DataMember]
		public long CompanyId { get; set; }

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x060004D0 RID: 1232 RVA: 0x000167EC File Offset: 0x000149EC
		// (set) Token: 0x060004D1 RID: 1233 RVA: 0x000167F4 File Offset: 0x000149F4
		[DataMember]
		public virtual Company Company { get; set; }
	}
}
