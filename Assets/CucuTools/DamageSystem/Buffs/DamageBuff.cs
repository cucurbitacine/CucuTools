namespace CucuTools.DamageSystem.Buffs
{
    public abstract class DamageBuff
    {
        public abstract void HandleDamage(DamageEvent e);

        public virtual void Start()
        {
        }

        public virtual void Stop()
        {
        }
    }
}