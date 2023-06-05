using System;
using ACTSkill;
using UnityEngine;

namespace ACTSkillDemo
{
    [Serializable]
    public class InputMoveCondition : NotableCondition
    {
        public InputMoveCondition() { }

        public InputMoveCondition(InputMoveCondition other)
        {
            Copy(other);
        }

        public void Copy(InputMoveCondition other)
        {
            if (other == null) return;
            CopyBase(other);
        }
        
        public override bool Check(ActionNode actionNode)
        {
            return Input.GetButton("Horizontal") || Input.GetButton("Vertical");
        }

        public override NotableCondition Clone()
        {
            return new InputMoveCondition(this);
        }
    }
}
