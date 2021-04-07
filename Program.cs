using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Network
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandRegister.ForceInitialize();
            ClientReceivers.ForceInitialize();
            ServerReceivers.ForceInitialize();

            while (true)
            {
                string[] input = Console.ReadLine().Split(" ");
                string cmd = input[0];
                string[] arguments = new string[input.Length - 1];

                Array.Copy(input, 1, arguments, 0, input.Length - 1);

                Commands.ExecuteCommand(null, cmd, arguments);
            }
        }
    }
}
