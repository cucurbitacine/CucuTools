using System;
using System.Collections;
using CucuTools;
using CucuTools.PlayerSystem2D;
using UnityEngine;

namespace Samples.Playground2D.Scripts.Tools
{
    public class DodgeController2D : ToolController2D
    {
        [Header("Dodge")]
        [Min(0)] public float dodgeDistance = 5f;
        [Min(0)] public float dodgeSpeed = 20f;

        [Space] public LayerMask obstacleLayer = default;

        private Coroutine _dodge;
        
        public void Dodge(Vector2 direction)
        {
            if (player2d.IsStatic()) return;

            if (_dodge != null) StopCoroutine(_dodge);
            _dodge = StartCoroutine(_Dodge(direction));
        }

        private IEnumerator _Dodge(Vector2 direction)
        {
            var capsule2d = player2d.capsule2d;
            var hit = Physics2D.CapsuleCast(capsule2d.bounds.center, capsule2d.size, capsule2d.direction,
                    capsule2d.attachedRigidbody.rotation, direction, dodgeDistance, obstacleLayer);

            var distance = dodgeDistance;
            
            if (hit)
            {
                distance = hit.distance;
            }

            if (distance > 0.1f)
            {
                player2d.rigidbody2d.bodyType = RigidbodyType2D.Kinematic;
                player2d.rigidbody2d.velocity = direction * dodgeSpeed;
                yield return new WaitForSeconds(distance / dodgeSpeed);
                player2d.rigidbody2d.velocity = Vector2.zero;
                player2d.rigidbody2d.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }
}