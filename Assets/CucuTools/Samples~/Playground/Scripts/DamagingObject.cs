using System.Collections;
using CucuTools.Attributes;
using CucuTools.DamageSystem;
using CucuTools.DamageSystem.Impl;
using CucuTools.Others;
using UnityEngine;

namespace Examples.Playground.Scripts
{
    public class DamagingObject : DamageSourceFactory
    {
        [Space]
        public bool isEnabled = true;
        public float damageTimeout = 1f;

        [Space]
        public CucuCollision collision = null;

        private Coroutine _damageTimeout = null;
        
        private IEnumerator _DamageTimeout()
        {
            yield return new WaitForSeconds(damageTimeout);
            isEnabled = true;
        }

        [DrawButton()]
        private void Timeout()
        {
            isEnabled = false;

            if (_damageTimeout != null) StopCoroutine(_damageTimeout);
            _damageTimeout = StartCoroutine(_DamageTimeout());
        }
        
        private void Damage(Collision cls)
        {
            if (isEnabled && cls.collider.TryGetComponent<DamageReceiver>(out var dr))
            {
                SendDamage(dr);

                Timeout();
            }
        }

        private void Awake()
        {
            if (collision == null) collision = GetComponent<CucuCollision>();
        }

        private void OnEnable()
        {
            if (collision != null) collision.onCollisionChanged.AddListener(Damage);
        }
        
        private void OnDisable()
        {
            if (collision != null) collision.onCollisionChanged.AddListener(Damage);
        }
    }
}
