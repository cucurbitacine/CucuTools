using CucuTools.DamageSystem;

namespace Samples.Demo.DamageSystem.Scripts
{
    public class ElementalDamage : Damage
    {
        public ElementalType elemental;

        public ElementalDamage(Damage damage)
        {
            amount = damage.amount;
            critical = damage.critical;
        }
        
        public override string ToString()
        {
            return $"{base.ToString()} {elemental}";
        }
    }

    public enum ElementalType
    {
        Fire,
        Grass,
        Water,
    }
}