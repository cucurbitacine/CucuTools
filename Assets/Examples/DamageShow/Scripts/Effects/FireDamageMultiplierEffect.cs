using CucuTools.DamageSystem;
using CucuTools.DamageSystem.Extended.Effects.Impl;
using UnityEngine;

namespace Examples.DamageShow.Scripts.Effects
{
    [CreateAssetMenu(menuName = "Examples/Damage/Effect/"+ AssetName, fileName = AssetName, order = 0)]
    public class FireDamageMultiplierEffect : DamageMultiplierEffect
    {
        public const string AssetName = nameof(FireDamageMultiplierEffect);
        
        public override void HandleDamage(DamageEvent e)
        {
            Debug.Log($"Check damage is fire: {e.damage is FireDamage} it {e.damage.GetType().Name}");
            
            if (e.damage is FireDamage)
            {
                base.HandleDamage(e);
            }
        }
    }
}