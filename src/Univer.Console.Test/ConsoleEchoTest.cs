using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Univer.Console;

namespace Univer.Console.Test
{
    class ConsoleEchoTest
    {
        private ProcessManager ProcessManager
        {
            get;
            set;
        }

        public ConsoleEchoTest()
        {
            this.ProcessManager = new ProcessManager();
        }

        public void Start()
        {
            var sandbox = this.ProcessManager.CreateProcess("ConsoleEcho.exe");
            sandbox.StartProcess();
            sandbox.OutputDataReceived += OnConsoleStandardOutput;
            sandbox.ErrorDataReceived += OnConsoleStandardError;

            while (true)
            {
                var line = System.Console.ReadLine();

                if (!string.IsNullOrEmpty(line))
                {
                    if (line == "exit")
                        break;

                    sandbox.SendInput(line);
                }
            }

            // The external process will automatically be killed when the sandbox is destructed or disposed.
        }

        private void OnConsoleStandardOutput(object sender, DataReceivedEventArgs e)
        {
            System.Console.WriteLine(string.Format("Output: {0}", e.Data));
        }

        private void OnConsoleStandardError(object sender, DataReceivedEventArgs e)
        {
            System.Console.WriteLine(string.Format("Error: {0}", e.Data));
        }
    }
}
