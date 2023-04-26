using CucuTools.DamageSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Samples.DamageSystem.Scripts
{
    public class ElementalDamageHandler : MonoBehaviour
    {
        public Elemental elementalSelf = Elemental.Fire;
        
        [Space] public UnityEvent<int> onHealed = new UnityEvent<int>();
        [Space] public UnityEvent<int> onDamaged = new UnityEvent<int>();

        public void DamageReceive(DamageEvent e)
        {
            if (e.damage is ElementalDamage damage)
            {
                if (elementalSelf == damage.elemental)
                {
                    onHealed.Invoke(damage.amount);
                }
                else 
                {
                    onDamaged.Invoke(damage.amount);
                }
            }
        }
    }
}