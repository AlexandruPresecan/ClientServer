using System;
using System.Collections.Generic;

namespace Network
{
    static class NetworkReceivers
    {
        static Dictionary<string, Action<Client, NetworkPacket>> clientReceivers = new Dictionary<string, Action<Client, NetworkPacket>>();
        static Dictionary<string, Action<Client, NetworkPacket>> serverReceivers = new Dictionary<string, Action<Client, NetworkPacket>>();

        public static void ClientReceive(Client client, NetworkPacket packet)
        {
            if (clientReceivers.ContainsKey(packet.name))
                clientReceivers[packet.name](client, packet);
            else
                Console.WriteLine("No receiver found for packet " + packet.name);
        }

        public static void ServerReceive(Client client, NetworkPacket packet)
        {
            if (serverReceivers.ContainsKey(packet.name))
                serverReceivers[packet.name](client, packet);
            else
                Console.WriteLine("No receiver found for packet " + packet.name);
        }

        public static void RegisterClientReceiver(string name, Action<Client, NetworkPacket> function)
        {
            if (clientReceivers.ContainsKey(name))
                Console.WriteLine("Receiver " + name + " already exists");
            else
                clientReceivers[name] = function;
        }

        public static void RegisterServerReceiver(string name, Action<Client, NetworkPacket> function)
        {
            if (serverReceivers.ContainsKey(name))
                Console.WriteLine("Receiver " + name + " already exists");
            else
                serverReceivers[name] = function;
        }
    }
}
