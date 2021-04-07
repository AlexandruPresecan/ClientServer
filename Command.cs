using System;

namespace Network
{
    class Command
    { 
        public int argNumber = 0;
        public Type[] argTypes;
        public Action<Client, string[]> Execute;

        public bool ValidateArgs(string[] args)
        {
            if (argNumber == -1)
                return true;

            if (argNumber != args.Length)
            {
                Console.WriteLine(argNumber + " arguments needed");
                return false;
            }

            for (int i = 0; i < argNumber; i++)
                if (Convert.ChangeType(args[i], argTypes[i]) == null)
                {
                    Console.WriteLine(args[i] + " must be of type " + argTypes[i]);
                    return false;
                }

            return true;
        }
    }
}
