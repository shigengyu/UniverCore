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
using Univer.WorkflowLight.Services;
using Univer.WorkflowLight.Execution;

namespace Univer.WorkflowLight
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowRuntime
    {
        private IExecutionService _executionService;
        private IRepositoryService _repositoryService;

        /// <summary>
        /// Gets or sets the execution service.
        /// </summary>
        /// <value>
        /// The execution service.
        /// </value>
        public IExecutionService ExecutionService
        {
            get { return _executionService; }
            set
            {
                _executionService = value;
                _executionService.Runtime = this;
            }
        }

        /// <summary>
        /// Gets or sets the repository service.
        /// </summary>
        /// <value>
        /// The repository service.
        /// </value>
        public IRepositoryService RepositoryService
        {
            get { return _repositoryService; }
            set
            {
                _repositoryService = value;
                _repositoryService.Runtime = this;
            }
        }

        public ProcessInstance CreateProcessInstance<TWorkflowDefinition>()
        {
            return this.RepositoryService.CreateProcessInstance<TWorkflowDefinition>();
        }
    }
}
