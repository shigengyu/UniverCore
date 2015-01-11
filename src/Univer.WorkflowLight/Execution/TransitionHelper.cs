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
using System.Linq;
using System.Text;
using Univer.Common;

namespace Univer.WorkflowLight.Execution
{
    /// <summary>
    /// 
    /// </summary>
    public static class TransitionHelper
    {
        /// <summary>
        /// Transits the state of the process.
        /// </summary>
        /// <param name="processInstance">The process instance.</param>
        /// <param name="fromState">From state.</param>
        /// <param name="toStates">To states.</param>
        public static void TransitProcessState(ProcessInstance processInstance, WorkflowState fromState, WorkflowState[] toStates)
        {
            lock (processInstance.SyncRoot)
            {
                if (!processInstance.InnerActiveStates.Contains(fromState))
                    throw new WorkflowTransitionException(
                        "Process instance [{0}] does not have active state [{1}]"
                            .FormatWith(processInstance.ProcessId, fromState.GetType().FullName));

                processInstance.InnerActiveStates.Remove(fromState);

                foreach (var toState in toStates)
                {
                    if (!processInstance.InnerActiveStates.Contains(toState))
                    {
                        processInstance.InnerActiveStates.Add(toState);
                    }
                }
            }
        }

        /// <summary>
        /// Transits the state of the process.
        /// </summary>
        /// <param name="processInstance">The process instance.</param>
        /// <param name="fromStates">From states.</param>
        /// <param name="toStates">To states.</param>
        public static void TransitProcessState(ProcessInstance processInstance, WorkflowState[] fromStates, WorkflowState[] toStates)
        {
            lock (processInstance.SyncRoot)
            {
                var duplicatedStates = fromStates.GetDuplicates(item => item);
                if (duplicatedStates.Any())
                {
                    var duplicatedStateNames = duplicatedStates.Select(item => item.Key.GetType().FullName);
                    throw new WorkflowTransitionException(
@"From states contains duplicated values. Duplicated = [{0}]".FormatWith(duplicatedStateNames.AsString()));
                }

                var nonExistingStates = fromStates.Where(item => !processInstance.InnerActiveStates.Contains(item)).ToList();
                if (nonExistingStates.Count > 0)
                {
                    throw new WorkflowTransitionException(
                        "Process instance [{0}] does not have active states [{1}]"
                            .FormatWith(processInstance.ProcessId, nonExistingStates.Select(item => item.GetType().FullName).AsString()));
                }

                foreach (var fromState in fromStates)
                {
                    processInstance.InnerActiveStates.Remove(fromState);
                }

                foreach (var toState in toStates)
                {
                    processInstance.InnerActiveStates.Add(toState);
                }
            }
        }
    }
}
