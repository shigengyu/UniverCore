using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Univer.WorkflowLight.Services;
using Univer.WorkflowLight.Execution;

namespace Univer.WorkflowLight.UnitTest.Branched
{
    [TestFixture]
    public class BranchedWorkflowTest
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
            var processInstance = this.Runtime.RepositoryService.CreateProcessInstance<BranchedWorkflowDefinition>();

            this.Runtime.ExecutionService.Execute(processInstance, "Start");
            Assert.AreEqual(2, processInstance.ActiveStates.Count);
            Assert.Contains(WorkflowState.OfType<WorkingStateA>(), processInstance.ActiveStates);
            Assert.Contains(WorkflowState.OfType<WorkingStateB>(), processInstance.ActiveStates);

            this.Runtime.ExecutionService.Execute(processInstance, "Stop");
            Assert.AreEqual(1, processInstance.ActiveStates.Count);
            Assert.AreEqual(WorkflowState.Completed, processInstance.ActiveStates.Single());
        }
    }
}
