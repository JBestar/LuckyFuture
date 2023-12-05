using Goodbyte.TradingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VisionAsset.Client.CurrencyService
{
	public class CurrencyServiceClient : ServiceBase<ICurrencyService>, ICurrencyService
	{
		// Token: 0x06000A1C RID: 2588 RVA: 0x0002D837 File Offset: 0x0002BA37
		public CurrencyServiceClient()
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
				"net.tcp://218.239.223.29/Goodbyte/TradingSystem/Service/CurrencyService"
			);
		}

        // Token: 0x06000BCF RID: 3023 RVA: 0x0003C09C File Offset: 0x0003A29C
        public List<Currency> GetCurrencies(Certification certification)
        {
            return base.Channel.GetCurrencies(certification);
        }

        // Token: 0x06000BD0 RID: 3024 RVA: 0x0003C0AA File Offset: 0x0003A2AA
        public Currency GetCurrency(Certification certification, CurrencyType targetId)
        {
            return base.Channel.GetCurrency(certification, targetId);
        }

        // Token: 0x06000BD1 RID: 3025 RVA: 0x0003C0B9 File Offset: 0x0003A2B9
        public void CreateCurrency(Certification certification, Currency currency)
        {
            base.Channel.CreateCurrency(certification, currency);
        }

        // Token: 0x06000BD2 RID: 3026 RVA: 0x0003C0C8 File Offset: 0x0003A2C8
        public void UpdateCurrency(Certification certification, Currency currency)
        {
            base.Channel.UpdateCurrency(certification, currency);
        }

        // Token: 0x06000BD3 RID: 3027 RVA: 0x0003C0D7 File Offset: 0x0003A2D7
        public void DeleteCurrency(Certification certification, CurrencyType targetId)
        {
            base.Channel.DeleteCurrency(certification, targetId);
        }

        // Token: 0x06000BD4 RID: 3028 RVA: 0x0003C0E6 File Offset: 0x0003A2E6
        public void UpdateExchange(Certification certification)
        {
            base.Channel.UpdateExchange(certification);
        }
    }
}

