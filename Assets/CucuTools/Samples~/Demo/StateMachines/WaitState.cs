using CucuTools.StateMachines;
using UnityEngine;

namespace Samples.Demo.StateMachines
{
    public class WaitState : StateBase
    {
        [Header("Waiting")]
        [Min(0f)] public float duration = 1f;
        [Min(0f)] public float threshold = 0f;

        private float _delay = 0f;
        
        protected override void OnEnter()
        {
            if (threshold > 0)
            {
                _delay = Random.value * threshold;
            }
        }

        protected override void OnUpdate(float deltaTime)
        {
            isDone = time > (duration + _delay);
        }
    }
}