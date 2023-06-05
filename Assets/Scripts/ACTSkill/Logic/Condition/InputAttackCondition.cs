using System;
using System.Collections.Generic;
using ACTSkill;
using UnityEngine;

namespace ACTSkillDemo
{
    [Serializable]
    public class InputAttackCondition : NotableCondition
    {
        public InputAttackCondition() { }

        public InputAttackCondition(InputAttackCondition other)
        {
            Copy(other);
        }

        public void Copy(InputAttackCondition other)
        {
            if (other == null) return;
            CopyBase(other);
        }

        public override bool Check(ActionNode actionNode)
        {
            return actionNode.Owner.Owner.Controller.TryGetData(MachineDataKeys.InputKeys, out List<KeyCode> inputKeys) &&
                   inputKeys.Contains(KeyCode.J);
        }

        public override NotableCondition Clone()
        {
            return new InputAttackCondition(this);
        }
    }
}
