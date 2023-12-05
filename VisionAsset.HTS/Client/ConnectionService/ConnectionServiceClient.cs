using Goodbyte.TradingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VisionAsset.Client.ConnectionService
{
	public class ConnectionServiceClient : ServiceBase<IConnectionService>, IConnectionService
	{
		// Token: 0x06000A30 RID: 2608 RVA: 0x0002D8BE File Offset: 0x0002BABE
		public ConnectionServiceClient()
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
				"net.tcp://218.239.223.29/Goodbyte/TradingSystem/Service/ConnectionService"
			);
		}

        // Token: 0x06000BE4 RID: 3044 RVA: 0x0003C123 File Offset: 0x0003A323
        public List<Connection> GetConnections(Certification certification)
        {
            return base.Channel.GetConnections(certification);
        }

        // Token: 0x06000BE5 RID: 3045 RVA: 0x0003C131 File Offset: 0x0003A331
        public List<Connection> GetDayRangeConnections(Certification certification, DateTime startQueryDate, DateTime endQueryDate)
        {
            return base.Channel.GetDayRangeConnections(certification, startQueryDate, endQueryDate);
        }

        // Token: 0x06000BE6 RID: 3046 RVA: 0x0003C141 File Offset: 0x0003A341
        public List<Connection> GetDayConnections(Certification certification, DateTime date)
        {
            return base.Channel.GetDayConnections(certification, date);
        }

        // Token: 0x06000BE7 RID: 3047 RVA: 0x0003C150 File Offset: 0x0003A350
        public Connection GetConnection(Certification certification, long connectionId)
        {
            return base.Channel.GetConnection(certification, connectionId);
        }

        // Token: 0x06000BE8 RID: 3048 RVA: 0x0003C15F File Offset: 0x0003A35F
        public void CreateConnection(Certification certification, Connection connection)
        {
            base.Channel.CreateConnection(certification, connection);
        }

        // Token: 0x06000BE9 RID: 3049 RVA: 0x0003C16E File Offset: 0x0003A36E
        public void UpdateConnection(Certification certification, Connection connection)
        {
            base.Channel.UpdateConnection(certification, connection);
        }

        // Token: 0x06000BEA RID: 3050 RVA: 0x0003C17D File Offset: 0x0003A37D
        public bool IsLogin(Certification certification, long userId)
        {
            return base.Channel.IsLogin(certification, userId);
        }

        // Token: 0x06000BEB RID: 3051 RVA: 0x0003C18C File Offset: 0x0003A38C
        public ConnectionType GetLoginState(Certification certification, long userId)
        {
            return base.Channel.GetLoginState(certification, userId);
        }

        // Token: 0x06000BEC RID: 3052 RVA: 0x0003C19B File Offset: 0x0003A39B
        public void DisconnectUser(Certification certification, Connection connection)
        {
            base.Channel.DisconnectUser(certification, connection);
        }

        // Token: 0x06000BED RID: 3053 RVA: 0x0003C1AA File Offset: 0x0003A3AA
        public Dictionary<string, string> GetIpAdressInfo(Certification certification, string realIp)
        {
            return base.Channel.GetIpAdressInfo(certification, realIp);
        }
    }
}

