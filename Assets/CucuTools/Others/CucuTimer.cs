using System.Collections;
using CucuTools.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.Others
{
    public class CucuTimer : CucuBehaviour
    {
        [Space] public bool isRunning = false;
        [Space] [Min(0f)] public float time = 0f;
        [Min(0f)] public float duration = 1f;

        [Space] public UnityEvent onDone = new UnityEvent();

        private Coroutine _ticking = null;

        [CucuButton("Start", group: "Timer")]
        public void StartTimer()
        {
            time = 0f;
            isRunning = true;

            if (_ticking != null) StopCoroutine(_ticking);
            _ticking = StartCoroutine(_Ticking());
        }

        [CucuButton("Stop", group: "Timer")]
        public void StopTimer()
        {
            if (_ticking != null) StopCoroutine(_ticking);

            isRunning = false;
            time = 0f;
        }

        private IEnumerator _Ticking()
        {
            while (time < duration)
            {
                time += Time.deltaTime;
                yield return null;
            }

            StopTimer();
            
            onDone.Invoke();
        }
    }
}