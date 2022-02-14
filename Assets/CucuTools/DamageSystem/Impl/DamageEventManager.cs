namespace CucuTools.DamageSystem.Impl
{
    public static class DamageEventManager
    {
        public delegate void DamageEventHandle(DamageEvent e);
        
        public static event DamageEventHandle OnDamageGenerated;
        public static event DamageEventHandle OnDamageApplied;
        
        public static void WasGenerated(DamageEvent e)
        {
            OnDamageGenerated?.Invoke(e);
        }
        
        public static void WasApplied(DamageEvent e)
        {
            OnDamageApplied?.Invoke(e);
        }
    }
}