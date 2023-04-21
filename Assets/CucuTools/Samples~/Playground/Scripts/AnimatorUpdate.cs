using CucuTools.PlayerSystem;
using UnityEngine;

namespace Examples.Playground.Scripts
{
    public class AnimatorUpdate : MonoBehaviour
    {
        public PlayerController player;
        public Animator animator;
        public AudioSource audioSource;
        
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int FreeFall = Animator.StringToHash("FreeFall");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Speed = Animator.StringToHash("Speed");

        public float fallTimeout = 0.2f;
        public float speedChangeRate = 8f;

        [Space]
        public AudioClip landSfx;
        public AudioClip[] footstepSfx;
        
        private float _speed = 0f;
        private float _fallTimeoutDelta = 0f;

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (footstepSfx.Length > 0)
                {
                    var index = Random.Range(0, footstepSfx.Length);
                    audioSource.PlayOneShot(footstepSfx[index]);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                audioSource.PlayOneShot(landSfx);
            }
        }
        
        private void LateUpdate()
        {
            if (player.info.falling)
            {
                if (_fallTimeoutDelta > 0)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    animator.SetBool(FreeFall, player.info.falling);
                }
            }
            else
            {
                _fallTimeoutDelta = fallTimeout;
                animator.SetBool(FreeFall, player.info.falling);
            }
            
            animator.SetBool(Grounded, player.ground.grounded);
            animator.SetBool(Jump, player.info.jumping);

            var speed = player.info.moving ? (player.settings.moveSpeed > player.settings.moveSpeedMax ? 2 : 1f) : 0f;

            _speed = Mathf.Lerp(_speed, speed, speedChangeRate * Time.deltaTime);
            
            animator.SetFloat(Speed, _speed);
        }
    }
}
