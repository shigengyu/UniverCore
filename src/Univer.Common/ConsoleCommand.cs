// The MIT License (MIT)
// 
// Copyright (c) 2012-2013 Univer Shi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace Univer.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConsoleCommand
    {
        /// <summary>
        /// Run a windows console application.
        /// </summary>
        /// <param name="fileName">Name of the executable.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static int Run(string fileName, params string[] arguments)
        {
            var processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = fileName;
            processStartInfo.Arguments = string.Join(" ", arguments);
            processStartInfo.CreateNoWindow = true;
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            var process = Process.Start(processStartInfo);
            process.WaitForExit();

            return process.ExitCode;
        }

        /// <summary>
        /// Starts the batch.
        /// </summary>
        /// <param name="outputReceivedHandler">The output received handler.</param>
        /// <param name="errorReceivedHandler">The error received handler.</param>
        /// <returns></returns>
        public static ConsoleCommandBatch StartBatch(DataReceivedEventHandler outputReceivedHandler = null,
            DataReceivedEventHandler errorReceivedHandler = null)
        {
            return new ConsoleCommandBatch(outputReceivedHandler, errorReceivedHandler);
        }

        /// <summary>
        /// 
        /// </summary>
        public class ConsoleCommandBatch : IDisposable
        {
            /// <summary>
            /// The _commands
            /// </summary>
            private readonly IList<Tuple<string, string[]>> _commands = new List<Tuple<string, string[]>>();

            private readonly ConsoleProcessSandbox _sandbox = new ConsoleProcessSandbox("cmd.exe");

            /// <summary>
            /// Initializes a new instance of the <see cref="ConsoleCommandBatch"/> class.
            /// </summary>
            /// <param name="outputReceivedHandler">The output received handler.</param>
            /// <param name="errorReceivedHandler">The error received handler.</param>
            public ConsoleCommandBatch(DataReceivedEventHandler outputReceivedHandler,
                DataReceivedEventHandler errorReceivedHandler)
            {
                _sandbox.StartProcess();

                if (outputReceivedHandler != null)
                    _sandbox.OutputDataReceived += outputReceivedHandler;

                if (errorReceivedHandler != null)
                    _sandbox.ErrorDataReceived += errorReceivedHandler;
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                Run();

                _sandbox.SendInput("exit");
                _sandbox.Process.WaitForExit();
                _sandbox.Dispose();
            }

            /// <summary>
            /// Adds the specified command.
            /// </summary>
            /// <param name="command">The command.</param>
            /// <param name="arguments">The arguments.</param>
            /// <returns></returns>
            public ConsoleCommandBatch Add(string command, params string[] arguments)
            {
                _commands.Add(new Tuple<string, string[]>(command, arguments));
                return this;
            }

            /// <summary>
            /// Runs this instance.
            /// </summary>
            public void Run()
            {
                if (_commands.Count == 0)
                    return;

                foreach (var command in _commands)
                {
                    var input = command.Item1 + " " + string.Join(" ", command.Item2);
                    _sandbox.SendInput(input);
                }

                _commands.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [TestFixture]
        public class ConsoleCommandTest
        {
            /// <summary>
            /// Tests the batch command.
            /// </summary>
            [Test]
            public void TestBatchCommand()
            {
                using (var batch = StartBatch((sender, e) => Console.WriteLine(e.Data)))
                {
                    batch.Add("echo %MY_NAME%");
                    batch.Add("set MY_NAME=Univer");
                    batch.Add("echo %MY_NAME%");
                }
            }
        }
    }
}