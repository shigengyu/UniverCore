using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Univer.Web
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class HttpResponseBase
    {
        /// <summary>
        /// Gets the type of the response.
        /// </summary>
        /// <value>
        /// The type of the response.
        /// </value>
        public HttpResponseType ResponseType { get; private set; }

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        public string ContentType
        {
            get
            {
                switch (ResponseType)
                {
                    case HttpResponseType.Text:
                        return "text/plain";
                    case HttpResponseType.Html:
                        return "text/html";
                    case HttpResponseType.Xml:
                        return "text/xml";
                    case HttpResponseType.Json:
                        return "application/json";
                    default:
                        throw new RestServiceException("Unknown response type");
                }
            }
        }

        /// <summary>
        /// Gets the length of the content.
        /// </summary>
        /// <value>
        /// The length of the content.
        /// </value>
        public abstract long ContentLength
        {
            get;
        }

        /// <summary>
        /// Gets or sets the content encoding.
        /// </summary>
        /// <value>
        /// The content encoding.
        /// </value>
        public Encoding ContentEncoding
        {
            get;
            protected set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResponseBase"/> class.
        /// </summary>
        /// <param name="responseType">Type of the response.</param>
        protected HttpResponseBase(HttpResponseType responseType)
        {
            this.ResponseType = responseType;
            this.ContentEncoding = Encoding.UTF8;
        }

        /// <summary>
        /// Writes the content to the response output stream.
        /// </summary>
        /// <param name="responseOutputStream">The response output stream.</param>
        public abstract void WriteTo(Stream responseOutputStream);
    }
}
