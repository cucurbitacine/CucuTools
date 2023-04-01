using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.DamageSystem.Extended.Effects.Impl
{
    [CreateAssetMenu(menuName = CreateEffect + AssetName, fileName = AssetName, order = 0)]
    public class CriticalDamageHandlerEffect : DamageEffect
    {
        public const string AssetName = nameof(CriticalDamageHandlerEffect);
        
        public UnityEvent<DamageEvent> onCriticalDamaged = new UnityEvent<DamageEvent>();

        public override void HandleDamage(DamageEvent e)
        {
            if (e.damage.critical) onCriticalDamaged.Invoke(e);
        }
    }
}
