using System.Linq;
using CucuTools;
using CucuTools.StateMachines;
using Samples.Demo.StateMachines.Example.States;
using UnityEngine;

namespace Samples.Demo.StateMachines.Example
{
    public class NpcStateMachine : StateMachineBase
    {
        [Header("NPC")]
        public Transform npc;

        [Space]
        public float visibilityDistance = 3f;
        public LayerMask targetLayerMask = 1;
        [SerializeField] private Collider2D _targetCollider2D;

        [Space]
        public PatrolState patrol;
        public ChaseState chase;
        public NpcAttackState attack;
        public SearchState search;
        
        private readonly Collider2D[] _overlap = new Collider2D[32];
        
        public bool hasTarget => target &&
                                 target.gameObject.activeInHierarchy &&
                                 Vector2.Distance(position, targetPosition) < visibilityDistance;

        public Transform target => _targetCollider2D ? _targetCollider2D.transform : null;

        public Vector2 targetPosition => target.position;
        public Vector2 lastTargetPosition { get; private set; }

        public Vector2 position
        {
            get => npc.position;
            set => npc.position = value;
        }
        
        protected override void OnStartStateMachine()
        {
            patrol.SetContext(npc);
            chase.SetContext(this);
            attack.SetContext(this);
            search.SetContext(this);
            
            EntryState = patrol;
        }
        
        public override StateBase GetNextState()
        {
            if (ActiveState == attack) // ATTACK -> CHASE / PATROL
            {
                return hasTarget ? chase : patrol;
            }
            
            if (ActiveState == chase) // CHASE -> ATTACK / CHASE / SEARCH
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

        protected override void OnUpdateStateMachine(float deltaTime)
        {
            if (hasTarget)
            {
                lastTargetPosition = targetPosition;
                
                if (ActiveState == patrol) // PATROL -> CHASE
                {
                    SetNextState(chase);
                } 
                else if (ActiveState == search) // SEARCH -> CHASE
                {
                    SetNextState(chase);
                }
            }
            else
            {
                if (ActiveState == chase) // CHASE -> SEARCH
                {
                    SetNextState(search);
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
            if (npc)
            {
                Gizmos.DrawWireSphere(position, visibilityDistance);
            }
        }
    }
}