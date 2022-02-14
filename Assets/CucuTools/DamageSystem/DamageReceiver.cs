using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.DamageSystem
{
    [DisallowMultipleComponent]
    public class DamageReceiver : DamageEffector
    {
        [SerializeField] private bool isEnabled = true;
        [Space]
        [SerializeField] private UnityEvent<DamageEvent> onDamageReceived;

        public override bool IsEnabled
        {
            get => isEnabled;
            set => isEnabled = value;
        }

        public UnityEvent<DamageEvent> OnDamageReceived => onDamageReceived != null
            ? onDamageReceived
            : (onDamageReceived = new UnityEvent<DamageEvent>());

        public void ReceiveDamage(DamageEvent e)
        {
            if (e.receiver == this)
            {
                OnDamageReceived.Invoke(new DamageEvent(Evaluate(e.damage), e));
            }
        }
    }
}