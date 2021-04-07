using System;
using System.Collections.Generic;


namespace Network
{
    static class Commands
    {
        static Dictionary<string, Command> commands = new Dictionary<string, Command>();

        public static void ExecuteCommand(Client sender, string cmd, string[] args)
        {
            if (commands.ContainsKey(cmd) && commands[cmd].ValidateArgs(args))
                commands[cmd].Execute(sender, args);
            else
                Console.WriteLine("Invalid command " + cmd);
        }

        public static void RegisterCommand(string name, Command cmd)
        {
            if (commands.ContainsKey(name))
                Console.WriteLine("Command name already taken");
            else
                commands[name] = cmd;
        }

        public static void PrintCommands()
        {
            foreach (string cmd in commands.Keys)
                Console.WriteLine(cmd);
        }
    }
}
