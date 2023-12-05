using Goodbyte.TradingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VisionAsset.Client.OrderService
{
	[ServiceContract]
	public interface IOrderService
	{
        // Token: 0x06000ADC RID: 2780
        [OperationContract(Action = "http://tempuri.org/IOrderService/GetUnclearOrders", ReplyAction = "http://tempuri.org/IOrderService/GetUnclearOrdersResponse")]
        List<Order> GetUnclearOrders(Certification certification, long userAccountId);

        // Token: 0x06000ADD RID: 2781
        [OperationContract(Action = "http://tempuri.org/IOrderService/GetAllUnclearOrders", ReplyAction = "http://tempuri.org/IOrderService/GetAllUnclearOrdersResponse")]
        List<Order> GetAllUnclearOrders(Certification certification);

        // Token: 0x06000ADE RID: 2782
        [OperationContract(Action = "http://tempuri.org/IOrderService/GetExpertUnclearOrders", ReplyAction = "http://tempuri.org/IOrderService/GetExpertUnclearOrdersResponse")]
        List<Order> GetExpertUnclearOrders(Certification certification, string expertId);

        // Token: 0x06000ADF RID: 2783
        [OperationContract(Action = "http://tempuri.org/IOrderService/GetExpertDayOrders", ReplyAction = "http://tempuri.org/IOrderService/GetExpertDayOrdersResponse")]
        List<Order> GetExpertDayOrders(Certification certification, string expertId, long userAccountId, DateTime marketDate);

        // Token: 0x06000AE0 RID: 2784
        [OperationContract(Action = "http://tempuri.org/IOrderService/GetDayOrders", ReplyAction = "http://tempuri.org/IOrderService/GetDayOrdersResponse")]
        List<Order> GetDayOrders(Certification certification, long userAccountId, DateTime marketDate);

        // Token: 0x06000AE1 RID: 2785
        [OperationContract(Action = "http://tempuri.org/IOrderService/GetAcceptable", ReplyAction = "http://tempuri.org/IOrderService/GetAcceptableResponse")]
        List<int> GetAcceptable(Certification certification, long userAccountId, long marketId);

        // Token: 0x06000AE2 RID: 2786
        [OperationContract(Action = "http://tempuri.org/IOrderService/NewOrder", ReplyAction = "http://tempuri.org/IOrderService/NewOrderResponse")]
        void NewOrder(Certification certification, List<Order> newOrders);

        // Token: 0x06000AE3 RID: 2787
        [OperationContract(Action = "http://tempuri.org/IOrderService/ChangeOrder", ReplyAction = "http://tempuri.org/IOrderService/ChangeOrderResponse")]
        void ChangeOrder(Certification certification, List<Order> changeOrders);

        // Token: 0x06000AE4 RID: 2788
        [OperationContract(Action = "http://tempuri.org/IOrderService/CancelOrder", ReplyAction = "http://tempuri.org/IOrderService/CancelOrderResponse")]
        void CancelOrder(Certification certification, List<Order> cancelOrders);

        // Token: 0x06000AE5 RID: 2789
        [OperationContract(Action = "http://tempuri.org/IOrderService/ConcludeOrder", ReplyAction = "http://tempuri.org/IOrderService/ConcludeOrderResponse")]
        void ConcludeOrder(Certification certification, Order order, int conclusionQty, double conclusionPrice);

        // Token: 0x06000AE6 RID: 2790
        [OperationContract(Action = "http://tempuri.org/IOrderService/SuperOrder", ReplyAction = "http://tempuri.org/IOrderService/SuperOrderResponse")]
        void SuperOrder(Certification certification1, Certification certification2, List<Order> newOrders, double orderPrice, TimeSpan timeSpan);
    }
}

