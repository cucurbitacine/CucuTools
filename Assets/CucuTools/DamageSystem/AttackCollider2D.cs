using UnityEngine;

namespace CucuTools.DamageSystem
{
    [RequireComponent(typeof(Collider2D))]
    public class AttackCollider2D : AttackCollider
    {
        private Collider2D _collider;

        public Collider2D GetCollider()
        {
            return _collider;
        }
        
        public override DamageData BuildDamage(HitBox hitBox, Vector3 point, Vector3 normal)
        {
            var damage = DamageAsset.Generate(Owner, gameObject, hitBox.Owner, hitBox.gameObject);
            
            damage.AddPayload(new HitPointInfo(point, normal));

            return damage;
        }

        protected virtual void OnCollisionEnter2D(Collision2D other)
        {
            if (AttackColliderType == AttackColliderType.Collision)
            {
                var contact = other.GetContact(0);
                DamagePipeline.AttackTargetAtPoint(this, contact.otherCollider, contact.point, contact.normal);
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (AttackColliderType == AttackColliderType.Trigger)
            {
                DamagePipeline.AttackTarget(this, other);
            }
        }
        
        protected virtual void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }
        
        protected virtual void Start()
        {
            GetCollider().isTrigger = AttackColliderType == AttackColliderType.Trigger;
        }
    }
}