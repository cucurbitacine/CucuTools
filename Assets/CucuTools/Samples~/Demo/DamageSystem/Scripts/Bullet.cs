using CucuTools.DamageSystem.Utils;
using UnityEngine;

namespace Samples.Demo.DamageSystem.Scripts
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speedMax = 10;
        
        [field: SerializeField] public DamageBox DamageBox { get; private set; }
        
        private Rigidbody2D rigid2D;

        private void Awake()
        {
            rigid2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            rigid2D.velocity = transform.up * speedMax;
        }
    }
}