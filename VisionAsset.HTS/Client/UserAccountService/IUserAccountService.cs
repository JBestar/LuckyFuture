using Goodbyte.TradingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VisionAsset.Client.UserAccountService
{
	[ServiceContract]
    public interface IUserAccountService
    {
        // Token: 0x06000A71 RID: 2673
        [OperationContract(Action = "http://tempuri.org/IUserAccountService/GetAllUserAccounts", ReplyAction = "http://tempuri.org/IUserAccountService/GetAllUserAccountsResponse")]
        List<UserAccount> GetAllUserAccounts(Certification certification);

        // Token: 0x06000A72 RID: 2674
        [OperationContract(Action = "http://tempuri.org/IUserAccountService/GetExpertUserAccounts", ReplyAction = "http://tempuri.org/IUserAccountService/GetExpertUserAccountsResponse")]
        List<UserAccount> GetExpertUserAccounts(Certification certification, string expertId);

        // Token: 0x06000A73 RID: 2675
        [OperationContract(Action = "http://tempuri.org/IUserAccountService/GetUserAccounts", ReplyAction = "http://tempuri.org/IUserAccountService/GetUserAccountsResponse")]
        List<UserAccount> GetUserAccounts(Certification certification, long userId);

        // Token: 0x06000A74 RID: 2676
        [OperationContract(Action = "http://tempuri.org/IUserAccountService/GetTodayUsingUserAccounts", ReplyAction = "http://tempuri.org/IUserAccountService/GetTodayUsingUserAccountsResponse")]
        List<UserAccount> GetTodayUsingUserAccounts(Certification certification);

        // Token: 0x06000A75 RID: 2677
        [OperationContract(Action = "http://tempuri.org/IUserAccountService/GetUserAccount", ReplyAction = "http://tempuri.org/IUserAccountService/GetUserAccountResponse")]
        UserAccount GetUserAccount(Certification certification, long userAccountId);

        // Token: 0x06000A76 RID: 2678
        [OperationContract(Action = "http://tempuri.org/IUserAccountService/CreateUserAccount", ReplyAction = "http://tempuri.org/IUserAccountService/CreateUserAccountResponse")]
        void CreateUserAccount(Certification certification, UserAccount userAccount);

        // Token: 0x06000A77 RID: 2679
        [OperationContract(Action = "http://tempuri.org/IUserAccountService/UpdateUserAccount", ReplyAction = "http://tempuri.org/IUserAccountService/UpdateUserAccountResponse")]
        void UpdateUserAccount(Certification certification, UserAccount userAccount);

        // Token: 0x06000A78 RID: 2680
        [OperationContract(Action = "http://tempuri.org/IUserAccountService/DeleteUserAccount", ReplyAction = "http://tempuri.org/IUserAccountService/DeleteUserAccountResponse")]
        void DeleteUserAccount(Certification certification, long targetId);

        // Token: 0x06000A79 RID: 2681
        [OperationContract(Action = "http://tempuri.org/IUserAccountService/ChangeLeverage", ReplyAction = "http://tempuri.org/IUserAccountService/ChangeLeverageResponse")]
        void ChangeLeverage(Certification certification, long userAccountId, int newLeverage);

        // Token: 0x06000A7A RID: 2682
        [OperationContract(Action = "http://tempuri.org/IUserAccountService/ChangeOvernight", ReplyAction = "http://tempuri.org/IUserAccountService/ChangeOvernightResponse")]
        void ChangeOvernight(Certification certification, UserAccount userAccount);

        // Token: 0x06000A7B RID: 2683
        [OperationContract(Action = "http://tempuri.org/IUserAccountService/ChangeOvernightByAll", ReplyAction = "http://tempuri.org/IUserAccountService/ChangeOvernightByAllResponse")]
        void ChangeOvernightByAll(Certification certification, bool isFutures, bool isFuturesOvernight, bool isFuturesOvernightSettingPermission, bool isOption, bool isOptionOvernight, bool isOptionOvernightSettingPermission, bool isForeign, bool isForeignOvernight, bool isForeignOvernightSettingPermission, bool isStock, bool isStockOvernight, bool isStockOvernightSettingPermission);

        // Token: 0x06000A7C RID: 2684
        [OperationContract(Action = "http://tempuri.org/IUserAccountService/ChangeOrderSignal", ReplyAction = "http://tempuri.org/IUserAccountService/ChangeOrderSignalResponse")]
        void ChangeOrderSignal(Certification certification, UserAccount userAccount);

        // Token: 0x06000A7D RID: 2685
        [OperationContract(Action = "http://tempuri.org/IUserAccountService/GetValuation", ReplyAction = "http://tempuri.org/IUserAccountService/GetValuationResponse")]
        long GetValuation(Certification certification, long userAccountId);

        // Token: 0x06000A7E RID: 2686
        [OperationContract(Action = "http://tempuri.org/IUserAccountService/GetOvernightNeedBalance", ReplyAction = "http://tempuri.org/IUserAccountService/GetOvernightNeedBalanceResponse")]
        long GetOvernightNeedBalance(Certification certification, long userAccountId, bool isFuturesOvernight, bool isOptionOvernight, bool isForeignOvernight, bool isStockOvernight);

        // Token: 0x06000A7F RID: 2687
        [OperationContract(Action = "http://tempuri.org/IUserAccountService/CreateBitcoinAddress", ReplyAction = "http://tempuri.org/IUserAccountService/CreateBitcoinAddressResponse")]
        void CreateBitcoinAddress(Certification certification, long userAccountId);

        // Token: 0x06000A80 RID: 2688
        [OperationContract(Action = "http://tempuri.org/IUserAccountService/CreateBitcoinAddressCallback", ReplyAction = "http://tempuri.org/IUserAccountService/CreateBitcoinAddressCallbackResponse")]
        bool CreateBitcoinAddressCallback(string phone, string bitcoinAddress);
    }
}
