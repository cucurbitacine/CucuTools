using UnityEngine;

namespace CucuTools.DamageSystem.Buffs
{
    public abstract class DamageBuffFactory : ScriptableObject
    {
        public const string BuffGroup = "Buff/";
        public const string CreateEffectMenu = Cucu.CreateAsset + Cucu.DamageGroup + BuffGroup;

        public abstract DamageBuff CreateBuff();
    }
}