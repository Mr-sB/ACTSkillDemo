using System;
using ACTSkill;
using CustomizationInspector.Runtime;
using UnityEngine;

namespace ACTSkillDemo
{
    public interface IAttackConfig : ICopyable
    {
        new IAttackConfig Clone();
    }
    
    [Serializable]
    public class AttackHitConfig : IAttackConfig
    {
        public float HitForce;
        
        object ICloneable.Clone()
        {
            return Clone();
        }

        public void Copy(object obj)
        {
            if (obj is not AttackHitConfig other) return;
            HitForce = other.HitForce;
        }

        public IAttackConfig Clone()
        {
            return new AttackHitConfig {HitForce = HitForce};
        }
    }
    
    [Serializable]
    public class AttackLevitateConfig : IAttackConfig
    {
        public float UpHeight;
        
        object ICloneable.Clone()
        {
            return Clone();
        }

        public void Copy(object obj)
        {
            if (obj is not AttackLevitateConfig other) return;
            UpHeight = other.UpHeight;
        }

        public IAttackConfig Clone()
        {
            return new AttackLevitateConfig {UpHeight = UpHeight};
        }
    }
    
    [Serializable]
    public class AttackBlowUpConfig : IAttackConfig
    {
        public float BlowUpDistance;

        object ICloneable.Clone()
        {
            return Clone();
        }

        public void Copy(object obj)
        {
            if (obj is not AttackBlowUpConfig other) return;
            BlowUpDistance = other.BlowUpDistance;
        }

        public IAttackConfig Clone()
        {
            return new AttackBlowUpConfig {BlowUpDistance = BlowUpDistance};
        }
    }
    
    [Serializable]
    public class AttackAction : ActionBase
    {
        public AttackMode AttackMode;
        public Vector3 AttackDirection = Vector3.forward;
        public int PauseFrameCount;
        [SerializeReference, SerializeReferenceSelector]
        public IAttackConfig ExtraData;

        public AttackAction() { }

        public AttackAction(AttackAction other) : this()
        {
            Copy(other);
        }
        
        public void Copy(AttackAction other)
        {
            if (other == null) return;
            CopyBase(other);
            AttackMode = other.AttackMode;
            AttackDirection = other.AttackDirection;
            PauseFrameCount = other.PauseFrameCount;
            ExtraData = other.ExtraData.Clone();
        }
        
        public override IActionHandler CreateHandler()
        {
            return Pools.GetActionHandler<AttackActionHandler>();
        }

        public override void OnReleaseHandler(IActionHandler handler)
        {
            if (handler is not AttackActionHandler attackActionHandler) return;
            Pools.ReleaseActionHandler(attackActionHandler);
        }

        public override ActionBase Clone()
        {
            return new AttackAction(this);
        }

        public override void Copy(object obj)
        {
            if (obj is AttackAction other)
                Copy(other);
        }
    }
}
