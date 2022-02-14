using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.DamageSystem.Impl
{
    public abstract class HitBox : MonoBehaviour
    {
        public enum HitMode
        {
            Trigger,
            Collision,
        }
        
        [SerializeField] private bool isEnabled = true;
        [SerializeField] private HitMode mode = HitMode.Trigger;
        
        [Space]
        [SerializeField] private LayerMask hitMask = 1;
        
        [Space]
        [SerializeField] private UnityEvent<DamageEvent> onDamageReceived = default;
        
        [Space]
        [SerializeField] private DamageReceiver receiver = default;

        public bool IsEnabled
        {
            get => isEnabled;
            set => isEnabled = value;
        }

        public HitMode Mode
        {
            get => mode;
            set => mode = value;
        }

        public LayerMask HitMask
        {
            get => hitMask;
            set => hitMask = value;
        }

        public UnityEvent<DamageEvent> OnDamageReceived => onDamageReceived != null
            ? onDamageReceived
            : (onDamageReceived = new UnityEvent<DamageEvent>());
        
        public DamageReceiver Receiver
        {
            get => receiver;
            set => receiver = value;
        }

        public void Hit(DamageBox box)
        {
            if (!IsEnabled || !Receiver.IsEnabled) return;

            if (!HitMask.Contains(box.gameObject.layer)) return;
            
            if (box.IsEnabled) ReceiveDamage(GenerateDamageEvent(box.Source));
        }

        public void Hit(DamageSource source)
        {
            if (!IsEnabled || !Receiver.IsEnabled) return;
            
            if (!HitMask.Contains(source.gameObject.layer)) return;
            
            if (source.IsEnabled) ReceiveDamage(GenerateDamageEvent(source));
        }

        public void Hit(params DamageSource[] sources)
        {
            foreach (var source in sources)
            {
                Hit(source);
            }
        }

        public void Hit(params DamageBox[] boxes)
        {
            foreach (var box in boxes)
            {
                Hit(box);
            }
        }
        
        private DamageEvent GenerateDamageEvent(DamageSource source)
        {
            var dmg = source.GenerateDamage();
            var cmd = new DamageEvent(dmg, Receiver, source);
            
            DamageEventManager.WasGenerated(cmd);

            return cmd;
        }
        
        private void ReceiveDamage(DamageEvent e)
        {
            OnDamageReceived.Invoke(e);
            
            Receiver.ReceiveDamage(e);
        }
    }
}