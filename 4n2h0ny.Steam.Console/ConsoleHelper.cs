using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4n2h0ny.Steam
{
    internal class ConsoleHelper
    {
        public static void ConsoleWriteError(string outputString)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(outputString);
            Console.ResetColor();
        }

        public static void ConsoleWriteSuccess(string outputString)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(outputString);
            Console.ResetColor();
        }
    }
}
