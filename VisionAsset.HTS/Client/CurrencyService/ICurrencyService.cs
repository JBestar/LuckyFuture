using Goodbyte.TradingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VisionAsset.Client.CurrencyService
{
	[ServiceContract]
	public interface ICurrencyService
	{
		// Token: 0x06000A16 RID: 2582
		[OperationContract(Action = "http://tempuri.org/ICurrencyService/GetCurrencies", ReplyAction = "http://tempuri.org/ICurrencyService/GetCurrenciesResponse")]
		List<Currency> GetCurrencies(Certification certification);

		// Token: 0x06000A17 RID: 2583
		[OperationContract(Action = "http://tempuri.org/ICurrencyService/GetCurrency", ReplyAction = "http://tempuri.org/ICurrencyService/GetCurrencyResponse")]
		Currency GetCurrency(Certification certification, CurrencyType targetId);

		// Token: 0x06000A18 RID: 2584
		[OperationContract(Action = "http://tempuri.org/ICurrencyService/CreateCurrency", ReplyAction = "http://tempuri.org/ICurrencyService/CreateCurrencyResponse")]
		void CreateCurrency(Certification certification, Currency currency);

		// Token: 0x06000A19 RID: 2585
		[OperationContract(Action = "http://tempuri.org/ICurrencyService/UpdateCurrency", ReplyAction = "http://tempuri.org/ICurrencyService/UpdateCurrencyResponse")]
		void UpdateCurrency(Certification certification, Currency currency);

		// Token: 0x06000A1A RID: 2586
		[OperationContract(Action = "http://tempuri.org/ICurrencyService/DeleteCurrency", ReplyAction = "http://tempuri.org/ICurrencyService/DeleteCurrencyResponse")]
		void DeleteCurrency(Certification certification, CurrencyType targetId);

		// Token: 0x06000A1B RID: 2587
		[OperationContract(Action = "http://tempuri.org/ICurrencyService/UpdateExchange", ReplyAction = "http://tempuri.org/ICurrencyService/UpdateExchangeResponse")]
		void UpdateExchange(Certification certification);
	}
}
