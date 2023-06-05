using System;
using ACTSkill;
using UnityEngine;

namespace ACTSkillDemo
{
    [Serializable]
    public class RotateExecution : IExecution
    {
        public float MaxRotateSpeed = 90;
        
        public RotateExecution() { }

        public RotateExecution(RotateExecution other)
        {
            if (other == null) return;
            MaxRotateSpeed = other.MaxRotateSpeed;
        }
        
        public void Execute(ActionNode actionNode)
        {
            MachineController controller = (MachineController) actionNode.Owner.Owner.Controller;

            Vector3 forward = controller.LocalToWorld.MultiplyVector(Vector3.forward);
            forward.y = 0;
            forward.Normalize();
            Vector3 direction = Camera.main.transform.TransformDirection(new Vector3(
                Input.GetAxis("Horizontal"),
                0,
                Input.GetAxis("Vertical")
            ).normalized);
            direction.y = 0;
            direction.Normalize();
            var signedAngle = Vector3.SignedAngle(forward, direction, Vector3.up);
            var maxAngle = MaxRotateSpeed * actionNode.Owner.Owner.LogicPerFrameTime;
            if (Mathf.Abs(signedAngle) > maxAngle)
                direction = Quaternion.Euler(0, signedAngle >= 0 ? maxAngle : -maxAngle, 0) * forward;
            controller.transform.forward = direction;
        }

        public IExecution Clone()
        {
            return new RotateExecution(this);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}