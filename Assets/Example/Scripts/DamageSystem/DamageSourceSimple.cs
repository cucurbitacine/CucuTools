using CucuTools.DamageSystem;

namespace Example.Scripts.DamageSystem
{
    public class DamageSourceSimple : DamageSource
    {
        public DamageInfo DamageInfo;

        protected override DamageInfo GenerateClearDamage()
        {
            return DamageInfo;
        }
    }
}