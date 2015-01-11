using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace Univer.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class XmlResponse : HttpResponseBase
    {
        public static readonly XDeclaration DefaultDeclaration = new XDeclaration("1.0", "utf-8", "yes");

        protected XDocument Document { get; private set; }

        protected string ContentString
        {
            get
            {
                var sb = new StringBuilder();
                if (Document.Declaration != null)
                    sb.AppendLine(Document.Declaration.ToString());

                sb.Append(Document.ToString());
                return sb.ToString();
            }
        }

        public override long ContentLength
        {
            get { return ContentString.Length; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlResponse"/> class.
        /// </summary>
        protected XmlResponse()
            : base(HttpResponseType.Xml)
        {
        }

        /// <summary>
        /// Writes the content to the response output stream.
        /// </summary>
        /// <param name="responseOutputStream">The response output stream.</param>
        public override void WriteTo(Stream responseOutputStream)
        {
            responseOutputStream.Write(Encoding.UTF8.GetBytes(ContentString), 0, ContentString.Length);
        }

        public static XmlResponse Create(XDeclaration declaration = null)
        {
            var response = new XmlResponse();
            response.Document = declaration != null ? new XDocument(declaration) : new XDocument();
            return response;
        }

        public XmlResponse With(XElement xElement)
        {
            this.Document.Add(xElement);
            return this;
        }
    }
}
