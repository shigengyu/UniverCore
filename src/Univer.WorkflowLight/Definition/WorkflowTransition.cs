/*
 * Copyright (c) 2013-2015 Univer Shi
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Univer.WorkflowLight.Definition;
using Univer.Common;
using Univer.WorkflowLight.Execution;

namespace Univer.WorkflowLight.Definition
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowTransition
    {
        private WorkflowTransitionType _type = WorkflowTransitionType.Unknown;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets from state.
        /// </summary>
        /// <value>
        /// From state.
        /// </value>
        public Type FromState { get; set; }

        /// <summary>
        /// Gets or sets to state.
        /// </summary>
        /// <value>
        /// To state.
        /// </value>
        public Type ToState { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public WorkflowTransitionType Type
        {
            get { return _type; }
            set
            {
                _type = value;

                if (_type == WorkflowTransitionType.FromStart)
                {
                    this.FromState = typeof(WorkflowStartState);
                }
                else if (_type == WorkflowTransitionType.ToCompleted)
                {
                    this.ToState = typeof(WorkflowCompletedState);
                }
            }
        }

        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        public MethodInfo Method { get; set; }

        /// <summary>
        /// Invokes the specified process instance.
        /// </summary>
        /// <param name="processInstance">The process instance.</param>
        public void Invoke(ProcessInstance processInstance)
        {
            try
            {
                if (this.Method.IsStatic)
                {
                    this.Method.Invoke(null, new object[] { processInstance });
                }
                else
                {
                    this.Method.Invoke(processInstance.WorkflowDefinition, new object[] { processInstance });
                }
            }
            catch (Exception e)
            {
                throw new WorkflowExecutionException(@"Exception occured when executing workflow transition.
Transition = [{0}]. Workflow = [{1}]".FormatWith(this.Name, processInstance.WorkflowDefinition.GetType().FullName), e);
            }
        }

        public override string ToString()
        {
            return "Name = [{0}], Type = [{1}]".FormatWith(this.Name, this.GetType().FullName);
        }
    }
}
