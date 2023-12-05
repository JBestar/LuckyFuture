using Goodbyte.TradingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VisionAsset.Client.DayProfitLossService
{
	public class DayProfitLossServiceClient : ServiceBase<IDayProfitLossService>, IDayProfitLossService
	{
		// Token: 0x060009FE RID: 2558 RVA: 0x0002D718 File Offset: 0x0002B918
		public DayProfitLossServiceClient()
		{
			NetTcpBinding binding = new NetTcpBinding
			{
				OpenTimeout = TimeSpan.FromSeconds(10),
				CloseTimeout = TimeSpan.FromSeconds(10),
				SendTimeout = TimeSpan.FromSeconds(10),
				ReceiveTimeout = TimeSpan.FromMinutes(5),
				MaxBufferPoolSize = 1048576,
				MaxBufferSize = 1048576,
				MaxReceivedMessageSize = 1048576
			};

			binding.ReaderQuotas.MaxDepth = Int32.MaxValue;
			binding.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;
			binding.ReaderQuotas.MaxArrayLength = Int32.MaxValue;
			binding.ReaderQuotas.MaxBytesPerRead = Int32.MaxValue;
			binding.ReaderQuotas.MaxNameTableCharCount = Int32.MaxValue;
			binding.Security.Mode = SecurityMode.None;

			Create(
				binding,
				"net.tcp://218.239.223.29/Goodbyte/TradingSystem/Service/DayProfitLossService"
			);
		}

        // Token: 0x06000BB0 RID: 2992 RVA: 0x0003BF69 File Offset: 0x0003A169
        public List<DayProfitLoss> GetCompanyDayProfitLosses(Certification certification, long companyId, DateTime marketDate)
        {
            return base.Channel.GetCompanyDayProfitLosses(certification, companyId, marketDate);
        }

        // Token: 0x06000BB1 RID: 2993 RVA: 0x0003BF79 File Offset: 0x0003A179
        public List<DayProfitLoss> GetCompanyDayRangeProfitLosses(Certification certification, long companyId, DateTime startDate, DateTime endDate)
        {
            return base.Channel.GetCompanyDayRangeProfitLosses(certification, companyId, startDate, endDate);
        }

        // Token: 0x06000BB2 RID: 2994 RVA: 0x0003BF8B File Offset: 0x0003A18B
        public List<DayProfitLoss> GetSumCompanyDayRangeProfitLosses(Certification certification, long companyId, DateTime startDate, DateTime endDate)
        {
            return base.Channel.GetSumCompanyDayRangeProfitLosses(certification, companyId, startDate, endDate);
        }

        // Token: 0x06000BB3 RID: 2995 RVA: 0x0003BF9D File Offset: 0x0003A19D
        public List<DayProfitLoss> GetExpertDayRangeProfitLosses(Certification certification, long companyId, string expertId, DateTime startDate, DateTime endDate)
        {
            return base.Channel.GetExpertDayRangeProfitLosses(certification, companyId, expertId, startDate, endDate);
        }

        // Token: 0x06000BB4 RID: 2996 RVA: 0x0003BFB1 File Offset: 0x0003A1B1
        public List<DayProfitLoss> GetSumExpertDayRangeProfitLosses(Certification certification, long companyId, string expertId, DateTime startDate, DateTime endDate)
        {
            return base.Channel.GetSumExpertDayRangeProfitLosses(certification, companyId, expertId, startDate, endDate);
        }

        // Token: 0x06000BB5 RID: 2997 RVA: 0x0003BFC5 File Offset: 0x0003A1C5
        public List<DayProfitLoss> GetDayRangeProfitLosses(Certification certification, long accountId, DateTime startDate, DateTime endDate)
        {
            return base.Channel.GetDayRangeProfitLosses(certification, accountId, startDate, endDate);
        }

        // Token: 0x06000BB6 RID: 2998 RVA: 0x0003BFD7 File Offset: 0x0003A1D7
        public List<DayProfitLoss> GetTodayUsingProfitLosses(Certification certification)
        {
            return base.Channel.GetTodayUsingProfitLosses(certification);
        }

        // Token: 0x06000BB7 RID: 2999 RVA: 0x0003BFE5 File Offset: 0x0003A1E5
        public DayProfitLoss GetDayProfitLoss(Certification certification, long accountId, DateTime marketDate)
        {
            return base.Channel.GetDayProfitLoss(certification, accountId, marketDate);
        }

        // Token: 0x06000BB8 RID: 3000 RVA: 0x0003BFF5 File Offset: 0x0003A1F5
        public void CreateDayProfitLosses(Certification certification, DateTime marketDate)
        {
            base.Channel.CreateDayProfitLosses(certification, marketDate);
        }

        // Token: 0x06000BB9 RID: 3001 RVA: 0x0003C004 File Offset: 0x0003A204
        public void CreateDayProfitLoss(Certification certification, long userAccountId, DateTime marketDate)
        {
            base.Channel.CreateDayProfitLoss(certification, userAccountId, marketDate);
        }

        // Token: 0x06000BBA RID: 3002 RVA: 0x0003C014 File Offset: 0x0003A214
        public long GetTotalProfit(Certification certification, long userAccountId)
        {
            return base.Channel.GetTotalProfit(certification, userAccountId);
        }
    }
}
