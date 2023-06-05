using ACTSkill;

namespace ACTSkillDemo
{
    public class ConditionActionHandler : IActionHandler
    {
        public void OnCreate(ActionNode actionNode)
        {
            
        }

        public void OnEnter(ActionNode actionNode)
        {
            
        }

        public void OnUpdate(ActionNode actionNode)
        {
            ConditionAction config = (ConditionAction) actionNode.Config;
            if (config.Conditions == null) return;
            bool success = true;
            foreach (var configCondition in config.Conditions)
            {
                if (configCondition != null && !(configCondition.Not ^ configCondition.Check(actionNode)))
                {
                    success = false;
                    break;
                }
            }
            if (!success || config.Executions == null) return;
            
            foreach (var execution in config.Executions)
            {
                if (execution == null) return;
                execution.Execute(actionNode);
            }
        }

        public void OnExit(ActionNode actionNode)
        {
            
        }

        public void OnRelease(ActionNode actionNode)
        {
            
        }
    }
}
