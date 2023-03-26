using UnityEngine;

namespace CucuTools.DamageSystem
{
    public abstract class DamageFactory : ScriptableObject
    {
        public abstract Damage CreateDamage();
    }
}