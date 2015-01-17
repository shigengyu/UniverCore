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
    public class TransitionNotFoundException : WorkflowException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransitionNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public TransitionNotFoundException(string transitionName, Type workflowDefinitionType)
            : base("Cannot find transition [{0}] in definition [{1}].".FormatWith(transitionName, workflowDefinitionType.FullName))
        {
        }
    }
}
