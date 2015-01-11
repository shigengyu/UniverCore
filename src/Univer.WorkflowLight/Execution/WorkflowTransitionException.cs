using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Univer.WorkflowLight.Execution
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class WorkflowTransitionException : WorkflowException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowTransitionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public WorkflowTransitionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowTransitionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public WorkflowTransitionException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
