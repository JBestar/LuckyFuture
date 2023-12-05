using Goodbyte.TradingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using VisionAsset.Client;

namespace Goodbyte.TradingSystem.Client.RealTimeService
{	
	public class RealTimeServiceClient : DuplexServiceBase<IRealTimeService>, IRealTimeService
	{
		// Token: 0x06000928 RID: 2344 RVA: 0x0002D0AC File Offset: 0x0002B2AC
		public RealTimeServiceClient(InstanceContext callbackInstance) : base(callbackInstance)
		{
			NetTcpBinding binding = new NetTcpBinding
			{
				OpenTimeout = TimeSpan.FromSeconds(10),
				CloseTimeout = TimeSpan.FromSeconds(10),
				SendTimeout = TimeSpan.FromSeconds(10),
				ReceiveTimeout = TimeSpan.FromMinutes(5)
			};

			binding.Security.Mode = SecurityMode.None;

			Create(
				callbackInstance,
				binding,
				"net.tcp://218.239.223.29/Goodbyte/TradingSystem/Service/RealTimeService"
			);
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0002D0E0 File Offset: 0x0002B2E0
		public string Subscribe(Certification certification)
		{
			return base.Channel.Subscribe(certification);
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0002D0EE File Offset: 0x0002B2EE
		public void Unsubscribe()
		{
			base.Channel.Unsubscribe();
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0002D0FB File Offset: 0x0002B2FB
		public void UpdateClientItems(string sessionId, Dictionary<long, Item> clientItems)
		{
			base.Channel.UpdateClientItems(sessionId, clientItems);
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0002D10A File Offset: 0x0002B30A
		public bool ConnectionTest()
		{
			return base.Channel.ConnectionTest();
		}
	}
}
