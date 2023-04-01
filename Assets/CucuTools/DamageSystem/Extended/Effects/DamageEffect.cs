using UnityEngine;

namespace CucuTools.DamageSystem.Extended.Effects
{
    public abstract class DamageEffect : ScriptableObject
    {
        public const string EffectGroup = "Effect/";
        public const string CreateEffect = Cucu.CreateAsset + Cucu.DamageGroup + EffectGroup;
        
        public abstract void HandleDamage(DamageEvent e);
    }
}