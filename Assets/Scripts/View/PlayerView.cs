using System.Collections.Generic;
using UnityEngine;

namespace ACTSkillDemo
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerView : MachineController
    {
        [SerializeField]
        private List<KeyCode> inputKeys = new List<KeyCode>();
        
        protected override void Awake()
        {
            Animator = GetComponent<Animator>();
            Animator.enabled = false;
            Rigidbody = GetComponent<Rigidbody>();
            SetData(MachineDataKeys.InputKeys, inputKeys);
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.J))
                inputKeys.Add(KeyCode.J);
            if (Input.GetKeyDown(KeyCode.K))
                inputKeys.Add(KeyCode.K);
        }

        protected override void OnLogicUpdated()
        {
            base.OnLogicUpdated();
            inputKeys.Clear();
        }
    }
}
