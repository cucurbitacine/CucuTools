using CucuTools.DamageSystem;
using UnityEngine;

namespace Example.Scripts.DamageSystem
{
    public class DamageTypeEffect : MonoBehaviour
    {
        public DamageType State = DamageType.Physical;
        
        [Space]
        public HealthBehaviour Health = default;
        
        public void ReceiveDamage(DamageEvent e)
        {
            if (State == DamageType.Physical)
            {
                State = e.damage.type;
                return;
            }
            
            if (State == DamageType.Fire)
            {
                if (e.damage.type == DamageType.Water)
                {
                    State = DamageType.Physical;
                    return;
                }
                
                State = DamageType.Fire;
                return;
            }
            
            if (State == DamageType.Water)
            {
                if (e.damage.type == DamageType.Fire)
                {
                    State = DamageType.Physical;
                    return;
                }
                
                if (e.damage.type == DamageType.Lightning)
                {
                    var dmg = new DamageInfo()
                    {
                        amount = Mathf.CeilToInt( e.damage.amount * 0.5f),
                        type = DamageType.Lightning,
                    };
                    var actual = new DamageEvent(dmg, e);
                    Health.ReceiveDamage(actual);
                    return;
                }
                
                State = DamageType.Water;
                return;
            }
        }
    }
}