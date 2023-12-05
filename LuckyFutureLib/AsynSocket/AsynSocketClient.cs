using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LuckyFutureLib.AsynSocket
{
    public static class Constants
    {
        public const int BUFFER_MAX = 0x80000;
        public const int BUFFER_UNIT = 0x10000;
    }

    public enum SOCKET_EVENTTYPE
    {
        CONNECT,
        SEND,
        RECEIVE,
    }

    public class SocketEventArgs : EventArgs
    {
        public SocketEventArgs(object data)
        {
            this.Data = data;
        }
        public object Data { get; set; }
    }
    class StateObject
    {
        // Client socket.  
        public Socket workSocket = null;
        // Size of buffer.  
        public const int bufferSize = Constants.BUFFER_UNIT;
        // Offset Size of wired buffer.  
        public int offsetSize = 0;
        // Receive buffer.  
        public byte[] buffer = new byte[Constants.BUFFER_UNIT];

        
    }
    public class AsynSocketClient
    {


        private ManualResetEvent _SendDone = new ManualResetEvent(false);
        private ManualResetEvent _recvDone = new ManualResetEvent(false);
        
        protected byte[] _ReceiveBuffer = new byte[Constants.BUFFER_MAX];
        protected int _ReceiveSize = 0;
        protected List<String> _ResponseList = new List<string>();

        protected Socket _ClientSocket = null;
        private Thread _RecvThread = null;
        private string _IpAddr = "";
        private int _Port = 0;

        //public byte[] ReceiveBuffer { get { return _ReceiveBuffer; } }
        //public int ReceiveSize { get { return _ReceiveSize; } }
        public List<string> ResponseList { get { return _ResponseList; } }
        public AsynSocketClient(string ipAddr, int nPort)
        {
            _IpAddr = ipAddr;
            _Port = nPort;
        }
        public bool ConnectSocket()
        {
            CloseSocket();

            bool bConnected = false;
            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPAddress ipAddress = IPAddress.Parse(_IpAddr);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, _Port);

                // Create a TCP/IP  socket.  
                _ClientSocket = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                //_ClientSocket.ReceiveBufferSize = Constants.BUFFER_UNIT/2;
                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    _ClientSocket.Connect(remoteEP);
                    bConnected = _ClientSocket.Connected;

                    _RecvThread = new Thread(ReceiveData);
                    _RecvThread.IsBackground = true;
                    _RecvThread.Start();
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return bConnected;
        }

        public virtual void CloseSocket()
        {

            // Release the socket.  
            if (_ClientSocket != null && _ClientSocket.Connected)
            {
                _ClientSocket.Shutdown(SocketShutdown.Both);
                _ClientSocket.Close();
                _ClientSocket = null;
            }
            
            if(_RecvThread != null && _RecvThread.IsAlive)
            {
                _RecvThread.Abort();
                _RecvThread = null;
            }

        }

        public bool CheckConnection()
        {
            if (_ClientSocket == null || !_ClientSocket.Connected)
            {
                return false;
            }
            else if(_RecvThread == null || !_RecvThread.IsAlive)
            {
                return false;
            }
            return true;

        }

        public void ReceiveData()
        {
            if (_ClientSocket == null || !_ClientSocket.Connected)
                return;
            while (true)
            {
                Receive(_ClientSocket);
                _recvDone.WaitOne();
            }
        }
        
        protected virtual void SendData(byte[] byteData)
        {
            if (_ClientSocket == null || !_ClientSocket.Connected)
                return ;

            Send(_ClientSocket, byteData);
            _SendDone.WaitOne();
        }

        private void Receive(Socket client)
        {
            try
            {
                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = client;

                // Begin receiving the data from the remote device.  
                client.BeginReceive(state.buffer, 0, StateObject.bufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;
                
                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.  
                    lock (_ReceiveBuffer)
                    {
                        if(_ReceiveSize + bytesRead < Constants.BUFFER_MAX)
                        {
                            Buffer.BlockCopy(state.buffer, 0, _ReceiveBuffer, _ReceiveSize, bytesRead);
                            _ReceiveSize += bytesRead;
                            ExtractPacket();
                        }
                        
                    }
                    // Get the rest of the data.  
                    client.BeginReceive(state.buffer, 0, StateObject.bufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                    
                }
                else
                {
                    // Signal that all bytes have been received.  
                    _recvDone.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void Send(Socket client, byte[] byteData)
        {
            try
            {
                // Begin sending the data to the remote device.  
                client.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), client);
            }
            catch (Exception)
            {
                CloseSocket();
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.  
                _SendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public virtual void ExtractPacket()
        {

        }


    }
}
