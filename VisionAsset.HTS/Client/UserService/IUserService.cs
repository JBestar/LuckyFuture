using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Goodbyte.TradingSystem.Domain.Entities;

namespace VisionAsset.Client.UserService
{
	[ServiceContract]
    public interface IUserService
    {
        // Token: 0x06000A37 RID: 2615
        [OperationContract(Action = "http://tempuri.org/IUserService/GetAllUsers", ReplyAction = "http://tempuri.org/IUserService/GetAllUsersResponse")]
        List<User> GetAllUsers(Certification certification);

        // Token: 0x06000A38 RID: 2616
        [OperationContract(Action = "http://tempuri.org/IUserService/GetCompanyUsers", ReplyAction = "http://tempuri.org/IUserService/GetCompanyUsersResponse")]
        List<User> GetCompanyUsers(Certification certification, long companyId);

        // Token: 0x06000A39 RID: 2617
        [OperationContract(Action = "http://tempuri.org/IUserService/GetExpertUsers", ReplyAction = "http://tempuri.org/IUserService/GetExpertUsersResponse")]
        List<User> GetExpertUsers(Certification certification, long companyId, string expertLoginId);

        // Token: 0x06000A3A RID: 2618
        [OperationContract(Action = "http://tempuri.org/IUserService/GetUser", ReplyAction = "http://tempuri.org/IUserService/GetUserResponse")]
        User GetUser(Certification certification, long userId);

        // Token: 0x06000A3B RID: 2619
        [OperationContract(Action = "http://tempuri.org/IUserService/CreateUser", ReplyAction = "http://tempuri.org/IUserService/CreateUserResponse")]
        void CreateUser(Certification certification, User user);

        // Token: 0x06000A3C RID: 2620
        [OperationContract(Action = "http://tempuri.org/IUserService/UpdateUser", ReplyAction = "http://tempuri.org/IUserService/UpdateUserResponse")]
        void UpdateUser(Certification certification, User user);

        // Token: 0x06000A3D RID: 2621
        [OperationContract(Action = "http://tempuri.org/IUserService/DeleteUser", ReplyAction = "http://tempuri.org/IUserService/DeleteUserResponse")]
        void DeleteUser(Certification certification, long targetId);

        // Token: 0x06000A3E RID: 2622
        [OperationContract(Action = "http://tempuri.org/IUserService/ChangePassword", ReplyAction = "http://tempuri.org/IUserService/ChangePasswordResponse")]
        bool ChangePassword(Certification certification, [MessageParameter(Name = "changePassword")] string changePassword1);

        // Token: 0x06000A3F RID: 2623
        [OperationContract(Action = "http://tempuri.org/IUserService/UpdateLatestLoginDate", ReplyAction = "http://tempuri.org/IUserService/UpdateLatestLoginDateResponse")]
        void UpdateLatestLoginDate(Certification certification);

        // Token: 0x06000A40 RID: 2624
        [OperationContract(Action = "http://tempuri.org/IUserService/LoginUser", ReplyAction = "http://tempuri.org/IUserService/LoginUserResponse")]
        User LoginUser(long companyId, string loginId, string userPassword);

        // Token: 0x06000A41 RID: 2625
        [OperationContract(Action = "http://tempuri.org/IUserService/CheckLoginId", ReplyAction = "http://tempuri.org/IUserService/CheckLoginIdResponse")]
        bool CheckLoginId(long companyId, string loginId);

        // Token: 0x06000A42 RID: 2626
        [OperationContract(Action = "http://tempuri.org/IUserService/CheckNickName", ReplyAction = "http://tempuri.org/IUserService/CheckNickNameResponse")]
        bool CheckNickName(long companyId, string loginName);

        // Token: 0x06000A43 RID: 2627
        [OperationContract(Action = "http://tempuri.org/IUserService/Join", ReplyAction = "http://tempuri.org/IUserService/JoinResponse")]
        void Join(User user);

        // Token: 0x06000A44 RID: 2628
        [OperationContract(Action = "http://tempuri.org/IUserService/JoinByWeb", ReplyAction = "http://tempuri.org/IUserService/JoinByWebResponse")]
        long JoinByWeb(User user);
    }
}
