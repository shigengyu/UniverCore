using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Univer.Console.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            char keyChar;

            PrintHelp();

            while (true)
            {
                keyChar = System.Console.ReadKey().KeyChar;
                System.Console.WriteLine();
                if (keyChar == 'A' || keyChar == 'a')
                {
                    System.Console.WriteLine("Starting console echo test...");
                    new ConsoleEchoTest().Start();
                    break;
                }
                else if (keyChar == 'B' || keyChar == 'b')
                {
                    System.Console.WriteLine("Starting web console on the following prefixes...");
                    var webConsole = new WebConsoleTest();
                    webConsole.Start();
                    webConsole.PrintServerPrefixes();
                    break;
                }
                else
                {
                    PrintHelp();
                }
            }
        }

        private static void PrintHelp()
        {
            System.Console.WriteLine("Please enter your choice:");
            System.Console.WriteLine("[A] Start console echo test");
            System.Console.WriteLine("[B] Start web console test");
        }
    }
}
