using System.Collections;
using UnityEngine;

namespace CucuTools.Async
{
    /// <summary>
    /// Tool for start/stop coroutines outside MonoBehaviour
    /// </summary>
    public class CucuCoroutine : Singleton<CucuCoroutine>
    {
        #region Public API

        public static Coroutine Start(IEnumerator routine)
        {
            return singleton.StartCoroutine(routine);
        }

        public static void Stop(Coroutine coroutine)
        {
            singleton.StopCoroutine(coroutine);
        }

        public static void StopAll()
        {
            singleton.StopAllCoroutines();
        }

        #endregion
    }
}