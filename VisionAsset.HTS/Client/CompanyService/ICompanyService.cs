using Goodbyte.TradingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VisionAsset.Client.CompanyService
{
	[ServiceContract]
	public interface ICompanyService
	{
		// Token: 0x06000A3E RID: 2622
		[OperationContract(Action = "http://tempuri.org/ICompanyService/GetCompanies", ReplyAction = "http://tempuri.org/ICompanyService/GetCompaniesResponse")]
		List<Company> GetCompanies(Certification certification);

		// Token: 0x06000A3F RID: 2623
		[OperationContract(Action = "http://tempuri.org/ICompanyService/GetCompany", ReplyAction = "http://tempuri.org/ICompanyService/GetCompanyResponse")]
		Company GetCompany(Certification certification, long companyId);

		// Token: 0x06000A40 RID: 2624
		[OperationContract(Action = "http://tempuri.org/ICompanyService/CreateCompany", ReplyAction = "http://tempuri.org/ICompanyService/CreateCompanyResponse")]
		void CreateCompany(Certification certification, Company company);

		// Token: 0x06000A41 RID: 2625
		[OperationContract(Action = "http://tempuri.org/ICompanyService/UpdateCompany", ReplyAction = "http://tempuri.org/ICompanyService/UpdateCompanyResponse")]
		void UpdateCompany(Certification certification, Company company);

		// Token: 0x06000A42 RID: 2626
		[OperationContract(Action = "http://tempuri.org/ICompanyService/DeleteCompany", ReplyAction = "http://tempuri.org/ICompanyService/DeleteCompanyResponse")]
		void DeleteCompany(Certification certification, long targetId);
	}
}
