using ACTSkill;
using UnityEngine;

namespace ACTSkillDemo
{
    public class HitActionHandler : IActionHandler
    {
        public void OnCreate(ActionNode actionNode)
        {
            
        }

        public void OnEnter(ActionNode actionNode)
        {
            MachineController controller = (MachineController) actionNode.Owner.Owner.Controller;
            InjuredInfo injuredInfo = controller.GetData<InjuredInfo>(MachineDataKeys.CurInjuredInfo);
            controller.SetPauseFrameCount(injuredInfo.Config.PauseFrameCount, true);
            if (injuredInfo.Config.ExtraData is AttackHitConfig hitConfig)
            {
                controller.Rigidbody.velocity = injuredInfo.Attacker.LocalToWorld.MultiplyVector(injuredInfo.Config.AttackDirection * hitConfig.HitForce);
            }
            else
            {
                controller.Rigidbody.velocity = Vector3.zero;
            }
        }

        public void OnUpdate(ActionNode actionNode)
        {
            
        }

        public void OnExit(ActionNode actionNode)
        {
            
        }

        public void OnRelease(ActionNode actionNode)
        {
            
        }
    }
}
