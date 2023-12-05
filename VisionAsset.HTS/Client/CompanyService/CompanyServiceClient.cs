using Goodbyte.TradingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VisionAsset.Client.CompanyService
{
	public class CompanyServiceClient : ServiceBase<ICompanyService>, ICompanyService
	{
		// Token: 0x06000A43 RID: 2627 RVA: 0x0002D974 File Offset: 0x0002BB74
		public CompanyServiceClient()
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
				"net.tcp://218.239.223.29/Goodbyte/TradingSystem/Service/CompanyService"
			);
		}

        // Token: 0x06000BF8 RID: 3064 RVA: 0x0003C1E8 File Offset: 0x0003A3E8
        public List<Company> GetCompanies(Certification certification)
        {
            return base.Channel.GetCompanies(certification);
        }

        // Token: 0x06000BF9 RID: 3065 RVA: 0x0003C1F6 File Offset: 0x0003A3F6
        public Company GetCompany(Certification certification, long companyId)
        {
            return base.Channel.GetCompany(certification, companyId);
        }

        // Token: 0x06000BFA RID: 3066 RVA: 0x0003C205 File Offset: 0x0003A405
        public void CreateCompany(Certification certification, Company company)
        {
            base.Channel.CreateCompany(certification, company);
        }

        // Token: 0x06000BFB RID: 3067 RVA: 0x0003C214 File Offset: 0x0003A414
        public void UpdateCompany(Certification certification, Company company)
        {
            base.Channel.UpdateCompany(certification, company);
        }

        // Token: 0x06000BFC RID: 3068 RVA: 0x0003C223 File Offset: 0x0003A423
        public void DeleteCompany(Certification certification, long targetId)
        {
            base.Channel.DeleteCompany(certification, targetId);
        }
    }
}

