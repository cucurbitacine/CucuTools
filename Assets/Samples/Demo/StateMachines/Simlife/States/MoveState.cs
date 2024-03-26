using CucuTools.StateMachines;
using UnityEngine;

namespace Samples.Demo.StateMachines.Simlife.States
{
    public class MoveState : StateBase<CellController>
    {
        [Header("Moving")]
        [Min(0.001f)] public float speedScale = 1f;
        [Min(0.001f)] public float threshold = 0.05f;
        public Vector2 destination = Vector2.zero;
        
        protected override void OnUpdate(float deltaTime)
        {
            var target = core.fixedArea.Evaluate(destination);

            if (Vector2.Distance(core.position, target) > threshold)
            {
                var speed = core.speed * speedScale;
                
                var direction = (target - core.position).normalized;
                core.position += direction * (speed * deltaTime);
            }
            else
            {
                isDone = true;
            }
        }
    }
}