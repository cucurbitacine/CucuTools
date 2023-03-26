using CucuTools.DamageSystem;

namespace Examples.DamageShow.Scripts.Receivers
{
    public class HeadShotDamageReceiver : DamageReceiver
    {
        protected override void HandleDamage(DamageEvent e)
        {
            e.damage.amount *= 2;
        }
    }
}