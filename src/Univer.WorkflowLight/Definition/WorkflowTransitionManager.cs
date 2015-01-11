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
using System.Reflection;
using Univer.WorkflowLight.Execution;

namespace Univer.WorkflowLight.Definition
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowTransitionManager
    {
        #region Singleton

        /// <summary>
        /// 
        /// </summary>
        private static readonly Lazy<WorkflowTransitionManager> _instance =
            new Lazy<WorkflowTransitionManager>(() => new WorkflowTransitionManager(), true);

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static WorkflowTransitionManager Instance
        {
            get { return _instance.Value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        private static readonly IDictionary<WorkflowDefinition, ListDictionary<string, WorkflowTransition>> _transitionCache =
            new Dictionary<WorkflowDefinition, ListDictionary<string, WorkflowTransition>>();

        /// <summary>
        /// 
        /// </summary>
        private static readonly IDictionary<WorkflowDefinition, List<PostTransitProcessor>> _postTransitProcessorCache =
            new Dictionary<WorkflowDefinition, List<PostTransitProcessor>>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the workflow transitions.
        /// </summary>
        /// <param name="workflowDefinition">The workflow definition.</param>
        /// <param name="transitionName">Name of the transition.</param>
        /// <returns></returns>
        internal IEnumerable<WorkflowTransition> GetWorkflowTransitions(WorkflowDefinition workflowDefinition, string transitionName)
        {
            if (!_transitionCache.ContainsKey(workflowDefinition))
            {
                _transitionCache.Add(workflowDefinition, new ListDictionary<string, WorkflowTransition>());
            }

            if (_transitionCache[workflowDefinition].Count == 0)
            {
                foreach (var transition in this.GetDefinedWorkflowTransitions(workflowDefinition))
                {
                    _transitionCache[workflowDefinition].AddListItem(transition.Name, transition);
                }
            }

            return _transitionCache[workflowDefinition][transitionName];
        }

        /// <summary>
        /// Gets the post transit processors.
        /// </summary>
        /// <param name="workflowDefinition">The workflow definition.</param>
        /// <returns></returns>
        internal IEnumerable<PostTransitProcessor> GetPostTransitProcessors(WorkflowDefinition workflowDefinition)
        {
            if (!_postTransitProcessorCache.ContainsKey(workflowDefinition))
            {
                _postTransitProcessorCache.Add(workflowDefinition, new List<PostTransitProcessor>());

                foreach (var postTransitProcessor in this.GetDefinedPostTransitProcessors(workflowDefinition))
                {
                    _postTransitProcessorCache[workflowDefinition].Add(postTransitProcessor);
                }
            }

            return _postTransitProcessorCache[workflowDefinition];
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the defined workflow transitions.
        /// </summary>
        /// <param name="workflowDefinition">The workflow definition.</param>
        /// <returns></returns>
        private IEnumerable<WorkflowTransition> GetDefinedWorkflowTransitions(WorkflowDefinition workflowDefinition)
        {
            var publicMethods = workflowDefinition.GetType().GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
            var transitionMethods = publicMethods.Where(item => item.IsDefined<WorkflowTransitionAttribute>()).ToList();

            if (transitionMethods.Count == 0)
            {
                throw new WorkflowDefinitionException("No transitions found in workflow definition [{0}]".FormatWith(workflowDefinition.GetType().FullName));
            }

            foreach (var method in transitionMethods)
            {
                if (!method.IsDefined<WorkflowTransitionAttribute>())
                    continue;

                var parameters = method.GetParameters();
                if (parameters.Length != 1 || parameters[0].ParameterType != typeof(ProcessInstance))
                {
                    throw new InvalidTransitionDefinitionException(
                        @"Invalid transition definition. Method = [{0}]
Workflow transitions need to have a single parameter of type [Univer.WorkflowLight.ProcessInstance]"
                            .FormatWith(method.GetFullName()));
                }

                if (method.ReturnType != typeof(void))
                {
                    throw new InvalidTransitionDefinitionException(
                        @"Invalid transition definition. Method = [{0}]
Workflow transitions cannot have return value."
                            .FormatWith(method.GetFullName()));
                }

                foreach (var transitionAttribute in method.GetCustomAttributes<WorkflowTransitionAttribute>())
                {
                    var workflowTransition = new WorkflowTransition
                    {
                        Name = transitionAttribute.Name,
                        FromState = transitionAttribute.FromState,
                        ToState = transitionAttribute.ToState,
                        Method = method
                    };

                    workflowTransition.Type = transitionAttribute.Type;

                    if (workflowTransition.FromState == null)
                    {
                        throw new InvalidTransitionDefinitionException(
@"From state not defined in transition [{0}]".FormatWith(transitionAttribute.Name));
                    }

                    if (workflowTransition.ToState == null)
                    {
                        throw new InvalidTransitionDefinitionException(
@"To states not defined in transition [{0}]".FormatWith(transitionAttribute.Name));
                    }

                    yield return workflowTransition;
                }
            }
        }

        /// <summary>
        /// Gets the defined post transit processors.
        /// </summary>
        /// <param name="workflowDefinition">The workflow definition.</param>
        /// <returns></returns>
        private IEnumerable<PostTransitProcessor> GetDefinedPostTransitProcessors(WorkflowDefinition workflowDefinition)
        {
            var publicMethods = workflowDefinition.GetType().GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
            var postTransitProcessorMethods = publicMethods.Where(item => item.IsDefined<PostTransitProcessorAttribute>()).ToList();

            foreach (var method in postTransitProcessorMethods)
            {
                if (!method.IsDefined<PostTransitProcessorAttribute>())
                    continue;

                var parameters = method.GetParameters();
                if (parameters.Length != 1 || parameters[0].ParameterType != typeof(ProcessInstance))
                {
                    throw new InvalidPostTransitProcessorException(
                        @"Invalid post transition processor definition. Method = [{0}]
Workflow post transition processor need to have a single parameter of type [Univer.WorkflowLight.ProcessInstance]"
                            .FormatWith(method.GetFullName()));
                }

                if (method.ReturnType != typeof(void))
                {
                    throw new InvalidTransitionDefinitionException(@"Invalid transition definition. Method = [{0}]
Workflow post transition processor cannot have return value."
                            .FormatWith(method.GetFullName()));
                }

                foreach (var postTransitProcessorAttribute in method.GetCustomAttributes<PostTransitProcessorAttribute>())
                {
                    var postTransitProcessor = new PostTransitProcessor
                    {
                        Method = method
                    };

                    yield return postTransitProcessor;
                }
            }
        }

        #endregion
    }
}
