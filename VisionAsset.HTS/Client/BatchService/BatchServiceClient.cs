using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using VisionAsset.Client;

namespace Goodbyte.TradingSystem.Client.BatchService
{
	public class BatchServiceClient : ServiceBase<IBatchService>, IBatchService
	{
		// Token: 0x06000A63 RID: 2659 RVA: 0x0002DA95 File Offset: 0x0002BC95
		public BatchServiceClient()
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
				"net.tcp://218.239.223.29/Goodbyte/TradingSystem/Service/BatchService"
			);
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x0002DAC4 File Offset: 0x0002BCC4
		public LoadItemResult LoadItem(Certification certification, long itemId, long userAccountId)
		{
			return base.Channel.LoadItem(certification, itemId, userAccountId);
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x0002DAD4 File Offset: 0x0002BCD4
		public LoadSpeedOrderViewResult LoadSpeedOrderView(Certification certification, long companyId, long userId)
		{
			return base.Channel.LoadSpeedOrderView(certification, companyId, userId);
		}
	}
}
