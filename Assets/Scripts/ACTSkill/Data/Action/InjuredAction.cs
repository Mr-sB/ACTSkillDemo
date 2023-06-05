using System;
using ACTSkill;

namespace ACTSkillDemo
{
    [Serializable]
    public class InjuredAction : EditableFramesAction
    {
        public string HitStateName;
        public int HitPriority;
        public AnimationTransitionConfig HitTransition;
        public string LevitateStateName;
        public int LevitatePriority;
        public AnimationTransitionConfig LevitateTransition;
        public string BlowUpStateName;
        public int BlowUpPriority;
        public AnimationTransitionConfig BlowUpTransition;
        
        public InjuredAction() { }

        public InjuredAction(InjuredAction other) : this()
        {
            Copy(other);
        }
        
        public void Copy(InjuredAction other)
        {
            if (other == null) return;
            CopyBase(other);
            HitStateName = other.HitStateName;
            LevitateStateName = other.LevitateStateName;
            BlowUpStateName = other.BlowUpStateName;
        }
        
        public override IActionHandler CreateHandler()
        {
            return Pools.GetActionHandler<InjuredActionHandler>();
        }

        public override void OnReleaseHandler(IActionHandler handler)
        {
            if (handler is not InjuredActionHandler injuredActionHandler) return;
            Pools.ReleaseActionHandler(injuredActionHandler);
        }

        public override EditableFramesAction Clone()
        {
            return new InjuredAction(this);
        }

        public override void Copy(object obj)
        {
            if (obj is InjuredAction other)
                Copy(other);
        }
    }
}