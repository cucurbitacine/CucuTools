using CucuTools.DamageSystem;

namespace Examples.DamageSystem
{
    public class HeadShotDamageReceiver : DamageReceiver
    {
        protected override void HandleDamage(DamageInfo info)
        {
            info.damage.amount *= 2;
        }
    }
}