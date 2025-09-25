using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LuckyFutureLib.AsynSocket;
using WebSocketSharp;

namespace LuckyFuture
{
    class JsonObj
    {
        public string Command { get; set; }
        public string Users { get; set; }
        public string Value { get; set; }
    }
    class AppWebSocket
    {
        public event EventHandler<AuthorEventArgs> NoticeEvent;
        private string _Uri;
        private Thread _SocketThread = null;
        private string _WsState = "";
        private string _Message = "";
        public AppWebSocket(string uri)
        {
            _Uri = uri;

            ConnectSocket();
        }

        public string ConnectState { get => _WsState; }

        public void ConnectSocket()
        {
            _WsState = "";
            CloseSocket();
            _SocketThread = new Thread(WebSocketTask);
            _SocketThread.IsBackground = true;
            _SocketThread.Start();
        }

        private void WebSocketTask()
        {
            using (var ws = new WebSocket(_Uri))
            {
                _WsState = ws.ReadyState.ToString();  //Connecting

                ws.OnOpen += (sender, e) =>
                {
                    _WsState = ws.ReadyState.ToString();     //Open
                };

                ws.OnClose += (sender, e) =>
                {
                    _WsState = "Closed";
                };

                ws.OnMessage += (sender, e) =>
                {
                    OnAuthorNoticeEvent(e.Data);
                };

                ws.Connect();

                while (true)
                {
                    if(_Message.Length > 0)
                    {
                        ws.Send(_Message);
                        _Message = "";
                    }
                    Thread.Sleep(3000);
                }
            }

        }

        public void SendMsg(string msg)
        {
            _Message = msg;
        }

        public void CloseSocket()
        {
            if (_SocketThread != null && _SocketThread.IsAlive)
            {
                _SocketThread.Abort();
                _SocketThread = null;
            }

        }
        protected virtual void OnAuthorNoticeEvent(Object obj)
        {
            if (NoticeEvent != null)
                NoticeEvent(this, new AuthorEventArgs(obj));
        }


    }
}
