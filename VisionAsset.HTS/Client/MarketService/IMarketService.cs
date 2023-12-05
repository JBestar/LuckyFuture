using Goodbyte.TradingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VisionAsset.Client.MarketService
{
	[ServiceContract]
	public interface IMarketService
	{
        // Token: 0x06000B1F RID: 2847
        [OperationContract(Action = "http://tempuri.org/IMarketService/GetMarkets", ReplyAction = "http://tempuri.org/IMarketService/GetMarketsResponse")]
        List<Market> GetMarkets(Certification certification);

        // Token: 0x06000B20 RID: 2848
        [OperationContract(Action = "http://tempuri.org/IMarketService/GetMarket", ReplyAction = "http://tempuri.org/IMarketService/GetMarketResponse")]
        Market GetMarket(Certification certification, long marketId);

        // Token: 0x06000B21 RID: 2849
        [OperationContract(Action = "http://tempuri.org/IMarketService/GetMarketByMarketDateAndItemType", ReplyAction = "http://tempuri.org/IMarketService/GetMarketByMarketDateAndItemTypeResponse")]
        Market GetMarketByMarketDateAndItemType(Certification certification, DateTime marketDate, ItemType itemType);

        // Token: 0x06000B22 RID: 2850
        [OperationContract(Action = "http://tempuri.org/IMarketService/GetDayMarkets", ReplyAction = "http://tempuri.org/IMarketService/GetDayMarketsResponse")]
        List<Market> GetDayMarkets(Certification certification, DateTime marketDate);

        // Token: 0x06000B23 RID: 2851
        [OperationContract(Action = "http://tempuri.org/IMarketService/GetTodayMarkets", ReplyAction = "http://tempuri.org/IMarketService/GetTodayMarketsResponse")]
        List<Market> GetTodayMarkets(Certification certification);

        // Token: 0x06000B24 RID: 2852
        [OperationContract(Action = "http://tempuri.org/IMarketService/GetLatestMarketDate", ReplyAction = "http://tempuri.org/IMarketService/GetLatestMarketDateResponse")]
        DateTime GetLatestMarketDate(Certification certification);

        // Token: 0x06000B25 RID: 2853
        [OperationContract(Action = "http://tempuri.org/IMarketService/GetItemType", ReplyAction = "http://tempuri.org/IMarketService/GetItemTypeResponse")]
        ItemType GetItemType(Certification certification, long marketId);

        // Token: 0x06000B26 RID: 2854
        [OperationContract(Action = "http://tempuri.org/IMarketService/CreateMarket", ReplyAction = "http://tempuri.org/IMarketService/CreateMarketResponse")]
        void CreateMarket(Certification certification, Market market);

        // Token: 0x06000B27 RID: 2855
        [OperationContract(Action = "http://tempuri.org/IMarketService/CreateUseItemMarket", ReplyAction = "http://tempuri.org/IMarketService/CreateUseItemMarketResponse")]
        void CreateUseItemMarket(Certification certification, DateTime marketDate);

        // Token: 0x06000B28 RID: 2856
        [OperationContract(Action = "http://tempuri.org/IMarketService/UpdateMarket", ReplyAction = "http://tempuri.org/IMarketService/UpdateMarketResponse")]
        void UpdateMarket(Certification certification, Market market);

        // Token: 0x06000B29 RID: 2857
        [OperationContract(Action = "http://tempuri.org/IMarketService/UpdateMarketByItemType", ReplyAction = "http://tempuri.org/IMarketService/UpdateMarketByItemTypeResponse")]
        void UpdateMarketByItemType(Certification certification, ItemType itemType, Market marketInfo);

        // Token: 0x06000B2A RID: 2858
        [OperationContract(Action = "http://tempuri.org/IMarketService/DeleteMarket", ReplyAction = "http://tempuri.org/IMarketService/DeleteMarketResponse")]
        void DeleteMarket(Certification certification, long targetId);

        // Token: 0x06000B2B RID: 2859
        [OperationContract(Action = "http://tempuri.org/IMarketService/DeletePriceRangeOptionMarket", ReplyAction = "http://tempuri.org/IMarketService/DeletePriceRangeOptionMarketResponse")]
        void DeletePriceRangeOptionMarket(Certification certification, DateTime marketDate, double upPrice, double downPrice);

        // Token: 0x06000B2C RID: 2860
        [OperationContract(Action = "http://tempuri.org/IMarketService/DeleteOldMarket", ReplyAction = "http://tempuri.org/IMarketService/DeleteOldMarketResponse")]
        void DeleteOldMarket(Certification certification, DateTime marketDate);

        // Token: 0x06000B2D RID: 2861
        [OperationContract(Action = "http://tempuri.org/IMarketService/Delete7DaysOldMarket", ReplyAction = "http://tempuri.org/IMarketService/Delete7DaysOldMarketResponse")]
        void Delete7DaysOldMarket(Certification certification);

        // Token: 0x06000B2E RID: 2862
        [OperationContract(Action = "http://tempuri.org/IMarketService/Delete30DaysOldMarket", ReplyAction = "http://tempuri.org/IMarketService/Delete30DaysOldMarketResponse")]
        void Delete30DaysOldMarket(Certification certification);
    }
}

