using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Univer.WorkflowLight.Execution;
using Univer.WorkflowLight.Services;

namespace Univer.WorkflowLight.UnitTest.Simple
{
    [TestFixture]
    public class SimpleWorkflowTest
    {
        private WorkflowRuntime Runtime
        {
            get;
            set;
        }

        [SetUp]
        public void Setup()
        {
            this.Runtime = new WorkflowRuntime();
            this.Runtime.ExecutionService = new ExecutionService();
            this.Runtime.RepositoryService = new RepositoryService();
        }

        [Test]
        public void RunTest()
        {
            var processInstance = this.Runtime.RepositoryService.CreateProcessInstance<SimpleWorkflowDefinition>();

            this.Runtime.ExecutionService.Execute(processInstance, "Start");
            Assert.AreEqual(1, processInstance.ActiveStates.Count);
            Assert.AreEqual(WorkflowState.OfType<SimpleWorkingState>(), processInstance.ActiveStates.Single());

            this.Runtime.ExecutionService.Execute(processInstance, "Stop");
            Assert.AreEqual(1, processInstance.ActiveStates.Count);
            Assert.AreEqual(WorkflowState.Completed, processInstance.ActiveStates.Single());
        }
    }
}
