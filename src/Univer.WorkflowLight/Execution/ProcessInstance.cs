/*
 * Copyright (c) 2013 Univer Shi
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Univer.Common;
using Univer.WorkflowLight.Definition;

namespace Univer.WorkflowLight.Execution
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcessInstance
    {
        internal readonly object SyncRoot = new object();

        internal IList<WorkflowState> InnerActiveStates { get; private set; }

        /// <summary>
        /// Gets the process id.
        /// </summary>
        public Guid ProcessId { get; private set; }

        /// <summary>
        /// Gets the workflow definition.
        /// </summary>
        public WorkflowDefinition WorkflowDefinition { get; private set; }

        /// <summary>
        /// Gets the context.
        /// </summary>
        public WorkflowContext Context { get; private set; }

        /// <summary>
        /// Gets the current states.
        /// </summary>
        public ReadOnlyCollection<WorkflowState> ActiveStates
        {
            get { return new ReadOnlyCollection<WorkflowState>(InnerActiveStates); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessInstance"/> class.
        /// </summary>
        /// <param name="workflowDefinitionType">Type of the workflow definition.</param>
        public ProcessInstance(Type workflowDefinitionType)
            : this(Guid.NewGuid(), workflowDefinitionType)
        {
            InnerActiveStates.Add(WorkflowState.Start);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessInstance"/> class.
        /// </summary>
        /// <param name="processId">The process id.</param>
        /// <param name="workflowDefinitionType">Type of the workflow definition.</param>
        public ProcessInstance(Guid processId, Type workflowDefinitionType)
        {
            this.InnerActiveStates = new List<WorkflowState>();
            this.ProcessId = processId;
            this.WorkflowDefinition = (WorkflowDefinition)Activator.CreateInstance(workflowDefinitionType);
        }
    }
}
