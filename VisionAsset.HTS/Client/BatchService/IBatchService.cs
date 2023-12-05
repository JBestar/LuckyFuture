using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Goodbyte.TradingSystem.Client.BatchService
{
	[ServiceContract]
	public interface IBatchService
	{
		// Token: 0x06000A61 RID: 2657
		[OperationContract(Action = "http://tempuri.org/IBatchService/LoadItem", ReplyAction = "http://tempuri.org/IBatchService/LoadItemResponse")]
		LoadItemResult LoadItem(Certification certification, long itemId, long userAccountId);

		// Token: 0x06000A62 RID: 2658
		[OperationContract(Action = "http://tempuri.org/IBatchService/LoadSpeedOrderView", ReplyAction = "http://tempuri.org/IBatchService/LoadSpeedOrderViewResponse")]
		LoadSpeedOrderViewResult LoadSpeedOrderView(Certification certification, long companyId, long userId);
	}
}
