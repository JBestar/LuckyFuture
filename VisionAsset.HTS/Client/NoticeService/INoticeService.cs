using Goodbyte.TradingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VisionAsset.Client.NoticeService
{
	[ServiceContract]
	public interface INoticeService
	{
        // Token: 0x0600095B RID: 2395
        [OperationContract(Action = "http://tempuri.org/INoticeService/GetNotices", ReplyAction = "http://tempuri.org/INoticeService/GetNoticesResponse")]
        List<Notice> GetNotices(Certification certification);

        // Token: 0x06000AF8 RID: 2808
        [OperationContract(Action = "http://tempuri.org/INoticeService/GetCompanyNotices", ReplyAction = "http://tempuri.org/INoticeService/GetCompanyNoticesResponse")]
        List<Notice> GetCompanyNotices(Certification certification, long companyId);

        // Token: 0x06000AF9 RID: 2809
        [OperationContract(Action = "http://tempuri.org/INoticeService/GetNotice", ReplyAction = "http://tempuri.org/INoticeService/GetNoticeResponse")]
        Notice GetNotice(Certification certification, long noticeId);

        // Token: 0x06000AFA RID: 2810
        [OperationContract(Action = "http://tempuri.org/INoticeService/CreateNotice", ReplyAction = "http://tempuri.org/INoticeService/CreateNoticeResponse")]
        void CreateNotice(Certification certification, Notice notice);

        // Token: 0x06000AFB RID: 2811
        [OperationContract(Action = "http://tempuri.org/INoticeService/UpdateNotice", ReplyAction = "http://tempuri.org/INoticeService/UpdateNoticeResponse")]
        void UpdateNotice(Certification certification, Notice notice);

        // Token: 0x06000AFC RID: 2812
        [OperationContract(Action = "http://tempuri.org/INoticeService/DeleteNotice", ReplyAction = "http://tempuri.org/INoticeService/DeleteNoticeResponse")]
        void DeleteNotice(Certification certification, long targetId);
    }
}
