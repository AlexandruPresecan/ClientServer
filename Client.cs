using System;
using System.Net;
using System.Net.Sockets;

namespace Network
{
    class Client
    {  
        public string name;
        public bool admin = false;

        public TcpClient socket;
        public NetworkStream networkStream;

        int dataBuffferSize = 4096;
        byte[] dataBuffer = new byte[4096];

        public Client(TcpClient socket)
        {
            this.socket = socket;

            socket.ReceiveBufferSize = dataBuffferSize;
            socket.SendBufferSize = dataBuffferSize;

            networkStream = socket.GetStream();
            networkStream.BeginRead(dataBuffer, 0, dataBuffferSize, ReceiveCallback, null);
        }

        public void Disconnect()
        {
            networkStream.Close();
            socket.Close();

            Server.clients.Remove(this);
            Server.NotifyAll(name + " left the server");
        }

        public bool IsConnected()
        {
            return socket.Connected;
        }

        private void ReceiveCallback(IAsyncResult result)
        {  
            networkStream.EndRead(result);

            if (!IsConnected())
                Disconnect();

            try
            {
                NetworkReceivers.ServerReceive(this, new NetworkPacket(dataBuffer));
                networkStream.BeginRead(dataBuffer, 0, dataBuffferSize, ReceiveCallback, null);
            }
            catch
            {

            }
        }

        public string GetIp()
        {
            if (socket == null)
                return null;

            return ((IPEndPoint)socket.Client.RemoteEndPoint).Address.ToString();
        }   
    }
}
