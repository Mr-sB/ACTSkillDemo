using System;
using ACTSkill;
using UnityEngine;

namespace ACTSkillDemo
{
    [Serializable]
    public class CanAttackCondition : NotableCondition
    {
        private static readonly Collider[] colliders = new Collider[1];
        
        public CanAttackCondition() { }

        public CanAttackCondition(CanAttackCondition other)
        {
            Copy(other);
        }

        public void Copy(CanAttackCondition other)
        {
            if (other == null) return;
            CopyBase(other);
        }
        
        public override bool Check(ActionNode actionNode)
        {
            //Whether if self attack ranges overlap other body ranges.
            MachineController controller = (MachineController) actionNode.Owner.Owner.Controller;
            Matrix4x4 localToWorld = actionNode.Owner.Owner.Controller.LocalToWorld;
            var rangeConfig = actionNode.Owner.Config.GetAttackRange(actionNode.Owner.Frame);
            if (rangeConfig == null || rangeConfig.Ranges.Count == 0) return false;

            bool success = false;
            rangeConfig.Overlap(localToWorld, controller.AttackMask, colliders, ProcessOverlap, ref success);
            return success;
        }

        public override NotableCondition Clone()
        {
            return new CanAttackCondition(this);
        }

        private static bool ProcessOverlap(int count, ref bool success)
        {
            if (count <= 0) return true;
            success = true;
            return false;
        }
    }
}
