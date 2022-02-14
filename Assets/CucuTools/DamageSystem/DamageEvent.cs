using System;

namespace CucuTools.DamageSystem
{
    [Serializable]
    public struct DamageEvent
    {
        public DamageInfo damage;
        public DamageReceiver receiver;
        public DamageSource source;

        public DamageEvent(DamageInfo damage, DamageEvent e) : this(damage, e.receiver, e.source)
        {
        }
        
        public DamageEvent(DamageEvent e) : this(e.damage, e.receiver, e.source)
        {
        }
        
        public DamageEvent(DamageInfo damage, DamageReceiver receiver, DamageSource source = null) 
        {
            this.damage = damage;
            this.receiver = receiver;
            this.source = source;
        }
    }

    [Serializable]
    public struct DamageInfo
    {
        public int amount;
        public DamageType type;
        public bool isCritical;
    }

    public enum DamageType
    {
        Physical,
        Fire,
        Water,
        Earth,
        Air,
        Lightning,
    }
}