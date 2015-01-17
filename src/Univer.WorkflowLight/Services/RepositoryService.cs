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
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Univer.WorkflowLight.Execution;

namespace Univer.WorkflowLight.Services
{
    public class RepositoryService : WorkflowServiceBase, IRepositoryService
    {
        public ProcessInstance CreateProcessInstance(Type workflowDefinitionType)
        {
            return new ProcessInstance(workflowDefinitionType);
        }

        public ProcessInstance CreateProcessInstance<TWorkflowDefinition>()
        {
            return new ProcessInstance(typeof(TWorkflowDefinition));
        }

        public ProcessInstance GetExistingProcessInstance(Type workflowDefinitionType, Guid guid)
        {
            throw new NotImplementedException();
        }

        public ProcessInstance GetExistingProcessInstance<TWorkflowDefinition>(Guid guid)
        {
            throw new NotImplementedException();
        }
    }
}
