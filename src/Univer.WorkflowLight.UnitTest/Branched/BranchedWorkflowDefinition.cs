using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Univer.WorkflowLight.Definition;
using Univer.WorkflowLight.Execution;

namespace Univer.WorkflowLight.UnitTest.Branched
{
    public class BranchedWorkflowDefinition : WorkflowDefinition
    {
        [WorkflowTransition("Start", ToState = typeof(WorkingStateA), Type = WorkflowTransitionType.FromStart)]
        public static void StartWorkingToA(ProcessInstance processInstance)
        {
        }

        [WorkflowTransition("Start", ToState = typeof(WorkingStateB), Type = WorkflowTransitionType.FromStart)]
        public static void StartWorkingToB(ProcessInstance processInstance)
        {
        }

        [WorkflowTransition("Stop", FromState = typeof(WorkingStateA), ToState = typeof(WorkCompletedStateA))]
        public static void StopWorkingFromA(ProcessInstance processInstance)
        {
        }

        [WorkflowTransition("Stop", FromState = typeof(WorkingStateB), ToState = typeof(WorkCompletedStateB))]
        public static void StopWorkingFromB(ProcessInstance processInstance)
        {
        }

        [PostTransitProcessor]
        public static void SetWorkCompleted(ProcessInstance processInstance)
        {
            if (processInstance.ActiveStates.Contains(WorkflowState.OfType<WorkCompletedStateA>()) &&
                processInstance.ActiveStates.Contains(WorkflowState.OfType<WorkCompletedStateB>()))
            {
                TransitionHelper.TransitProcessState(processInstance,
                    new WorkflowState[] { WorkflowState.OfType<WorkCompletedStateA>(), WorkflowState.OfType<WorkCompletedStateB>() },
                    new WorkflowState[] { WorkflowState.Completed });
            }
        }
    }
}
