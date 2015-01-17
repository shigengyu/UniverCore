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
using Univer.WorkflowLight.Execution;

namespace Univer.WorkflowLight.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IExecutionService : IWorkflowService
    {
        /// <summary>
        /// Executes the specified process instance.
        /// </summary>
        /// <param name="processInstance">The process instance.</param>
        /// <param name="transitionName">Name of the transition.</param>
        void Execute(ProcessInstance processInstance, string transitionName);
    }
}
