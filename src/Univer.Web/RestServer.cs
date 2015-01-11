using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using Univer.Common;

namespace Univer.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class RestServer : HttpServer
    {
        private RestServiceMethodRegistry _methodRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestServer"/> class.
        /// </summary>
        public RestServer(params string[] uriPrefixes)
            : base(uriPrefixes)
        {
            _methodRegistry = new RestServiceMethodRegistry();
        }

        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void HandleRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            var rawUrl = Uri.UnescapeDataString(request.RawUrl.Substring(1));

            if (!string.IsNullOrEmpty(rawUrl))
            {
                var restMethod = rawUrl;
                var indexOfSlash = rawUrl.IndexOf('?');
                if (indexOfSlash >= 0)
                {
                    restMethod = rawUrl.Substring(0, indexOfSlash);
                }

                var method = _methodRegistry.GetMethod(restMethod);
                if (method != null)
                {
                    var result = InvokeMethod(method, request.QueryString);
                    response.ContentType = result.ContentType;
                    response.ContentLength64 = result.ContentLength;
                    response.ContentEncoding = result.ContentEncoding;
                    result.WriteTo(response.OutputStream);
                }
            }
            else
            {
                PrintMethodList(response);
            }
        }

        private void PrintMethodList(HttpListenerResponse response)
        {
            response.ContentType = "text/html";
            response.ContentEncoding = Encoding.UTF8;

            StringBuilder contentBuilder = new StringBuilder();
            contentBuilder.Append("<h1>Available Methods</h1>");

            contentBuilder.Append(@"
<table border=""1"" cellpadding=""5"" cellspacing=""0"" bordercolorlight=""#000000"" bordercolordark=""#ffffff"">
    <tr>
        <th>Prefix</th>
        <th>Method</th>
    </tr>
");

            foreach (var method in _methodRegistry.Methods)
            {
                contentBuilder.Append(@"
    <tr>
        <td>{0}</td>
        <td>{1}</td>
    </tr>
".FormatWith(method.Key, method.Value.GetMethodSignature()));
            }

            contentBuilder.Append("</table>");

            var content = contentBuilder.ToString();
            response.ContentLength64 = content.Length;
            response.OutputStream.Write(Encoding.UTF8.GetBytes(content), 0, content.Length);
        }

        private HttpResponseBase InvokeMethod(MethodInfo method, NameValueCollection parameters)
        {
            var target = _methodRegistry.GetServiceInstance(method.DeclaringType);

            List<object> parameterValues = new List<object>();
            foreach (var parameter in method.GetNamedParameters())
            {
                object parameterValue = null;
                if (parameters.AllKeys.Contains(parameter.Key))
                {
                    parameterValue = parameters[parameter.Key];
                }
                else
                {
                    var optional = parameter.Value.GetCustomAttribute<ParameterAttribute>().Optional;
                    if (!optional)
                    {
                        throw new RestServiceException("Parameter [{0}] required but not provided.".FormatWith(parameter.Key));
                    }
                }

                if (parameterValue != null)
                {
                    parameterValue = Convert.ChangeType(parameterValue, parameter.Value.ParameterType);
                }

                parameterValues.Add(parameterValue);
            }

            HttpResponseBase response = null;
            try
            {
                response = (HttpResponseBase)method.Invoke(target, parameterValues.ToArray());
            }
            catch (Exception e)
            {
                response = TextErrorResponse.With(e.Message, e);
            }

            return response;
        }

        /// <summary>
        /// Starts the server on the specified port.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        public static RestServer WithPort(int port)
        {
            var uriPrefix = "http://" + Network.LocalIPv4Address + ":" + port + "/";
            var restServer = new RestServer(uriPrefix);
            return restServer;
        }

        /// <summary>
        /// Registers all REST service methods in the specified class.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Register<T>() where T : new()
        {
            _methodRegistry.Register<T>();
        }

        class RestServiceMethodRegistry
        {
            private Dictionary<Type, object> _restServiceInstances;
            private Dictionary<string, MethodInfo> _restServiceMethods;

            public IEnumerable<KeyValuePair<string, MethodInfo>> Methods
            {
                get { return _restServiceMethods; }
            }

            internal RestServiceMethodRegistry()
            {
                _restServiceInstances = new Dictionary<Type, object>();
                _restServiceMethods = new Dictionary<string, MethodInfo>();
            }

            internal void Register<TRestService>() where TRestService : new()
            {
                lock (_restServiceInstances)
                {
                    if (_restServiceInstances.ContainsKey(typeof(TRestService)))
                    {
                        throw new RestServiceException("REST service [{0}] already registered".FormatWith(typeof(TRestService).FullName));
                    }
                }

                _restServiceInstances.Add(typeof(TRestService), new TRestService());

                var methods = typeof(TRestService).GetMethodsWithAttribute<RestServiceMethodAttribute>();
                foreach (var method in methods)
                {
                    if (!method.ReturnType.CanAssignTo<HttpResponseBase>())
                    {
                        throw new RestServiceException("Invalid REST service method definition. Method must have return type [HttpResponseBase].");
                    }

                    var attribute = method.GetCustomAttribute<RestServiceMethodAttribute>();
                    var prefix = attribute.Prefix ?? method.DeclaringType.GetDisplayName().Replace('.', '/') + "/" + method.Name;
                    _restServiceMethods.Add(prefix, method);
                }
            }

            internal Object GetServiceInstance(Type type)
            {
                return _restServiceInstances[type];
            }

            internal MethodInfo GetMethod(string prefix)
            {
                if (_restServiceMethods.ContainsKey(prefix))
                {
                    return _restServiceMethods[prefix];
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
