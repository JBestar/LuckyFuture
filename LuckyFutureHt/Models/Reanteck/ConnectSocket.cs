#define WRITE_LOG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using LuckyFutureLib.AsynSocket;
using LuckyFuture.Models.ValueObjects;

namespace LuckyFuture.Models.Reanteck
{
    class ConnectSocket : AsynSocketClient
    {

        public event EventHandler<SocketEventArgs> DataReceiveEvent;
        private Thread _ExtractThread = null;

        public const string HEAD_KEEPALIVE = "TA000";     //KeepAlive
        public const string HEAD_POPUPLOGIN = "TA002";     //로그인
        public const string HEAD_MAINLOGIN = "TA003";     //메인로그인
        public const string HEAD_MESSAGE = "TA004";      //Message

        public const string HEAD_LOGOUTEND = "TA006";      //로그아웃 마지막
        public const string HEAD_DUPLOGIN = "TA007";     //중복로그인 

        public const string HEAD_LOGOUT = "TA011";     //로그아웃 
        public const string HEAD_ORDER = "TA101";     //주문, 청산요청
        public const string HEAD_CANCEL = "TA105";     //취소요청
        public const string HEAD_RECEIPT_R = "TA151";     //주문 접수응답
        public const string HEAD_ORDER_R = "TA153";     //주문 체결응답
        public const string HEAD_CANCEL_R = "TA154";     //주문 취소응답

        public const string HEAD_ACCOUNT = "TA201";   //유저정보 
        public const string HEAD_ITEMLIST = "TA304";  //종목리스트정보
        public const string HEAD_ITEMQUOTE = "TA307";  //종목별 호가정보 요청
        public const string HEAD_ITEMQUOTE_RANGE = "TA308";
        public const string HEAD_ITEMQUOTE_DATA = "TA309";


        public const string HEAD_PROFIT = "TA403";      //손익정보
        public const string HEAD_ORDERLIST = "TA404";   //주문 체결리스트 상태
        public const string HEAD_RECEIPTLIST = "TA405";   //주문 접수리스트 상태
        public const string HEAD_RESERVELIST = "TA406";


        public const string HEAD_401 = "TA401";      //로그인시
        public const string HEAD_005 = "TA005";      //로그인시
        public const string HEAD_301 = "TA301";      //로그인시
        public const string HEAD_302 = "TA302";      //로그인시
        public const string HEAD_402 = "TA402";      //로그인시
        public const string HEAD_202 = "TA202";      //로그인시
        public const string HEAD_001 = "TA001";      //로그인시
        public const string HEAD_307 = "TA307";      //로그인시
        public const string HEAD_421 = "TA421";      //로그인시
        public const string HEAD_411 = "TA411";      //로그인시
        public const string HEAD_504 = "TA504";      //로그인시


        public const int HEAD_SIZE_LENGTH = 5;
        public const int HEAD_NAME_LENGTH = 5;
        public const int HEAD_PACKETNO_LENGTH = 5;

        const char BODY_0 = '0';
        const char BODY_9 = '9';
        const char BODY_SPACE = ' ';

        const int BODY_REPEAT_83 = 83;

        protected int _PacketNo = 0;
        protected string _id = "";
        protected string _account = "";
        string _logPath = "";
        public ConnectSocket(string ipAddr, int nPort) : base(ipAddr, nPort)
        {

            _PacketNo = 0;
            _id = "";
            _account = "";
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
            _ExtractThread = new Thread(ExtractPacketLoop);
            _ExtractThread.IsBackground = true;
            _ExtractThread.Start();
        }
        private void CloseExtract()
        {
            if (_ExtractThread != null && _ExtractThread.IsAlive)
            {
                _ExtractThread.Abort();
                _ExtractThread = null;
            }
        }

        protected void ExtractPacketLoop()
        {
            while (true)
            {
                if (_ResponseList.Count > 0)
                    OnDataReceiveEvent(SOCKET_EVENTTYPE.RECEIVE);

                Thread.Sleep(10);
            }

        }

        protected string GetPacketNo()
        {
            if (_PacketNo > 99990)
                _PacketNo = 0;

            _PacketNo++;
            return String.Format("{0:00000}", _PacketNo);
        }

        public bool RequestDialogLogin(string id, string pwd, string app)
        {
            _PacketNo = 0;
            //TA00300001test001                       LOGIN/test001/3366
            string sendData = HEAD_POPUPLOGIN;
            sendData += GetPacketNo();
            sendData += "".PadRight(30, BODY_SPACE);
            sendData += "TR_0100_S_02/";
            sendData += id + "/" + pwd + app;

            StartExtract();
            return SendData(sendData);
        }

        public bool RequestDupLogin(string id)
        {
            _PacketNo = 0;
            //TA00300001test001                       LOGIN/test001/3366
            string sendData = HEAD_DUPLOGIN;
            sendData += GetPacketNo();
            sendData += "".PadRight(30, BODY_SPACE);
            sendData += "TR_0100_S_05/";
            sendData += id;

            return SendData(sendData);
        }

        public bool RequestMainLogin(string id, string pwd)
        {
            _PacketNo = 0;
            //TA00300001test001                       LOGIN/test001/3366
            string sendData = HEAD_MAINLOGIN;
            sendData += GetPacketNo();
            sendData += id.PadRight(30, BODY_SPACE);
            sendData += "LOGIN/";
            sendData += id + "/" + pwd;

            _id = id;
            _account = "";

            StartExtract();
            return SendData(sendData);
        }

        public bool RequestKeepAlive()
        {
            if (_id.Length < 1 || _account.Length < 1)
                return false;

            string sendData = HEAD_KEEPALIVE;
            sendData += GetPacketNo();
            sendData += _id.PadRight(10, BODY_SPACE);
            sendData += _account.PadRight(20, BODY_SPACE);
            sendData += "TA000/" + _id + "/1/";
            sendData += new string(BODY_9, BODY_REPEAT_83);

            return SendData(sendData);
        }

        public bool RequestAccount()
        {
            string id = _id;
            string sendData = HEAD_ACCOUNT;
            sendData += GetPacketNo();
            sendData += id.PadRight(30, BODY_SPACE);
            sendData += "TR_0100_S_04/";
            sendData += id + "/";
            sendData += new string(BODY_9, BODY_REPEAT_83);

            return SendData(sendData);
        }

        public bool Request401()
        {
            string id = _id;
            string sendData = HEAD_401;
            sendData += GetPacketNo();
            sendData += id.PadRight(30, BODY_SPACE);
            sendData += "TR_5400_S_02/";
            sendData += id + "#7#";
            sendData += new string(BODY_9, BODY_REPEAT_83);

            return SendData(sendData);
        }
        public bool Request005()
        {
            string id = _id;
            string sendData = HEAD_005;
            sendData += GetPacketNo();
            sendData += id.PadRight(30, BODY_SPACE);
            sendData += "TR_6200_S_03/";
            sendData += "TODAY/";
            sendData += new string(BODY_9, BODY_REPEAT_83);

            return SendData(sendData);
        }
        public bool Request301()
        {
            string id = _id;
            string sendData = HEAD_301;
            sendData += GetPacketNo();
            sendData += id.PadRight(30, BODY_SPACE);
            sendData += "future/";
            sendData += id + "/";
            sendData += new string(BODY_9, BODY_REPEAT_83);

            return SendData(sendData);
        }
        public bool Request302()
        {
            string id = _id;
            string sendData = HEAD_302;
            sendData += GetPacketNo();
            sendData += id.PadRight(30, BODY_SPACE);
            sendData += "option/";
            sendData += id + "/";
            sendData += new string(BODY_9, BODY_REPEAT_83);

            return SendData(sendData);
        }
        public bool Request402()
        {
            string id = _id;
            string sendData = HEAD_402;
            sendData += GetPacketNo();
            sendData += id.PadRight(30, BODY_SPACE);
            sendData += "TR_6200_S_01/";
            sendData += id + "/";
            sendData += new string(BODY_9, BODY_REPEAT_83);

            return SendData(sendData);
        }
        public bool Request202()
        {
            string id = _id;
            string sendData = HEAD_202;
            sendData += GetPacketNo();
            sendData += id.PadRight(30, BODY_SPACE);
            sendData += "TR_6100_S_31/";
            sendData += id + "/";
            sendData += new string(BODY_9, BODY_REPEAT_83);

            return SendData(sendData);
        }
        public bool Request001()
        {
            string id = _id;
            string sendData = HEAD_001;
            sendData += GetPacketNo();
            sendData += id.PadRight(30, BODY_SPACE);
            sendData += "TR_4100_S_01/";
            sendData += id + "/";
            sendData += new string(BODY_9, BODY_REPEAT_83);

            return SendData(sendData);
        }
        public bool Request307()
        {
            string id = _id;
            string sendData = HEAD_307;
            sendData += GetPacketNo();
            sendData += id.PadRight(30, BODY_SPACE);
            sendData += "TA307_S_01/";
            sendData += id + "/";
            sendData += new string(BODY_9, BODY_REPEAT_83);

            return SendData(sendData);
        }
        public bool Request421()
        {
            string id = _id;

            string sendData = HEAD_421;
            sendData += GetPacketNo();
            sendData += id.PadRight(30, BODY_SPACE);
            sendData += "SelectShtcdHname/";
            sendData += id + "/";
            sendData += new string(BODY_9, BODY_REPEAT_83);

            return SendData(sendData);
        }
        public bool Request411()
        {
            string id = _id;

            string sendData = HEAD_411;
            sendData += GetPacketNo();
            sendData += id.PadRight(30, BODY_SPACE);
            sendData += "TR_5100_S_04/";
            sendData += id + "/";
            sendData += new string(BODY_9, BODY_REPEAT_83);

            return SendData(sendData);
        }
        public bool Request504()
        {
            string id = _id;

            string sendData = HEAD_504;
            sendData += GetPacketNo();
            sendData += id.PadRight(30, BODY_SPACE);
            sendData += "TR_5100_S_01/";
            sendData += id + "/";
            sendData += new string(BODY_9, BODY_REPEAT_83);

            return SendData(sendData);
        }
        public bool RequestMsg(string msg)
        {
            string id = _id;

            string sendData = HEAD_MESSAGE;
            sendData += GetPacketNo();
            sendData += id.PadRight(10, BODY_SPACE);
            sendData += _account.PadRight(20, BODY_SPACE);
            sendData += "TR_0000_S_01/";

            sendData += id + "#" + _account + "#" + msg;

            return SendData(sendData);
        }

        public bool RequestProfit(string account = "")
        {
            if (account.Length > 0)
                _account = account;
            string id = _id;

            string sendData = HEAD_PROFIT;
            sendData += GetPacketNo();
            sendData += id.PadRight(10, BODY_SPACE);
            sendData += _account.PadRight(20, BODY_SPACE);
            sendData += "topInfo/" + id + "#1#";
            sendData += new string(BODY_9, BODY_REPEAT_83);

            return SendData(sendData);
        }

        public bool RequestItemlist()
        {
            //TA30400009test001                       OSList/test001/
            //99999999999999999999999999999999999999999999999999999999999999999999999999999999999
            string id = _id;


            string sendData = HEAD_ITEMLIST;
            // _PacketNo += 6;
            sendData += GetPacketNo();
            sendData += id.PadRight(30, BODY_SPACE);
            sendData += "OSList/";
            sendData += id + "/";
            sendData += new string(BODY_9, BODY_REPEAT_83);

            return SendData(sendData);
        }

        public bool RequestItemQuote()
        {
            string id = _id;
            //TA30700011test001                       TA307_S_01/test001/
            //99999999999999999999999999999999999999999999999999999999999999999999999999999999999
            string sendData = HEAD_ITEMQUOTE;
            sendData += GetPacketNo();
            sendData += id.PadRight(30, BODY_SPACE);
            sendData += "TA307_S_01/";
            sendData += id + "/";
            sendData += new string(BODY_9, BODY_REPEAT_83);

            return SendData(sendData);
        }

        public bool RequestOrderlist()
        {
            string id = _id;
            string account = _account;
            //TA40400012test001                       getBalance/test001/
            //99999999999999999999999999999999999999999999999999999999999999999999999999999999999
            string sendData = HEAD_ORDERLIST;
            sendData += GetPacketNo();
            sendData += id.PadRight(10, BODY_SPACE);
            sendData += account.PadRight(20, BODY_SPACE);
            sendData += "getBalance/";
            sendData += id + "/";
            sendData += new string(BODY_9, BODY_REPEAT_83);

            return SendData(sendData);
        }
        public bool RequestReservelist()
        {
            string id = _id;
            string account = _account;
            //TA40400012test001                       getBalance/test001/
            //99999999999999999999999999999999999999999999999999999999999999999999999999999999999
            string sendData = HEAD_RESERVELIST;
            sendData += GetPacketNo();
            sendData += id.PadRight(10, BODY_SPACE);
            sendData += account.PadRight(20, BODY_SPACE);
            sendData += "getReserve/";
            sendData += id + "/";
            sendData += new string(BODY_9, BODY_REPEAT_83);

            return SendData(sendData);
        }
        public bool RequestReceiptlist()
        {
            string id = _id;
            string account = _account;
            //TA40500021test003   086204217-01        getPreorder/test003/
            //99999999999999999999999999999999999999999999999999999999999999999999999999999999999
            string sendData = HEAD_RECEIPTLIST;
            sendData += GetPacketNo();
            sendData += id.PadRight(10, BODY_SPACE);
            sendData += account.PadRight(20, BODY_SPACE);
            sendData += "getPreorder/";
            sendData += id + "/";
            sendData += new string(BODY_9, BODY_REPEAT_83);

            return SendData(sendData);
        }

        public bool RequestOrder(TRADETYPE tradeType, string symbol, string price, int quantity, bool marketing)
        {
            string id = _id;
            string account = _account;
            //TA10100041test001   405102911-01        TR_6100_S_11/0056T101405102911-011       ESZ2100000000.00000000000110/test001/0/000
            string sendData = HEAD_ORDER;
            sendData += GetPacketNo();
            sendData += id.PadRight(10, BODY_SPACE);
            sendData += account.PadRight(20, BODY_SPACE);
            sendData += "TR_6100_S_11/0056T101";
            sendData += account;
            sendData += tradeType == TRADETYPE.SELL ? "1" : "2";
            sendData += symbol.PadLeft(12, BODY_SPACE);
            sendData += price.PadLeft(11, BODY_0);
            sendData += "00000";
            sendData += string.Format("{0:00000}", quantity);
            sendData += marketing ? "10/" : "00/";
            sendData += id + "/0/000";

            return SendData(sendData);
        }

        public bool RequestCancel(TRADETYPE tradeType, string symbol, string price, int quantity, long orderNo)
        {
            string id = _id;
            string account = _account;
            //TA10500135test001   405102911-01        TR_6100_S_14/0076T102405102911-011
            //ESZ214000966991700004424.0000000000.000000000001/test001/0
            string sendData = HEAD_CANCEL;
            sendData += GetPacketNo();
            sendData += id.PadRight(10, BODY_SPACE);
            sendData += account.PadRight(20, BODY_SPACE);
            sendData += "TR_6100_S_14/0076T102";
            sendData += account;
            sendData += tradeType == TRADETYPE.SELL ? "1" : "2";
            sendData += symbol.PadLeft(12, BODY_SPACE) + "4";
            sendData += string.Format("{0:0000000000}", orderNo);
            sendData += price.PadLeft(11, BODY_0);
            sendData += "00000000.00" + "00000" + string.Format("{0:00000}", quantity);
            sendData += "/" + id + "/0";
            return SendData(sendData);
        }

        public bool RequestLogout()
        {
            string id = _id;
            string account = _account;
            //TA01100026test001   405102911-01        SaveRejectTTGmsg/test001/00/99999999999999999999999999999999999999999999999999999999999999999999999999999999999
            string sendData = HEAD_LOGOUT;
            sendData += GetPacketNo();
            sendData += id.PadRight(10, BODY_SPACE);
            sendData += account.PadRight(20, BODY_SPACE);
            sendData += "SaveRejectTTGmsg/";
            sendData += id + "/00/";
            sendData += new string(BODY_9, BODY_REPEAT_83);

            return SendData(sendData);
        }

        public bool RequestLogoutEnd()
        {
            string id = _id;
            string account = _account;
            //TA00600035test003   086204217-01        TR_0100_S_03/test003/99999999999999999999999999999999999999999999999999999999999999999999999999999999999
            string sendData = HEAD_LOGOUTEND;
            sendData += GetPacketNo();
            sendData += id.PadRight(10, BODY_SPACE);
            sendData += account.PadRight(20, BODY_SPACE);
            sendData += "TR_0100_S_03/";
            sendData += id + "/";
            sendData += new string(BODY_9, BODY_REPEAT_83);

            return SendData(sendData);
        }

        protected bool SendData(string sendData)
        {
            if (sendData.Length < 1)
                return false;

            if (_ClientSocket == null || !_ClientSocket.Connected)
                return false;
#if WRITE_LOG
            WriteLog("<<<" + sendData);
#endif
            byte[] byteData = EncryptMsg(Encoding.Default.GetBytes(sendData));

            base.SendData(byteData);
            return true;
        }

        protected byte[] _PackBuffer = null;

        public override void ExtractPacket()
        {
            byte[] recvBytes;
            int recvLength = 0;
            List<string> recvMsgs = new List<string>();
            
            lock (_ReceiveBuffer)
            {
                recvLength = _ReceiveSize;
                _ReceiveSize = 0;

                if (recvLength < HEAD_SIZE_LENGTH)
                    return;

                string strLen = Encoding.ASCII.GetString(_ReceiveBuffer, 0, HEAD_SIZE_LENGTH);
                int nPacketLen = 0;
                if (!int.TryParse(strLen, out nPacketLen))
                {
                    nPacketLen = 0;
                }

                if (nPacketLen == 0 || recvLength != HEAD_SIZE_LENGTH + nPacketLen)
                {
                    int packSize = getPacketLen(_ReceiveBuffer);
#if WRITE_LOG
                    WriteLog(">>>pk_2>>>len=" + strLen + " packSize=" + packSize);
                    // WriteBytes(ByteArrayToString(_ReceiveBuffer, packSize));
#endif
                    if (packSize < 1)
                        return;
                    if (_PackBuffer != null)
                    {
                        Array.Resize(ref _PackBuffer, _PackBuffer.Length + packSize);
                    }
                    else
                    {
                        _PackBuffer = new byte[packSize];
                    }

                    Buffer.BlockCopy(_ReceiveBuffer, 0, _PackBuffer, _PackBuffer.Length - packSize, packSize);

                    strLen = Encoding.ASCII.GetString(_PackBuffer, 0, HEAD_SIZE_LENGTH);
                    if (!int.TryParse(strLen, out nPacketLen))
                    {
                        return;
                    }

                    if (_PackBuffer.Length < nPacketLen)
                        return;

                    int i = 0;
                    int bufferPos = HEAD_SIZE_LENGTH;
                    string recvMsg = "";
                    byte[] byteDecode = null;
                    while (bufferPos + nPacketLen <= _PackBuffer.Length)
                    {
                        recvBytes = new byte[nPacketLen];
                        Buffer.BlockCopy(_PackBuffer, bufferPos, recvBytes, 0, nPacketLen);

                        byteDecode = DecryptMsg(recvBytes);
                        if (byteDecode == null)
                            break ;

                        recvMsg = Encoding.Default.GetString(byteDecode);
#if WRITE_LOG
                        WriteLog(">>>pk_2>>>" + recvMsg);
#endif
                        if (recvMsg.Length <= HEAD_NAME_LENGTH)
                            break;

                        recvMsgs.Add(recvMsg);

                        bufferPos += nPacketLen;

                        if (bufferPos + HEAD_SIZE_LENGTH >= _PackBuffer.Length)
                            break;

                        nPacketLen = 0;
                        strLen = Encoding.ASCII.GetString(_PackBuffer, bufferPos, HEAD_SIZE_LENGTH);
                        if (!int.TryParse(strLen, out nPacketLen))
                        {
                            break ;
                        }
                        bufferPos += HEAD_SIZE_LENGTH;

                        if (nPacketLen < 1)
                            break;

                        if (i++ > 5)
                            break;

                    }


                    _PackBuffer = null;

                }
                else
                {
                    recvBytes = new byte[nPacketLen];
                    Buffer.BlockCopy(_ReceiveBuffer, HEAD_SIZE_LENGTH, recvBytes, 0, nPacketLen);
                    _PackBuffer = null;

                    byte[] byteDecode = DecryptMsg(recvBytes);
                    if (byteDecode == null)
                        return;

                    string recvMsg = Encoding.Default.GetString(byteDecode);
#if WRITE_LOG
                    WriteLog(">>>>>>" + recvMsg);
#endif
                    if (recvMsg.Length <= HEAD_NAME_LENGTH)
                        return;

                    recvMsgs.Add(recvMsg);
                }




            }


            if (_ResponseList.Count < 2000 && recvMsgs.Count > 0)
            {

                lock (_ResponseList)
                {
                    _ResponseList.AddRange(recvMsgs);
                }
            }


        }

        private void CreateLogFile()
        {
            _logPath = "D://BinHts/Rean_conn_" + DateTime.Now.ToString("yyyyMMdd");
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
        public void WriteBytes(string strLog)
        {
            try
            {
                string path = "D://BinHts/Rean_byte_" + DateTime.Now.ToString("yyyyMMdd");
                path += ".txt";

                DateTime dtServer = DateTime.Now;
                string log = string.Format("[{0:D2}:{1:D2}:{2:D2}] \n", dtServer.Hour, dtServer.Minute, dtServer.Second);
                log += strLog;
                using (StreamWriter outputFile = new StreamWriter(path, true))
                {
                    outputFile.WriteLine(log);
                }

            }
            catch (Exception ex)
            { string error = ex.Message; }

        }

        public string ByteArrayToString(byte[] bytes, int len)
        {
            StringBuilder hex = new StringBuilder();
            if (len > bytes.Length)
                return "[ error ] bytes";
            int ln = 0;
            byte b = 0;
            for (int i = 0; i < len; i++)
            {
                b = bytes[i];
                hex.AppendFormat("{0:x2} ", b);
                if (ln++ >= 15)
                {
                    ln = 0;
                    hex.Append("\n");
                }
            }
            return hex.ToString();
        }

        public int getPacketLen(byte[] recBytes)
        {
            int len = 0;
            int bSize = recBytes.Length;
            for (int i = 0; i < bSize - 5; i++)
            {
                if (recBytes[i] == 0 && recBytes[i + 1] == 0 && recBytes[i + 2] == 0 && recBytes[i + 3] == 0 && recBytes[i + 4] == 0)
                    break;
                len++;
            }
            return len;
        }

        public void OnDataReceiveEvent(Object obj)
        {
            if (DataReceiveEvent != null)
            {
                DataReceiveEvent(this, new SocketEventArgs(obj));

            }

        }

        protected byte[] EncryptMsg(byte[] byteData)
        {
            if (byteData.Length < 1)
                return null;

            byte btMask = 0x1F;
            string encodeBuffer = "";
            for (int i = 0; i < byteData.Length; i++)
            {
                encodeBuffer += (byteData[i] ^ btMask).ToString("X2");
                btMask += 0xF;
            }

            int bufferSize = Encoding.ASCII.GetBytes(encodeBuffer).Length;

            encodeBuffer = bufferSize.ToString("D5") + encodeBuffer;

            return Encoding.ASCII.GetBytes(encodeBuffer);

        }

        protected byte[] DecryptMsg(byte[] byteData)
        {
            if (byteData.Length < 2)
                return null;
            int nDecLen = byteData.Length / 2;

            byte[] byteDecode = new byte[nDecLen];
            string strEncode = Encoding.ASCII.GetString(byteData);

            byte btMask = 0x1F;
            for (int i = 0; i < nDecLen; i++)
            {
                byteDecode[i] = Convert.ToByte(strEncode.Substring(i * 2, 2), 16);
                byteDecode[i] ^= btMask;
                btMask += 0xF;
            }
            return byteDecode;
        }
    }
}
