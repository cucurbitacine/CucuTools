using CucuTools.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.Others
{
    public class CucuTimer : CucuBehaviour
    {
        [Space]
        public bool isRunning = false;
        [Space]
        [Min(0f)]
        public float time = 0f;
        [Min(0f)]
        public float duration = 1f;

        [Space]
        public UnityEvent onDone = new UnityEvent();
        
        [CucuButton("Start", group:"Timer")]
        public void StartTimer()
        {
            time = 0f;
            isRunning = true;
        }
        
        [CucuButton("Stop", group:"Timer")]
        public void StopTimer()
        {
            isRunning = false;
            time = 0f;
        }

        private void UpdateTimer(float deltaTime)
        {
            if (time >= duration)
            {
                StopTimer();
                    
                onDone.Invoke();
            }
            else
            {
                time += deltaTime;
            }
        }
        
        private void Update()
        {
            if (isRunning) UpdateTimer(Time.deltaTime);
        }
    }
}