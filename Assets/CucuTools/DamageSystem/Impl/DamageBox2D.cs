using UnityEngine;

namespace CucuTools.DamageSystem.Impl
{
    [RequireComponent(typeof(Collider2D))]
    public class DamageBox2D : DamageBox
    {
        protected virtual void OnTriggerEnter2D(Collider2D other) => HandleTarget(other.gameObject, HitType.Trigger, HitMode.Enter);
        protected virtual void OnTriggerStay2D(Collider2D other) => HandleTarget(other.gameObject, HitType.Trigger, HitMode.Stay);
        protected virtual void OnTriggerExit2D(Collider2D other) => HandleTarget(other.gameObject, HitType.Trigger, HitMode.Exit);

        protected virtual void OnCollisionEnter2D(Collision2D other) => HandleTarget(other.gameObject, HitType.Collision, HitMode.Enter);
        protected virtual void OnCollisionStay2D(Collision2D other) => HandleTarget(other.gameObject, HitType.Collision, HitMode.Stay);
        protected virtual void OnCollisionExit2D(Collision2D other) => HandleTarget(other.gameObject, HitType.Collision, HitMode.Exit);
    }
}