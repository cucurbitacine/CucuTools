using System;
using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.DamageSystem.Impl
{
    [DisallowMultipleComponent]
    public abstract class DamageBox : MonoBehaviour
    {
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

        protected void Hit(GameObject target)
        {
            var hitBox = target.GetComponent<HitBox>();

            if (hitBox)
            {
                Hit(hitBox);
            }
        }
        
        protected void Handle(GameObject target, HitType type, HitMode mode)
        {
            if (CheckTarget(target, type, mode))
            {
                Hit(target);
            }
        }

        private bool CheckTarget(GameObject target, HitType type, HitMode mode)
        {
            return CheckLayer(targetLayerMask, target.layer) && (hitType == type || hitType == HitType.TriggerOrCollision) && hitMode == mode;
        }
        
        private static bool CheckLayer(LayerMask layerMask, int layerNumber)
        {
            return (layerMask.value & (1 << layerNumber)) > 0;
        }

        protected void OnValidate()
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