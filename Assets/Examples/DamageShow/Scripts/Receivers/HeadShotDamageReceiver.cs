using CucuTools.DamageSystem;

namespace Examples.DamageShow.Scripts.Receivers
{
    public class HeadShotDamageReceiver : DamageReceiver
    {
        protected override void HandleDamage(DamageInfo info)
        {
            info.damage.amount *= 2;
        }
    }
}