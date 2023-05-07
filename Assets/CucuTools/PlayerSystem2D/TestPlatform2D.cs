using System;
using UnityEngine;

namespace CucuTools.PlayerSystem2D
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class TestPlatform2D : MonoBehaviour
    {
        public float speedMax = 1;
        public float frequency = 1;
        
        private Rigidbody2D _rigidbody2d;
        
        private void Awake()
        {
            _rigidbody2d = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            var side = Mathf.Sign(Mathf.Sin(frequency * Time.fixedTime)); 
            _rigidbody2d.velocity = Vector2.right * (speedMax * side);
        }
    }
}