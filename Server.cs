using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace Network
{
    static class Server
    {
        public static int port = 6969;
        public static IPAddress ip = IPAddress.Any;
        public static int maxPlayers = 32;

        public static TcpListener tcpListener = new TcpListener(ip, port);
        public static List<Client> clients = new List<Client>();

        public static void Start(int playerLimit = 32)
        {
            if (IsRunning())
                Stop();

            maxPlayers = playerLimit;

            tcpListener.Start();
            tcpListener.Beg

            Console.WriteLine("Server started on ip = " + ip + " port = " + port);
        }

        private static void TcpConnectCallback(IAsyncResult result)
        {
            Client client = new Client(tcpListener.EndAcceptTcpClient(result));
            AddClient(client);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpConnectCallback), null); 
        }

        private static void AddClient(Client client)
        {
            if (clients.Count >= maxPlayers)
            {
                Server.NotifyAll("Server full!");
                return;
            }

            clients.Add(client);

            if (clients.Count == 1)
                client.admin = true;

            Server.NotifyAll(client.name + " joined the server!");
        }

        public static void Notify(Client client, string message)
        {
            NetworkPacket packet = new NetworkPacket("print");
            packet.WriteString(message);
            packet.SendToClient(client);
        }

        public static void NotifyAll(string message)
        {
            NetworkPacket packet = new NetworkPacket("print");
            packet.WriteString(message);
            packet.SendToClients();
        }

        public static void Stop()
        {
            NotifyAll("Server closing");

            foreach (Client client in clients)
                if (client.IsConnected())
                    client.Disconnect();

            tcpListener.Server.Close();
            tcpListener.Stop();
        }

        public static Client GetClient(string name)
        {
            foreach (Client client in clients)
                if (client.name == name && client.IsConnected())
                    return client;

            return null;
        }

        public static bool IsRunning()
        {
            return tcpListener.Server.Connected;
        }
    }
}
