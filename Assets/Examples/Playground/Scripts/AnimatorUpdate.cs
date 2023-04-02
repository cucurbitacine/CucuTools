using System;
using CucuTools.PlayerSystem;
using PlasticPipe.PlasticProtocol.Messages;
using UnityEngine;

namespace Examples.Playground.Scripts
{
    public class AnimatorUpdate : MonoBehaviour
    {
        public RigidPersonController person;
        public Animator animator;
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int FreeFall = Animator.StringToHash("FreeFall");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Speed = Animator.StringToHash("Speed");

        public float fallTimeout = 0.2f;
        public float speedChangeRate = 8f;
        
        private float _speed = 0f;
        private float _fallTimeoutDelta = 0f;
        
        private void LateUpdate()
        {
            if (person.info.falling)
            {
                if (_fallTimeoutDelta > 0)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    animator.SetBool(FreeFall, person.info.falling);
                }
            }
            else
            {
                _fallTimeoutDelta = fallTimeout;
                animator.SetBool(FreeFall, person.info.falling);
            }
            
            animator.SetBool(Grounded, person.ground.grounded);
            animator.SetBool(Jump, person.info.jumping);

            var speed = person.info.moving ? (person.settings.moveSpeed > person.settings.moveSpeedMax ? 2 : 1f) : 0f;

            _speed = Mathf.Lerp(_speed, speed, speedChangeRate * Time.deltaTime);
            
            animator.SetFloat(Speed, _speed);
        }
    }
}
