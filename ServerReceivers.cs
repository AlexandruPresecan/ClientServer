namespace Network
{
    static class ServerReceivers
    {
        public static void ForceInitialize()
        {

        }

        static ServerReceivers()
        {
            NetworkReceivers.RegisterServerReceiver("say", (sender, packet) =>
            {
                Server.NotifyAll(sender.name + ": " + packet.ReadString());
            });

            NetworkReceivers.RegisterServerReceiver("kick", (sender, packet) =>
            {
                if (!sender.admin)
                {
                    Server.Notify(sender, "Command denied");
                    return;
                }

                Client target = Server.GetClient(packet.ReadString());

                if (target == null)
                { 
                    Server.Notify(sender, "User not found");
                    return;
                }

                target.Disconnect();
                Server.NotifyAll(target.name + " has been kicked from the server");
            });

            NetworkReceivers.RegisterServerReceiver("stopserver", (sender, packet) =>
            {
                if (!sender.admin)
                {
                    Server.Notify(sender, "Command denied");
                    return;
                }

                Server.Stop();
            });

            NetworkReceivers.RegisterServerReceiver("clientJoinInfo", (sender, packet) =>
            {
                sender.name = packet.ReadString();
                Server.NotifyAll(sender.name + " with ip " + sender.GetIp() + " joined the server");
                Server.NotifyAll("admin = " + sender.admin);
            });

            NetworkReceivers.RegisterServerReceiver("name", (sender, packet) =>
            {
                string name = sender.name;
                sender.name = packet.ReadString();
                Server.NotifyAll(name + " changed name to " + sender.name);
            });
        }
    }
}
