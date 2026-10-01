using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.DamageSystem
{
    [DisallowMultipleComponent]
    public abstract class HitBox : MonoBehaviour
    {
        private static readonly Dictionary<GameObject, HitBox> Cache = new Dictionary<GameObject, HitBox>();
        
        [field: SerializeField] public DamageAgent Owner { get; private set; }
        [field: Tooltip("Can Be Null")]
        [field: SerializeField] public DamageHitHandler HitHandler { get; private set; }
        
        public void ChangeOwner(DamageAgent owner)
        {
            Owner = owner;
        }

        public void ChangeHitHandler(DamageHitHandler hitHandler)
        {
            HitHandler = hitHandler;
        }
        
        public static bool Lookup(Component component, out HitBox hitBox)
        {
            return Cache.TryGetValue(component.gameObject, out hitBox);
        }
        
        private void OnEnable()
        {
            Cache[gameObject] = this;
        }

        private void OnDisable()
        {
            Cache.Remove(gameObject);
        }
    }
}