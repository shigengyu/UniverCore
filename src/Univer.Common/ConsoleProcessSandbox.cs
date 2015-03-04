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

namespace Univer.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class ConsoleProcessSandbox : IDisposable
    {
        #region Public Properties

        /// <summary>
        /// Gets the process start information.
        /// </summary>
        /// <value>
        /// The process start information.
        /// </value>
        public ProcessStartInfo ProcessStartInfo { get; private set; }

        /// <summary>
        /// Gets the process.
        /// </summary>
        /// <value>
        /// The process.
        /// </value>
        public Process Process { get; private set; }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when [output data received].
        /// </summary>
        public event DataReceivedEventHandler OutputDataReceived
        {
            add { Process.OutputDataReceived += value; }
            remove { Process.OutputDataReceived -= value; }
        }

        /// <summary>
        /// Occurs when [error data received].
        /// </summary>
        public event DataReceivedEventHandler ErrorDataReceived
        {
            add { Process.ErrorDataReceived += value; }
            remove { Process.ErrorDataReceived -= value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleProcessSandbox"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="args">The arguments.</param>
        public ConsoleProcessSandbox(string fileName, params string[] args)
        {
            ProcessStartInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                FileName = fileName,
                Arguments = string.Join(" ", args),
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the process.
        /// </summary>
        /// <returns></returns>
        public Process StartProcess()
        {
            Process = new Process
            {
                StartInfo = ProcessStartInfo,
                EnableRaisingEvents = true
            };
            Process.Start();
            Process.BeginOutputReadLine();
            Process.BeginErrorReadLine();
            return Process;
        }

        /// <summary>
        /// Kills the process.
        /// </summary>
        public void KillProcess()
        {
            Process.Kill();
        }

        /// <summary>
        /// Sends the input.
        /// </summary>
        /// <param name="input">The input.</param>
        public void SendInput(string input)
        {
            Process.StandardInput.WriteLine(input);
        }

        #endregion

        #region Disposing

        private bool _isDisposed;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ConsoleProcessSandbox"/> class.
        /// </summary>
        ~ConsoleProcessSandbox()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            if (!Process.HasExited)
                Process.Kill();

            _isDisposed = true;
        }

        #endregion
    }
}