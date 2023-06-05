using System;
using ACTSkill;

namespace ACTSkillDemo
{
    [Serializable]
    public class HitAction : EditableFramesAction
    {
        public HitAction() { }

        public HitAction(HitAction other) : this()
        {
            Copy(other);
        }
        
        public void Copy(HitAction other)
        {
            if (other == null) return;
            CopyBase(other);
        }
        
        public override IActionHandler CreateHandler()
        {
            return Pools.GetActionHandler<HitActionHandler>();
        }

        public override void OnReleaseHandler(IActionHandler handler)
        {
            if (handler is not HitActionHandler hitActionHandler) return;
            Pools.ReleaseActionHandler(hitActionHandler);
        }

        public override EditableFramesAction Clone()
        {
            return new HitAction(this);
        }

        public override void Copy(object obj)
        {
            if (obj is HitAction other)
                Copy(other);
        }
    }
}