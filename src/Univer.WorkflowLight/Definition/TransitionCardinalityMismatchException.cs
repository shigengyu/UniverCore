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

namespace Univer.WorkflowLight.Definition
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class TransitionCardinalityMismatchException : WorkflowException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransitionCardinalityMismatchException"/> class.
        /// </summary>
        /// <param name="transitionName">Name of the transition.</param>
        /// <param name="workflowDefinitionType">Type of the workflow definition.</param>
        /// <param name="actualCount">The actual count.</param>
        /// <param name="expectedCount">The expected count.</param>
        public TransitionCardinalityMismatchException(string transitionName, Type workflowDefinitionType, int actualCount, int expectedCount = 1)
            : base("Transition cardinality mismatch for transition [{0}] defined in [{1}]. Expected = [{2}]. Actual = [{3}]".FormatWith(transitionName, workflowDefinitionType.FullName, expectedCount, actualCount))
        {

        }
    }
}
