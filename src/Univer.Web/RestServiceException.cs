using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Univer.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class RestServiceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RestServiceException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public RestServiceException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestServiceException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public RestServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
