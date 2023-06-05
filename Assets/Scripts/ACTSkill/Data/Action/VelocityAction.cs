using System;
using ACTSkill;
using UnityEngine;

namespace ACTSkillDemo
{
    [Serializable]
    public class VelocityAction : EditableFramesAction
    {
        public Vector3 Velocity;
        
        public VelocityAction() { }

        public VelocityAction(VelocityAction other) : this()
        {
            Copy(other);
        }
        
        public void Copy(VelocityAction other)
        {
            if (other == null) return;
            CopyBase(other);
            Velocity = other.Velocity;
        }
        
        public override IActionHandler CreateHandler()
        {
            return Pools.GetActionHandler<VelocityActionHandler>();
        }

        public override void OnReleaseHandler(IActionHandler handler)
        {
            if (handler is not VelocityActionHandler velocityActionHandler) return;
            Pools.ReleaseActionHandler(velocityActionHandler);
        }

        public override EditableFramesAction Clone()
        {
            return new VelocityAction(this);
        }

        public override void Copy(object obj)
        {
            if (obj is VelocityAction other)
                Copy(other);
        }
    }
}
