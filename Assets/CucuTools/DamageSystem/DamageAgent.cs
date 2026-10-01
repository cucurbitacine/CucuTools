using System;
using UnityEngine;

namespace CucuTools.DamageSystem
{
    [DisallowMultipleComponent]
    public class DamageAgent : MonoBehaviour
    {
        [field: SerializeField] public bool AllowHitSelf { get; private set; }

        [field: Space]
        [field: Tooltip("Can Be Null")]
        [field: SerializeField] public DamageAttackHandler AttackHandler { get; private set; }
        [field: Tooltip("Can Be Null")]
        [field: SerializeField] public DamageHitHandler HitHandler { get; private set; }
        
        public event Action<DamageEvent> DamageSent;
        public event Action<DamageEvent> DamageReceived;

        public void ChangeAttackHandler(DamageAttackHandler attackHandler)
        {
            AttackHandler = attackHandler;
        }
        
        public void ChangeHitHandler(DamageHitHandler hitHandler)
        {
            HitHandler = hitHandler;
        }
        
        public void OnDamageSent(DamageEvent request)
        {
            DamageSent?.Invoke(request);
        }
        
        public void OnDamageReceived(DamageEvent request)
        {
            DamageReceived?.Invoke(request);
        }
    }
}
