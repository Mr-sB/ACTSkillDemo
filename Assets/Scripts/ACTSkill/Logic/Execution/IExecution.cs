using System;
using ACTSkill;

namespace ACTSkillDemo
{
    public interface IExecution : ICloneable
    {
        void Execute(ActionNode actionNode);
        new IExecution Clone();
    }
}
