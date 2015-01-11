using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Univer.Console.Web;
using System.Diagnostics;

namespace Univer.Console.Test
{
    public class WebConsoleTest
    {
        private WebConsoleServer _webConsoleServer;

        public WebConsoleTest()
        {
            _webConsoleServer = new WebConsoleServer();
        }

        public void Start()
        {
            _webConsoleServer.Start(9876);
        }

        public void PrintServerPrefixes()
        {
            foreach (var item in _webConsoleServer.RestServer.UriPrefixes)
            {
                System.Console.WriteLine(item);
                Process.Start(item);
            }
        }
    }
}
