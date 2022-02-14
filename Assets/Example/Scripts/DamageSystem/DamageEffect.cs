using CucuTools.DamageSystem;
using UnityEngine;

namespace Example.Scripts.DamageSystem
{
    public class DamageEffect : MonoBehaviour
    {
        public DamageType State = DamageType.Physical;
        
        [Space]
        public HealthBehaviour Health = default;
        
        public void ReceiveDamage(DamageCommand cmd)
        {
            if (State == DamageType.Physical)
            {
                State = cmd.damage.type;
                return;
            }
            
            if (State == DamageType.Fire)
            {
                if (cmd.damage.type == DamageType.Water)
                {
                    State = DamageType.Physical;
                    return;
                }
                
                State = DamageType.Fire;
                return;
            }
            
            if (State == DamageType.Water)
            {
                if (cmd.damage.type == DamageType.Fire)
                {
                    State = DamageType.Physical;
                    return;
                }
                
                if (cmd.damage.type == DamageType.Lightning)
                {
                    var dmg = new DamageInfo()
                    {
                        amount = Mathf.CeilToInt( cmd.damage.amount * 0.5f),
                        type = DamageType.Lightning,
                    };
                    var actual = new DamageCommand(dmg, cmd);
                    Health.ApplyDamage(actual);
                    return;
                }
                
                State = DamageType.Water;
                return;
            }
        }
    }
}