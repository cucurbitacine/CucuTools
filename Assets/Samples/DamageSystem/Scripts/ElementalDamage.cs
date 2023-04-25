using System;
using System.Collections.Generic;
using CucuTools.DamageSystem;

namespace Samples.DamageSystem.Scripts
{
    [Serializable]
    public class ElementalDamage : Damage
    {
        public Elemental elemental = Elemental.Fire;

        public ElementalDamage()
        {
        }

        public ElementalDamage(Damage damage, Elemental elemental)
        {
            this.amount = damage.amount;
            this.critical = damage.critical;
            
            this.elemental = elemental;
        }
        
        public ElementalDamage(ElementalDamage damage) : this(damage, damage.elemental)
        {
        }

        private static readonly Dictionary<Elemental, Elemental> AdvantageElement = new()
        {
            { Elemental.Fire, Elemental.Grass },
            { Elemental.Grass, Elemental.Water },
            { Elemental.Water, Elemental.Fire },
        };
        
        private static readonly Dictionary<Elemental, Elemental> DisadvantageElement = new()
        {
            { Elemental.Fire, Elemental.Water },
            { Elemental.Water, Elemental.Grass },
            { Elemental.Grass, Elemental.Fire },
        };

        public static Elemental GetAdvantage(Elemental element)
        {
            return AdvantageElement[element];
        }
        
        public static Elemental GetDisadvantage(Elemental element)
        {
            return DisadvantageElement[element];
        }
    }

    public enum Elemental
    {
        Fire,
        Grass,
        Water,
    }
}
