using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.Async
{
    /// <summary>
    /// Representing some objects to Tasks
    /// </summary>
    public static class CucuAsyncExt
    {
        /// <summary>
        /// Start enumerator as coroutine and return as task.
        /// <paramref name="root"/> create coroutine
        /// </summary>
        /// <param name="enumerator"></param>
        /// <param name="root"></param>
        /// <returns>Task</returns>
        public static async Task AsTask(this IEnumerator enumerator, MonoBehaviour root)
        {
            var taskSource = new TaskCompletionSource<object>();

            root.StartCoroutine(Coroutine(taskSource, enumerator));

            await taskSource.Task;
        }

        /// <summary>
        /// Start enumerator as coroutine and return as task
        /// </summary>
        /// <param name="enumerator"></param>
        /// <returns>Task</returns>
        public static async Task AsTask(this IEnumerator enumerator)
        {
            await enumerator.AsTask(CucuCoroutine.Instance);
        }

        /// <summary>
        /// Convert coroutine to task
        /// </summary>
        /// <param name="coroutine"></param>
        /// <returns></returns>
        public static async Task AsTask(this Coroutine coroutine)
        {
            var taskSource = new TaskCompletionSource<object>();

            CucuCoroutine.Start(Coroutine(taskSource, coroutine));

            await taskSource.Task;
        }

        /// <summary>
        /// Convert unity event to task
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unityEvent"></param>
        /// <returns></returns>
        public static async Task<T> AsTask<T>(this UnityEvent<T> unityEvent)
        {
            return await new UnityEventTask<T>(unityEvent).Task;
        }

        /// <summary>
        /// Convert unity event to task
        /// </summary>
        /// <param name="unityEvent"></param>
        /// <returns></returns>
        public static async Task AsTask(this UnityEvent unityEvent)
        {
            await new UnityEventTask(unityEvent).Task;
        }

        private static IEnumerator Coroutine<T>(TaskCompletionSource<T> tcs, IEnumerator enumerator)
        {
            while (enumerator.MoveNext()) yield return enumerator.Current;
            tcs.TrySetResult(default);
        }
        
        private static IEnumerator Coroutine<T>(TaskCompletionSource<T> tcs, Coroutine coroutine)
        {
            yield return coroutine;
            tcs.TrySetResult(default);
        }
    }

    /// <summary>
    /// Task<T> which waiting UnityEvent<T>
    /// </summary>
    /// <typeparam name="T">Result type</typeparam>
    public class UnityEventTask<T>
    {
        public TaskCompletionSource<T> TaskCompletionSource { get; private set; }
        public UnityEvent<T> UnityEvent { get; private set; }
        public Task<T> Task => TaskCompletionSource.Task;

        public UnityEventTask(UnityEvent<T> unityEvent)
        {
            TaskCompletionSource = new TaskCompletionSource<T>();

            UnityEvent = unityEvent;
            UnityEvent.AddListener(SetResult);
        }

        public static explicit operator Task<T>(UnityEventTask<T> et) => et.TaskCompletionSource.Task;
        public static explicit operator TaskCompletionSource<T>(UnityEventTask<T> et) => et.TaskCompletionSource;

        private void SetResult(T t)
        {
            UnityEvent.RemoveListener(SetResult);

            TaskCompletionSource.TrySetResult(t);
        }
    }

    /// <summary>
    /// Task which waiting UnityEvent
    /// </summary>
    public class UnityEventTask
    {
        public TaskCompletionSource<object> TaskCompletionSource { get; private set; }     
        public UnityEvent UnityEvent { get; private set; }
        public Task Task => TaskCompletionSource.Task;

        public UnityEventTask(UnityEvent unityEvent)
        {
            TaskCompletionSource = new TaskCompletionSource<object>();

            UnityEvent = unityEvent;
            UnityEvent.AddListener(SetResult);
        }

        public static explicit operator Task(UnityEventTask et) => et.TaskCompletionSource.Task;
        public static explicit operator TaskCompletionSource<object>(UnityEventTask et) => et.TaskCompletionSource;

        private void SetResult()
        {
            UnityEvent.RemoveListener(SetResult);

            TaskCompletionSource.TrySetResult(default);
        }
    }
}