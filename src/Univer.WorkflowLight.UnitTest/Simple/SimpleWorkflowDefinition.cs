using Univer.WorkflowLight.Definition;
using Univer.WorkflowLight.Execution;

namespace Univer.WorkflowLight.UnitTest.Simple
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
    }
}
