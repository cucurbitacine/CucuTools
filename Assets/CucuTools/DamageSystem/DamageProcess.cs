using UnityEngine.Pool;

namespace CucuTools.DamageSystem
{
    public sealed class DamageProcess
    {
        public DamageAgent Sender { get; private set; }
        public DamageAgent Receiver { get; private set; }
        public DamageData BaseDamage { get; private set; }
        public DamageData TotalDamage { get; private set; }

        private static readonly ObjectPool<DamageProcess> Pool = new(
            createFunc: () => new DamageProcess(),
            actionOnGet: null,
            actionOnRelease: process => process.Reset(),
            actionOnDestroy: null,
            collectionCheck: false,
            defaultCapacity: 32,
            maxSize: 256
        );

        private DamageProcess()
        {
        }
        
        public static DamageProcess Get(DamageAgent sender, DamageAgent receiver, DamageData damage)
        {
            var process = Pool.Get();
            process.Initialize(sender, receiver, damage);
            return process;
        }
        
        public static void Release(DamageProcess process)
        {
            Pool.Release(process);
        }
        
        private void Initialize(DamageAgent sender, DamageAgent receiver, DamageData damage)
        {
            Sender = sender;
            Receiver = receiver;
            BaseDamage = damage;
            TotalDamage = damage.Copy();
        }
        
        private void Reset()
        {
            Sender = null;
            Receiver = null;
            BaseDamage = null;
            TotalDamage = null;
        }
    }
}