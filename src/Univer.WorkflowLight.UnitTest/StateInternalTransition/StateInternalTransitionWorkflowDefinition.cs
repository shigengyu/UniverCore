using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Univer.WorkflowLight.Definition;
using Univer.WorkflowLight.Execution;

namespace Univer.WorkflowLight.UnitTest.StateInternalTransition
{
    public class StateInternalTransitionWorkflowDefinition : WorkflowDefinition
    {
        [WorkflowTransition("Start", ToState = typeof(SampleState), Type = WorkflowTransitionType.FromStart)]
        public static void StartWorking(ProcessInstance processInstance)
        {
        }

        [WorkflowTransition("Transit", FromState = typeof(SampleState), ToState = typeof(SampleState))]
        public static void StateInternalTransit(ProcessInstance processInstance)
        {
        }

        [WorkflowTransition("Stop", FromState = typeof(SampleState), Type = WorkflowTransitionType.ToCompleted)]
        public static void StopWorking(ProcessInstance processInstance)
        {
        }
    }
}
