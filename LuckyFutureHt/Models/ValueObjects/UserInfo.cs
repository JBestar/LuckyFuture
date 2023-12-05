using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyFuture.Models.ValueObjects
{
	public class UserInfo
	{
		public long UserId { get; set; }
		public string LoginId { get; set; }
		public string UserPassword { get; set; }
		public string UserName { get; set; }
		public string NickName { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public string BankName { get; set; }
		public string BankUserName { get; set; }
		public string BankAccount { get; set; }
		public DateTime RegistrationDate { get; set; }
		public DateTime LatestLoginDate { get; set; }
		public long CompanyId { get; set; }
	}
}
