using System;

namespace CucuTools.DamageSystem
{
    [Serializable]
    public class Damage
    {
        public int amount;
        public bool critical;

        public readonly Guid guid = Guid.NewGuid();
        
        public delegate void DamageCallback(DamageEvent e);

        public static DamageCallback Event { get; set; }
    }

    public class DamageEvent
    {
        public readonly Damage damage = null;
        public readonly DamageSource source = null;
        public readonly DamageReceiver receiver = null;
        
        public DamageEvent(Damage dmg, DamageSource src, DamageReceiver rcv)
        {
            damage = dmg;
            source = src;
            receiver = rcv;
        }

        public DamageEvent(DamageEvent e) : this(e.damage, e.source, e.receiver)
        {
        }
    }
}