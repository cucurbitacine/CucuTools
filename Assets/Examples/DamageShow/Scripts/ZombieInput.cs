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
        
        private void ReceiveDamage(DamageEvent info)
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
                    playerCurrent.Stop();
                    yield return new WaitForSeconds(freezeDurationAfterHit);
                    wasHit = false;
                }

                var move = target.position - playerCurrent.position;
                var look = target.eyes.position;

                _lastMove = Vector3.Lerp(_lastMove, move, Time.deltaTime * damp);
                _lastLook = Vector3.Lerp(_lastLook, look, Time.deltaTime * damp);
                
                playerCurrent.Move(_lastMove);
                playerCurrent.LookAt(_lastLook);

                yield return null;
            }
        }

        private void Awake()
        {
            if (playerCurrent == null) playerCurrent = GetComponent<PlayerRigidController>();
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