using CucuTools.DamageSystem;
using UnityEngine;

namespace Examples.DamageSystem
{
    public class ArmShotDamageReceiver : DamageReceiver
    {
        protected override void HandleDamage(DamageInfo info)
        {
            info.damage.amount =  Mathf.FloorToInt(info.damage.amount * 0.5f);
        }
    }
}