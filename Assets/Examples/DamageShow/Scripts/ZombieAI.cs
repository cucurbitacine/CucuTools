using System.Collections;
using CucuTools;
using CucuTools.DamageSystem;
using CucuTools.PlayerSystem;
using UnityEngine;
using UnityEngine.AI;

namespace Examples.DamageShow.Scripts
{
    public class ZombieAI : PlayerInput
    {
        public float damp = 8f;
        public bool wasHit;
        public float freezeDurationAfterHit = 0.2f;
        
        [Space]
        public PlayerController target;
        public ZombieDamageManager zombie;
        
        private PlayerController _player;
        
        private Vector3 _lastMove;
        private Vector3 _lastLook;
        private NavMeshPath _path = null;

        private Coroutine _ai;

        private Vector3 _lastValidPosition = Vector3.zero;

        public override PlayerController player => _player;
        
        private void ReceiveDamage(DamageEvent info)
        {
            wasHit = true;
        }
        
        private IEnumerator AI()
        {
            if (_path == null) _path = new NavMeshPath();
            
            while (target != null)
            {
                if (wasHit)
                {
                    player.Stop();
                    yield return new WaitForSeconds(freezeDurationAfterHit);
                    wasHit = false;
                }

                var havePath = NavMesh.CalculatePath(player.position, target.position, NavMesh.AllAreas, _path);

                if (havePath)
                {
                    _lastValidPosition = target.position;
                }
                else
                {
                    havePath = NavMesh.CalculatePath(player.position, _lastValidPosition, NavMesh.AllAreas, _path);
                }
                
                if (havePath)
                {
                    var move = _path.corners[1] - player.position;

                    _lastMove = move;
                    _lastLook = Vector3.Lerp(_lastLook, _lastMove, Time.deltaTime * damp);
                    
                    player.MoveIn(_lastMove);
                    player.LookIn(Vector3.ProjectOnPlane(_lastLook, Vector3.up).normalized);
                }
                else
                {
                    player.Stop();
                }

                yield return null;
            }
        }

        private void Awake()
        {
            if (_player == null) _player = GetComponent<PlayerController>();
            if (zombie == null) zombie = GetComponent<ZombieDamageManager>();
        }

        private void OnEnable()
        {
            zombie.onDamageReceived.AddListener(ReceiveDamage);
            
            _ai = StartCoroutine(AI());
        }

        private void OnDisable()
        {
            zombie.onDamageReceived.RemoveListener(ReceiveDamage);

            if (_ai != null) StopCoroutine(_ai);
        }

        private void OnDrawGizmos()
        {
            if (_path != null)
            {
                CucuGizmos.DrawLines(_path.corners);
            }
        }
    }
}