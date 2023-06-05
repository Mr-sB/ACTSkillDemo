using UnityEngine;

namespace ACTSkillDemo
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyView : MachineController
    {
        protected override void Awake()
        {
            Animator = GetComponent<Animator>();
            Animator.enabled = false;
            Rigidbody = GetComponent<Rigidbody>();
            base.Awake();
        }
    }
}