using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Univer.WorkflowLight.Services;
using Univer.WorkflowLight.Execution;

namespace Univer.WorkflowLight.UnitTest.StateInternalTransition
{
    [TestFixture]
    public class StateInternalTransitionWorkflowTest
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
            var processInstance = this.Runtime.RepositoryService.CreateProcessInstance<StateInternalTransitionWorkflowDefinition>();

            this.Runtime.ExecutionService.Execute(processInstance, "Start");
            Assert.AreEqual(1, processInstance.ActiveStates.Count);
            Assert.AreEqual(typeof(SampleState), processInstance.ActiveStates[0].GetType());

            this.Runtime.ExecutionService.Execute(processInstance, "Transit");
            Assert.AreEqual(1, processInstance.ActiveStates.Count);
            Assert.AreEqual(typeof(SampleState), processInstance.ActiveStates[0].GetType());

            this.Runtime.ExecutionService.Execute(processInstance, "Stop");
            Assert.AreEqual(1, processInstance.ActiveStates.Count);
            Assert.AreEqual(typeof(WorkflowCompletedState), processInstance.ActiveStates[0].GetType());
        }
    }
}
