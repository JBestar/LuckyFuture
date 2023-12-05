using Goodbyte.TradingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Goodbyte.TradingSystem.Client.RealTimeService
{
	[ServiceContract(CallbackContract = typeof(IRealTimeServiceCallback), SessionMode = SessionMode.Required)]
	public interface IRealTimeService
	{
		// Token: 0x0600091F RID: 2335
		[OperationContract(Action = "http://tempuri.org/IRealTimeService/Subscribe", ReplyAction = "http://tempuri.org/IRealTimeService/SubscribeResponse")]
		string Subscribe(Certification certification);

		// Token: 0x06000920 RID: 2336
		[OperationContract(IsOneWay = true, Action = "http://tempuri.org/IRealTimeService/Unsubscribe")]
		void Unsubscribe();

		// Token: 0x06000921 RID: 2337
		[OperationContract(Action = "http://tempuri.org/IRealTimeService/UpdateClientItems", ReplyAction = "http://tempuri.org/IRealTimeService/UpdateClientItemsResponse")]
		void UpdateClientItems(string sessionId, Dictionary<long, Item> clientItems);

		// Token: 0x06000922 RID: 2338
		[OperationContract(Action = "http://tempuri.org/IRealTimeService/ConnectionTest", ReplyAction = "http://tempuri.org/IRealTimeService/ConnectionTestResponse")]
		bool ConnectionTest();
	}
}
