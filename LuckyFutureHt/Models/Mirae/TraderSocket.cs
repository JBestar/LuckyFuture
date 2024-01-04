// #define WRITE_LOG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using LuckyFutureLib.AsynSocket;
using LuckyFuture.Models.ValueObjects;
using LuckyFutureLib.Include;

namespace LuckyFuture.Models.Reanteck
{
    class TraderSocket : AsynSocketClient
    {
        //[DllImport("Zip.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true, CharSet = CharSet.Unicode)]
        [DllImport("zip.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ts_uncompress(
            [In][MarshalAs(UnmanagedType.LPArray)] Byte[] src,
            [In] int srcLen,
            [In, Out][MarshalAs(UnmanagedType.LPArray)] Byte[] dst,
            [In, Out][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U4)] IntPtr[] dstLen
        );

        public event EventHandler<SocketEventArgs> DataReceiveEvent;
        private Thread _ExtractThread = null;
        public List<byte[]> _ResponsePkList = new List<byte[]>();

        public const int HEAD_SIZE_LENGTH = 10;
        public const int HEAD_INFO_LENGTH = 100;
        public const int HEAD_ID_LENGTH = 11;

        public const int BODY_INFO_LENGTH = 210;
        public const int BODY_ID_LENGTH = 6;


        const char CHAR_SPACE = ' ';
        const char CHAR_ZERO = '0';

        public const string HEAD_PREFIX_I = "SVC_I";      //SEND
        public const string HEAD_PREFIX_O = "SVC_O";      //RECV

        public const string HEAD_TR0010 = "TR0010";

        public const string HEAD_TR0020 = "TR0020"; //Control Tick Signal
        public const string HEAD_TR0021 = "TR0021"; //Control Quote Signal
        public const string HEAD_TR0030 = "TR0030";

        public const string HEAD_TR0099 = "TR0099";
        public const string HEAD_BQ0005 = "BQ0005";
        public const string HEAD_BQ0006 = "BQ0006";//ItemList
        public const string HEAD_BQ0008 = "BQ0008";
        public const string HEAD_BQ0009 = "BQ0009";
        public const string HEAD_CMO001 = "CMO001";
        public const string HEAD_CMO101 = "CMO101"; //Order
        public const string HEAD_CMO104 = "CMO104"; //Cancel Order
        public const string HEAD_CMO401 = "CMO401"; //Liquid

        public const string HEAD_CQ2001 = "CQ2001"; //Receipt List
        public const string HEAD_CQ3001 = "CQ3001";
        public const string HEAD_CQ3002 = "CQ3002"; //User Account
        public const string HEAD_CQ3003 = "CQ3003";
        public const string HEAD_CQ4001 = "CQ4001"; //Ordered List

        public const string HEAD_PI501Q = "PI501Q"; //Control Signal Tick and Quote

        public const string HEAD_HBPKT = "HBPKT";
        public const string HEAD_RDPKT = "RDPKT";

        public const string BODY_ID_TICK = "R_TICK";
        public const string BODY_ID_QUOT = "R_QUOT";
        public const string BODY_ID_CNDL = "R_CNDL";
        public const string BODY_ID_ORDR = "R_ORDR";
        public const string BODY_ID_NTRD = "R_NTRD";
        //public const string BODY_ID_TRAD = "R_TRAD";
        public const string BODY_ID_OPEN = "R_OPEN";
        //public const string BODY_ID_WLET = "R_WLET";
        public const string BODY_ID_ASET = "R_ASET";
        public const string BODY_ID_EARN = "R_EARN";
        public const string BODY_ID_IOAM = "R_IOAM";

        protected int _PacketNo = 0;
        protected string _id = "";
        protected string _account = "";
        string _logPath = "";

        public TraderSocket(string ipAddr, int nPort) : base(ipAddr, nPort)
        {
            _PacketNo = 1;
            _id = "";
            _account = "";
#if WRITE_LOG
            CreateLogFile();
#endif
        }
        public void SetUserInfo(string id, string account)
        {
            _id = id;
            _account = account;
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
        protected string GetPacketNo()
        {
            if (_PacketNo > 99990)
                _PacketNo = 0;

            _PacketNo++;
            return String.Format("{0:0000000000}", _PacketNo);
        }
        public void RequestLogin(string id)
        {
            _id = id;
            string headId = "LOGIN";
            //"0000000122LOGIN                                                                                     mbc01   mbc01    
            string headData = headId.PadRight(90, CHAR_SPACE);

            string bodyData = id.PadRight(8, CHAR_SPACE);
            bodyData += id.PadRight(9, CHAR_SPACE);
            byte[] bodyBytes = Encoding.Default.GetBytes(bodyData);
            byte[] endBytes = new byte[] { 0x00, 0x00, 0x00, 0x53, 0x56, 0x43, 0x5F, 0x20,
                                            0x00, 0x00, 0x00, 0x53, 0x56, 0x43, 0x5F
            };
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);

            int cntBytes = Encoding.Default.GetByteCount(headData) + bodyBytes.Length;
            headData = cntBytes.ToString().PadLeft(HEAD_SIZE_LENGTH, CHAR_ZERO) + headData;
            byte[] headBytes = Encoding.Default.GetBytes(headData);

            byte[] sendBytes = Extension.CombineBytes(headBytes, bodyBytes);
            StartExtract();
            SendBytes(sendBytes, headId);
        }
        public bool RequestLogout()
        {
            //             string sendData = HEAD_PREFIX_I;
            // 
            //             return SendData(sendData);
            return true;
        }

        public bool RequestLogoutEnd()
        {
            RequestTr0030(false);

            //             string sendData = HEAD_PREFIX_I;
            // 
            //             return SendData(sendData);
            return true;
        }

        public bool RequestPacket(string headId, byte[] bodyBytes, bool bLog = false)
        {
            string pkHead = HEAD_PREFIX_I + headId;
            //Set Body Size
            int cntBytes = bodyBytes.Length;
            string sCnt = cntBytes.ToString().PadLeft(HEAD_SIZE_LENGTH, CHAR_ZERO);
            bodyBytes = Extension.CombineBytes(Encoding.Default.GetBytes(sCnt), bodyBytes);

            //Set Head Data
            string headData = pkHead.PadRight(21, CHAR_SPACE);
            headData += GetPacketNo();
            headData += "".PadRight(5, CHAR_ZERO);
            headData += "".PadRight(54, CHAR_SPACE);

            //Set Total Size
            cntBytes = Encoding.Default.GetByteCount(headData) + bodyBytes.Length;
            headData = cntBytes.ToString().PadLeft(HEAD_SIZE_LENGTH, CHAR_ZERO) + headData;
            byte[] headBytes = Encoding.Default.GetBytes(headData);

            byte[] sendBytes = Extension.CombineBytes(headBytes, bodyBytes);
            SendBytes(sendBytes, headId, bLog);
            return true;

        }

        public bool RequestTr0010()
        {
            //             "0000000404SVC_ITR0010          000000000100000                                                      " +
            //             "0000000304 mbc01             TR0010   00           O0002                                                                                                                                                         " +
            //             "mbc01   192.168.92.1   5A-FB-84-0E-CE-64             DESKTOP-61Q2H0A                                   N"
            string headId = HEAD_TR0010;
            string id = _id;

            string bodyData = CHAR_SPACE + id.PadRight(18, CHAR_SPACE);
            bodyData += headId.PadRight(9, CHAR_SPACE);
            bodyData += "00".PadRight(13, CHAR_SPACE);
            byte[] bodyBytes = Encoding.Default.GetBytes(bodyData);
            byte[] endBytes = new byte[] { 0x00 };
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);

            string ipAddr = "192.168.92.1";
            string ipName = "5A-FB-84-0E-CE-64";
            string comName = "DESKTOP-61Q2H0A";
            string bodyData2 = "O0002".PadRight(158, CHAR_SPACE);
            bodyData2 += id.PadRight(8, CHAR_SPACE); //len=8
            bodyData2 += ipAddr.PadRight(15, CHAR_SPACE); //len=15
            bodyData2 += ipName.PadRight(30, CHAR_SPACE); //len=30
            bodyData2 += comName.PadRight(50, CHAR_SPACE); //len=50
            bodyData2 += "N";
            endBytes = Encoding.Default.GetBytes(bodyData2);
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);

            return RequestPacket(headId, bodyBytes);
        }

        public bool RequestAccount()
        {
            string id = _id;
            string headId = HEAD_CQ3002;

            string bodyData = CHAR_SPACE + id.PadRight(18, CHAR_SPACE);
            bodyData += headId.PadRight(9, CHAR_SPACE);
            bodyData += "00".PadRight(11, CHAR_SPACE) + "00";
            byte[] bodyBytes = Encoding.Default.GetBytes(bodyData);
            byte[] endBytes = new byte[] { 0x00 };
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);


            string bodyData2 = "Q0002".PadRight(17, CHAR_SPACE);
            bodyData2 += "00000".PadRight(141, CHAR_SPACE); //len=8
            bodyData2 += id.PadRight(8, CHAR_SPACE);

            endBytes = Encoding.Default.GetBytes(bodyData2);
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);

            return RequestPacket(headId, bodyBytes);

        }

        public bool RequestCq3001()
        {
            string id = _id;
            string headId = HEAD_CQ3001;

            string bodyData = CHAR_SPACE + id.PadRight(18, CHAR_SPACE);
            bodyData += headId.PadRight(9, CHAR_SPACE);
            bodyData += "00".PadRight(11, CHAR_SPACE) + "00";
            byte[] bodyBytes = Encoding.Default.GetBytes(bodyData);
            byte[] endBytes = new byte[] { 0x00 };
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);


            string bodyData2 = "Q0002".PadRight(17, CHAR_SPACE);
            bodyData2 += "00000".PadRight(141, CHAR_SPACE); //len=8
            bodyData2 += id.PadRight(8, CHAR_SPACE);

            endBytes = Encoding.Default.GetBytes(bodyData2);
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);

            return RequestPacket(headId, bodyBytes);

        }

        public bool RequestBq0005()
        {
            string id = _id;
            string headId = HEAD_BQ0005;

            string bodyData = CHAR_SPACE + id.PadRight(18, CHAR_SPACE);
            bodyData += headId.PadRight(9, CHAR_SPACE);
            bodyData += "00".PadRight(13, CHAR_SPACE);
            byte[] bodyBytes = Encoding.Default.GetBytes(bodyData);
            byte[] endBytes = new byte[] { 0x00 };
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);

            string bodyData2 = "Q0002".PadRight(17, CHAR_SPACE);
            bodyData2 += "00000".PadRight(173, CHAR_SPACE);
            endBytes = Encoding.Default.GetBytes(bodyData2);
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);

            return RequestPacket(headId, bodyBytes);
        }

        public bool RequestCommon(string headId) //HEAD_BQ0009 HEAD_BQ0008 HEAD_CQ3003
        {
            string id = _id;
            //string headId = HEAD_BQ0009;

            string bodyData = CHAR_SPACE + id.PadRight(18, CHAR_SPACE);
            bodyData += headId.PadRight(9, CHAR_SPACE);
            bodyData += "00".PadRight(13, CHAR_SPACE);
            byte[] bodyBytes = Encoding.Default.GetBytes(bodyData);
            byte[] endBytes = new byte[] { 0x00 };
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);

            string bodyData2 = "Q0002".PadRight(17, CHAR_SPACE);
            bodyData2 += "00000".PadRight(141, CHAR_SPACE);
            bodyData2 += id.PadRight(8, CHAR_SPACE);
            endBytes = Encoding.Default.GetBytes(bodyData2);
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);

            return RequestPacket(headId, bodyBytes);
        }

        public bool RequestTr0030(bool bOpen)
        {
            string id = _id;
            string headId = HEAD_TR0030;

            string bodyData = CHAR_SPACE + id.PadRight(18, CHAR_SPACE);
            bodyData += headId.PadRight(9, CHAR_SPACE);
            bodyData += "00".PadRight(13, CHAR_SPACE);
            bodyData += "1O0001".PadRight(159, CHAR_SPACE);
            bodyData += (bOpen ? "1" : "0");
            byte[] bodyBytes = Encoding.Default.GetBytes(bodyData);
            byte[] endBytes = new byte[] { 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00 };
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);

            string bodyData2 = _account.PadRight(15, CHAR_SPACE);
            byte[] bodyBytes2 = Encoding.Default.GetBytes(bodyData2);
            endBytes = new byte[0x3A89];
            Array.Clear(endBytes, 0, endBytes.Length);
            bodyBytes2 = Extension.CombineBytes(bodyBytes2, endBytes);

            bodyBytes = Extension.CombineBytes(bodyBytes, bodyBytes2);
            return RequestPacket(headId, bodyBytes, false);

        }

        public bool RequestCM001A()
        {
            string id = _id;
            string headId = HEAD_TR0030;

            string bodyData = CHAR_SPACE + id.PadRight(18, CHAR_SPACE);
            bodyData += headId.PadRight(9, CHAR_SPACE);
            bodyData += "00".PadRight(13, CHAR_SPACE);
            byte[] bodyBytes = Encoding.Default.GetBytes(bodyData);
            byte[] endBytes = new byte[] { 0x00 };
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);


            string bodyData2 = "O0003".PadRight(158, CHAR_SPACE);
            bodyData2 += id.PadRight(8, CHAR_SPACE);
            bodyData2 += "(빠른주문 0) - 원클릭설정 (N) - 101V3000 종목선택";
            bodyData2 += "".PadRight(51, CHAR_SPACE);

            byte[] bodyBytes2 = Encoding.Default.GetBytes(bodyData2);
            endBytes = new byte[0x64];
            Array.Clear(endBytes, 0, endBytes.Length);
            bodyBytes2 = Extension.CombineBytes(bodyBytes2, endBytes);
            //0x20 * 3A
            bodyData2 = "".PadRight(0x3A, CHAR_SPACE);
            endBytes = Encoding.Default.GetBytes(bodyData2);
            bodyBytes2 = Extension.CombineBytes(bodyBytes2, endBytes);

            bodyBytes = Extension.CombineBytes(bodyBytes, bodyBytes2);
            return RequestPacket(headId, bodyBytes, false);

        }

        public bool RequestItemlist()
        {
            string id = _id;
            string headId = HEAD_BQ0006;

            string bodyData = CHAR_SPACE + id.PadRight(18, CHAR_SPACE);
            bodyData += headId.PadRight(9, CHAR_SPACE);
            bodyData += "00".PadRight(13, CHAR_SPACE);
            byte[] bodyBytes = Encoding.Default.GetBytes(bodyData);
            byte[] endBytes = new byte[] { 0x00 };
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);

            string bodyData2 = "Q0002".PadRight(17, CHAR_SPACE);
            bodyData2 += "00000".PadRight(141, CHAR_SPACE) + "00000";
            endBytes = Encoding.Default.GetBytes(bodyData2);
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);

            return RequestPacket(headId, bodyBytes);
        }
        public bool RequestCurrency(string symbol, int index)
        {
            string id = _id;
            string headId = HEAD_PI501Q;

            string bodyData = CHAR_SPACE + id.PadRight(18, CHAR_SPACE);
            bodyData += headId.PadRight(9, CHAR_SPACE);
            bodyData += "00".PadRight(11, CHAR_SPACE) + "00";
            byte[] bodyBytes = Encoding.Default.GetBytes(bodyData);
            byte[] endBytes = new byte[] { 0x00 };
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);

            string bodyData2 = "Q0003".PadRight(17, CHAR_SPACE);
            bodyData2 += "00000".PadRight(141, CHAR_SPACE);
            bodyData2 += "00001" + symbol.PadRight(32, CHAR_SPACE);
            bodyData2 += index.ToString().PadLeft(5, CHAR_ZERO);
            endBytes = Encoding.Default.GetBytes(bodyData2);

            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);
            return RequestPacket(headId, bodyBytes);

        }
        public bool RequestControlTick(string symbol, bool bOpen)
        {
            string id = _id;
            string headId = HEAD_TR0020;

            string bodyData = CHAR_SPACE + id.PadRight(18, CHAR_SPACE);
            bodyData += headId.PadRight(9, CHAR_SPACE);
            bodyData += "00".PadRight(13, CHAR_SPACE);
            bodyData += "1O0001".PadRight(159, CHAR_SPACE);
            bodyData += (bOpen ? "1" : "0");
            byte[] bodyBytes = Encoding.Default.GetBytes(bodyData);
            byte[] endBytes = new byte[] { 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00 };
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);

            bodyBytes = Extension.CombineBytes(bodyBytes, Encoding.Default.GetBytes(BODY_ID_TICK));
            endBytes = new byte[] { 0x00, 0x00 };
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);

            string bodyData2 = symbol.PadRight(32, CHAR_SPACE);
            endBytes = Encoding.Default.GetBytes(bodyData2);

            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);
            return RequestPacket(headId, bodyBytes, true);

        }

        public bool RequestControlQuote(string symbol, bool bOpen)
        {
            string id = _id;
            string headId = HEAD_TR0021;

            string bodyData = CHAR_SPACE + id.PadRight(18, CHAR_SPACE);
            bodyData += headId.PadRight(9, CHAR_SPACE);
            bodyData += "00".PadRight(13, CHAR_SPACE);
            bodyData += "1O0001".PadRight(159, CHAR_SPACE);
            bodyData += (bOpen ? "1" : "0");
            byte[] bodyBytes = Encoding.Default.GetBytes(bodyData);
            byte[] endBytes = new byte[] { 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00 };
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);

            bodyBytes = Extension.CombineBytes(bodyBytes, Encoding.Default.GetBytes(BODY_ID_QUOT));
            endBytes = new byte[] { 0x00, 0x00 };
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);

            string bodyData2 = symbol.PadRight(32, CHAR_SPACE);
            endBytes = Encoding.Default.GetBytes(bodyData2);

            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);
            return RequestPacket(headId, bodyBytes, true);

        }

        public bool RequestReceiptlist()
        {
            string id = _id;
            string headId = HEAD_CQ2001;

            string bodyData = CHAR_SPACE + id.PadRight(18, CHAR_SPACE);
            bodyData += headId.PadRight(9, CHAR_SPACE);
            bodyData += "00".PadRight(11, CHAR_SPACE) + "00";
            byte[] bodyBytes = Encoding.Default.GetBytes(bodyData);
            byte[] endBytes = new byte[] { 0x00 };
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);

            string bodyData2 = "Q0002".PadRight(17, CHAR_SPACE);
            bodyData2 += "00000".PadRight(141, CHAR_SPACE);
            bodyData2 += id.PadRight(8, CHAR_SPACE);
            byte[] bodyBytes2 = Encoding.Default.GetBytes(bodyData2);

            bodyBytes = Extension.CombineBytes(bodyBytes, bodyBytes2);
            return RequestPacket(headId, bodyBytes, true);

        }
        public bool RequestOrderlist()
        {
            string id = _id;
            string headId = HEAD_CQ4001;

            string bodyData = CHAR_SPACE + id.PadRight(18, CHAR_SPACE);
            bodyData += headId.PadRight(9, CHAR_SPACE);
            bodyData += "00".PadRight(11, CHAR_SPACE) + "00";
            byte[] bodyBytes = Encoding.Default.GetBytes(bodyData);
            byte[] endBytes = new byte[] { 0x00 };
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);

            string bodyData2 = "Q0002".PadRight(17, CHAR_SPACE);
            bodyData2 += "00000".PadRight(141, CHAR_SPACE);
            bodyData2 += id.PadRight(8, CHAR_SPACE);
            byte[] bodyBytes2 = Encoding.Default.GetBytes(bodyData2);

            bodyBytes = Extension.CombineBytes(bodyBytes, bodyBytes2);
            return RequestPacket(headId, bodyBytes, true);
        }
        public bool RequestKeepAlive()
        {
            string id = _id;
            string headId = HEAD_TR0099;

            string bodyData = CHAR_SPACE + id.PadRight(18, CHAR_SPACE);
            bodyData += headId.PadRight(9, CHAR_SPACE);
            bodyData += "00".PadRight(13, CHAR_SPACE);
            bodyData += "1O0001".PadRight(159, CHAR_SPACE);
            bodyData += id.PadRight(8, CHAR_SPACE);
            byte[] bodyBytes = Encoding.Default.GetBytes(bodyData);

            return RequestPacket(headId, bodyBytes);
        }
        public bool RequestOrder(string account, TRADETYPE tradeType, string symbol, double orderPrice, int quantity, bool marketing, double currentPrice)
        {
            string id = _id;
            string headId = HEAD_CMO101;

            string bodyData = CHAR_SPACE + id.PadRight(18, CHAR_SPACE);
            bodyData += headId.PadRight(9, CHAR_SPACE);
            bodyData += "00".PadRight(13, CHAR_SPACE);
            byte[] endBytes = new byte[] { 0x00 };
            byte[] bodyBytes = Encoding.Default.GetBytes(bodyData);
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);

            string bodyData2 = "O0003".PadRight(158, CHAR_SPACE);
            bodyData2 += account.PadRight(23, CHAR_SPACE);
            bodyData2 += symbol.PadRight(32, CHAR_SPACE);
            bodyData2 += tradeType == TRADETYPE.SELL ? "S" : "B";
            if (marketing)
            {
                bodyData2 += "00000000000.00000000";
                bodyData2 += "00000000000.00000000";
            }
            else
            {
                bodyData2 += string.Format("{0:00000000000.00000000}", currentPrice);
                bodyData2 += string.Format("{0:00000000000.00000000}", orderPrice);
            }
            bodyData2 += quantity.ToString().PadLeft(11, CHAR_ZERO);
            bodyData2 += ".00000000";
            bodyData2 += marketing ? "21" : "23";
            bodyData2 += "".PadRight(21, CHAR_SPACE);
            if (marketing)
            {
                bodyData2 += "(빠른주문 1) - 시장가 " + (tradeType == TRADETYPE.SELL ? "매도주문" : "매수주문");
                bodyData2 += "".PadRight(50, CHAR_SPACE);
            }
            else
            {
                bodyData2 += "(빠른주문 1) - 호가더블클릭 - MIT " + (tradeType == TRADETYPE.SELL ? "매도주문" : "매수주문");
                bodyData2 += "".PadRight(38, CHAR_SPACE);
            }

            endBytes = Encoding.Default.GetBytes(bodyData2);

            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);
            return RequestPacket(headId, bodyBytes, true);

        }
        public bool RequestLiquid(string account, TRADETYPE tradeType, string symbol, string price, int quantity, bool marketing)
        {
            string id = _id;
            string headId = HEAD_CMO401;

            string bodyData = CHAR_SPACE + id.PadRight(18, CHAR_SPACE);
            bodyData += headId.PadRight(9, CHAR_SPACE);
            bodyData += "00".PadRight(13, CHAR_SPACE);
            byte[] endBytes = new byte[] { 0x00 };
            byte[] bodyBytes = Encoding.Default.GetBytes(bodyData);
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);

            string bodyData2 = "O0004".PadRight(158, CHAR_SPACE);
            bodyData2 += account.PadRight(23, CHAR_SPACE);
            bodyData2 += symbol.PadRight(32, CHAR_SPACE);
            bodyData2 += tradeType == TRADETYPE.SELL ? "S" : "B";

            bodyData2 += "(빠른주문 1) - 그리드 청산(" + symbol + ")-" + (tradeType == TRADETYPE.SELL ? "매도" : "매수");
            bodyData2 += "".PadRight(42, CHAR_SPACE);
            endBytes = Encoding.Default.GetBytes(bodyData2);

            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);
            return RequestPacket(headId, bodyBytes, true);

        }
        public bool RequestCancel(string account, TRADETYPE tradeType, string symbol, string price, int quantity, long orderNo)
        {
            string id = _id;
            string headId = HEAD_CMO104;

            string bodyData = CHAR_SPACE + id.PadRight(18, CHAR_SPACE);
            bodyData += headId.PadRight(9, CHAR_SPACE);
            bodyData += "00".PadRight(13, CHAR_SPACE);
            byte[] endBytes = new byte[] { 0x00 };
            byte[] bodyBytes = Encoding.Default.GetBytes(bodyData);
            bodyBytes = Extension.CombineBytes(bodyBytes, endBytes);

            string bodyData2 = "O0003".PadRight(158, CHAR_SPACE);
            bodyData2 += account.PadRight(23, CHAR_SPACE);
            bodyData2 += symbol.PadRight(32, CHAR_SPACE);
            bodyData2 += orderNo.ToString().PadRight(20, CHAR_SPACE);


            bodyData2 += tradeType == TRADETYPE.SELL ? "S" : "B";
            bodyData2 += "3(빠른주문 1) - 그리드 취소(" + symbol + "-";
            bodyData2 += orderNo.ToString().PadLeft(6, CHAR_ZERO) + ")";
            bodyData2 += "".PadRight(39, CHAR_SPACE);
            byte[] bodyBytes2 = Encoding.Default.GetBytes(bodyData2);

            endBytes = new byte[0x50];
            Array.Clear(endBytes, 0, endBytes.Length);
            bodyBytes2 = Extension.CombineBytes(bodyBytes2, endBytes);

            bodyBytes = Extension.CombineBytes(bodyBytes, bodyBytes2);
            return RequestPacket(headId, bodyBytes, true);

        }
        protected bool SendData(string sendData)
        {
            byte[] byteData = Encoding.Default.GetBytes(sendData);
#if WRITE_LOG
            WriteLog("<<<" + sendData);
#endif
            base.SendData(byteData);
            return true;
        }
        protected void SendBytes(byte[] sendBytes, string sName = "", bool bLog = false)
        {
#if WRITE_LOG
            if(bLog)
                WriteBytes(Extension.ByteArrayToString(sendBytes, sendBytes.Length), true, sName);
#endif
            base.SendData(sendBytes);
        }
        public override void ExtractPacket()
        {
            bool isReceived = true;
            byte[] recvBytes = new byte[Constants.BUFFER_MAX];
            byte[] packBytes = null;
            int recvLength = 0;
            List<byte[]> packetList = new List<byte[]>();

            string strLen = "";
            int nPacketLen = 0;

            isReceived = false;
            if (_ReceiveSize > HEAD_SIZE_LENGTH && recvLength + _ReceiveSize < Constants.BUFFER_MAX)
            {
                lock (_ReceiveBuffer)
                {
                    strLen = Encoding.ASCII.GetString(_ReceiveBuffer, 0, HEAD_SIZE_LENGTH);
                    nPacketLen = 0;
                    if (int.TryParse(strLen, out nPacketLen))
                    {
                        if (nPacketLen > 0x80000)
                        {
#if WRITE_LOG
                            WriteLog(string.Format(" Extra PacketLen={0}, ReceiveSize={1}  ", nPacketLen, _ReceiveSize));
#endif
                            _ReceiveSize = 0;
                        }
                        else if (HEAD_SIZE_LENGTH + nPacketLen <= _ReceiveSize)
                        {
                            Buffer.BlockCopy(_ReceiveBuffer, 0, recvBytes, recvLength, _ReceiveSize);
                            recvLength += _ReceiveSize;
                            _ReceiveSize = 0;
                            isReceived = true;
#if WRITE_LOG
                            WriteLog(string.Format(" Parse PacketLen={0}, ReceiveSize={1}  ", HEAD_SIZE_LENGTH + nPacketLen, recvLength));
#endif
                        }
                        else
                        {
#if WRITE_LOG
                            WriteLog(string.Format(" PacketLen={0}, ReceiveSize={1}  ", HEAD_SIZE_LENGTH + nPacketLen, _ReceiveSize));
#endif
                        }
                    }
                    else
                    {
#if WRITE_LOG
                        WriteLog("<<< _ReceiveSize Error=" + _ReceiveSize);
#endif
                        _ReceiveSize = 0;
                    }
                }
            }
            else if (recvLength + _ReceiveSize >= Constants.BUFFER_MAX)
            {
                _ReceiveSize = 0;
            }

            if (isReceived)
            {
                int pCurPos = 0;

                while (recvLength > HEAD_SIZE_LENGTH)
                {
                    strLen = Encoding.ASCII.GetString(recvBytes, pCurPos, HEAD_SIZE_LENGTH);
                    nPacketLen = 0;
                    if (int.TryParse(strLen, out nPacketLen))
                    {
                        if (HEAD_SIZE_LENGTH + nPacketLen >= HEAD_INFO_LENGTH && HEAD_SIZE_LENGTH + nPacketLen <= recvLength)
                        {
                            packBytes = new byte[HEAD_SIZE_LENGTH + nPacketLen];
                            Buffer.BlockCopy(recvBytes, pCurPos, packBytes, 0, HEAD_SIZE_LENGTH + nPacketLen);
#if WRITE_LOG
                            //WriteBytes(Extension.ByteArrayToString(packBytes, HEAD_SIZE_LENGTH + nPacketLen), false);
#endif
                            packetList.Add(packBytes);

                            pCurPos += HEAD_SIZE_LENGTH;
                            pCurPos += nPacketLen;
                            recvLength -= (HEAD_SIZE_LENGTH + nPacketLen);
                        }
                        else
                        {
                            break;   //if less than length of Packet, break;
                        }
                    }
                    else
                    {
                        break; //pCurPos += HEAD_SIZE_LENGTH;
                    }

                }
                if (packetList.Count > 0)
                {

                    if (_ResponsePkList.Count < 2000)
                    {
                        lock (_ResponsePkList)
                        {
                            _ResponsePkList.AddRange(packetList);
                        }
                    }

                }
            }
        }
        protected void ExtractPacketLoop()
        {
            while (true)
            {
                if (_ResponsePkList.Count > 0)
                {
                    lock (_ResponsePkList)
                    {
                        string headId = "";
                        foreach (byte[] packBytes in _ResponsePkList)
                        {
                            headId = getHeaderId(packBytes);
                            if (headId == HEAD_HBPKT)
                            {
#if WRITE_LOG
                                WriteLog("<<< ExtractPacketLoop Send HBPKT length=" + packBytes.Length);
#endif
                                SendData(packBytes);
                            }
                            string bodyId = getBodyId(packBytes);

#if WRITE_LOG
                            WriteLog("<<< ExtractPacketLoop ResponsePkList header=" + headId + " bodyId=" + bodyId + " size=" + packBytes.Length);
#endif
                            if (headId == HEAD_RDPKT && bodyId == BODY_ID_ORDR)
                            {
#if WRITE_LOG
                                WriteBytes(Extension.ByteArrayToString(packBytes, packBytes.Length), false);
#endif
                            }
                        }
                        OnDataReceiveEvent(SOCKET_EVENTTYPE.RECEIVE);
                    }
                }
                Thread.Sleep(10);
            }

        }
        public string getHeaderId(byte[] recvBytes)
        {
            if (recvBytes.Length < HEAD_INFO_LENGTH)
                return "";

            return Encoding.Default.GetString(recvBytes, HEAD_SIZE_LENGTH, HEAD_ID_LENGTH).Trim();
        }
        public string getBodyId(byte[] recvBytes)
        {
            if (recvBytes.Length < HEAD_INFO_LENGTH + BODY_INFO_LENGTH)
                return "";

            return Encoding.Default.GetString(recvBytes, HEAD_INFO_LENGTH + 0x1D, BODY_ID_LENGTH).Trim();
        }
        public int GetZipLen(byte[] recvBytes)
        {
            string strLen = Encoding.ASCII.GetString(recvBytes, HEAD_INFO_LENGTH + 0x6A, HEAD_SIZE_LENGTH);
            int nPacketLen = -1;
            if (!int.TryParse(strLen, out nPacketLen))
            {
                return nPacketLen;
            }
            return nPacketLen;
        }
        public int GetDatLen(byte[] recvBytes)
        {
            string strLen = Encoding.ASCII.GetString(recvBytes, HEAD_INFO_LENGTH + 0x60, HEAD_SIZE_LENGTH);
            int nPacketLen = -1;
            if (!int.TryParse(strLen, out nPacketLen))
            {
                return nPacketLen;
            }
            return nPacketLen;
        }

        public List<string> GetItemList(byte[] recvBytes)
        {
            List<string> listItem = new List<string>();
            int iEndPos = 0;
            int iCurPos = 0;
            int nCnt = recvBytes.Length;
            string sMsg = "";
            while (iCurPos < nCnt - 0xF)
            {
                iEndPos = GenEndPos(recvBytes, iCurPos);
                if (iEndPos < 0)
                {
                    break;
                }

                sMsg = Encoding.Default.GetString(recvBytes, iCurPos, iEndPos - iCurPos);
                listItem.Add(sMsg);
                iCurPos = iEndPos + 0xF;
                if (listItem.Count > 15)
                    break;
            }
            return listItem;
        }

        public static int GenEndPos(byte[] recvBytes, int offset)
        {
            int iPos = -1;
            int cnt = 0;
            for (int i = offset; i < recvBytes.Length - 0xF; i++)
            {
                cnt = 0;
                for (int j = i; j <= i + 0xE; j++)
                {
                    if (recvBytes[j] == 0)
                    {
                        cnt++;
                    }
                    else break;
                }
                if (cnt >= 0xF)
                {
                    iPos = i;
                    break;
                }

            }
            return iPos;
        }

        private void CreateLogFile()
        {
            _logPath = "D://BinHts/Mirae_trader_" + DateTime.Now.ToString("yyyyMMdd");
            _logPath += ".txt";
            WriteLog("<!=============시작중입니다.===============>");
            WriteBytes("<!=============시작중입니다.===============>");

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
        public void WriteBytes(string strLog, bool bSend = true, string sName = "")
        {
            try
            {
                string path = "D://BinHts/Merae_byte_" + DateTime.Now.ToString("yyyyMMdd");
                path += ".txt";

                DateTime dtServer = DateTime.Now;
                string log = string.Format("[{0:D2}:{1:D2}:{2:D2}] Trader {3} {4} \n", dtServer.Hour, dtServer.Minute, dtServer.Second, sName, bSend ? ">>>>Send" : "<<<<<<Receive");
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
