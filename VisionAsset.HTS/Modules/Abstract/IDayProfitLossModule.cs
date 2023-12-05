using System;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Modules.Abstract
{
	// Token: 0x02000049 RID: 73
	public interface IDayProfitLossModule
	{
		// Token: 0x0600038F RID: 911
		void AddDeposit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long amount);

		// Token: 0x06000390 RID: 912
		void AddWithdraw(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long amount);

		// Token: 0x06000391 RID: 913
		void AddFuturesRealCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission);

		// Token: 0x06000392 RID: 914
		void AddFuturesRealProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit);

		// Token: 0x06000393 RID: 915
		void AddCmeRealCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission);

		// Token: 0x06000394 RID: 916
		void AddCmeRealProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit);

		// Token: 0x06000395 RID: 917
		void AddOptionRealCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission);

		// Token: 0x06000396 RID: 918
		void AddOptionRealProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit);

		// Token: 0x06000397 RID: 919
		void AddEurexRealCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission);

		// Token: 0x06000398 RID: 920
		void AddEurexRealProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit);

		// Token: 0x06000399 RID: 921
		void AddForeignRealCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission);

		// Token: 0x0600039A RID: 922
		void AddForeignRealProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit);

		// Token: 0x0600039B RID: 923
		void AddKospiRealCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission);

		// Token: 0x0600039C RID: 924
		void AddKospiRealProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit);

		// Token: 0x0600039D RID: 925
		void AddKosdaqRealCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission);

		// Token: 0x0600039E RID: 926
		void AddKosdaqRealProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit);

		// Token: 0x0600039F RID: 927
		void AddFuturesVirtualCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission);

		// Token: 0x060003A0 RID: 928
		void AddFuturesVirtualProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit);

		// Token: 0x060003A1 RID: 929
		void AddCmeVirtualCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission);

		// Token: 0x060003A2 RID: 930
		void AddCmeVirtualProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit);

		// Token: 0x060003A3 RID: 931
		void AddOptionVirtualCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission);

		// Token: 0x060003A4 RID: 932
		void AddOptionVirtualProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit);

		// Token: 0x060003A5 RID: 933
		void AddEurexVirtualCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission);

		// Token: 0x060003A6 RID: 934
		void AddEurexVirtualProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit);

		// Token: 0x060003A7 RID: 935
		void AddForeignVirtualCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission);

		// Token: 0x060003A8 RID: 936
		void AddForeignVirtualProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit);

		// Token: 0x060003A9 RID: 937
		void AddKospiVirtualCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission);

		// Token: 0x060003AA RID: 938
		void AddKospiVirtualProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit);

		// Token: 0x060003AB RID: 939
		void AddKosdaqVirtualCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission);

		// Token: 0x060003AC RID: 940
		void AddKosdaqVirtualProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit);
	}
}
