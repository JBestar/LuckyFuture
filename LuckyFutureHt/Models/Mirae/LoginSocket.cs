// #define WRITE_LOG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using LuckyFutureLib.AsynSocket;
using LuckyFutureLib.Include;

namespace LuckyFuture.Models.Reanteck
{
    class LoginSocket : AsynSocketClient
    {
        public event EventHandler<SocketEventArgs> DataReceiveEvent;
        private Thread _ExtractThread = null;

        const int HEAD_LENGTH = 0x30;
        const int HEAD_SIZE_LENGTH = 10;

        const char CHAR_SPACE = ' ';
        const char CHAR_ZERO = '0';

        public const string PACK_AT0001 = "at0001";
        public const string PACK_AT0002 = "at0002";
        public const string PACK_AQ0003 = "aq0003";
        public const string PACK_AT0006 = "at0006";

        protected string _id = "";
        protected string _pwd = "";
        string _logPath = "";


        public LoginSocket(string ipAddr, int nPort) : base(ipAddr, nPort)
        {
            _id = "";
#if WRITE_LOG
            CreateLogFile();
#endif
        }

        public override void CloseSocket()
        {
            base.CloseSocket();
            CloseExtract();
        }

        private void StartExtract()
        {
            CloseExtract();
            //             _ExtractThread = new Thread(ExtractPacketLoop);
            //             _ExtractThread.IsBackground = true;
            //             _ExtractThread.Start();
        }
        private void CloseExtract()
        {
            if (_ExtractThread != null && _ExtractThread.IsAlive)
            {
                _ExtractThread.Abort();
                _ExtractThread = null;
            }
        }

        public void RequestDialogLogin(string id, string pwd)
        {
            _id = id;
            _pwd = pwd;
            StartExtract();
            RequestAt001();
        }

        public void RequestAt001()
        {
            string id = _id;
            //             byte[] headBytes = new byte[] {
            //                 0xDA, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            //                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x61, 0x74, 0x30, 0x30, 0x30, 0x31, 0x00, 0x00,
            //                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xD6, 0x00, 0x00, 0x00
            //             }; //len=48
            //body data
            string packId = PACK_AT0001;

            string sendData = CHAR_SPACE + id.PadRight(18, CHAR_SPACE); //len=19
            sendData += packId.PadRight(9, CHAR_SPACE); //len=9
            sendData += "00".PadRight(176, CHAR_SPACE); //len=176
            int cntData = Encoding.Default.GetByteCount(sendData); //total=204
            sendData = cntData.ToString().PadLeft(HEAD_SIZE_LENGTH, CHAR_ZERO) + sendData;
            byte[] dataBytes = Encoding.Default.GetBytes(sendData);

            byte[] headBytes = MakeHeader(packId, dataBytes.Length);

            SendBytes(Extension.CombineBytes(headBytes, dataBytes));
        }

        public void RequestAt002()
        {
            string id = _id;
            string pwd = _pwd;
            string packId = PACK_AT0002;

            string ipAddr = "192.168.92.1";
            string ipName = "5A-FB-84-0E-CE-64";
            string comName = "DESKTOP-61Q2H0A";

            //             byte[] headBytes = new byte[] {
            //                 0xAA, 0x01, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            //                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x61, 0x74, 0x30, 0x30, 0x30, 0x32, 0x00, 0x00,
            //                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xA6, 0x01, 0x00, 0x00
            //             }; //len=48

            //body data
            string sendData = CHAR_SPACE + id.PadRight(18, CHAR_SPACE); //len=19
            sendData += packId.PadRight(9, CHAR_SPACE); //len=9
            sendData += "00".PadRight(172, CHAR_SPACE); //len=172
            int cntBytes = Encoding.Default.GetByteCount(sendData);
            sendData = cntBytes.ToString().PadLeft(HEAD_SIZE_LENGTH, CHAR_ZERO) + sendData;//len=10+200
            sendData += id.PadRight(8, CHAR_SPACE); //len=8
            sendData += pwd.PadRight(9, CHAR_SPACE); //len=9
            sendData += ipAddr.PadRight(15, CHAR_SPACE); //len=15
            sendData += ipName.PadRight(30, CHAR_SPACE); //len=30
            sendData += comName.PadRight(150, CHAR_SPACE); //len=150

            byte[] dataBytes = Encoding.Default.GetBytes(sendData); //422(0x1A6)
            byte[] headBytes = MakeHeader(packId, dataBytes.Length);

            SendBytes(Extension.CombineBytes(headBytes, dataBytes));
        }

        public void RequestAq003()
        {
            string id = _id;
            string packId = PACK_AQ0003;

            //             byte[] headBytes = new byte[] {
            //                 0xDA, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            //                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x61, 0x71, 0x30, 0x30, 0x30, 0x33, 0x00, 0x00,
            //                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xD6, 0x00, 0x00, 0x00
            //             }; //len=48

            //body data
            string sendData = CHAR_SPACE + id.PadRight(18, CHAR_SPACE); //len=19
            sendData += packId.PadRight(9, CHAR_SPACE); //len=9
            sendData += "00".PadRight(172, CHAR_SPACE); //len=172
            sendData += "A".PadRight(4, CHAR_SPACE); //len=4
            int cntBytes = Encoding.Default.GetByteCount(sendData); //total=204
            sendData = cntBytes.ToString().PadLeft(HEAD_SIZE_LENGTH, CHAR_ZERO) + sendData;
            byte[] dataBytes = Encoding.Default.GetBytes(sendData);

            byte[] headBytes = MakeHeader(packId, dataBytes.Length);

            SendBytes(Extension.CombineBytes(headBytes, dataBytes));
        }

        public void RequestAt006()
        {
            string id = _id;
            string packId = PACK_AT0006;

            //             byte[] headBytes = new byte[] {
            //                 0xDE, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            //                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x61, 0x74, 0x30, 0x30, 0x30, 0x36, 0x00, 0x00,
            //                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xDA, 0x00, 0x00, 0x00
            //             }; //len=48

            //body data
            string sendData = CHAR_SPACE + id.PadRight(18, CHAR_SPACE); //len=19
            sendData += packId.PadRight(9, CHAR_SPACE); //len=9
            sendData += "00".PadRight(172, CHAR_SPACE); //len=172
            sendData += "m".PadRight(8, CHAR_SPACE); //len=8
            int cntBytes = Encoding.Default.GetByteCount(sendData); //total=208
            sendData = cntBytes.ToString().PadLeft(HEAD_SIZE_LENGTH, CHAR_ZERO) + sendData;
            byte[] dataBytes = Encoding.Default.GetBytes(sendData);

            byte[] headBytes = MakeHeader(packId, dataBytes.Length);

            SendBytes(Extension.CombineBytes(headBytes, dataBytes));
        }
        protected byte[] MakeHeader(string headId, int cntBody)
        {
            byte[] headBytes = new byte[HEAD_LENGTH];
            Array.Clear(headBytes, 0, headBytes.Length);

            byte[] intBytes = BitConverter.GetBytes(cntBody + 4);
            Buffer.BlockCopy(intBytes, 0, headBytes, 0, 4);

            headBytes[4] = 0x01;

            byte[] nameBytes = Encoding.Default.GetBytes(headId);
            Buffer.BlockCopy(nameBytes, 0, headBytes, 0x18, nameBytes.Length);

            intBytes = BitConverter.GetBytes(cntBody);
            Buffer.BlockCopy(intBytes, 0, headBytes, 0x2C, 4);
            return headBytes;
        }
        protected void SendData(string sendData)
        {
            byte[] byteData = Encoding.Default.GetBytes(sendData);
#if WRITE_LOG
            WriteLog("<<<" + sendData);
#endif
            base.SendData(byteData);
        }
        protected void SendBytes(byte[] sendBytes)
        {
#if WRITE_LOG
            WriteBytes(Extension.ByteArrayToString(sendBytes, sendBytes.Length), true);
#endif
            base.SendData(sendBytes);
        }
        public override void ExtractPacket()
        {
            int lenRecv = 0;
            string sPacket = "";
            lock (_ReceiveBuffer)
            {
                lenRecv = _ReceiveSize;
                _ReceiveSize = 0;

#if WRITE_LOG
                WriteBytes(Extension.ByteArrayToString(_ReceiveBuffer, lenRecv), false);
#endif
                if (lenRecv < 0x10)
                    return;

                sPacket = Encoding.Default.GetString(_ReceiveBuffer, 0, lenRecv);
            }
            if (sPacket.Length > 0)
            {
                if (_ResponseList.Count < 2000)
                {
                    lock (_ResponseList)
                    {
                        _ResponseList.Add(sPacket);
                    }
                    OnDataReceiveEvent(SOCKET_EVENTTYPE.RECEIVE);
                }
            }
        }

        private void CreateLogFile()
        {
            _logPath = "D://BinHts/Mirae_starter_" + DateTime.Now.ToString("yyyyMMdd");
            _logPath += ".txt";
            WriteLog("<!=============시작중입니다.===============>");
        }

        public void WriteLog(string strLog)
        {
            try
            {
                DateTime dtServer = DateTime.Now;
                string log = string.Format("[{0:D2}:{1:D2}:{2:D2}] ", dtServer.Hour, dtServer.Minute, dtServer.Second);
                log += strLog;
                using (StreamWriter outputFile = new StreamWriter(_logPath, true))
                {
                    outputFile.WriteLine(log);
                }

            }
            catch (Exception ex)
            { string error = ex.Message; }

        }
        public void WriteBytes(string strLog, bool bSend = true)
        {
            try
            {
                string path = "D://BinHts/Merae_byte_" + DateTime.Now.ToString("yyyyMMdd");
                path += ".txt";

                DateTime dtServer = DateTime.Now;
                string log = string.Format("[{0:D2}:{1:D2}:{2:D2}] Starter {3} \n", dtServer.Hour, dtServer.Minute, dtServer.Second, bSend ? ">>>>Send" : "<<<<<<Receive");
                log += strLog;
                using (StreamWriter outputFile = new StreamWriter(path, true))
                {
                    outputFile.WriteLine(log);
                }

            }
            catch (Exception ex)
            { string error = ex.Message; }
        }

        public void OnDataReceiveEvent(Object obj)
        {
            if (DataReceiveEvent != null)
            {
                DataReceiveEvent(this, new SocketEventArgs(obj));

            }

        }



    }
}

