using Goodbyte.TradingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VisionAsset.Client.DayProfitLossService
{
	[ServiceContract]
	public interface IDayProfitLossService
	{
        // Token: 0x06000BA0 RID: 2976
        [OperationContract(Action = "http://tempuri.org/IDayProfitLossService/GetCompanyDayProfitLosses", ReplyAction = "http://tempuri.org/IDayProfitLossService/GetCompanyDayProfitLossesResponse")]
        List<DayProfitLoss> GetCompanyDayProfitLosses(Certification certification, long companyId, DateTime marketDate);

        // Token: 0x06000BA1 RID: 2977
        [OperationContract(Action = "http://tempuri.org/IDayProfitLossService/GetCompanyDayRangeProfitLosses", ReplyAction = "http://tempuri.org/IDayProfitLossService/GetCompanyDayRangeProfitLossesResponse")]
        List<DayProfitLoss> GetCompanyDayRangeProfitLosses(Certification certification, long companyId, DateTime startDate, DateTime endDate);

        // Token: 0x06000BA2 RID: 2978
        [OperationContract(Action = "http://tempuri.org/IDayProfitLossService/GetSumCompanyDayRangeProfitLosses", ReplyAction = "http://tempuri.org/IDayProfitLossService/GetSumCompanyDayRangeProfitLossesResponse")]
        List<DayProfitLoss> GetSumCompanyDayRangeProfitLosses(Certification certification, long companyId, DateTime startDate, DateTime endDate);

        // Token: 0x06000BA3 RID: 2979
        [OperationContract(Action = "http://tempuri.org/IDayProfitLossService/GetExpertDayRangeProfitLosses", ReplyAction = "http://tempuri.org/IDayProfitLossService/GetExpertDayRangeProfitLossesResponse")]
        List<DayProfitLoss> GetExpertDayRangeProfitLosses(Certification certification, long companyId, string expertId, DateTime startDate, DateTime endDate);

        // Token: 0x06000BA4 RID: 2980
        [OperationContract(Action = "http://tempuri.org/IDayProfitLossService/GetSumExpertDayRangeProfitLosses", ReplyAction = "http://tempuri.org/IDayProfitLossService/GetSumExpertDayRangeProfitLossesResponse")]
        List<DayProfitLoss> GetSumExpertDayRangeProfitLosses(Certification certification, long companyId, string expertId, DateTime startDate, DateTime endDate);

        // Token: 0x06000BA5 RID: 2981
        [OperationContract(Action = "http://tempuri.org/IDayProfitLossService/GetDayRangeProfitLosses", ReplyAction = "http://tempuri.org/IDayProfitLossService/GetDayRangeProfitLossesResponse")]
        List<DayProfitLoss> GetDayRangeProfitLosses(Certification certification, long accountId, DateTime startDate, DateTime endDate);

        // Token: 0x06000BA6 RID: 2982
        [OperationContract(Action = "http://tempuri.org/IDayProfitLossService/GetTodayUsingProfitLosses", ReplyAction = "http://tempuri.org/IDayProfitLossService/GetTodayUsingProfitLossesResponse")]
        List<DayProfitLoss> GetTodayUsingProfitLosses(Certification certification);

        // Token: 0x06000BA7 RID: 2983
        [OperationContract(Action = "http://tempuri.org/IDayProfitLossService/GetDayProfitLoss", ReplyAction = "http://tempuri.org/IDayProfitLossService/GetDayProfitLossResponse")]
        DayProfitLoss GetDayProfitLoss(Certification certification, long accountId, DateTime marketDate);

        // Token: 0x06000BA8 RID: 2984
        [OperationContract(Action = "http://tempuri.org/IDayProfitLossService/CreateDayProfitLosses", ReplyAction = "http://tempuri.org/IDayProfitLossService/CreateDayProfitLossesResponse")]
        void CreateDayProfitLosses(Certification certification, DateTime marketDate);

        // Token: 0x06000BA9 RID: 2985
        [OperationContract(Action = "http://tempuri.org/IDayProfitLossService/CreateDayProfitLoss", ReplyAction = "http://tempuri.org/IDayProfitLossService/CreateDayProfitLossResponse")]
        void CreateDayProfitLoss(Certification certification, long userAccountId, DateTime marketDate);

        // Token: 0x06000BAA RID: 2986
        [OperationContract(Action = "http://tempuri.org/IDayProfitLossService/GetTotalProfit", ReplyAction = "http://tempuri.org/IDayProfitLossService/GetTotalProfitResponse")]
        long GetTotalProfit(Certification certification, long userAccountId);
    }
}
