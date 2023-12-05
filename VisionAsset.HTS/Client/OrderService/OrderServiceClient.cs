using Goodbyte.TradingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace VisionAsset.Client.OrderService
{
	public class OrderServiceClient : ServiceBase<IOrderService>, IOrderService
	{
		// Token: 0x0600094C RID: 2380 RVA: 0x0002D1A0 File Offset: 0x0002B3A0
		public OrderServiceClient()
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
				"net.tcp://218.239.223.29/Goodbyte/TradingSystem/Service/OrderService"
			);
		}

        // Token: 0x06000AEC RID: 2796 RVA: 0x0003B961 File Offset: 0x00039B61
        public List<Order> GetUnclearOrders(Certification certification, long userAccountId)
        {
            return base.Channel.GetUnclearOrders(certification, userAccountId);
        }

        // Token: 0x06000AED RID: 2797 RVA: 0x0003B970 File Offset: 0x00039B70
        public List<Order> GetAllUnclearOrders(Certification certification)
        {
            return base.Channel.GetAllUnclearOrders(certification);
        }

        // Token: 0x06000AEE RID: 2798 RVA: 0x0003B97E File Offset: 0x00039B7E
        public List<Order> GetExpertUnclearOrders(Certification certification, string expertId)
        {
            return base.Channel.GetExpertUnclearOrders(certification, expertId);
        }

        // Token: 0x06000AEF RID: 2799 RVA: 0x0003B98D File Offset: 0x00039B8D
        public List<Order> GetExpertDayOrders(Certification certification, string expertId, long userAccountId, DateTime marketDate)
        {
            return base.Channel.GetExpertDayOrders(certification, expertId, userAccountId, marketDate);
        }

        // Token: 0x06000AF0 RID: 2800 RVA: 0x0003B99F File Offset: 0x00039B9F
        public List<Order> GetDayOrders(Certification certification, long userAccountId, DateTime marketDate)
        {
            return base.Channel.GetDayOrders(certification, userAccountId, marketDate);
        }

        // Token: 0x06000AF1 RID: 2801 RVA: 0x0003B9AF File Offset: 0x00039BAF
        public List<int> GetAcceptable(Certification certification, long userAccountId, long marketId)
        {
            return base.Channel.GetAcceptable(certification, userAccountId, marketId);
        }

        // Token: 0x06000AF2 RID: 2802 RVA: 0x0003B9BF File Offset: 0x00039BBF
        public void NewOrder(Certification certification, List<Order> newOrders)
        {
            base.Channel.NewOrder(certification, newOrders);
        }

        // Token: 0x06000AF3 RID: 2803 RVA: 0x0003B9CE File Offset: 0x00039BCE
        public void ChangeOrder(Certification certification, List<Order> changeOrders)
        {
            base.Channel.ChangeOrder(certification, changeOrders);
        }

        // Token: 0x06000AF4 RID: 2804 RVA: 0x0003B9DD File Offset: 0x00039BDD
        public void CancelOrder(Certification certification, List<Order> cancelOrders)
        {
            base.Channel.CancelOrder(certification, cancelOrders);
        }

        // Token: 0x06000AF5 RID: 2805 RVA: 0x0003B9EC File Offset: 0x00039BEC
        public void ConcludeOrder(Certification certification, Order order, int conclusionQty, double conclusionPrice)
        {
            base.Channel.ConcludeOrder(certification, order, conclusionQty, conclusionPrice);
        }

        // Token: 0x06000AF6 RID: 2806 RVA: 0x0003B9FE File Offset: 0x00039BFE
        public void SuperOrder(Certification certification1, Certification certification2, List<Order> newOrders, double orderPrice, TimeSpan timeSpan)
        {
            base.Channel.SuperOrder(certification1, certification2, newOrders, orderPrice, timeSpan);
        }
    }
}

