using System;
using Univer.Common;

namespace Univer.Web
{
    public class TextErrorResponse : TextResponse
    {
        public string Message { get; private set; }

        public Exception Exception { get; private set; }

        public override string Text
        {
            get
            {
                return this.Message + Environment.NewLine.Duplicate(2) + this.Exception.ToString();
            }
        }

        private TextErrorResponse()
        {
        }

        public static TextErrorResponse With(string message, Exception exception = null)
        {
            return new TextErrorResponse
            {
                Message = message,
                Exception = exception
            };
        }
    }
}
