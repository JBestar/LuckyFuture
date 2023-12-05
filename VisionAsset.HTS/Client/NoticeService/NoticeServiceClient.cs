using Goodbyte.TradingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VisionAsset.Client.NoticeService
{
	public class NoticeServiceClient : ServiceBase<INoticeService>, INoticeService
	{
		// Token: 0x06000961 RID: 2401 RVA: 0x0002D26C File Offset: 0x0002B46C
		public NoticeServiceClient()
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
				"net.tcp://218.239.223.29/Goodbyte/TradingSystem/Service/NoticeService"
			);
		}

        // Token: 0x06000B02 RID: 2818 RVA: 0x0003BA41 File Offset: 0x00039C41
        public List<Notice> GetNotices(Certification certification)
        {
            return base.Channel.GetNotices(certification);
        }

        // Token: 0x06000B03 RID: 2819 RVA: 0x0003BA4F File Offset: 0x00039C4F
        public List<Notice> GetCompanyNotices(Certification certification, long companyId)
        {
            return base.Channel.GetCompanyNotices(certification, companyId);
        }

        // Token: 0x06000B04 RID: 2820 RVA: 0x0003BA5E File Offset: 0x00039C5E
        public Notice GetNotice(Certification certification, long noticeId)
        {
            return base.Channel.GetNotice(certification, noticeId);
        }

        // Token: 0x06000B05 RID: 2821 RVA: 0x0003BA6D File Offset: 0x00039C6D
        public void CreateNotice(Certification certification, Notice notice)
        {
            base.Channel.CreateNotice(certification, notice);
        }

        // Token: 0x06000B06 RID: 2822 RVA: 0x0003BA7C File Offset: 0x00039C7C
        public void UpdateNotice(Certification certification, Notice notice)
        {
            base.Channel.UpdateNotice(certification, notice);
        }

        // Token: 0x06000B07 RID: 2823 RVA: 0x0003BA8B File Offset: 0x00039C8B
        public void DeleteNotice(Certification certification, long targetId)
        {
            base.Channel.DeleteNotice(certification, targetId);
        }
    }
}
