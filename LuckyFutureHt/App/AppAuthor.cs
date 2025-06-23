using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http.Headers;
using System.Reflection;
using System.IO;
using System.Text.Json;
using System.Diagnostics;
using System.Configuration;
using System.Xml;
using LuckyFuture.Properties;
using LuckyFuture.Logic;
using LuckyFutureLib.Include;


namespace LuckyFuture
{
    public enum AUTHOR_EVENTTYPE
    {
        LOGOUT,
    }
    public class AuthorEventArgs : EventArgs
    {
        public AuthorEventArgs(object data)
        {
            this.Data = data;
        }

        public object Data { get; set; }
    }
    class NoticeInfo
    {
        public string Content { get; set; }
        public DateTime UpdateTime { get; set; }
    }
    class AppAuthor : StageThreadEx
    {
        public readonly static AppAuthor Default = new AppAuthor();

        private readonly HttpClientEx _httpClient = new HttpClientEx();
        private readonly WebClientEx _webClient = new WebClientEx();

        public event EventHandler<AuthorEventArgs> NoticeEvent;

        public const string URL_MAIN = "http://localhost:7082/Temp/"; //localhost:7082/
        public const string URL_UPDATE = "http://localhost:7082/Update/"; //210.90.155.77:8080
        public const string URL_DOWNLOAD = "http://localhost:7082/Download/";
        public const string URL_WS2 = "ws://localhost:7082/Temp/TempWebSocket.ashx?websession="; //
        private const string URL_CERT_LOGIN = "Login.aspx";
        private const string URL_CERT_LOGOUT = "LogOut.aspx";
        private const string URL_CERT_KEEPALIVE = "ActiveKeep.aspx";
        private const string URL_UPDATE_VERSION = "Version.aspx";
        private const string URL_UPLOAD = "Uploadpy.aspx";
        public const string URL_NOTICE = "getnotice.aspx";
        private const string WEBSESSION_KEY = "ASP.NET_SessionId";

        private const int SEND_KEEP_ALIVE_INTERVAL = 20000; // 60000
        private const int RELOGIN_INTERVAL = 10000;

        string _id = "";
        string _pwd = "";
        string _sessionId = "";
        public UInt32 _remainedTime;
        bool _vip = false;
        string _version = "";
        string _configPath = "";

        string _userName = "";
        string _siteName = "";
        long _startMoney = 0;
        long _currentMoney = 0;
        int _loginTry = 0;

        public UInt32 RemainedTime
        {
            get => _remainedTime;
        }

        public bool VIP
        {
            get => _vip;
        }

        public string Uid
        {
            get => _id;
        }
        public string SessionId
        {
            get => _sessionId;
        }
        public AppAuthor()
        {
            Reset();
            _httpClient.MainUrl = URL_MAIN;

            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
            _configPath = configuration.FilePath;
        }

        public void Reset()
        {
            _remainedTime = 0;
            _id = _pwd = "";
            _httpClient.Reset();

        }

        public void SetUserAccount(string userName = "", string siteName = "", long startMoney = -1, long currentMoney = -1)
        {
            if (!string.IsNullOrEmpty(userName))
                _userName = userName;

            if (!string.IsNullOrEmpty(siteName))
                _siteName = siteName;

            if (startMoney >= 0)
                _startMoney = startMoney;

            if (currentMoney >= 0)
                _currentMoney = currentMoney;
        }

        public APPLOGINRESULT Login(string id, string pwd, bool force)
        {
            Reset();
            return DoLogin(id, pwd, force);
        }

        private APPLOGINRESULT DoLogin(string id = "", string pwd = "", bool bForce = true)
        {
            if (string.IsNullOrEmpty(id))
                id = _id;

            if (string.IsNullOrEmpty(pwd))
                pwd = _pwd;

            var param_list = new Dictionary<string, string>
            {
                { "post_id", "gologin" },
                { "username", id },
                { "password", pwd },
                { "force", bForce ? "1" : "0" }
            };

            if (!_httpClient.SendRequest(out string body, out HttpHeaders headers, HTTPREQUEST_TYPE.POST, URL_MAIN + URL_CERT_LOGIN, param_list))
                return APPLOGINRESULT.CANNOT_CONNECT;

            // asp session id
            string sessionId = HttpClientEx.GetHeaderKeyValue(headers, "Set-Cookie", WEBSESSION_KEY);
            if (sessionId.Length > 0)
                _sessionId = sessionId;

            // login status
            JsonDocument doc = JsonDocument.Parse(body);
            int iLoginResult;
            try
            {
                iLoginResult = Convert.ToInt32(doc.RootElement.GetProperty("result").GetString());
                // remained time
                if ((APPLOGINRESULT)iLoginResult == APPLOGINRESULT.SUCCESS)
                {
                    _remainedTime = Convert.ToUInt32(doc.RootElement.GetProperty("remained").GetString());
                    var tmp = Convert.ToUInt32(doc.RootElement.GetProperty("vip").GetString());
                    _vip = tmp != 0;
                    _id = id;
                    _pwd = pwd;

                    try
                    {
                        tmp = Convert.ToUInt32(doc.RootElement.GetProperty("order").GetString());
                        if (tmp >= 0 && tmp <= 10)
                            Settings.Default.OrderMax = (int)tmp;
                    }
                    catch (KeyNotFoundException)
                    {
                        Settings.Default.OrderMax = 10;
                    }

                }
            }
            catch (Exception)
            {
                return APPLOGINRESULT.INVALID_SERVER;
            }

            return (APPLOGINRESULT)iLoginResult;
        }
        APPLOGINRESULT SendKeepAlive()
        {
            var param_list = new Dictionary<string, string>
            {
                { "websession", _sessionId },
                { "hostname", Environment.MachineName },
                { "address", Common.GetLocalIPAddress() },
                { "running", LogicAuto.Default.IsRunning?"1":"0" },
                { "betting_domain", _siteName },
                { "betting_user", _userName},
                { "betting", "1" },
                { "money_begin", _startMoney.ToString() },
                { "money_current", _currentMoney.ToString() },
                { "app_title", Assembly.GetExecutingAssembly().GetName().Name},
                { "app_version", _version},
                { "memo_1", Settings.Default.IsAutoMode?"자동":"수동" },
            };

            if (!_httpClient.SendRequest(out string body, out _, HTTPREQUEST_TYPE.POST, URL_MAIN + URL_CERT_KEEPALIVE, param_list))
                return APPLOGINRESULT.CANNOT_CONNECT;

            JsonDocument doc = JsonDocument.Parse(body);
            int iResult = 0;
            try
            {
                iResult = Convert.ToInt32(doc.RootElement.GetProperty("result").GetString());
                // remained time
                if ((APPLOGINRESULT)iResult == APPLOGINRESULT.SUCCESS)
                {
                    _remainedTime = Convert.ToUInt32(doc.RootElement.GetProperty("remained").GetString());
                    //var tmp = Convert.ToUInt32(doc.RootElement.GetProperty("order").GetString());
                    //if (tmp >= 0 && tmp <= 10)
                    //    Settings.Default.OrderMax = (int)tmp;

                }


            }
            catch (Exception)
            {
                return APPLOGINRESULT.INVALID_SERVER;
            }

            return (APPLOGINRESULT)iResult;
        }
        public NoticeInfo GetNotice()
        {
            NoticeInfo noticeInfo = new NoticeInfo();
            if (!_httpClient.SendRequest(out string body, out _, HTTPREQUEST_TYPE.GET, URL_MAIN + URL_NOTICE))
                return null;

            XmlDocument xmlDoc = new XmlDocument();


            try
            {
                xmlDoc.LoadXml(body);

                XmlNodeList xmlNodes = xmlDoc.GetElementsByTagName("content");
                foreach (XmlNode childNode in xmlNodes)
                {
                    noticeInfo.Content = childNode.InnerText;
                    noticeInfo.UpdateTime = DateTime.Now.AddDays(-7);
                    break;
                }
                DateTime updateTime = DateTime.Now;
                xmlNodes = xmlDoc.GetElementsByTagName("updated");
                foreach (XmlNode childNode in xmlNodes)
                {
                    if (DateTime.TryParse(childNode.InnerText, out updateTime))
                    {
                        noticeInfo.UpdateTime = updateTime;
                    }
                    break;
                }

            }
            catch (Exception)
            {
                return null;
            }

            return noticeInfo;
        }
        public APPLOGINRESULT Logout()
        {
            if (!_httpClient.SendRequest(out _, out _, HTTPREQUEST_TYPE.GET, URL_MAIN + URL_CERT_LOGOUT))
                return APPLOGINRESULT.CANNOT_CONNECT;
            return APPLOGINRESULT.SUCCESS;
        }
        public string GetAppLogPath()
        {
            string sLogPath = "";
            try
            {
                string sDay = DateTime.Now.ToString("yyyyMMdd");
                var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);

                int nConfigLen = configuration.FilePath.LastIndexOf("user.config");
                if (nConfigLen > 0)
                {
                    string sPath = configuration.FilePath.Substring(0, nConfigLen) + "log";
                    if (!Directory.Exists(sPath))
                    {
                        Directory.CreateDirectory(sPath);
                    }
                    sPath = sPath + Path.DirectorySeparatorChar + sDay + "_ht.txt";
                    if (!File.Exists(sPath))
                    {
                        File.Create(sPath);
                    }
                    sLogPath = sPath;
                }
            }
            catch { }
            return sLogPath;
        }
        public string CreatePathFolder(string folder)
        {
            string path = "";
            if (folder.Length > 0)
            {
                path = Environment.CurrentDirectory + "/" + folder;

                DirectoryInfo dirInfo = new DirectoryInfo(path);

                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

            }
            return path;
        }
        public string GetAppName()
        {
            string sVerFile = Assembly.GetExecutingAssembly().Location;
            return FileVersionInfo.GetVersionInfo(sVerFile).ProductName;

        }

        public string GetAppVersion()
        {
            //sCurVersion = File.ReadAllText(Environment.CurrentDirectory + Path.DirectorySeparatorChar.ToString() + VERSION_FILE);
            //sCurVersion = sCurVersion.Trim();

            string sVerFile = Assembly.GetExecutingAssembly().Location;
            return FileVersionInfo.GetVersionInfo(sVerFile).FileVersion;

        }
        public string GetUpdateVersion(Dictionary<string, string> ver_data)
        {

            //Get Assembly Version	
            _version = GetAppVersion();

            if (!_httpClient.SendRequest(out string body, out _, HTTPREQUEST_TYPE.GET, URL_UPDATE + URL_UPDATE_VERSION))
                return "";

            ver_data.Clear();

            string sUpVersion = "";

            try
            {
                JsonDocument doc = JsonDocument.Parse(body);
                JsonElement rootElement = doc.RootElement;
                sUpVersion = rootElement.GetProperty("version").GetString();
                // update files
                JsonElement filesArray = rootElement.GetProperty("files");

                int nLength = filesArray.GetArrayLength();
                string sKey = "", sValue = "";
                for (int i = 0; i < nLength; i++)
                {
                    sKey = filesArray[i].GetProperty("path").GetString();
                    sValue = filesArray[i].GetProperty("value").GetString();
                    ver_data.Add(sKey, sValue);
                }

                if (_version.CompareTo(sUpVersion) > 0)
                {
                    sUpVersion = "";

                }
            }
            catch (Exception)
            {
                return "";
            }


            return sUpVersion;

        }

        int _tickGet;
        public void ReqIpDt()
        {
            if (Math.Abs(Environment.TickCount - _tickGet) < 60000)
                return;
            _tickGet = Environment.TickCount;

            DateTime dtNow = DateTime.Now;
            if (!_httpClient.SendRequest(out string body, out _, HTTPREQUEST_TYPE.GET, "http://worldtimeapi.org/api/timezone/Asia/Seoul?t="+ dtNow.Second))
                return;

//             {
//                 "abbreviation": "KST",
//                 "client_ip": "58.138.233.78",
//                 "datetime": "2024-04-11T12:50:27.602350+09:00",
//                 "day_of_week": 4,
//                 "day_of_year": 102,
//                 "timezone": "Asia/Seoul",
//                 "unixtime": 1712807427,
//                 "utc_datetime": "2024-04-11T03:50:27.602350+00:00",
//                 "utc_offset": "+09:00",
//                 "week_number": 15
//             }

            try
            {
                JsonDocument doc = JsonDocument.Parse(body);
                JsonElement rootElement = doc.RootElement;
                AppConfig._IpAddr = rootElement.GetProperty("client_ip").GetString();
                string sDt = rootElement.GetProperty("datetime").GetString();
                if(sDt.Length > 0)
                {
                    DateTime dtServ = DateTime.Parse(sDt);
                    TimeSpan tmSpan = dtServ.Subtract(dtNow);
                    AppConfig._DtDelay = (int)(tmSpan.TotalSeconds);
                }
                
            }
            catch (Exception)
            {
            }

        }
        public bool UploadConfig()
        {
            return _webClient.UploadFile(URL_MAIN + URL_UPLOAD, _configPath, "Cookie", WEBSESSION_KEY + "=" + _sessionId);

        }

        private enum LHSTAGE : Int32
        {
            KEEPALIVE = STAGE.LAST,
            DISCONNECTED = KEEPALIVE + 1,
        }


        protected override bool Run()
        {
            bool bAutoStop = false;
            APPLOGINRESULT nError;
            switch (_stage)
            {
                case (int)STAGE.NONE:
                    SetStage((int)LHSTAGE.KEEPALIVE);
                    break;
                case (int)LHSTAGE.KEEPALIVE:
                    // send keep-alive 
                    nError = SendKeepAlive();
                    if (nError == APPLOGINRESULT.SUCCESS)
                    {
                        ReqIpDt();
                        // Trace.TraceError("<KEEPALIVE> nError = {0} loginTry = {1} ", nError, _loginTry);
                        SetStageWait((int)LHSTAGE.KEEPALIVE, SEND_KEEP_ALIVE_INTERVAL); // <- next keep-alive after cycle
                    }
                    else
                    {
                        Trace.TraceError("<AppAuthor> SendKeepAlive nError = {0} ", nError);
                        // disconnected
                        if (nError < 0)
                        {
                            Logout();
                            SetStageWait((int)LHSTAGE.DISCONNECTED, RELOGIN_INTERVAL);
                        }
                        else
                        {
                            OnAuthorNoticeEvent(AUTHOR_EVENTTYPE.LOGOUT);
                            bAutoStop = true;
                        }

                    }
                    break;
                case (int)LHSTAGE.DISCONNECTED:
                    nError = DoLogin();
                    Trace.TraceError("<AppAuthor> DISCONNECTED DoLogin = {0} ", nError);

                    if (nError == APPLOGINRESULT.SUCCESS)
                    {
                        _loginTry = 0;
                        SetStageWait((int)LHSTAGE.KEEPALIVE, SEND_KEEP_ALIVE_INTERVAL);
                    }
                    else if (nError < 0 || nError == APPLOGINRESULT.MULTI_USE)
                    {
                        Trace.TraceError("<AppAuthor> DISCONNECTED nError = {0} loginTry = {1} ", nError, _loginTry);
                        _loginTry++;
                        if (_loginTry > 10)
                        {
                            OnAuthorNoticeEvent(AUTHOR_EVENTTYPE.LOGOUT);
                            bAutoStop = true;
                        }
                        else SetStageWait((int)LHSTAGE.DISCONNECTED, RELOGIN_INTERVAL);

                    }
                    else
                    {
                        bAutoStop = true;
                    }
                    break;
                default:
                    bAutoStop = !base.Run();
                    break;
            }
            return !bAutoStop;
        }

        protected override void OnStarted()
        {

        }

        protected override void OnStopped(bool bAutoStop)
        {

        }

        protected virtual void OnAuthorNoticeEvent(Object obj)
        {
            if (NoticeEvent != null)
                NoticeEvent(this, new AuthorEventArgs(obj));
        }

    }
    public enum APPLOGINRESULT : int
    {
        // connection errors
        CANNOT_CONNECT = -1,        // could not connect to server
        INVALID_SERVER = -2,        // invalid server
        INVALID_URL = -3,           // invalid url to be requested
                                    // logical errors
        SUCCESS = 1,                // success
        INVALID_ID = 0,             // invalid id
        INVALID_PWD = 2,            // incorrect pwd
        USAGE_EXPIRED = 3,          // usage period expired
        USER_BLOCKED = 4,           // blocked
        MULTI_USE = 5,              // multiple use
        SESSION_OUT = 10,           // session out
    }
}
