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
using Univer.Common;
using System.Reflection;

namespace Univer.WorkflowLight.Execution
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class WorkflowState
    {
        #region Nested Types

        /// <summary>
        /// 
        /// </summary>
        internal class WorkflowStateTypeComparer : IEqualityComparer<WorkflowState>
        {
            private static readonly Lazy<WorkflowStateTypeComparer> _instance =
                new Lazy<WorkflowStateTypeComparer>(() => new WorkflowStateTypeComparer(), true);

            internal static readonly WorkflowStateTypeComparer Instance = _instance.Value;

            internal WorkflowStateTypeComparer()
            {
            }

            public bool Equals(WorkflowState x, WorkflowState y)
            {
                if (x == null || y == null)
                    return false;

                return x.Attribute == y.Attribute && x.GetType() == y.GetType();
            }

            public int GetHashCode(WorkflowState obj)
            {
                return obj.Attribute.GetHashCode() ^
                       obj.GetType().GetHashCode();
            }
        }

        #endregion

        #region State Instance Caching

        private static readonly CompositeConcurrentDictionary<Type, WorkflowStateAttribute, WorkflowState> _cache =
            new CompositeConcurrentDictionary<Type, WorkflowStateAttribute, WorkflowState>();

        #endregion

        #region Predefined States

        public static readonly WorkflowStartState Start = OfType<WorkflowStartState>();
        public static readonly WorkflowCompletedState Completed = OfType<WorkflowCompletedState>();
        public static readonly WorkflowCompletedState PendingCompleted = OfType<WorkflowCompletedState>();

        #endregion

        public WorkflowStateAttribute Attribute { get; set; }

        #region Static Methods

        public static WorkflowState OfType(Type workflowStateType, WorkflowStateAttribute workflowStateAttribute = WorkflowStateAttribute.Normal)
        {
            if (!_cache.ContainsItem(workflowStateType, workflowStateAttribute))
            {
                var workflowStateConstructor = workflowStateType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { }, null);
                var instance = (WorkflowState)workflowStateConstructor.Invoke(null);
                instance.Attribute = workflowStateAttribute;
                _cache.TryAddItem(workflowStateType, workflowStateAttribute, instance);
            }

            return _cache[workflowStateType][workflowStateAttribute];
        }

        public static TWorkflowState OfType<TWorkflowState>(WorkflowStateAttribute workflowStateAttribute = WorkflowStateAttribute.Normal)
            where TWorkflowState : WorkflowState
        {
            return (TWorkflowState)OfType(typeof(TWorkflowState), workflowStateAttribute);
        }

        #endregion

        #region Overridden Methods

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return this.GetType() == obj.GetType();
        }

        public override int GetHashCode()
        {
            return this.GetType().GetHashCode();
        }

        #endregion
    }
}
