using System;
using ACTSkill;
using UnityEngine;

namespace ACTSkillDemo
{
    [Serializable]
    public abstract class NotableCondition : ICondition
    {
        [SerializeField]
        protected bool not;

        public virtual bool Not
        {
            get => not;
            set => not = value;
        }

        public abstract bool Check(ActionNode actionNode);
        public abstract NotableCondition Clone();

        public void CopyBase(NotableCondition other)
        {
            if (other == null) return;
            Not = other.Not;
        }
        
        ICondition ICondition.Clone()
        {
            return Clone();
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
