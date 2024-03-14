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
        #region SerializeField

        public bool paused = false;
        
        [Space]
        public LayerMask targetLayerMask = 0;
        public HitType hitType = HitType.TriggerOrCollision;
        public HitMode hitMode = HitMode.Stay;
        [Min(0)] public float hitTimeout = 0.5f;
        
        [Space]
        public DamageSource source;

        #endregion

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

                SendDamage(hitBox);
            }
        }

        #region Virtual API

        protected virtual bool IsValidTargetInternal(GameObject target)
        {
            return true;
        }
        
        protected virtual bool IsValidHitBoxInternal(HitBox hitBox)
        {
            return true;
        }

        protected virtual void SendDamage(HitBox hitBox)
        {
            source.SendDamage(hitBox.receiver);
        }

        #endregion

        #region Protected & Private API

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
        
        private bool IsValidTarget(GameObject target, HitType type, HitMode mode)
        {
            return (targetLayerMask.value & (1 << target.layer)) > 0 &&
                   (hitType == HitType.TriggerOrCollision || hitType == type) &&
                   hitMode == mode && 
                   IsValidTargetInternal(target);
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

            return IsValidHitBoxInternal(hitBox);
        }

        #endregion
        
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