using System.Linq;
using CucuTools.StateMachines;
using UnityEngine;

namespace Samples.Demo.StateMachines.Simlife.States
{
    public class AvoidState : StateBase<CellController>
    {
        [Header("Avoiding")]
        public float avoidDistance = 5f;
        public float scatterRadius = 1f;
        public CellController target;
        
        [Space]
        public MoveState move;

        public Vector2 GetAvoidDestination()
        {
            var point = core.position + (core.position - target.position).normalized * avoidDistance;

            point += Random.insideUnitCircle * scatterRadius;
            
            return point;
        }
        
        protected override void OnEnter()
        {
            move.destination = GetAvoidDestination();
            SetSubState(move);
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (core.sensor.Contains(target))
            {
                if (move.isDone)
                {
                    move.destination = GetAvoidDestination();
                    SetSubState(move);
                }
            }
            else
            {
                isDone = true;
            }
        }
    }
}
