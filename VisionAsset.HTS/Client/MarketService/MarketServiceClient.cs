using Goodbyte.TradingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VisionAsset.Client.MarketService
{
	public class MarketServiceClient : ServiceBase<IMarketService>, IMarketService
	{
		// Token: 0x06000993 RID: 2451 RVA: 0x0002D3AC File Offset: 0x0002B5AC
		public MarketServiceClient()
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
				"net.tcp://218.239.223.29/Goodbyte/TradingSystem/Service/MarketService"
			);
		}

        // Token: 0x06000B34 RID: 2868 RVA: 0x0003BB81 File Offset: 0x00039D81
        public List<Market> GetMarkets(Certification certification)
        {
            return base.Channel.GetMarkets(certification);
        }

        // Token: 0x06000B35 RID: 2869 RVA: 0x0003BB8F File Offset: 0x00039D8F
        public Market GetMarket(Certification certification, long marketId)
        {
            return base.Channel.GetMarket(certification, marketId);
        }

        // Token: 0x06000B36 RID: 2870 RVA: 0x0003BB9E File Offset: 0x00039D9E
        public Market GetMarketByMarketDateAndItemType(Certification certification, DateTime marketDate, ItemType itemType)
        {
            return base.Channel.GetMarketByMarketDateAndItemType(certification, marketDate, itemType);
        }

        // Token: 0x06000B37 RID: 2871 RVA: 0x0003BBAE File Offset: 0x00039DAE
        public List<Market> GetDayMarkets(Certification certification, DateTime marketDate)
        {
            return base.Channel.GetDayMarkets(certification, marketDate);
        }

        // Token: 0x06000B38 RID: 2872 RVA: 0x0003BBBD File Offset: 0x00039DBD
        public List<Market> GetTodayMarkets(Certification certification)
        {
            return base.Channel.GetTodayMarkets(certification);
        }

        // Token: 0x06000B39 RID: 2873 RVA: 0x0003BBCB File Offset: 0x00039DCB
        public DateTime GetLatestMarketDate(Certification certification)
        {
            return base.Channel.GetLatestMarketDate(certification);
        }

        // Token: 0x06000B3A RID: 2874 RVA: 0x0003BBD9 File Offset: 0x00039DD9
        public ItemType GetItemType(Certification certification, long marketId)
        {
            return base.Channel.GetItemType(certification, marketId);
        }

        // Token: 0x06000B3B RID: 2875 RVA: 0x0003BBE8 File Offset: 0x00039DE8
        public void CreateMarket(Certification certification, Market market)
        {
            base.Channel.CreateMarket(certification, market);
        }

        // Token: 0x06000B3C RID: 2876 RVA: 0x0003BBF7 File Offset: 0x00039DF7
        public void CreateUseItemMarket(Certification certification, DateTime marketDate)
        {
            base.Channel.CreateUseItemMarket(certification, marketDate);
        }

        // Token: 0x06000B3D RID: 2877 RVA: 0x0003BC06 File Offset: 0x00039E06
        public void UpdateMarket(Certification certification, Market market)
        {
            base.Channel.UpdateMarket(certification, market);
        }

        // Token: 0x06000B3E RID: 2878 RVA: 0x0003BC15 File Offset: 0x00039E15
        public void UpdateMarketByItemType(Certification certification, ItemType itemType, Market marketInfo)
        {
            base.Channel.UpdateMarketByItemType(certification, itemType, marketInfo);
        }

        // Token: 0x06000B3F RID: 2879 RVA: 0x0003BC25 File Offset: 0x00039E25
        public void DeleteMarket(Certification certification, long targetId)
        {
            base.Channel.DeleteMarket(certification, targetId);
        }

        // Token: 0x06000B40 RID: 2880 RVA: 0x0003BC34 File Offset: 0x00039E34
        public void DeletePriceRangeOptionMarket(Certification certification, DateTime marketDate, double upPrice, double downPrice)
        {
            base.Channel.DeletePriceRangeOptionMarket(certification, marketDate, upPrice, downPrice);
        }

        // Token: 0x06000B41 RID: 2881 RVA: 0x0003BC46 File Offset: 0x00039E46
        public void DeleteOldMarket(Certification certification, DateTime marketDate)
        {
            base.Channel.DeleteOldMarket(certification, marketDate);
        }

        // Token: 0x06000B42 RID: 2882 RVA: 0x0003BC55 File Offset: 0x00039E55
        public void Delete7DaysOldMarket(Certification certification)
        {
            base.Channel.Delete7DaysOldMarket(certification);
        }

        // Token: 0x06000B43 RID: 2883 RVA: 0x0003BC63 File Offset: 0x00039E63
        public void Delete30DaysOldMarket(Certification certification)
        {
            base.Channel.Delete30DaysOldMarket(certification);
        }
    }
}

