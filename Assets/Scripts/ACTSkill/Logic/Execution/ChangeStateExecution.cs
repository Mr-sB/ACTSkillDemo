using System;
using ACTSkill;

namespace ACTSkillDemo
{
    [Serializable]
    public class ChangeStateExecution : IExecution
    {
        public string StateName;
        public int Priority;
        public AnimationTransitionConfig Transition = new AnimationTransitionConfig();
        
        public ChangeStateExecution() { }
        public ChangeStateExecution(ChangeStateExecution other)
        {
            Copy(other);
        }

        public void Copy(ChangeStateExecution other)
        {
            if (other == null) return;
            StateName = other.StateName;
            Priority = other.Priority;
            Transition.Copy(other.Transition);
        }
        
        public void Execute(ActionNode actionNode)
        {
            actionNode.Owner.Owner.ChangeState(StateName, Priority, Transition);
        }

        public ChangeStateExecution Clone()
        {
            return new ChangeStateExecution(this);
        }
        
        IExecution IExecution.Clone()
        {
            return Clone();
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
