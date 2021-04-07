using System;

namespace Network
{
    static class ClientReceivers
    {
        public static void ForceInitialize()
        {

        }

        static ClientReceivers()
        {
            NetworkReceivers.RegisterClientReceiver("print", (sender, packet) =>
            {
                Console.WriteLine(packet.ReadString());
            });

            NetworkReceivers.RegisterClientReceiver("serverJoinInfo", (sender, packet) =>
            {
                Console.WriteLine("Connected to " + packet.ReadString());
            });
        }
    }
}
