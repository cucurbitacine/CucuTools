using System.Linq;
using CucuTools;
using CucuTools.StateMachines;
using Samples.Demo.StateMachines.Example.States;
using UnityEngine;

namespace Samples.Demo.StateMachines.Example
{
    public class NpcStateMachine : StateMachineBase, INpcCore
    {
        [Header("NPC")]
        public Transform origin;

        [Space]
        public float visibilityDistance = 3f;
        public LayerMask targetLayerMask = 1;
        [SerializeField] private Collider2D _targetCollider2D;

        [Space]
        public PatrolState patrol;
        public ChaseTargetState chase;
        public NpcAttackState attack;
        public SearchTargetState search;
        

        private readonly Collider2D[] _overlap = new Collider2D[32];
        
        public bool hasTarget => target &&
                                 target.gameObject.activeInHierarchy &&
                                 Vector2.Distance(position, targetPosition) < visibilityDistance;

        public Transform target => _targetCollider2D ? _targetCollider2D.transform : null;

        public Vector2 targetPosition => target.position;
        public Vector2 lastTargetPosition { get; private set; }

        public Vector2 position
        {
            get => origin.position;
            set => origin.position = value;
        }

        public override StateBase GetEntryState()
        {
            return patrol;
        }

        public override StateBase GetNextState()
        {
            if (activeState == attack) // ATTACK -> CHASE / PATROL
            {
                return hasTarget ? chase : patrol;
            }
            
            if (activeState == chase) // CHASE -> ATTACK / CHASE / SEARCH
            {
                if (attack.CanAttack())
                {
                    return attack;
                }

                return hasTarget ? chase : search;
            }

            return hasTarget ? chase : patrol; // STATE -> CHASE / PATROL
        }
        
        private void Awake()
        {
            foreach (var stateNpc in GetComponentsInChildren<StateBase>())
            {
                stateNpc.TryInstall(this);
            }
        }
        
        private void LateUpdate()
        {
            if (hasTarget)
            {
                lastTargetPosition = targetPosition;
                
                if (activeState == patrol) // PATROL -> CHASE
                {
                    SetState(chase);
                } 
                else if (activeState == search) // SEARCH -> CHASE
                {
                    SetState(chase);
                }
            }
            else
            {
                if (activeState == chase) // CHASE -> SEARCH
                {
                    SetState(search);
                }
            }
        }
         
        private void FixedUpdate()
        {
            if (isActive)
            {
                var count = Physics2D.OverlapCircleNonAlloc(position, visibilityDistance, _overlap, targetLayerMask);

                if (count > 0)
                {
                    var validTargets = _overlap.Take(count);

                    if (!validTargets.Contains(_targetCollider2D))
                    {
                        _targetCollider2D = _overlap[0];
                    }
                }
                else
                {
                    _targetCollider2D = null;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (origin)
            {
                Gizmos.DrawWireSphere(position, visibilityDistance);
            }
        }
    }
}