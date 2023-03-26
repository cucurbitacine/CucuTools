using CucuTools.DamageSystem;
using UnityEngine;

namespace Examples.DamageShow.Scripts.Receivers
{
    public class ArmShotDamageReceiver : DamageReceiver
    {
        protected override void HandleDamage(DamageEvent e)
        {
            e.damage.amount = Mathf.FloorToInt(e.damage.amount * 0.5f);
        }
    }
}