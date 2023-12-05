using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goodbyte.TradingSystem.Client.Common
{
	public static class ClientState
	{

		// Token: 0x040004C1 RID: 1217
		public static string SessionId;

		// Token: 0x040004C2 RID: 1218
		public static Company Company;

		// Token: 0x040004C3 RID: 1219
		public static List<Currency> Currencies;

		// Token: 0x040004C4 RID: 1220
		public static Certification Certification = new Certification
        {
            CompanyId = 1L
        };
    
		// Token: 0x040004C5 RID: 1221
		public static bool IsRunMit = false;

		// Token: 0x040004C6 RID: 1222
		public static bool IsRunStopLoss = false;

		// Token: 0x040004C7 RID: 1223
		public static long UserId = 0L;

		// Token: 0x040004C8 RID: 1224
		public static string LoginId = string.Empty;

		// Token: 0x040004C9 RID: 1225
		public static string UserPassword = string.Empty;

		// Token: 0x040004CA RID: 1226
		public static UserType UserType = UserType.Standby;

		// Token: 0x040004CB RID: 1227
		public static long CompanyId = 1L;

		// Token: 0x040004CC RID: 1228
		public static string RealIp = "0.0.0.0";

        // Token: 0x0400054D RID: 1357
        internal static string Isp = string.Empty;

        // Token: 0x0400054E RID: 1358
        internal static string Location = string.Empty;

        // Token: 0x040004CD RID: 1229
        public static string MacAddress = string.Empty;

		// Token: 0x040004CE RID: 1230
		public static string UserDomainName = string.Empty;

		// Token: 0x040004CF RID: 1231
		public static string OsVersion = string.Empty;

		// Token: 0x040004D0 RID: 1232
		public static string OsUserName = string.Empty;

		// Token: 0x040004D1 RID: 1233
		public static bool IsOffSignal = false;

		// Token: 0x040004D2 RID: 1234
		public static TimeSpan TimeDifference = default(TimeSpan);

		public static List<Item> Items;

		public static double OptionAtm;

		/// <summary>
		/// New members added by jki 2021.04.02
		/// </summary>

		public static List<Market> Markets;
		public static Dictionary<long, Current> ItemCurrents;
		public static Dictionary<long, Current> OldItemCurrents;
		public static User CurrentUser;
		public static List<UserAccount> UserAccounts;
		public static List<UserAccountSpecific> UserAccountSpecs;
		public static List<Order> Orders;
		public static DayProfitLoss DayProfitLoss;
		public static Quote Quote;
		public static Quote OldQuote;
		public static List<MitOrder> MitOrders;
		
		public static Item Item;
		public static Market Market;
//		public static OptionMarketInfo OptionAtmRow;
		public static Current Current;
		public static Order Order;

		public static IUserAccountSpecificRepository UserAccountSpecifics { get; set; }
	}
}
