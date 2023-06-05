using System;
using ACTSkill;

namespace ACTSkillDemo
{
    public interface ICondition : ICloneable
    {
        bool Not { set; get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionNode"></param>
        /// <returns>success</returns>
        bool Check(ActionNode actionNode);
        new ICondition Clone();
    }
}
