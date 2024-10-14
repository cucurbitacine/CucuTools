using CucuTools.StateMachines;
using UnityEngine;

namespace Samples.Demo.StateMachines.Example.States
{
    public class WaitState : StateBase
    {
        [Header("WAIT")]
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

        protected override void OnExecute()
        {
            if (time > (duration + _delay))
            {
                SetDone(true);
            }
        }
    }
}