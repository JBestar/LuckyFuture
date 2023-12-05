using Goodbyte.TradingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace VisionAsset.Client.UserAccountService
{
	public class UserAccountServiceClient : ServiceBase<IUserAccountService>, IUserAccountService
	{
		// Token: 0x060008E9 RID: 2281 RVA: 0x0002CEBB File Offset: 0x0002B0BB
		public UserAccountServiceClient()
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
				"net.tcp://218.239.223.29/Goodbyte/TradingSystem/Service/UserAccountService"
			);
		}

			// Token: 0x060008ED RID: 2285 RVA: 0x0002CEE0 File Offset: 0x0002B0E0
		public UserAccountServiceClient(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
		{
		}

        // Token: 0x06000A86 RID: 2694 RVA: 0x0003B627 File Offset: 0x00039827
        public List<UserAccount> GetAllUserAccounts(Certification certification)
        {
            return base.Channel.GetAllUserAccounts(certification);
        }

        // Token: 0x06000A87 RID: 2695 RVA: 0x0003B635 File Offset: 0x00039835
        public List<UserAccount> GetExpertUserAccounts(Certification certification, string expertId)
        {
            return base.Channel.GetExpertUserAccounts(certification, expertId);
        }

        // Token: 0x06000A88 RID: 2696 RVA: 0x0003B644 File Offset: 0x00039844
        public List<UserAccount> GetUserAccounts(Certification certification, long userId)
        {
            return base.Channel.GetUserAccounts(certification, userId);
        }

        // Token: 0x06000A89 RID: 2697 RVA: 0x0003B653 File Offset: 0x00039853
        public List<UserAccount> GetTodayUsingUserAccounts(Certification certification)
        {
            return base.Channel.GetTodayUsingUserAccounts(certification);
        }

        // Token: 0x06000A8A RID: 2698 RVA: 0x0003B661 File Offset: 0x00039861
        public UserAccount GetUserAccount(Certification certification, long userAccountId)
        {
            return base.Channel.GetUserAccount(certification, userAccountId);
        }

        // Token: 0x06000A8B RID: 2699 RVA: 0x0003B670 File Offset: 0x00039870
        public void CreateUserAccount(Certification certification, UserAccount userAccount)
        {
            base.Channel.CreateUserAccount(certification, userAccount);
        }

        // Token: 0x06000A8C RID: 2700 RVA: 0x0003B67F File Offset: 0x0003987F
        public void UpdateUserAccount(Certification certification, UserAccount userAccount)
        {
            base.Channel.UpdateUserAccount(certification, userAccount);
        }

        // Token: 0x06000A8D RID: 2701 RVA: 0x0003B68E File Offset: 0x0003988E
        public void DeleteUserAccount(Certification certification, long targetId)
        {
            base.Channel.DeleteUserAccount(certification, targetId);
        }

        // Token: 0x06000A8E RID: 2702 RVA: 0x0003B69D File Offset: 0x0003989D
        public void ChangeLeverage(Certification certification, long userAccountId, int newLeverage)
        {
            base.Channel.ChangeLeverage(certification, userAccountId, newLeverage);
        }

        // Token: 0x06000A8F RID: 2703 RVA: 0x0003B6AD File Offset: 0x000398AD
        public void ChangeOvernight(Certification certification, UserAccount userAccount)
        {
            base.Channel.ChangeOvernight(certification, userAccount);
        }

        // Token: 0x06000A90 RID: 2704 RVA: 0x0003B6BC File Offset: 0x000398BC
        public void ChangeOvernightByAll(Certification certification, bool isFutures, bool isFuturesOvernight, bool isFuturesOvernightSettingPermission, bool isOption, bool isOptionOvernight, bool isOptionOvernightSettingPermission, bool isForeign, bool isForeignOvernight, bool isForeignOvernightSettingPermission, bool isStock, bool isStockOvernight, bool isStockOvernightSettingPermission)
        {
            base.Channel.ChangeOvernightByAll(certification, isFutures, isFuturesOvernight, isFuturesOvernightSettingPermission, isOption, isOptionOvernight, isOptionOvernightSettingPermission, isForeign, isForeignOvernight, isForeignOvernightSettingPermission, isStock, isStockOvernight, isStockOvernightSettingPermission);
        }

        // Token: 0x06000A91 RID: 2705 RVA: 0x0003B6EB File Offset: 0x000398EB
        public void ChangeOrderSignal(Certification certification, UserAccount userAccount)
        {
            base.Channel.ChangeOrderSignal(certification, userAccount);
        }

        // Token: 0x06000A92 RID: 2706 RVA: 0x0003B6FA File Offset: 0x000398FA
        public long GetValuation(Certification certification, long userAccountId)
        {
            return base.Channel.GetValuation(certification, userAccountId);
        }

        // Token: 0x06000A93 RID: 2707 RVA: 0x0003B709 File Offset: 0x00039909
        public long GetOvernightNeedBalance(Certification certification, long userAccountId, bool isFuturesOvernight, bool isOptionOvernight, bool isForeignOvernight, bool isStockOvernight)
        {
            return base.Channel.GetOvernightNeedBalance(certification, userAccountId, isFuturesOvernight, isOptionOvernight, isForeignOvernight, isStockOvernight);
        }

        // Token: 0x06000A94 RID: 2708 RVA: 0x0003B71F File Offset: 0x0003991F
        public void CreateBitcoinAddress(Certification certification, long userAccountId)
        {
            base.Channel.CreateBitcoinAddress(certification, userAccountId);
        }

        // Token: 0x06000A95 RID: 2709 RVA: 0x0003B72E File Offset: 0x0003992E
        public bool CreateBitcoinAddressCallback(string phone, string bitcoinAddress)
        {
            return base.Channel.CreateBitcoinAddressCallback(phone, bitcoinAddress);
        }
    }
}

