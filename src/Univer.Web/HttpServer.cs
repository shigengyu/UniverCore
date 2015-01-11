using System;
using System.Net;
using System.Threading;
using Univer.Common;

namespace Univer.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpServer
    {
        private HttpListener _listener;
        private Action<HttpListenerRequest, HttpListenerResponse> _requestHandler;
        private Thread _thread;

        /// <summary>
        /// Gets the URI prefixes.
        /// </summary>
        public HttpListenerPrefixCollection UriPrefixes
        {
            get { return _listener.Prefixes; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServer"/> class.
        /// </summary>
        /// <param name="uriPrefixes">The URI prefixes.</param>
        public HttpServer(params string[] uriPrefixes)
            : this(null, uriPrefixes)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServer"/> class.
        /// </summary>
        /// <param name="requestHandler">The request handler.</param>
        /// <param name="uriPrefixes">The URI prefixes.</param>
        public HttpServer(Action<HttpListenerRequest, HttpListenerResponse> requestHandler, params string[] uriPrefixes)
        {
            _listener = new HttpListener();

            if (requestHandler == null)
            {
                _requestHandler = new Action<HttpListenerRequest, HttpListenerResponse>(HandleRequest);
            }
            else
            {
                _requestHandler = requestHandler;
            }

            foreach (var item in uriPrefixes)
            {
                this.UriPrefixes.Add(item);
            }
        }

        /// <summary>
        /// Start an HTTP server on the specified port with a request handler method.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="requestHandler">The request handler.</param>
        /// <returns></returns>
        public static HttpServer StartOnPort(int port, Action<HttpListenerRequest, HttpListenerResponse> requestHandler)
        {
            var uriPrefix = "http://" + Network.LocalIPv4Address + ":" + port + "/";
            var httpServer = new HttpServer(requestHandler, uriPrefix);
            httpServer.Start();
            return httpServer;
        }

        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected virtual void HandleRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            throw new InvalidOperationException("Must implemented this method in the derived classes.");
        }

        /// <summary>
        /// Starts the server.
        /// </summary>
        public void Start()
        {
            _listener.Start();

            _thread = new Thread(() =>
            {
                while (_listener.IsListening)
                {
                    ThreadPool.QueueUserWorkItem(ctx =>
                    {
                        HttpListenerContext context = (HttpListenerContext)ctx;
                        var request = context.Request;
                        var response = context.Response;

                        try
                        {
                            _requestHandler(request, response);
                        }
                        finally
                        {
                            context.Response.OutputStream.Close();
                        }
                    }, _listener.GetContext());
                }
            });

            _thread.Start();
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }
    }
}
