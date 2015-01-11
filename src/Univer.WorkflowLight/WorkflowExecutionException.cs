using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Univer.WorkflowLight
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class WorkflowExecutionException : WorkflowException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowExecutionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public WorkflowExecutionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowExecutionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public WorkflowExecutionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
