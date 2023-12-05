using System;
using System.Linq;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.Modules.Abstract;
using Goodbyte.TradingSystem.Domain.Repositories.Abstract;

namespace Goodbyte.TradingSystem.Domain.Modules.Concrete
{
	// Token: 0x02000045 RID: 69
	public class DayProfitLossModule : IDayProfitLossModule
	{
		// Token: 0x0600035C RID: 860 RVA: 0x0000FA98 File Offset: 0x0000DC98
		public void AddDeposit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long amount)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				dayProfitLoss.TotalDeposit += Math.Abs(amount);
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000FB7C File Offset: 0x0000DD7C
		public void AddWithdraw(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long amount)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				dayProfitLoss.TotalWithdraw += Math.Abs(amount);
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000FC60 File Offset: 0x0000DE60
		public void AddFuturesRealCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				commission = Math.Abs(commission);
				parentCommission = Math.Abs(parentCommission);
				dayProfitLoss.FuturesRealCommission += commission;
				dayProfitLoss.TotalRealCommission += commission;
				dayProfitLoss.FuturesParentCommission += parentCommission;
				dayProfitLoss.TotalParentCommission += parentCommission;
				dayProfitLoss.TotalCommission += commission;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000FD88 File Offset: 0x0000DF88
		public void AddFuturesRealProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				dayProfitLoss.FuturesRealProfit += profit;
				dayProfitLoss.TotalRealProfit += profit;
				dayProfitLoss.TotalProfit += profit;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000FE84 File Offset: 0x0000E084
		public void AddCmeRealCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				commission = Math.Abs(commission);
				parentCommission = Math.Abs(parentCommission);
				dayProfitLoss.CmeRealCommission += commission;
				dayProfitLoss.TotalRealCommission += commission;
				dayProfitLoss.CmeParentCommission += parentCommission;
				dayProfitLoss.TotalParentCommission += parentCommission;
				dayProfitLoss.TotalCommission += commission;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000FFAC File Offset: 0x0000E1AC
		public void AddCmeRealProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				dayProfitLoss.CmeRealProfit += profit;
				dayProfitLoss.TotalRealProfit += profit;
				dayProfitLoss.TotalProfit += profit;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x06000362 RID: 866 RVA: 0x000100A8 File Offset: 0x0000E2A8
		public void AddOptionRealCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				commission = Math.Abs(commission);
				parentCommission = Math.Abs(parentCommission);
				dayProfitLoss.OptionRealCommission += commission;
				dayProfitLoss.TotalRealCommission += commission;
				dayProfitLoss.OptionParentCommission += parentCommission;
				dayProfitLoss.TotalParentCommission += parentCommission;
				dayProfitLoss.TotalCommission += commission;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x06000363 RID: 867 RVA: 0x000101D0 File Offset: 0x0000E3D0
		public void AddOptionRealProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				dayProfitLoss.OptionRealProfit += profit;
				dayProfitLoss.TotalRealProfit += profit;
				dayProfitLoss.TotalProfit += profit;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x06000364 RID: 868 RVA: 0x000102CC File Offset: 0x0000E4CC
		public void AddEurexRealCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				commission = Math.Abs(commission);
				parentCommission = Math.Abs(parentCommission);
				dayProfitLoss.EurexRealCommission += commission;
				dayProfitLoss.TotalRealCommission += commission;
				dayProfitLoss.EurexParentCommission += parentCommission;
				dayProfitLoss.TotalParentCommission += parentCommission;
				dayProfitLoss.TotalCommission += commission;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x06000365 RID: 869 RVA: 0x000103F4 File Offset: 0x0000E5F4
		public void AddEurexRealProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				dayProfitLoss.EurexRealProfit += profit;
				dayProfitLoss.TotalRealProfit += profit;
				dayProfitLoss.TotalProfit += profit;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x06000366 RID: 870 RVA: 0x000104F0 File Offset: 0x0000E6F0
		public void AddForeignRealCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				commission = Math.Abs(commission);
				parentCommission = Math.Abs(parentCommission);
				dayProfitLoss.ForeignRealCommission += commission;
				dayProfitLoss.TotalRealCommission += commission;
				dayProfitLoss.ForeignParentCommission += parentCommission;
				dayProfitLoss.TotalParentCommission += parentCommission;
				dayProfitLoss.TotalCommission += commission;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00010618 File Offset: 0x0000E818
		public void AddForeignRealProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				dayProfitLoss.ForeignRealProfit += profit;
				dayProfitLoss.TotalRealProfit += profit;
				dayProfitLoss.TotalProfit += profit;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00010714 File Offset: 0x0000E914
		public void AddKospiRealCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				commission = Math.Abs(commission);
				tax = Math.Abs(tax);
				parentCommission = Math.Abs(parentCommission);
				dayProfitLoss.KospiRealCommission += commission;
				dayProfitLoss.TotalRealCommission += commission;
				dayProfitLoss.KospiTax += tax;
				dayProfitLoss.TotalTax += tax;
				dayProfitLoss.KospiParentCommission += parentCommission;
				dayProfitLoss.TotalParentCommission += parentCommission;
				dayProfitLoss.TotalCommission += commission;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00010868 File Offset: 0x0000EA68
		public void AddKospiRealProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				dayProfitLoss.KospiRealProfit += profit;
				dayProfitLoss.TotalRealProfit += profit;
				dayProfitLoss.TotalProfit += profit;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00010964 File Offset: 0x0000EB64
		public void AddKosdaqRealCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				commission = Math.Abs(commission);
				tax = Math.Abs(tax);
				parentCommission = Math.Abs(parentCommission);
				dayProfitLoss.KosdaqRealCommission += commission;
				dayProfitLoss.TotalRealCommission += commission;
				dayProfitLoss.KosdaqTax += tax;
				dayProfitLoss.TotalTax += tax;
				dayProfitLoss.KosdaqParentCommission += parentCommission;
				dayProfitLoss.TotalParentCommission += parentCommission;
				dayProfitLoss.TotalCommission += commission;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00010AB8 File Offset: 0x0000ECB8
		public void AddKosdaqRealProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				dayProfitLoss.KosdaqRealProfit += profit;
				dayProfitLoss.TotalRealProfit += profit;
				dayProfitLoss.TotalProfit += profit;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00010BB4 File Offset: 0x0000EDB4
		public void AddFuturesVirtualCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				commission = Math.Abs(commission);
				parentCommission = Math.Abs(parentCommission);
				dayProfitLoss.FuturesVirtualCommission += commission;
				dayProfitLoss.TotalVirtualCommission += commission;
				dayProfitLoss.FuturesVirtualParentCommission += parentCommission;
				dayProfitLoss.TotalVirtualParentCommission += parentCommission;
				dayProfitLoss.TotalCommission += commission;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00010CDC File Offset: 0x0000EEDC
		public void AddFuturesVirtualProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				dayProfitLoss.FuturesVirtualProfit += profit;
				dayProfitLoss.TotalVirtualProfit += profit;
				dayProfitLoss.TotalProfit += profit;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00010DD8 File Offset: 0x0000EFD8
		public void AddCmeVirtualCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				commission = Math.Abs(commission);
				parentCommission = Math.Abs(parentCommission);
				dayProfitLoss.CmeVirtualCommission += commission;
				dayProfitLoss.TotalVirtualCommission += commission;
				dayProfitLoss.CmeVirtualParentCommission += parentCommission;
				dayProfitLoss.TotalVirtualParentCommission += parentCommission;
				dayProfitLoss.TotalCommission += commission;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00010F00 File Offset: 0x0000F100
		public void AddCmeVirtualProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				dayProfitLoss.CmeVirtualProfit += profit;
				dayProfitLoss.TotalVirtualProfit += profit;
				dayProfitLoss.TotalProfit += profit;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00010FFC File Offset: 0x0000F1FC
		public void AddOptionVirtualCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				commission = Math.Abs(commission);
				parentCommission = Math.Abs(parentCommission);
				dayProfitLoss.OptionVirtualCommission += commission;
				dayProfitLoss.TotalVirtualCommission += commission;
				dayProfitLoss.OptionVirtualParentCommission += parentCommission;
				dayProfitLoss.TotalVirtualParentCommission += parentCommission;
				dayProfitLoss.TotalCommission += commission;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00011124 File Offset: 0x0000F324
		public void AddOptionVirtualProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				dayProfitLoss.OptionVirtualProfit += profit;
				dayProfitLoss.TotalVirtualProfit += profit;
				dayProfitLoss.TotalProfit += profit;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00011220 File Offset: 0x0000F420
		public void AddEurexVirtualCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				commission = Math.Abs(commission);
				parentCommission = Math.Abs(parentCommission);
				dayProfitLoss.EurexVirtualCommission += commission;
				dayProfitLoss.TotalVirtualCommission += commission;
				dayProfitLoss.EurexVirtualParentCommission += parentCommission;
				dayProfitLoss.TotalVirtualParentCommission += parentCommission;
				dayProfitLoss.TotalCommission += commission;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00011348 File Offset: 0x0000F548
		public void AddEurexVirtualProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				dayProfitLoss.EurexVirtualProfit += profit;
				dayProfitLoss.TotalVirtualProfit += profit;
				dayProfitLoss.TotalProfit += profit;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00011444 File Offset: 0x0000F644
		public void AddForeignVirtualCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				commission = Math.Abs(commission);
				parentCommission = Math.Abs(parentCommission);
				dayProfitLoss.ForeignVirtualCommission += commission;
				dayProfitLoss.TotalVirtualCommission += commission;
				dayProfitLoss.ForeignVirtualParentCommission += parentCommission;
				dayProfitLoss.TotalVirtualParentCommission += parentCommission;
				dayProfitLoss.TotalCommission += commission;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0001156C File Offset: 0x0000F76C
		public void AddForeignVirtualProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				dayProfitLoss.ForeignVirtualProfit += profit;
				dayProfitLoss.TotalVirtualProfit += profit;
				dayProfitLoss.TotalProfit += profit;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00011668 File Offset: 0x0000F868
		public void AddKospiVirtualCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				commission = Math.Abs(commission);
				tax = Math.Abs(tax);
				parentCommission = Math.Abs(parentCommission);
				dayProfitLoss.KospiVirtualCommission += commission;
				dayProfitLoss.TotalVirtualCommission += commission;
				dayProfitLoss.KospiVirtualTax += tax;
				dayProfitLoss.TotalVirtualTax += tax;
				dayProfitLoss.KospiVirtualParentCommission += parentCommission;
				dayProfitLoss.TotalVirtualParentCommission += parentCommission;
				dayProfitLoss.TotalCommission += commission;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x06000377 RID: 887 RVA: 0x000117BC File Offset: 0x0000F9BC
		public void AddKospiVirtualProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				dayProfitLoss.KospiVirtualProfit += profit;
				dayProfitLoss.TotalVirtualProfit += profit;
				dayProfitLoss.TotalProfit += profit;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x06000378 RID: 888 RVA: 0x000118B8 File Offset: 0x0000FAB8
		public void AddKosdaqVirtualCommission(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long commission, long tax, long parentCommission)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				commission = Math.Abs(commission);
				tax = Math.Abs(tax);
				parentCommission = Math.Abs(parentCommission);
				dayProfitLoss.KosdaqVirtualCommission += commission;
				dayProfitLoss.TotalVirtualCommission += commission;
				dayProfitLoss.KosdaqVirtualTax += tax;
				dayProfitLoss.TotalVirtualTax += tax;
				dayProfitLoss.KosdaqVirtualParentCommission += parentCommission;
				dayProfitLoss.TotalVirtualParentCommission += parentCommission;
				dayProfitLoss.TotalCommission += commission;
				dayProfitLossRepo.Save();
			}
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00011A0C File Offset: 0x0000FC0C
		public void AddKosdaqVirtualProfit(IDayProfitLossRepository dayProfitLossRepo, long userAccountId, long profit)
		{
			DayProfitLoss dayProfitLoss = (from d in dayProfitLossRepo.Get()
			where d.UserAccountId == userAccountId
			orderby d.DayProfitLossId descending
			select d).FirstOrDefault<DayProfitLoss>();
			if (dayProfitLoss != null)
			{
				dayProfitLoss.KosdaqVirtualProfit += profit;
				dayProfitLoss.TotalVirtualProfit += profit;
				dayProfitLoss.TotalProfit += profit;
				dayProfitLossRepo.Save();
			}
		}
	}
}
