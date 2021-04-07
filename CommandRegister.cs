using System;

namespace Network
{

    static class CommandRegister
    {
        public static void ForceInitialize()
        {

        }

        static CommandRegister()
        {
            Commands.RegisterCommand("startserver", new Command
            {
                Execute = (sender, args) => Server.Start()
            });

            Commands.RegisterCommand("connect", new Command
            {
                argNumber = 1,
                argTypes = new Type[] { typeof(string) },
                Execute = (sender, args) => LocalClient.Connect(args[0])
            });

            Commands.RegisterCommand("disconnect", new Command
            {
                Execute = (sender, args) => LocalClient.Disconnect()
            });

            Commands.RegisterCommand("help", new Command
            {
                Execute = (sender, args) => Commands.PrintCommands()
            });

            Commands.RegisterCommand("say", new Command
            {
                argNumber = -1,
                Execute = (sender, args) =>
                {
                    NetworkPacket packet = new NetworkPacket("say");
                    packet.WriteString(string.Join(" ", args));
                    packet.SendToServer();
                }
            });

            Commands.RegisterCommand("name", new Command
            {
                argNumber = 1,
                argTypes = new Type[] { typeof(string) },
                Execute = (sender, args) =>
                {
                    LocalClient.name = args[0];

                    NetworkPacket packet = new NetworkPacket("name");
                    packet.WriteString(args[0]);
                    packet.SendToServer();
                }
            });

            Commands.RegisterCommand("kick", new Command
            {
                argNumber = 1,
                argTypes = new Type[] { typeof(string) },
                Execute = (sender, args) =>
                {
                    NetworkPacket packet = new NetworkPacket("kick");
                    packet.WriteString(args[0]);
                    packet.SendToServer();
                }
            });

            Commands.RegisterCommand("stopserver", new Command
            {
                Execute = (sender, args) =>
                {
                    NetworkPacket packet = new NetworkPacket("stopserver");
                    packet.SendToServer();
                }
            });
        }
    }
}
