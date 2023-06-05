namespace ACTSkillDemo
{
    public class InjuredInfo
    {
        public AttackAction Config;
        public MachineController Attacker;
        public int Damage;

        public InjuredInfo Init(AttackAction config, MachineController attacker, int damage)
        {
            Config = config;
            Attacker = attacker;
            Damage = damage;
            return this;
        }

        public InjuredInfo Init(InjuredInfo other)
        {
            return other == null ? this : Init(other.Config, other.Attacker, other.Damage);
        }
        
        public void Reset()
        {
            Config = null;
            Attacker = null;
            Damage = 0;
        }
    }
}
