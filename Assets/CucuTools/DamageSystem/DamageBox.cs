using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.DamageSystem
{
    /// <summary>
    /// Damage Box which interacting with <see cref="HitBox"/> as Source of damage
    /// <seealso cref="DamageSource"/>
    /// <seealso cref="HitBox"/>
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class DamageBox : CucuBehaviour
    {
        public bool paused = false;
        
        [Space]
        public LayerMask targetLayerMask = 0;
        public HitType hitType = HitType.TriggerOrCollision;
        public HitMode hitMode = HitMode.Stay;
        [Min(0)] public float hitTimeout = 0.5f;
        
        [Space]
        public DamageSource source;

        private readonly Dictionary<HitBox, float> timeouts = new Dictionary<HitBox, float>();
        
        public void Hit(HitBox hitBox)
        {
            if (!timeouts.TryGetValue(hitBox, out var timeLastHit))
            {
                timeLastHit = 0f;
                timeouts.Add(hitBox, timeLastHit);
            }
            
            var timeNow = Time.time;
            var timeSinceLastHit = timeNow - timeLastHit;
            
            if (hitTimeout <= timeSinceLastHit)
            {
                timeouts[hitBox] = timeNow;

                source.SendDamage(hitBox.receiver);
            }
        }

        protected void HandleTarget(GameObject target, HitType type, HitMode mode)
        {
            if (!paused && IsValidTarget(target, type, mode))
            {
                var hitBox = target.GetComponent<HitBox>();

                if (IsValidHitBox(hitBox))
                {
                    Hit(hitBox);
                }
            }
        }

        private bool IsValidHitBox(HitBox hitBox)
        {
            if (hitBox == null) return false;
            
            if (hitBox.receiver == null) return false;
            
            if (hitBox.paused) return false;
            
            if (hitBox.ignoreSelf)
            {
                if (source.manager && hitBox.receiver.manager)
                {
                    if (source.manager == hitBox.receiver.manager)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        
        private bool IsValidTarget(GameObject target, HitType type, HitMode mode)
        {
            return (targetLayerMask.value & (1 << target.layer)) > 0 && (hitType == type || hitType == HitType.TriggerOrCollision) && hitMode == mode;
        }
        
        protected virtual void OnValidate()
        {
            if (source == null) source = GetComponent<DamageSource>();
        }
    }

    public enum HitType
    {
        Trigger,
        Collision,
        TriggerOrCollision,
    }
    
    public enum HitMode
    {
        Enter,
        Stay,
        Exit,
    }
}