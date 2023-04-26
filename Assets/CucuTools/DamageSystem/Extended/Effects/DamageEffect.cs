using UnityEngine;

namespace CucuTools.DamageSystem.Extended.Effects
{
    public abstract class DamageEffect : ScriptableObject
    {
        public const string EffectGroup = "Effect/";
        public const string CreateEffect = Cucu.CreateAsset + Cucu.DamageGroup + EffectGroup;
        
        public abstract void HandleDamage(DamageEvent e);

        public DamageEffect Copy()
        {
            return GetCopy(this);
        }
        
        public static DamageEffect GetCopy(DamageEffect effect)
        {
            var type = effect.GetType();
            
            var output = (DamageEffect)CreateInstance(type);
            output.name = type.Name;

            var json = JsonUtility.ToJson(effect);
            JsonUtility.FromJsonOverwrite(json, output);

            return output;
        }
    }
}