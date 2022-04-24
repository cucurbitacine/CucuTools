using UnityEngine;

namespace CucuTools.DamageSystem.Impl
{
    /// <inheritdoc />
    [RequireComponent(typeof(Collider2D))]
    public class DamageBox2D : DamageBox
    {
        public Collider2D Collider { get; private set; }
        
        private void Awake()
        {
            if (Source == null) Source = GetComponentInParent<DamageSource>();
            if (Source == null) Debug.LogError($"[{name}] did not have {nameof(DamageSource)}");

            Collider = GetComponent<Collider2D>();
            if (Collider == null) Debug.LogError($"[{name}] did not have any {nameof(Collider2D)}");
        }
    }
}