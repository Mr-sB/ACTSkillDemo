using System;
using System.Collections.Generic;
using ACTSkill;
using UnityEngine;

namespace ACTSkillDemo
{
    [Serializable]
    public class InputStreakCondition : NotableCondition
    {
        // public static readonly KeyCode[] commands = new KeyCode[] {KeyCode.W, KeyCode.A, KeyCode.J};
        public InputStreakCondition() { }

        public InputStreakCondition(InputStreakCondition other)
        {
            Copy(other);
        }

        public void Copy(InputStreakCondition other)
        {
            if (other == null) return;
            CopyBase(other);
        }

        public override bool Check(ActionNode actionNode)
        {
            return actionNode.Owner.Owner.Controller.TryGetData(MachineDataKeys.InputKeys, out List<KeyCode> inputKeys) &&
                   inputKeys.Contains(KeyCode.K);
            // if (!actionNode.Owner.Owner.Controller.TryGetData(MachineDataKeys.InputKeys, out List<KeyCode> inputKeys)) return false;
            // int cmdLen = commands.Length;
            // for (int i = 0, count = inputKeys.Count - cmdLen; i <= count; i++)
            // {
            //     int matchCount = 0;
            //     for (int j = 0; j < cmdLen; j++)
            //     {
            //         if (inputKeys[i + j] == commands[j])
            //             matchCount++;
            //         else
            //             break;
            //     }
            //     if (matchCount >= cmdLen) return true;
            // }
            // return false;
        }

        public override NotableCondition Clone()
        {
            return new InputStreakCondition(this);
        }
    }
}
