using System.Collections;
using CucuTools.DamageSystem;
using CucuTools.PlayerSystem;
using UnityEngine;

namespace Examples.DamageShow.Scripts
{
    public class ZombieInput : PlayerInput<PlayerRigidController>
    {
        [Space]
        public PlayerRigidController target;
        public ZombieDamageManager zombie;

        public bool wasHit;
        public float freezeDurationAfterHit = 0.2f;
        
        private void ReceiveDamage(DamageInfo info)
        {
            wasHit = true;
        }

        public float damp = 8f;
        
        private Vector3 _lastMove;
        private Vector3 _lastLook;
        
        private IEnumerator AI()
        {
            while (target != null)
            {
                if (wasHit)
                {
                    player.Stop();
                    yield return new WaitForSeconds(freezeDurationAfterHit);
                    wasHit = false;
                }

                var move = target.position - player.position;
                var look = target.eyes.position;

                _lastMove = Vector3.Lerp(_lastMove, move, Time.deltaTime * damp);
                _lastLook = Vector3.Lerp(_lastLook, look, Time.deltaTime * damp);
                
                player.Move(_lastMove);
                player.LookAt(_lastLook);

                yield return null;
            }
        }

        private void Awake()
        {
            if (zombie == null) zombie = GetComponent<ZombieDamageManager>();
        }

        private void OnEnable()
        {
            zombie.onDamageReceived.AddListener(ReceiveDamage);
        }

        private void OnDisable()
        {
            zombie.onDamageReceived.RemoveListener(ReceiveDamage);
        }

        private void Start()
        {
            StartCoroutine(AI());
        }
    }
}