using System;
using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.DamageSystem.Utils
{
    public sealed class DamageBox : MonoBehaviour
    {
        [SerializeField] private DamageSource damageSource;
        
        [Header("Damage Box")]
        [SerializeField] private bool isTrigger = false;
        [SerializeField] private bool ownerDamage = false;
        [SerializeField] private LayerMask layerMask = 1;
        
        [Space]
        [SerializeField] private bool damageOnEnter = true;
        [SerializeField] private bool damageOnStay = false;
        
        [Space]
        [Min(0)]
        [SerializeField] private float damageRate = 1f;
        
        private float damageTimout => damageRate > 0f ? 1f / damageRate : float.MaxValue;

        private readonly Dictionary<DamageReceiver, float> timeLastDamageDict = new Dictionary<DamageReceiver, float>();

        public DamageSource DamageSource => damageSource ? damageSource : (damageSource = GetComponentInParent<DamageSource>());
        public event Action<DamageSource> OnDamageSourceChanged;
        
        public void SetDamageSource(DamageSource newDamageSource)
        {
            damageSource = newDamageSource;
            
            OnDamageSourceChanged?.Invoke(damageSource);
        }
        
        private void SendDamage(DamageReceiver receiver, bool forced = false)
        {
            if (DamageSource == null)
            {
                Debug.LogWarning($"\"{name} ({GetType().Name})\" doesn't have \"{nameof(DamageSource)}\"");
                return;
            }
            
            if (!ownerDamage && DamageSource.Owner == receiver.Owner) return;

            if (!timeLastDamageDict.TryGetValue(receiver, out var timeLastDamage))
            {
                timeLastDamage = -1f;
                timeLastDamageDict.Add(receiver, timeLastDamage);
            }

            var timeNow = Time.time;
            var timeSinceLastDamage = timeNow - timeLastDamage;

            if (!forced && 0f <= timeLastDamage && timeSinceLastDamage <= damageTimout) return;

            timeLastDamageDict[receiver] = timeNow;
            
            var damage = DamageSource.CreateDamage(receiver);
            
            DamageSource.SendDamage(damage, receiver);
        }

        private void SendDamage2D(Collider2D cld2d, bool forced = false)
        {
            if (!cld2d.gameObject.CompareLayer(layerMask)) return;
            
            if (!cld2d.TryGetComponent<DamageReceiver>(out var receiver))
            {
                if (!cld2d.attachedRigidbody || !cld2d.attachedRigidbody.TryGetComponent(out receiver))
                {
                    return;
                }
            }

            SendDamage(receiver, forced);
        }
        
        private void SendDamage3D(Collider cld3d, bool forced = false)
        {
            if (!cld3d.gameObject.CompareLayer(layerMask)) return;
            
            if (!cld3d.TryGetComponent<DamageReceiver>(out var receiver))
            {
                if (!cld3d.attachedRigidbody || !cld3d.attachedRigidbody.TryGetComponent(out receiver))
                {
                    return;
                }
            }

            SendDamage(receiver, forced);
        }
        
        #region Collision && Trigger 2D
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!isTrigger && damageOnEnter)
            {
                SendDamage2D(other.collider, true);
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (!isTrigger && damageOnStay)
            {
                SendDamage2D(other.collider);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isTrigger && damageOnEnter)
            {
                SendDamage2D(other, true);
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (isTrigger && damageOnStay)
            {
                SendDamage2D(other);
            }
        }

        #endregion

        #region Collision && Trigger 3D
        
        private void OnCollisionEnter(Collision other)
        {
            if (!isTrigger && damageOnEnter)
            {
                SendDamage3D(other.collider, true);
            }
        }

        private void OnCollisionStay(Collision other)
        {
            if (!isTrigger && damageOnStay)
            {
                SendDamage3D(other.collider);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (isTrigger && damageOnEnter)
            {
                SendDamage3D(other, true);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (isTrigger && damageOnStay)
            {
                SendDamage3D(other);
            }
        }

        #endregion
    }
}