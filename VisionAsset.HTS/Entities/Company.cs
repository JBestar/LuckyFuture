using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x0200005D RID: 93
	[DataContract]
	public class Company
	{
		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x000166CB File Offset: 0x000148CB
		// (set) Token: 0x060004AE RID: 1198 RVA: 0x000166D3 File Offset: 0x000148D3
		[DataMember]
		public long CompanyId { get; set; }

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x000166DC File Offset: 0x000148DC
		// (set) Token: 0x060004B0 RID: 1200 RVA: 0x000166E4 File Offset: 0x000148E4
		[DataMember]
		public string CompanyName { get; set; }

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x000166ED File Offset: 0x000148ED
		// (set) Token: 0x060004B2 RID: 1202 RVA: 0x000166F5 File Offset: 0x000148F5
		[DataMember]
		public string CustomerService { get; set; }

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x000166FE File Offset: 0x000148FE
		// (set) Token: 0x060004B4 RID: 1204 RVA: 0x00016706 File Offset: 0x00014906
		[DataMember]
		public string OperatingHourInfo { get; set; }

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x0001670F File Offset: 0x0001490F
		// (set) Token: 0x060004B6 RID: 1206 RVA: 0x00016717 File Offset: 0x00014917
		[DataMember]
		public string WebSiteUrl { get; set; }

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x00016720 File Offset: 0x00014920
		// (set) Token: 0x060004B8 RID: 1208 RVA: 0x00016728 File Offset: 0x00014928
		[DataMember]
		public bool IsSummerTime { get; set; }

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x00016731 File Offset: 0x00014931
		// (set) Token: 0x060004BA RID: 1210 RVA: 0x00016739 File Offset: 0x00014939
		[DataMember]
		public virtual ICollection<ParentAccount> ParentAccounts { get; set; }

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x00016742 File Offset: 0x00014942
		// (set) Token: 0x060004BC RID: 1212 RVA: 0x0001674A File Offset: 0x0001494A
		[DataMember]
		public virtual ICollection<CompanyAccount> CompanyAccounts { get; set; }

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x00016753 File Offset: 0x00014953
		// (set) Token: 0x060004BE RID: 1214 RVA: 0x0001675B File Offset: 0x0001495B
		[DataMember]
		public virtual ICollection<User> Users { get; set; }

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x00016764 File Offset: 0x00014964
		// (set) Token: 0x060004C0 RID: 1216 RVA: 0x0001676C File Offset: 0x0001496C
		[DataMember]
		public virtual ICollection<Notice> Notices { get; set; }

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x00016775 File Offset: 0x00014975
		// (set) Token: 0x060004C2 RID: 1218 RVA: 0x0001677D File Offset: 0x0001497D
		[DataMember]
		public virtual ICollection<Certification> Certifications { get; set; }
	}
}
