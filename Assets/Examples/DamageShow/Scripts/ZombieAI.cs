using System.Collections;
using CucuTools;
using CucuTools.DamageSystem;
using CucuTools.PlayerSystem;
using UnityEngine;
using UnityEngine.AI;

namespace Examples.DamageShow.Scripts
{
    public class ZombieAI : PersonInput<RigidPersonController>
    {
        public float damp = 8f;
        public bool wasHit;
        public float freezeDurationAfterHit = 0.2f;
        
        [Space]
        public PersonController target;
        public ZombieDamageManager zombie;
        
        private Vector3 _lastMove;
        private Vector3 _lastLook;
        private NavMeshPath _path = null;

        private Coroutine _ai;

        private Vector3 _lastValidPosition = Vector3.zero;

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
                    person.Stop();
                    yield return new WaitForSeconds(freezeDurationAfterHit);
                    wasHit = false;
                }

                var havePath = NavMesh.CalculatePath(person.position, target.position, NavMesh.AllAreas, _path);

                if (havePath)
                {
                    _lastValidPosition = target.position;
                }
                else
                {
                    havePath = NavMesh.CalculatePath(person.position, _lastValidPosition, NavMesh.AllAreas, _path);
                }
                
                if (havePath)
                {
                    var move = _path.corners[1] - person.position;

                    _lastMove = move;
                    _lastLook = Vector3.Lerp(_lastLook, _lastMove, Time.deltaTime * damp);
                    
                    person.MoveInDirection(_lastMove);
                    person.LookInDirection(Vector3.ProjectOnPlane(_lastLook, Vector3.up).normalized);
                }
                else
                {
                    person.Stop();
                }

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