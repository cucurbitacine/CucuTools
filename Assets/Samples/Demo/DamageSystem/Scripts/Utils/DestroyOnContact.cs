using UnityEngine;

namespace Samples.Demo.DamageSystem.Scripts.Utils
{
    [DisallowMultipleComponent]
    public class DestroyOnContact : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            Destroy(gameObject);
        }
    }
}