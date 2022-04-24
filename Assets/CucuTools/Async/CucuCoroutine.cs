using System.Collections;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CucuTools.Async
{
    /// <summary>
    /// Tool for start/stop coroutines outside MonoBehaviour
    /// </summary>
    public class CucuCoroutine : MonoBehaviour
    {
        /// <summary>
        /// Static MonoBehaviour for start/stop coroutines
        /// </summary>
        public static MonoBehaviour Instance
        {
            get
            {
                if (_instance != null) return _instance;
                
                var instances = Object.FindObjectsOfType<CucuCoroutine>();
                _instance = instances.FirstOrDefault();
                for (int i = 0; i < instances.Length; i++)
                {
                    if (instances[i] == _instance) continue;
                    Destroy(instances[i]);
                    Destroy(instances[i].gameObject);
                }

                if (_instance != null)
                {
                    DontDestroyOnLoad(_instance.gameObject);
                    return _instance;
                }
                
                _instance = new GameObject(nameof(CucuCoroutine)).AddComponent<CucuCoroutine>();
                DontDestroyOnLoad(_instance.gameObject);
                
                return _instance;
            }
        }

        private static CucuCoroutine _instance;

        #region Public API

        public static Coroutine Start(IEnumerator routine)
        {
            return Instance.StartCoroutine(routine);
        }

        public static void Stop(Coroutine coroutine)
        {
            Instance.StopCoroutine(coroutine);
        }

        public static void StopAll()
        {
            Instance.StopAllCoroutines();
        }

        #endregion

        #region Private API

        private static void Destroy(GameObject gameObject)
        {
            if (Application.isPlaying) Object.Destroy(gameObject);
            else Object.DestroyImmediate(gameObject);
        }
        
        private static void Destroy<T>(T component) where T : Component
        {
            if (Application.isPlaying) Object.Destroy(component);
            else Object.DestroyImmediate(component);
        }
        
        private static void DontDestroyOnLoad(GameObject gameObject)
        {
            gameObject.transform.SetParent(null);
            if (Application.isPlaying) Object.DontDestroyOnLoad(gameObject);
        }

        #endregion
    }
}