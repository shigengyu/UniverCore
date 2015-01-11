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
using System.Linq;
using Univer.Common;
using Univer.WorkflowLight.Definition;
using Univer.WorkflowLight.Execution;

namespace Univer.WorkflowLight.Services
{
    public class ExecutionService : WorkflowServiceBase, IExecutionService
    {
        public void Execute(ProcessInstance processInstance, string transitionName)
        {
            if (transitionName == null)
            {
                throw new ArgumentOutOfRangeException("transitionName");
            }

            DetectStateMerging(processInstance, transitionName);

            var transitionsByFromState = (from transition in processInstance.WorkflowDefinition.GetWorkflowTransitions(transitionName)
                                          group transition by transition.FromState into Transitions
                                          select new { Transitions.Key, Transitions }).ToList();

            foreach (var transitionByFromState in transitionsByFromState)
            {
                var fromStateType = transitionByFromState.Key;
                var transitions = transitionByFromState.Transitions.ToList();

                // Invoke transitions
                transitions.ForEach(trans => trans.Invoke(processInstance));

                var fromState = WorkflowState.OfType(fromStateType);
                var toStates = transitions.Select(trans => WorkflowState.OfType(trans.ToState)).Distinct().ToArray();

                TransitionHelper.TransitProcessState(processInstance, fromState, toStates);

                var postTransitProcessors = processInstance.WorkflowDefinition.GetPostTransitProcessors();

                foreach (var postTransitProcessor in postTransitProcessors)
                {
                    postTransitProcessor.Invoke(processInstance);
                }
            }
        }

        private static void DetectStateMerging(ProcessInstance processInstance, string transitionName)
        {
            var transitionsByToState = (from transition in processInstance.WorkflowDefinition.GetWorkflowTransitions(transitionName)
                                        group transition by transition.ToState into Transitions
                                        select new { Transitions.Key, Transitions }).ToList();

            foreach (var transition in transitionsByToState)
            {
                var toState = transition.Key;
                var transitions = transition.Transitions;

                if (transitions.Count() > 1)
                {
                    throw new InvalidTransitionDefinitionException(
@"Merging state transitions not supported.
Definition = [{0}]
Transition = [{1}]
ToStates = [{2}]
FromStates = [{3}]".FormatWith(
                        processInstance.WorkflowDefinition.GetType().FullName,
                        transitionName,
                        transition.Key.GetType().FullName,
                        transitions.Select(item => item.FromState.GetType().FullName).AsString()));
                }
            }
        }
    }
}
