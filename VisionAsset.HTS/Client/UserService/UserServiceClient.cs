using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Goodbyte.TradingSystem.Domain.Entities;

namespace VisionAsset.Client.UserService
{
	public class UserServiceClient : ServiceBase<IUserService>, IUserService
	{
		public UserServiceClient()
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
				"net.tcp://218.239.223.29/Goodbyte/TradingSystem/Service/UserService"
			);
		}

        // Token: 0x06000A4A RID: 2634 RVA: 0x0003B464 File Offset: 0x00039664
        public List<User> GetAllUsers(Certification certification)
        {
            return base.Channel.GetAllUsers(certification);
        }

        // Token: 0x06000A4B RID: 2635 RVA: 0x0003B472 File Offset: 0x00039672
        public List<User> GetCompanyUsers(Certification certification, long companyId)
        {
            return base.Channel.GetCompanyUsers(certification, companyId);
        }

        // Token: 0x06000A4C RID: 2636 RVA: 0x0003B481 File Offset: 0x00039681
        public List<User> GetExpertUsers(Certification certification, long companyId, string expertLoginId)
        {
            return base.Channel.GetExpertUsers(certification, companyId, expertLoginId);
        }

        // Token: 0x06000A4D RID: 2637 RVA: 0x0003B491 File Offset: 0x00039691
        public User GetUser(Certification certification, long userId)
        {
            return base.Channel.GetUser(certification, userId);
        }

        // Token: 0x06000A4E RID: 2638 RVA: 0x0003B4A0 File Offset: 0x000396A0
        public void CreateUser(Certification certification, User user)
        {
            base.Channel.CreateUser(certification, user);
        }

        // Token: 0x06000A4F RID: 2639 RVA: 0x0003B4AF File Offset: 0x000396AF
        public void UpdateUser(Certification certification, User user)
        {
            base.Channel.UpdateUser(certification, user);
        }

        // Token: 0x06000A50 RID: 2640 RVA: 0x0003B4BE File Offset: 0x000396BE
        public void DeleteUser(Certification certification, long targetId)
        {
            base.Channel.DeleteUser(certification, targetId);
        }

        // Token: 0x06000A51 RID: 2641 RVA: 0x0003B4CD File Offset: 0x000396CD
        public bool ChangePassword(Certification certification, string changePassword1)
        {
            return base.Channel.ChangePassword(certification, changePassword1);
        }

        // Token: 0x06000A52 RID: 2642 RVA: 0x0003B4DC File Offset: 0x000396DC
        public void UpdateLatestLoginDate(Certification certification)
        {
            base.Channel.UpdateLatestLoginDate(certification);
        }

        // Token: 0x06000A53 RID: 2643 RVA: 0x0003B4EA File Offset: 0x000396EA
        public User LoginUser(long companyId, string loginId, string userPassword)
        {
            return base.Channel.LoginUser(companyId, loginId, userPassword);
        }

        // Token: 0x06000A54 RID: 2644 RVA: 0x0003B4FA File Offset: 0x000396FA
        public bool CheckLoginId(long companyId, string loginId)
        {
            return base.Channel.CheckLoginId(companyId, loginId);
        }

        // Token: 0x06000A55 RID: 2645 RVA: 0x0003B509 File Offset: 0x00039709
        public bool CheckNickName(long companyId, string loginName)
        {
            return base.Channel.CheckNickName(companyId, loginName);
        }

        // Token: 0x06000A56 RID: 2646 RVA: 0x0003B518 File Offset: 0x00039718
        public void Join(User user)
        {
            base.Channel.Join(user);
        }

        // Token: 0x06000A57 RID: 2647 RVA: 0x0003B526 File Offset: 0x00039726
        public long JoinByWeb(User user)
        {
            return base.Channel.JoinByWeb(user);
        }
    }
}
