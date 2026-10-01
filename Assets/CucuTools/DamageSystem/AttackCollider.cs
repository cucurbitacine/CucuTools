using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace CucuTools.DamageSystem
{
    [DisallowMultipleComponent]
    public abstract class AttackCollider : MonoBehaviour
    {
        [field: SerializeField] public DamageAgent Owner { get; private set; }
        [field: Tooltip("Can Be Null")]
        [field: SerializeField] public DamageAttackHandler AttackHandler { get; private set; }
        [field: SerializeField] public AttackColliderType AttackColliderType { get; private set; }
        
        [field: Space]
        [field: SerializeField] public DamageAsset DamageAsset { get; private set; }
        [field: Min(0f)]
        [field: SerializeField] public float TargetCooldown  { get; protected set; } = 0.2f;
        [field: SerializeField] public bool AllowHitSelf { get; private set; }
        
        private readonly Dictionary<DamageAgent, float> timeNextAttackPerTarget = new Dictionary<DamageAgent, float>();
        
        public void ChangeOwner(DamageAgent owner)
        {
            Owner = owner;
        }

        public void ChangeAttackHandler(DamageAttackHandler attackHandler)
        {
            AttackHandler = attackHandler;
        }
        
        public abstract DamageData BuildDamage(HitBox hitBox, Vector3 point, Vector3 normal);
        
        public void OnHit(DamageAgent target)
        {
            CleanDeadRefs(this);
            
            UpdateCooldown(target);
        }
        
        public float GetCooldown(DamageAgent target)
        {
            if (timeNextAttackPerTarget.TryGetValue(target, out var nextTimeAttack))
            {
                return nextTimeAttack - Time.time;
            }

            return -1f;
        }

        public void UpdateCooldown(DamageAgent target)
        {
            timeNextAttackPerTarget[target] = Time.time + TargetCooldown;
        }
        
        private static void CleanDeadRefs(AttackCollider attackCollider)
        {
            var dict = attackCollider.timeNextAttackPerTarget;
            
            if (dict.Count == 0) return;
            
            var toRemove = ListPool<DamageAgent>.Get();

            foreach (var pair in dict)
            {
                if (IsDeadRefOfTarget(pair.Key))
                {
                    toRemove.Add(pair.Key);
                }
            }

            foreach (var target in toRemove)
            {
                dict.Remove(target);
            }
            
            ListPool<DamageAgent>.Release(toRemove);
        }
        
        private static bool IsDeadRefOfTarget(DamageAgent target)
        {
            return target == null || target.Equals(null) || !target.gameObject.activeInHierarchy;
        }
    }

    public enum AttackColliderType
    {
        Collision,
        Trigger,
    }
}