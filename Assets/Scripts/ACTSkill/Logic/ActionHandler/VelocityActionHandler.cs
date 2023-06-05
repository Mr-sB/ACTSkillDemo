using ACTSkill;

namespace ACTSkillDemo
{
    public class VelocityActionHandler : IActionHandler
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
            VelocityAction config = (VelocityAction) actionNode.Config;
            controller.Rigidbody.velocity = controller.LocalToWorld.MultiplyVector(config.Velocity);
        }

        public void OnExit(ActionNode actionNode)
        {
            
        }

        public void OnRelease(ActionNode actionNode)
        {
            
        }
    }
}
