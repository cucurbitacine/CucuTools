using System.Collections;
using CucuTools;
using CucuTools.Async;
using CucuTools.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace Example.Scripts
{
    public class ExampleAsync : CucuBehaviour
    {
        private const string GroupName = "For best experience click on Runtime";
        
        #region IEnumerator as Task

        private IEnumerator SomeEnumerator()
        {
            yield return new WaitForSeconds(1f);
        }

        [CucuButton("IEnumerator as Task", group:GroupName)]
        public async void CallEnumeratorAsTask()
        {
            Debug.Log($"IEnumerator as Task : Start : {Time.time}");
            await SomeEnumerator().AsTask();
            Debug.Log($"IEnumerator as Task : End : {Time.time}");
        }

        #endregion

        #region Coroutine as Task

        private Coroutine _coroutine;
        
        [CucuButton("Coroutine as Task", group:GroupName)]
        public async void CallCoroutineAsTask()
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(SomeEnumerator());
            
            Debug.Log($"Coroutine as Task : Start : {Time.time}");
            await _coroutine.AsTask();
            Debug.Log($"Coroutine as Task : End : {Time.time}");
        }

        #endregion

        #region UnityEvent as Task

        private UnityEvent _unityEvent;

        private void InvokeUnityEvent()
        {
            if (_unityEvent != null) _unityEvent.Invoke();
        }
        
        [CucuButton("UnityEvent as Task", group:GroupName)]
        public async void CallUnityEventAsTask()
        {
            _unityEvent = new UnityEvent();
            Invoke(nameof(InvokeUnityEvent), 1f);
            
            Debug.Log($"UnityEvent as Task : Start : {Time.time}");
            await _unityEvent.AsTask();
            Debug.Log($"UnityEvent as Task : End : {Time.time}");
        }

        #endregion

        #region UnityEvent<T> as Task<T>

        private UnityEvent<float> _unityEventFloat;
        
        public float floatValue = 1.618f;
        
        private void InvokeUnityEventFloat()
        {
            if (_unityEventFloat != null) _unityEventFloat.Invoke(floatValue);
        }
        
        [CucuButton("UnityEvent<T> as Task<T>", group:GroupName)]
        public async void CallUnityEventFloatAsTask()
        {
            _unityEventFloat = new UnityEvent<float>();
            Invoke(nameof(InvokeUnityEventFloat), 1f);
            
            Debug.Log($"UnityEvent as Task : Start : {Time.time}");
            var value = await _unityEventFloat.AsTask();
            Debug.Log($"UnityEvent as Task : End : {Time.time} : Value = {value}");
        }

        #endregion

        #region Coroutine without MonoBehaviour

        [CucuButton("Start coroutine without MonoBehaviour", group:GroupName)]
        public void StartCoroutineWithoutMonoBehaviour()
        {
            var notMonoBehaviour = new AbsolutelyNotMonoBehaviour();
            notMonoBehaviour.StartCoroutine();
        }
        
        private class AbsolutelyNotMonoBehaviour
        {
            public Coroutine coroutine;

            private IEnumerator SomeEnumerator()
            {
                Debug.Log($"Start enumerator : {Time.time}");

                yield return new WaitForSeconds(1f);
            
                Debug.Log($"End enumerator : {Time.time}");
            }
        
            public void StartCoroutine()
            {
                CucuCoroutine.Start(SomeEnumerator());
            }
        }

        #endregion
    }
}
