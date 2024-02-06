using UnityEngine;

namespace CucuTools.DamageSystem.Impl
{
    [RequireComponent(typeof(Collider))]
    public class DamageBox3D : DamageBox
    {
        protected virtual void OnTriggerEnter(Collider other) => HandleTarget(other.gameObject, HitType.Trigger, HitMode.Enter);
        protected virtual void OnTriggerStay(Collider other) => HandleTarget(other.gameObject, HitType.Trigger, HitMode.Stay);
        protected virtual void OnTriggerExit(Collider other) => HandleTarget(other.gameObject, HitType.Trigger, HitMode.Exit);

        protected virtual void OnCollisionEnter(Collision other) => HandleTarget(other.gameObject, HitType.Collision, HitMode.Enter);
        protected virtual void OnCollisionStay(Collision other) => HandleTarget(other.gameObject, HitType.Collision, HitMode.Stay);
        protected virtual void OnCollisionExit(Collision other) => HandleTarget(other.gameObject, HitType.Collision, HitMode.Exit);
    }
}