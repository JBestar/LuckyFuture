using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using LuckyFutureLib.AsynSocket;

namespace LuckyFuture.Models.Reanteck
{
    class CurrencySocket : AsynSocketClient
    {
        public event EventHandler<SocketEventArgs> DataReceiveEvent;

        const int HEAD_SIZE_LENGTH = 4;
        

        const byte HEAD_ATSIGN = (byte)'@';


        private Thread _ExtractThread = null;

        public CurrencySocket(string ipAddr, int nPort) : base(ipAddr, nPort)
        {

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
            if(_ExtractThread != null && _ExtractThread.IsAlive)
            {
                _ExtractThread.Abort();
                _ExtractThread = null;
            }
        }

        public void RequestLogin(string id)
        {
            string sendData = "LOGIN/";
            sendData += id;
            sendData += "/7";

            StartExtract();
            SendData(sendData);
            
        }

        protected void SendData(string sendData)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(sendData);

            base.SendData(byteData);
        }

        protected void ExtractPacketLoop()
        {
            bool isReceived = true;
            byte[] recvBytes = new byte[Constants.BUFFER_MAX];
            int recvLength = 0;
            List<string> packetList = new List<string>();

            string strLen = "";
            int nPacketLen = 0;
            int pCurPos = 0;
            int posAtSign = -1;
            while (true)
            {
                isReceived = false;
                if (_ReceiveSize > HEAD_SIZE_LENGTH && recvLength + _ReceiveSize < Constants.BUFFER_MAX)
                {
                    lock (_ReceiveBuffer)
                    {
                    
                        Buffer.BlockCopy(_ReceiveBuffer, 0, recvBytes, recvLength, _ReceiveSize);
                        recvLength += _ReceiveSize;
                        _ReceiveSize = 0;
                        isReceived = true;
                    }
                }
                else if(recvLength + _ReceiveSize >= Constants.BUFFER_MAX)
                {
                    recvLength = 0;
                }

                if (isReceived)
                {
                    packetList.Clear();

                    pCurPos = 0;
                    while (recvLength > HEAD_SIZE_LENGTH)
                    {
                        if (recvBytes[pCurPos+ HEAD_SIZE_LENGTH] == HEAD_ATSIGN)
                        {                            
                            strLen = Encoding.ASCII.GetString(recvBytes, pCurPos, HEAD_SIZE_LENGTH);
                            nPacketLen = 0;
                            if (int.TryParse(strLen, out nPacketLen))
                            {
                                if (HEAD_SIZE_LENGTH + nPacketLen <= recvLength)
                                {
                                    pCurPos += HEAD_SIZE_LENGTH;
                                    if (nPacketLen > 1)
                                        packetList.Add(Encoding.ASCII.GetString(recvBytes, pCurPos+1, nPacketLen - 1));
                                    pCurPos += nPacketLen;
                                    recvLength -= (HEAD_SIZE_LENGTH + nPacketLen);
                                } else break;   //if less than length of Packet, break;
                            }
                            else //if length of Packet is error, break;
                            {
                                recvLength = 0;
                                break;
                            }
                        }
                        else if (recvLength > 2000)
                        {
                            try { 
                                posAtSign = Array.IndexOf(recvBytes, HEAD_ATSIGN, pCurPos + HEAD_SIZE_LENGTH, recvLength - HEAD_SIZE_LENGTH);
                                if (posAtSign >= 0)
                                {
                                    recvLength -= (posAtSign + HEAD_SIZE_LENGTH - pCurPos) ;
                                    pCurPos = posAtSign - HEAD_SIZE_LENGTH;
                                
                                }
                                else
                                {
                                    recvLength = 0;
                                    break;
                                }
                            }
                            catch (Exception)
                            {
                                recvLength = 0;
                                break;
                            }
                        }
                        else break;
                    }
                    if(packetList.Count > 0)
                    {
                        
                        if (_ResponseList.Count < 2000)
                        {
                            lock (_ResponseList)
                            {
                                _ResponseList.AddRange(packetList);
                            }
                            OnDataReceiveEvent(SOCKET_EVENTTYPE.RECEIVE);
                        }
                        
                    }
                    Thread.Sleep(10);
                } else 
                    Thread.Sleep(10);
            }
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
