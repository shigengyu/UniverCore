using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Univer.Common;
using Univer.Web;
using System.Xml.Linq;

namespace Univer.Console.Web
{
    public class WebConsoleServer
    {
        private RestServer _restServer;

        public RestServer RestServer
        {
            get { return _restServer; }
        }

        public void Start(int port)
        {
            _restServer = RestServer.WithPort(port);
            _restServer.Register<WebConsoleRestService>();
            _restServer.Start();
        }
    }

    class WebConsoleRestService
    {
        private ConsoleProcessSandbox _sandBox;
        private List<ConsoleOutput> _output = new List<ConsoleOutput>();

        [RestServiceMethod]
        public TextResponse Start([Parameter("FileName", false)] string fileName, [Parameter("Arguments")] string arguments)
        {
            fileName = Uri.UnescapeDataString(fileName);
            arguments = arguments == null ? null : Uri.UnescapeDataString(arguments);
            var argumentArray = new string[] { };
            if (arguments != null)
            {
                argumentArray = arguments.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }
            _sandBox = new ConsoleProcessSandbox(fileName, argumentArray);
            _sandBox.StartProcess();

            _sandBox.OutputDataReceived += (sender, e) => AddOutput(e.Data, false);
            _sandBox.ErrorDataReceived += (sender, e) => AddOutput(e.Data, true);

            return TextResponse.With("Command started successfully...");
        }

        private void AddOutput(string message, bool isError)
        {
            lock (_output)
            {
                _output.Add(ConsoleOutput.With(message, isError));
            }
        }

        [RestServiceMethod]
        public TextResponse SendInput([Parameter("Input", false)] string input)
        {
            _sandBox.SendInput(input);
            return TextResponse.With("Success");
        }

        [RestServiceMethod]
        public JsonResponse GetOutput()
        {
            lock (this)
            {
                var response = JsonResponse.With(_output);
                _output.Clear();
                return response;
            }
        }

        public class ConsoleOutput
        {
            public DateTime Timestamp { get; set; }
            public string Message { get; set; }
            public bool IsError { get; set; }

            private ConsoleOutput()
            {
                this.Timestamp = DateTime.Now;
            }

            internal static ConsoleOutput With(string message, bool isError = false)
            {
                return new ConsoleOutput
                {
                    Message = message,
                    IsError = isError
                };
            }
        }
    }

    class TestRestService
    {
        [RestServiceMethod]
        public TextResponse HandleCommand([Parameter("Command")] string command)
        {
            return TextResponse.With("Hello World");
        }

        [RestServiceMethod]
        public JsonResponse Test([Parameter("Test")] int test)
        {
            return JsonResponse.With(new DummyJsonObject { Name = "Univer" });
        }

        [RestServiceMethod]
        public XmlResponse XmlTest([Parameter("Dummy")] int dummy)
        {
            return XmlResponse.Create(XmlResponse.DefaultDeclaration).With(
                new XElement("Root",
                    new XElement("Child",
                        new XAttribute("Name", "Univer"))));
        }
    }

    public class DummyJsonObject
    {
        public string Name { get; set; }
    }
}
