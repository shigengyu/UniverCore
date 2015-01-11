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
using System.Reflection;
using System.Text;
using Univer.Common;
using Univer.WorkflowLight.Definition;
using Univer.WorkflowLight.Execution;

namespace Univer.WorkflowLight.Definition
{
    /// <summary>
    /// 
    /// </summary>
    public static class WorkflowDefinitionExtension
    {
        /// <summary>
        /// Gets the workflow transitions.
        /// </summary>
        /// <param name="workflowDefinition">The workflow definition.</param>
        /// <param name="transitionName">Name of the transition.</param>
        /// <returns></returns>
        public static IEnumerable<WorkflowTransition> GetWorkflowTransitions(this WorkflowDefinition workflowDefinition, string transitionName)
        {
            return WorkflowTransitionManager.Instance.GetWorkflowTransitions(workflowDefinition, transitionName);
        }

        /// <summary>
        /// Gets the post transit processors.
        /// </summary>
        /// <param name="workflowDefinition">The workflow definition.</param>
        /// <returns></returns>
        public static IEnumerable<PostTransitProcessor> GetPostTransitProcessors(this WorkflowDefinition workflowDefinition)
        {
            return WorkflowTransitionManager.Instance.GetPostTransitProcessors(workflowDefinition);
        }
    }
}
