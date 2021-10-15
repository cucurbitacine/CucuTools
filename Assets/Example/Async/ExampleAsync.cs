using System.Collections;
using System.Threading.Tasks;
using CucuTools.Async;
using UnityEngine;
using UnityEngine.Events;

namespace Example.Async
{
    public class ExampleAsync : ExampleBlock
    {
        [Header("Time Out")]
        public bool useTimeOut;
        [Range(0, 10)]
        public float timeOutSec = 2f;
        
        [Header("General")]
        public bool isProgress;
        [Range(0f, 1f)] public float progress;

        [Header("Enumerator")]
        public bool isEnumerator;
        
        [Header("Coroutine")]
        public bool isCoroutine;
        
        [Header("UnityEvent")]
        public bool isUnityEvent;
        public UnityEvent<string> unityEvent;
        public string stringMsg;

        [Header("Task")]
        public bool isTask;
        public bool isTaskReal;
        
        public override void ShowGUI()
        {
            if (isProgress)
            {
                if (isEnumerator || isCoroutine)
                {
                    GUILayout.HorizontalSlider(progress, 0f, 1f);
                }
                
                if (isUnityEvent)
                {
                    stringMsg = GUILayout.TextField(stringMsg);
                    
                    if (GUILayout.Button("Invoke"))
                    {
                        unityEvent.Invoke(stringMsg);
                    }
                }

                if (isTask)
                {
                    GUILayout.Label("Waiting task...");
                }
            }
            else
            {
                useTimeOut = GUILayout.Toggle(useTimeOut, "Use Time Out" + (useTimeOut ? $": {timeOutSec:F2} sec" : string.Empty), GUILayout.Width(256));
                if (useTimeOut)
                {
                    timeOutSec = GUILayout.HorizontalSlider(timeOutSec, 0f, 10f, GUILayout.Width(256));
                    if (useTimeOut) GUILayout.Label("Be careful with waitings using timeout. Waitable entities does not stop automatically");
                }
                
                if (GUILayout.Button("Wait Enumerator"))
                {
                    WaitEnumerator();
                }
                
                if (GUILayout.Button("Wait Coroutine"))
                {
                    WaitCoroutine();
                }
                
                if (GUILayout.Button("Wait UnityEvent"))
                {
                    WaitUnityEvent();
                }
                
                if (GUILayout.Button("Wait Task"))
                {
                    WaitTask();
                }
            }
        }

        private IEnumerator Enumerator()
        {
            progress = 0f;
            
            var duration = 5f;
            var timer = 0f;
            while (timer < duration)
            {
                if (!isProgress) yield break;
                
                progress = timer / duration;
                yield return null;
                timer += Time.deltaTime;
            }

            progress = 1f;
        }
        
        private async void WaitEnumerator()
        {
            isProgress = true;
            isEnumerator = true;

            var enumerator = Enumerator();
            
            var wait = enumerator.ToTask();
            if (useTimeOut)
            {
                var res = await wait.WithTimeOut((int)(timeOutSec * 1000));
                if (res.TimeOut) Debug.LogWarning($"Time Out {nameof(WaitEnumerator)}");
            }
            else
            {
                await wait;
            }
            
            isProgress = false;
            isEnumerator = false;
        }
        
        private async void WaitCoroutine()
        {
            isProgress = true;
            isCoroutine = true;

            var coroutine = CucuCoroutine.Start(Enumerator());
            var wait = coroutine.ToTask();
            if (useTimeOut)
            {
                var res = await wait.WithTimeOut((int)(timeOutSec * 1000));
                if (res.TimeOut)
                {
                    Debug.LogWarning($"Time Out {nameof(WaitCoroutine)}");
                    CucuCoroutine.Stop(coroutine);
                }
            }
            else
            {
                await wait;
            }
            
            isProgress = false;
            isCoroutine = false;
        }

        private async void WaitUnityEvent()
        {
            isProgress = true;
            isUnityEvent = true;

            var msg = string.Empty;
            
            var wait = unityEvent.ToTask();
            if (useTimeOut)
            {
                var res = await wait.WithTimeOut((int)(timeOutSec * 1000));
                if (res.TimeOut) Debug.LogWarning($"Time Out {nameof(WaitCoroutine)}");
                else msg = res.Task.Result;
            }
            else
            {
                msg = await wait;
            }
            
            Debug.Log(msg);
            
            isProgress = false;
            isUnityEvent = false;
        }

        private async void WaitTask()
        {
            isProgress = true;
            isTask = true;
            
            var wait = AsyncTask();
            if (useTimeOut)
            {
                var res = await wait.WithTimeOut((int)(timeOutSec * 1000));
                if (res.TimeOut) Debug.LogWarning($"Time Out {nameof(WaitCoroutine)}");
            }
            else
            {
                await wait;
            }
            
            isProgress = false;
            isTask = false;
        }

        private async Task AsyncTask()
        {
            isTaskReal = true;
            await Task.Delay(5000);
            isTaskReal = false;
        }
    }
}