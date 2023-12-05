using Goodbyte.TradingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VisionAsset.Client.ConnectionService
{
	[ServiceContract]
	public interface IConnectionService
	{
        [OperationContract(Action = "http://tempuri.org/IConnectionService/GetConnections", ReplyAction = "http://tempuri.org/IConnectionService/GetConnectionsResponse")]
        List<Connection> GetConnections(Certification certification);

        // Token: 0x06000BD6 RID: 3030
        [OperationContract(Action = "http://tempuri.org/IConnectionService/GetDayRangeConnections", ReplyAction = "http://tempuri.org/IConnectionService/GetDayRangeConnectionsResponse")]
        List<Connection> GetDayRangeConnections(Certification certification, DateTime startQueryDate, DateTime endQueryDate);

        // Token: 0x06000BD7 RID: 3031
        [OperationContract(Action = "http://tempuri.org/IConnectionService/GetDayConnections", ReplyAction = "http://tempuri.org/IConnectionService/GetDayConnectionsResponse")]
        List<Connection> GetDayConnections(Certification certification, DateTime date);

        // Token: 0x06000BD8 RID: 3032
        [OperationContract(Action = "http://tempuri.org/IConnectionService/GetConnection", ReplyAction = "http://tempuri.org/IConnectionService/GetConnectionResponse")]
        Connection GetConnection(Certification certification, long connectionId);

        // Token: 0x06000BD9 RID: 3033
        [OperationContract(Action = "http://tempuri.org/IConnectionService/CreateConnection", ReplyAction = "http://tempuri.org/IConnectionService/CreateConnectionResponse")]
        void CreateConnection(Certification certification, Connection connection);

        // Token: 0x06000BDA RID: 3034
        [OperationContract(Action = "http://tempuri.org/IConnectionService/UpdateConnection", ReplyAction = "http://tempuri.org/IConnectionService/UpdateConnectionResponse")]
        void UpdateConnection(Certification certification, Connection connection);

        // Token: 0x06000BDB RID: 3035
        [OperationContract(Action = "http://tempuri.org/IConnectionService/IsLogin", ReplyAction = "http://tempuri.org/IConnectionService/IsLoginResponse")]
        bool IsLogin(Certification certification, long userId);

        // Token: 0x06000BDC RID: 3036
        [OperationContract(Action = "http://tempuri.org/IConnectionService/GetLoginState", ReplyAction = "http://tempuri.org/IConnectionService/GetLoginStateResponse")]
        ConnectionType GetLoginState(Certification certification, long userId);

        // Token: 0x06000BDD RID: 3037
        [OperationContract(Action = "http://tempuri.org/IConnectionService/DisconnectUser", ReplyAction = "http://tempuri.org/IConnectionService/DisconnectUserResponse")]
        void DisconnectUser(Certification certification, Connection connection);

        // Token: 0x06000BDE RID: 3038
        [OperationContract(Action = "http://tempuri.org/IConnectionService/GetIpAdressInfo", ReplyAction = "http://tempuri.org/IConnectionService/GetIpAdressInfoResponse")]
        Dictionary<string, string> GetIpAdressInfo(Certification certification, string realIp);
    }
}

