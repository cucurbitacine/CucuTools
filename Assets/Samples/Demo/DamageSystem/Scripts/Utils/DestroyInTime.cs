using UnityEngine;

namespace Samples.Demo.DamageSystem.Scripts.Utils
{
    [DisallowMultipleComponent]
    public class DestroyInTime : MonoBehaviour
    {
        [SerializeField] private float lifetime = 1f;

        private void Die()
        {
            Destroy(gameObject);
        }
        
        private void Start()
        {
            Invoke(nameof(Die), lifetime);
        }
    }
}