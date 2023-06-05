using System.Collections.Generic;
using ACTSkill;

namespace ACTSkillDemo
{
    public class InjuredActionHandler : IActionHandler
    {
        public void OnCreate(ActionNode actionNode)
        {
            
        }

        public void OnEnter(ActionNode actionNode)
        {
            
        }

        public void OnUpdate(ActionNode actionNode)
        {
            MachineController controller = (MachineController) actionNode.Owner.Owner.Controller;
            InjuredAction config = (InjuredAction) actionNode.Config;
            Queue<InjuredInfo> injuredInfos = controller.GetData<Queue<InjuredInfo>>(MachineDataKeys.InjuredInfos);
            if (injuredInfos.Count == 0) return;
            InjuredInfo lastInjuredInfo = null;
            int count = injuredInfos.Count;
            while (count > 0)
            {
                count--;
                lastInjuredInfo = injuredInfos.Dequeue();
                //TODO damage
                // injuredInfo.Damage

                if (count > 0)
                    Pools.InjuredInfoPool.Release(lastInjuredInfo);
            }
            
            if (controller.TryGetData(MachineDataKeys.CurInjuredInfo, out InjuredInfo oldInfo) && oldInfo != null)
                Pools.InjuredInfoPool.Release(oldInfo);
            controller.SetData(MachineDataKeys.CurInjuredInfo, lastInjuredInfo);
            switch (lastInjuredInfo.Config.AttackMode)
            {
                case AttackMode.Hit:
                    actionNode.Owner.Owner.ChangeState(config.HitStateName, config.HitPriority, config.HitTransition);
                    break;
                case AttackMode.Levitate:
                    actionNode.Owner.Owner.ChangeState(config.LevitateStateName, config.LevitatePriority, config.LevitateTransition);
                    break;
                case AttackMode.BlowUp:
                    actionNode.Owner.Owner.ChangeState(config.BlowUpStateName, config.BlowUpPriority, config.BlowUpTransition);
                    break;
            }
        }

        public void OnExit(ActionNode actionNode)
        {
            
        }

        public void OnRelease(ActionNode actionNode)
        {
            
        }
    }
}
