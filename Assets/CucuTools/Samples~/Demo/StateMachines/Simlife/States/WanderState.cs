using CucuTools.StateMachines;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Samples.Demo.StateMachines.Simlife.States
{
    public class WanderState : StateBase
    {
        [Header("Wandering")]
        [Min(0f)] public float outerRadiusWandering = 2f;
        [Min(0f)] public float innerRadiusWandering = 0.5f;
        public Vector2 around = Vector2.zero;

        [Space]
        public bool infinitely = false;
        public int currentWandering = 0;
        public int totalWandering = 5;

        [Space]
        public MoveState move;
        public WaitState wait;

        private void Wander()
        {
            var distance = Random.value * (outerRadiusWandering - innerRadiusWandering) + innerRadiusWandering;
            var direction = Random.insideUnitCircle.normalized;
            move.destination = around + distance * direction;

            SetSubState(move);
        }

        private void NextPoint()
        {
            if (infinitely)
            {
                Wander();

                return;
            }

            currentWandering++;

            if (currentWandering < totalWandering)
            {
                Wander();
            }
            else
            {
                isDone = true;
            }
        }

        protected override void OnEnter()
        {
            currentWandering = -1;

            SetSubState(wait);
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (subState == wait)
            {
                if (wait.isDone)
                {
                    NextPoint();
                }
            }
            else if (subState == move)
            {
                if (move.isDone)
                {
                    SetSubState(wait);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (isActive)
            {
                Gizmos.DrawWireSphere(around, innerRadiusWandering);
                Gizmos.DrawWireSphere(around, outerRadiusWandering);
            }
        }
    }
}