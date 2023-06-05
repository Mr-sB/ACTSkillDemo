using System.Collections.Generic;
using ACTSkill;
using UnityEngine;

namespace ACTSkillDemo
{
    public class AttackActionHandler : IActionHandler
    {
        private static readonly Collider[] colliders = new Collider[50];
        private HashSet<MachineController> attackedControllers = new HashSet<MachineController>();
        
        public void OnCreate(ActionNode actionNode)
        {
            
        }

        public void OnEnter(ActionNode actionNode)
        {
            attackedControllers.Clear();
        }

        public void OnUpdate(ActionNode actionNode)
        {
            if (actionNode.Config is not AttackAction attackAction) return;
            MachineController controller = (MachineController) actionNode.Owner.Owner.Controller;
            Matrix4x4 localToWorld = actionNode.Owner.Owner.Controller.LocalToWorld;
            var rangeConfig = actionNode.Owner.Config.GetAttackRange(actionNode.Owner.Frame);
            if (rangeConfig == null || rangeConfig.Ranges.Count == 0) return;

            rangeConfig.Overlap(localToWorld, controller.AttackMask, colliders, ProcessOverlap, ref actionNode);
        }

        public void OnExit(ActionNode actionNode)
        {
            attackedControllers.Clear();
        }

        public void OnRelease(ActionNode actionNode)
        {
            
        }
        
        /// <returns>Continue?</returns>
        private bool ProcessOverlap(int count, ref ActionNode actionNode)
        {
            if (count <= 0) return true;
            for (int i = 0; i < count; i++)
            {
                var attackedController = colliders[i].GetComponentInParent<MachineController>();
                if (attackedController == null || attackedController == (MachineController) actionNode.Owner.Owner.Controller) continue;
                Attack(actionNode, attackedController);
            }
            return true;
        }
        
        private void Attack(ActionNode actionNode, MachineController controller)
        {
            if (attackedControllers.Contains(controller)) return;
            attackedControllers.Add(controller);
            MachineController attacker = (MachineController)actionNode.Owner.Owner.Controller;
            //TODO calc real damage
            var info = Pools.InjuredInfoPool.Get().Init((AttackAction) actionNode.Config, attacker, 10);
            controller.Injured(info);
            attacker.OnAttack(Pools.InjuredInfoPool.Get().Init(info));
        }
    }
}
