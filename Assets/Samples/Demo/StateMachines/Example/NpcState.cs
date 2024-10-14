using System.Linq;
using CucuTools;
using CucuTools.StateMachines;
using Samples.Demo.StateMachines.Example.States;
using UnityEngine;

namespace Samples.Demo.StateMachines.Example
{
    public class NpcState : StateBase
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

        public StateBase GetNextState()
        {
            if (attack.IsRunning) // ATTACK -> CHASE / PATROL
            {
                return hasTarget ? chase : patrol;
            }
            
            if (chase.IsRunning) // CHASE -> ATTACK / CHASE / SEARCH
            {
                if (attack.CanAttack())
                {
                    return attack;
                }

                return hasTarget ? chase : search;
            }

            return hasTarget ? chase : patrol; // STATE -> CHASE / PATROL
        }

        protected override void OnEnter()
        {
            patrol.SetContext(npc);
            chase.SetContext(this);
            attack.SetContext(this);
            search.SetContext(this);
            
            SetSubState(patrol);
        }

        protected override void OnExecute()
        {
            if (SubState.IsDone)
            {
                var nextState = GetNextState();
                
                SetSubState(nextState);
                
                return;
            }
            
            if (hasTarget)
            {
                lastTargetPosition = targetPosition;
                
                if (patrol.IsRunning) // PATROL -> CHASE
                {
                    SetSubState(chase);
                } 
                else if (search.IsRunning) // SEARCH -> CHASE
                {
                    SetSubState(chase);
                }
            }
            else
            {
                if (chase.IsRunning) // CHASE -> SEARCH
                {
                    SetSubState(search);
                }
            }
        }
        
        private void Awake()
        {
            foreach (var stateNpc in GetComponentsInChildren<StateBase>())
            {
                stateNpc.TrySetContext(this);
            }
        }
        
        private void FixedUpdate()
        {
            if (IsRunning)
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