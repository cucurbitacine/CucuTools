using UnityEngine;

namespace CucuTools.PlayerSystem.Tools
{
    [RequireComponent(typeof(Rigidbody))]
    public class ConveyorRigid : MonoBehaviour
    {
        public Vector3 localDirection = Vector3.forward;
        public float speed = 1f;
        public bool inverse = false;
        
        public Rigidbody rigid => _rigid ??= GetComponent<Rigidbody>();
        public Vector3 direction =>
            rigid.transform.TransformDirection(localDirection.normalized).normalized * (inverse ? -1 : 1);
        public Vector3 velocity => direction.normalized * speed;

        private Rigidbody _rigid = null;
        
        private void Awake()
        {
            rigid.useGravity = false;
            rigid.isKinematic = true;
            rigid.interpolation = RigidbodyInterpolation.None;
            rigid.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }

        private void FixedUpdate()
        {
            rigid.velocity = velocity;
            var move = velocity * Time.fixedDeltaTime;
            rigid.position -= move;
            rigid.MovePosition(rigid.position + move);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.position + transform.up);
            Gizmos.DrawLine(transform.position + transform.up, transform.position + transform.up + velocity.normalized);
        }
    }
}
