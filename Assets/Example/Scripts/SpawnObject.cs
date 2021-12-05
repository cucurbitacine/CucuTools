using UnityEngine;

namespace Example.Scripts
{
    public class SpawnObject : MonoBehaviour
    {
        public GameObject[] Prefabs;
        public Vector3 LocalPoint;

        public void Spawn()
        {
            var Prefab = Prefabs[Random.Range(0, Prefabs.Length)];
            var obj = Instantiate(Prefab, transform, false);
            obj.transform.position = transform.TransformPoint(LocalPoint);
            obj.transform.rotation = Random.rotationUniform;
            obj.GetComponent<Rigidbody>()?.AddTorque(Random.onUnitSphere, ForceMode.Impulse);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(transform.TransformPoint(LocalPoint), 0.2f);
        }
    }
}
