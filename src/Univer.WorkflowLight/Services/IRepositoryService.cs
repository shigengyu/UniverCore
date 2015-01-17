﻿/*
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
    public interface IRepositoryService : IWorkflowService
    {
        ProcessInstance CreateProcessInstance(Type workflowDefinitionType);
        ProcessInstance CreateProcessInstance<TWorkflowDefinition>();

        ProcessInstance GetExistingProcessInstance(Type workflowDefinitionType, Guid guid);
        ProcessInstance GetExistingProcessInstance<TWorkflowDefinition>(Guid guid);
    }
}
