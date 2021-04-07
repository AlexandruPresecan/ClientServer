using System;
using System.Net.Sockets;
using System.Text;

namespace Network
{
    static class LocalClient
    { 
        public static string name = "Player";
  
        public static TcpClient socket;
        public static NetworkStream networkStream;

        static int dataBuffferSize = 4096;
        static byte[] dataBuffer = new byte[dataBuffferSize];

        public static void Connect(string ip)
        {
            if (IsConnected())
                Disconnect();

            socket = new TcpClient
            {
                ReceiveBufferSize = dataBuffferSize,
                SendBufferSize = dataBuffferSize
            };

            socket.BeginConnect(ip, 6969, ConnectCallback, null);
        }

        public static void Disconnect()
        {
            if (Server.IsRunning())
                Server.Stop();

            networkStream.Close();
            socket.Close();

            Console.WriteLine("disconnected from server");
        }

        public static bool IsConnected()
        {
            if (socket == null)
                return false;

            return socket.Connected;
        }

        private static void ConnectCallback(IAsyncResult result)
        {
            socket.EndConnect(result);

            if (!IsConnected())
            {
                Console.WriteLine("Could not connect to server");
                return;
            }

            networkStream = socket.GetStream();

            NetworkPacket packet = new NetworkPacket("clientJoinInfo");
            packet.WriteString(name);
            packet.SendToServer();

            networkStream.BeginRead(dataBuffer, 0, dataBuffferSize, ReceiveCallback, null);
        }

        private static void ReceiveCallback(IAsyncResult result)
        {   
            networkStream.EndRead(result);

            if (!IsConnected())
                Disconnect();

            try
            {
                NetworkReceivers.ClientReceive(null, new NetworkPacket(dataBuffer));
                networkStream.BeginRead(dataBuffer, 0, dataBuffferSize, ReceiveCallback, null);
            }
            catch
            {

            }
        }
    }
}
