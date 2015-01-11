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
    public class TextResponse : HttpResponseBase
    {
        public virtual string Text { get; private set; }

        public override long ContentLength
        {
            get { return Text.Length; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextResponse"/> class.
        /// </summary>
        protected TextResponse()
            : base(HttpResponseType.Text)
        {
        }

        /// <summary>
        /// Writes the content to the response output stream.
        /// </summary>
        /// <param name="responseOutputStream">The response output stream.</param>
        public override void WriteTo(Stream responseOutputStream)
        {
            responseOutputStream.Write(Encoding.UTF8.GetBytes(Text), 0, Text.Length);
        }

        /// <summary>
        /// Creates a new text response with the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static TextResponse With(string text)
        {
            return new TextResponse { Text = text };
        }
    }
}
