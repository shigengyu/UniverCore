using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Univer.WorkflowLight.Definition
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class WorkflowDefinitionException : WorkflowException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowDefinitionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public WorkflowDefinitionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowDefinitionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public WorkflowDefinitionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
