using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Goodbyte.TradingSystem.Client.RealTimeService
{
	public interface IRealTimeServiceCallback
	{
		// Token: 0x06000923 RID: 2339
		[OperationContract(Action = "http://tempuri.org/IRealTimeService/SendQuote", ReplyAction = "http://tempuri.org/IRealTimeService/SendQuoteResponse")]
		void SendQuote(Quote quote);

		// Token: 0x06000924 RID: 2340
		[OperationContract(Action = "http://tempuri.org/IRealTimeService/SendCurrent", ReplyAction = "http://tempuri.org/IRealTimeService/SendCurrentResponse")]
		void SendCurrent(Current current);

		// Token: 0x06000925 RID: 2341
		[OperationContract(Action = "http://tempuri.org/IRealTimeService/SendOrder", ReplyAction = "http://tempuri.org/IRealTimeService/SendOrderResponse")]
		void SendOrder(OrderResult orderResult);

		// Token: 0x06000926 RID: 2342
		[OperationContract(Action = "http://tempuri.org/IRealTimeService/SendMitOrder", ReplyAction = "http://tempuri.org/IRealTimeService/SendMitOrderResponse")]
		void SendMitOrder(MitOrderResult mitOrderResult);

		// Token: 0x06000927 RID: 2343
		[OperationContract(Action = "http://tempuri.org/IRealTimeService/SendChangeStateSignal", ReplyAction = "http://tempuri.org/IRealTimeService/SendChangeStateSignalResponse")]
		void SendChangeStateSignal(long userId);
	}
}
