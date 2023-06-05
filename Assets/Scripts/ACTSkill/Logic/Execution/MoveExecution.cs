using System;
using ACTSkill;
using UnityEngine;

namespace ACTSkillDemo
{
    [Serializable]
    public class MoveExecution : IExecution
    {
        public float Speed;
        public MoveExecution() { }
        public MoveExecution(MoveExecution other)
        {
            Copy(other);
        }

        public void Copy(MoveExecution other)
        {
            if (other == null) return;
            Speed = other.Speed;
        }
        
        public void Execute(ActionNode actionNode)
        {
            MachineController controller = (MachineController) actionNode.Owner.Owner.Controller;
            Vector3 direction = Camera.main.transform.TransformDirection(new Vector3(
                Input.GetAxis("Horizontal"),
                0,
                Input.GetAxis("Vertical")
            ).normalized);
            direction.y = 0;
            direction.Normalize();
            controller.transform.forward = direction;
            controller.Rigidbody.velocity = direction * Speed;
        }

        public MoveExecution Clone()
        {
            return new MoveExecution(this);
        }
        
        IExecution IExecution.Clone()
        {
            return Clone();
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
