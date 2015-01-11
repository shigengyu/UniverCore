using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Univer.WorkflowLight.Definition;
using Univer.WorkflowLight.Execution;

namespace Univer.WorkflowLight.UnitTest.Restart
{
    public class SimpleWorkflowDefinition : WorkflowDefinition
    {
        [WorkflowTransition("Start", ToState = typeof(SimpleWorkingState), Type = WorkflowTransitionType.FromStart)]
        public static void StartWorking(ProcessInstance processInstance)
        {
        }

        [WorkflowTransition("Stop", FromState = typeof(SimpleWorkingState), Type = WorkflowTransitionType.ToCompleted)]
        public static void StopWorking(ProcessInstance processInstance)
        {
        }

        [WorkflowTransition("Restart", FromState = typeof(WorkflowCompletedState), ToState = typeof(SimpleWorkingState))]
        public static void RestartWorking(ProcessInstance processInstance)
        {
        }
    }
}
