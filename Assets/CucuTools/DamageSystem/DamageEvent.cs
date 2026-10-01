namespace CucuTools.DamageSystem
{
    public readonly struct DamageEvent
    {
        public readonly DamageAgent Sender;
        public readonly DamageAgent Receiver;
        public readonly DamageData Damage;
        
        public DamageEvent(DamageAgent sender, DamageAgent receiver, DamageData damage)
        {
            Sender = sender;
            Receiver = receiver;
            Damage = damage;
        }
        
        public DamageEvent(DamageProcess process) : this(process.Sender, process.Receiver, process.TotalDamage)
        {
        }
    }
}