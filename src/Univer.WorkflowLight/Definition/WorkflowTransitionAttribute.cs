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
using System.Text;

namespace Univer.WorkflowLight.Definition
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class WorkflowTransitionAttribute : Attribute
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets from state.
        /// </summary>
        public Type FromState { get; set; }

        /// <summary>
        /// Gets to state.
        /// </summary>
        public Type ToState { get; set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public WorkflowTransitionType Type { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowTransitionAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public WorkflowTransitionAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowTransitionAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fromState">From state.</param>
        public WorkflowTransitionAttribute(string name, Type fromState, Type toState)
            : this(name)
        {
            this.FromState = fromState;
            this.ToState = toState;
        }
    }
}
