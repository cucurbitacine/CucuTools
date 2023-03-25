using System;
using UnityEngine;

namespace CucuTools.DamageSystem
{
    [Serializable]
    public class Damage
    {
        public int amount;
        public bool critical;
    }

    public class DamageInfo
    {
        public readonly Damage damage = null;
        public readonly DamageSource source = null;
        public readonly DamageReceiver receiver = null;
        
        public Vector3 point = Vector3.zero;
        public Vector3 normal = Vector3.up;
        
        public DamageInfo(Damage dmg, DamageSource src, DamageReceiver rcv)
        {
            damage = dmg;
            source = src;
            receiver = rcv;
        }
    }
}