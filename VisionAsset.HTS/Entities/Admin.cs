using System;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x0200005A RID: 90
	[DataContract]
	public class Admin
	{
		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x0001616A File Offset: 0x0001436A
		// (set) Token: 0x06000409 RID: 1033 RVA: 0x00016172 File Offset: 0x00014372
		[DataMember]
		public string AdminId { get; set; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x0001617B File Offset: 0x0001437B
		// (set) Token: 0x0600040B RID: 1035 RVA: 0x00016183 File Offset: 0x00014383
		[DataMember]
		public string AdminPassword { get; set; }

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x0001618C File Offset: 0x0001438C
		// (set) Token: 0x0600040D RID: 1037 RVA: 0x00016194 File Offset: 0x00014394
		[DataMember]
		public string AdminName { get; set; }

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x0001619D File Offset: 0x0001439D
		// (set) Token: 0x0600040F RID: 1039 RVA: 0x000161A5 File Offset: 0x000143A5
		[DataMember]
		public string NickName { get; set; }

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x000161AE File Offset: 0x000143AE
		// (set) Token: 0x06000411 RID: 1041 RVA: 0x000161B6 File Offset: 0x000143B6
		[DataMember]
		public string Phone { get; set; }

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x000161BF File Offset: 0x000143BF
		// (set) Token: 0x06000413 RID: 1043 RVA: 0x000161C7 File Offset: 0x000143C7
		[DataMember]
		public string Email { get; set; }

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x000161D0 File Offset: 0x000143D0
		// (set) Token: 0x06000415 RID: 1045 RVA: 0x000161D8 File Offset: 0x000143D8
		[DataMember]
		public AdminType AdminType { get; set; }

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x000161E1 File Offset: 0x000143E1
		// (set) Token: 0x06000417 RID: 1047 RVA: 0x000161E9 File Offset: 0x000143E9
		[DataMember]
		public string MacAddress { get; set; }
	}
}
