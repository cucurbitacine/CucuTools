using UnityEngine;

namespace CucuTools.DamageSystem.Impl
{
    [RequireComponent(typeof(Collider))]
    public class DamageBox3D : DamageBox
    {
        private void OnTriggerEnter(Collider other) => HandleTarget(other.gameObject, HitType.Trigger, HitMode.Enter);
        private void OnTriggerStay(Collider other) => HandleTarget(other.gameObject, HitType.Trigger, HitMode.Stay);
        private void OnTriggerExit(Collider other) => HandleTarget(other.gameObject, HitType.Trigger, HitMode.Exit);

        private void OnCollisionEnter(Collision other) => HandleTarget(other.gameObject, HitType.Collision, HitMode.Enter);
        private void OnCollisionStay(Collision other) => HandleTarget(other.gameObject, HitType.Collision, HitMode.Stay);
        private void OnCollisionExit(Collision other) => HandleTarget(other.gameObject, HitType.Collision, HitMode.Exit);
    }
}