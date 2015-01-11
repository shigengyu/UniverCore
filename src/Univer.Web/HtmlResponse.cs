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
    public class HtmlResponse : HttpResponseBase
    {
        public override long ContentLength
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlResponse"/> class.
        /// </summary>
        public HtmlResponse()
            : base(HttpResponseType.Html)
        {
        }

        /// <summary>
        /// Writes the content to the response output stream.
        /// </summary>
        /// <param name="responseOutputStream">The response output stream.</param>
        public override void WriteTo(Stream responseOutputStream)
        {
            throw new NotImplementedException();
        }
    }
}
