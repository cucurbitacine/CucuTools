using System;
using CucuTools.DamageSystem;
using UnityEngine;

namespace Example.Scripts.DamageSystem.Filters
{
    public abstract class DamageEffectBehaviour : MonoBehaviour
    {
        [SerializeField] private DamageEffector effector = default;

        public DamageEffector Effector
        {
            get => effector;
            set => effector = value;
        }
        
        public abstract IDamageEffect GetEffect();

        private void Awake()
        {
            if (Effector == null) Effector = GetComponentInParent<DamageEffector>();
        }

        protected virtual void OnEnable()
        {
            Effector.AddEffect(GetEffect());
        }
        
        protected virtual void OnDisable()
        {
            Effector.RemoveEffect(GetEffect());
        }
    }
}