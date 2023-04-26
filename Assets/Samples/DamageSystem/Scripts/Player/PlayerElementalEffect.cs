using UnityEngine;

namespace Samples.DamageSystem.Scripts.Player
{
    public class PlayerElementalEffect : MonoBehaviour
    {
        public PlayerController player = null;
        
        [Space]
        [Min(0)] public float factor = 2;
        
        private ElementalDamageEffect advantageEffect = null;
        private ElementalDamageEffect disadvantageEffect = null;

        private void Awake()
        {
            advantageEffect = ScriptableObject.CreateInstance<ElementalDamageEffect>();
            disadvantageEffect = ScriptableObject.CreateInstance<ElementalDamageEffect>();

            advantageEffect.name = nameof(advantageEffect);
            disadvantageEffect.name = nameof(disadvantageEffect);
        }
        
        private void OnEnable()
        {
            player.damageManager.receiverEffectManager.AddEffect(advantageEffect);
            player.damageManager.receiverEffectManager.AddEffect(disadvantageEffect);
        }

        private void Update()
        {
            var elemental = player.elementalSelf;
            
            advantageEffect.elemental = ElementalDamage.GetAdvantage(elemental);
            advantageEffect.factor = 1 / factor;
            
            disadvantageEffect.elemental = ElementalDamage.GetDisadvantage(elemental);
            disadvantageEffect.factor = factor;
        }

        private void OnDisable()
        {
            player.damageManager.receiverEffectManager.RemoveEffect(advantageEffect);
            player.damageManager.receiverEffectManager.RemoveEffect(disadvantageEffect);
        }
    }
}