using System;

namespace CucuTools.DamageSystem
{
    /// <summary>
    /// Damage information
    /// </summary>
    [Serializable]
    public class Damage // Use it or inherit it
    {
        public int amount;
        public bool critical;

        public readonly Guid guid = Guid.NewGuid();
        
        public delegate void DamageCallback(DamageEvent e);

        public static DamageCallback Event { get; set; }
    }

    /// <summary>
    /// Damage event information
    /// <seealso cref="Damage"/>
    /// <seealso cref="DamageSource"/>
    /// <seealso cref="DamageReceiver"/>
    /// </summary>
    public sealed class DamageEvent // Who hit who and how?
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