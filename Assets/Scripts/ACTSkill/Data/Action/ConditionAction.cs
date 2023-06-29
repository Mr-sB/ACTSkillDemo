using System;
using System.Collections.Generic;
using ACTSkill;
using CustomizationInspector.Runtime;
using UnityEngine;

namespace ACTSkillDemo
{
    [Serializable]
    public class ConditionAction : ActionBase
    {
        [LabelWidth(80)]
        [SerializeReference, SerializeReferenceSelector]
        public List<ICondition> Conditions = new List<ICondition>();
        [LabelWidth(80)]
        [SerializeReference, SerializeReferenceSelector]
        public List<IExecution> Executions = new List<IExecution>();
        
        public ConditionAction() { }

        public ConditionAction(ConditionAction other)
        {
            Copy(other);
        }
        
        public void Copy(ConditionAction other)
        {
            if (other == null) return;
            CopyBase(other);
            Conditions.Clear();
            Executions.Clear();
            if (other.Conditions != null)
                foreach (var condition in other.Conditions)
                    Conditions.Add(condition.Clone());
            if (other.Executions != null)
                foreach (var execution in other.Executions)
                    Executions.Add(execution.Clone());
        }
        
        public override IActionHandler CreateHandler()
        {
            return Pools.GetActionHandler<ConditionActionHandler>();
        }

        public override void OnReleaseHandler(IActionHandler handler)
        {
            if (handler is not ConditionActionHandler conditionActionHandler) return;
            Pools.ReleaseActionHandler(conditionActionHandler);
        }

        public override ActionBase Clone()
        {
            return new ConditionAction(this);
        }

        public override void Copy(object obj)
        {
            if (obj is ConditionAction other)
                Copy(other);
        }
    }
}
