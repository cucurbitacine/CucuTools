using UnityEngine;

namespace CucuTools.DamageSystem
{
    public abstract class DamageSource : DamageEffector
    {
        [SerializeField] private bool isEnabled = true;

        public override bool IsEnabled
        {
            get => isEnabled;
            set => isEnabled = value;
        }

        public DamageInfo GenerateDamage()
        {
            return Evaluate(GenerateClearDamage());
        }

        protected abstract DamageInfo GenerateClearDamage();
    }
}