using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Univer.Common;

namespace Univer.Console
{
    public class ProcessManager
    {
        public List<ConsoleProcessSandbox> Sandboxes
        {
            get;
            private set;
        }

        public ProcessManager()
        {
            this.Sandboxes = new List<ConsoleProcessSandbox>();
        }

        public ConsoleProcessSandbox CreateProcess(string fileName, params string[] args)
        {
            var sandbox = new ConsoleProcessSandbox(fileName);
            this.Sandboxes.Add(sandbox);
            return sandbox;
        }
    }
}
