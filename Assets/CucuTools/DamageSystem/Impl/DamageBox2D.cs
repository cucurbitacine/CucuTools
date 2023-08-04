using UnityEngine;

namespace CucuTools.DamageSystem.Impl
{
    [RequireComponent(typeof(Collider2D))]
    public class DamageBox2D : DamageBox
    {
        private void OnTriggerEnter2D(Collider2D other) => HandleTarget(other.gameObject, HitType.Trigger, HitMode.Enter);
        private void OnTriggerStay2D(Collider2D other) => HandleTarget(other.gameObject, HitType.Trigger, HitMode.Stay);
        private void OnTriggerExit2D(Collider2D other) => HandleTarget(other.gameObject, HitType.Trigger, HitMode.Exit);

        private void OnCollisionEnter2D(Collision2D other) => HandleTarget(other.gameObject, HitType.Collision, HitMode.Enter);
        private void OnCollisionStay2D(Collision2D other) => HandleTarget(other.gameObject, HitType.Collision, HitMode.Stay);
        private void OnCollisionExit2D(Collision2D other) => HandleTarget(other.gameObject, HitType.Collision, HitMode.Exit);
    }
}