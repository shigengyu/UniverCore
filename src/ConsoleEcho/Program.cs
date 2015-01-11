using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleEcho
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var line = Console.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    Console.WriteLine(line);
                }
            }
        }
    }
}
