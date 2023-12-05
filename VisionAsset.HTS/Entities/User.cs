using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Goodbyte.TradingSystem.Domain.Entities
{
	// Token: 0x0200007D RID: 125
	[DataContract]
	public class User
	{
		// Token: 0x17000328 RID: 808
		// (get) Token: 0x060006CC RID: 1740 RVA: 0x00017853 File Offset: 0x00015A53
		// (set) Token: 0x060006CD RID: 1741 RVA: 0x0001785B File Offset: 0x00015A5B
		[DataMember]
		public long UserId { get; set; }

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x060006CE RID: 1742 RVA: 0x00017864 File Offset: 0x00015A64
		// (set) Token: 0x060006CF RID: 1743 RVA: 0x0001786C File Offset: 0x00015A6C
		[DataMember]
		public string LoginId { get; set; }

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x00017875 File Offset: 0x00015A75
		// (set) Token: 0x060006D1 RID: 1745 RVA: 0x0001787D File Offset: 0x00015A7D
		[DataMember]
		public string UserPassword { get; set; }

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x00017886 File Offset: 0x00015A86
		// (set) Token: 0x060006D3 RID: 1747 RVA: 0x0001788E File Offset: 0x00015A8E
		[DataMember]
		public string UserName { get; set; }

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x060006D4 RID: 1748 RVA: 0x00017897 File Offset: 0x00015A97
		// (set) Token: 0x060006D5 RID: 1749 RVA: 0x0001789F File Offset: 0x00015A9F
		[DataMember]
		public string NickName { get; set; }

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x060006D6 RID: 1750 RVA: 0x000178A8 File Offset: 0x00015AA8
		// (set) Token: 0x060006D7 RID: 1751 RVA: 0x000178B0 File Offset: 0x00015AB0
		[DataMember]
		public string Phone { get; set; }

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x060006D8 RID: 1752 RVA: 0x000178B9 File Offset: 0x00015AB9
		// (set) Token: 0x060006D9 RID: 1753 RVA: 0x000178C1 File Offset: 0x00015AC1
		[DataMember]
		public string Email { get; set; }

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x000178CA File Offset: 0x00015ACA
		// (set) Token: 0x060006DB RID: 1755 RVA: 0x000178D2 File Offset: 0x00015AD2
		[DataMember]
		public string BankName { get; set; }

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x000178DB File Offset: 0x00015ADB
		// (set) Token: 0x060006DD RID: 1757 RVA: 0x000178E3 File Offset: 0x00015AE3
		[DataMember]
		public string BankUserName { get; set; }

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x000178EC File Offset: 0x00015AEC
		// (set) Token: 0x060006DF RID: 1759 RVA: 0x000178F4 File Offset: 0x00015AF4
		[DataMember]
		public string BankAccount { get; set; }

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x000178FD File Offset: 0x00015AFD
		// (set) Token: 0x060006E1 RID: 1761 RVA: 0x00017905 File Offset: 0x00015B05
		[DataMember]
		public UserType UserType { get; set; }

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x0001790E File Offset: 0x00015B0E
		// (set) Token: 0x060006E3 RID: 1763 RVA: 0x00017916 File Offset: 0x00015B16
		[DataMember]
		public bool IsBlocked { get; set; }

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x0001791F File Offset: 0x00015B1F
		// (set) Token: 0x060006E5 RID: 1765 RVA: 0x00017927 File Offset: 0x00015B27
		[DataMember]
		public string ExpertId { get; set; }

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x00017930 File Offset: 0x00015B30
		// (set) Token: 0x060006E7 RID: 1767 RVA: 0x00017938 File Offset: 0x00015B38
		[DataMember]
		public DateTime RegistrationDate { get; set; }

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x00017941 File Offset: 0x00015B41
		// (set) Token: 0x060006E9 RID: 1769 RVA: 0x00017949 File Offset: 0x00015B49
		[DataMember]
		public DateTime LatestLoginDate { get; set; }

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x00017952 File Offset: 0x00015B52
		// (set) Token: 0x060006EB RID: 1771 RVA: 0x0001795A File Offset: 0x00015B5A
		[DataMember]
		public string Memo { get; set; }

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x00017963 File Offset: 0x00015B63
		// (set) Token: 0x060006ED RID: 1773 RVA: 0x0001796B File Offset: 0x00015B6B
		[DataMember]
		public bool IsConnect { get; set; }

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x00017974 File Offset: 0x00015B74
		// (set) Token: 0x060006EF RID: 1775 RVA: 0x0001797C File Offset: 0x00015B7C
		[DataMember]
		public long CompanyId { get; set; }

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x00017985 File Offset: 0x00015B85
		// (set) Token: 0x060006F1 RID: 1777 RVA: 0x0001798D File Offset: 0x00015B8D
		[DataMember]
		public virtual Company Company { get; set; }

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x00017996 File Offset: 0x00015B96
		// (set) Token: 0x060006F3 RID: 1779 RVA: 0x0001799E File Offset: 0x00015B9E
		[DataMember]
		public virtual ICollection<UserAccount> UserAccounts { get; set; }

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x000179A7 File Offset: 0x00015BA7
		// (set) Token: 0x060006F5 RID: 1781 RVA: 0x000179AF File Offset: 0x00015BAF
		[DataMember]
		public virtual ICollection<Connection> Connections { get; set; }
	}
}
