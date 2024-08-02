using UnityEngine;

namespace CucuTools.Utils
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        #region Static

        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                }

                return _instance;
            }
        }

        #endregion

        protected virtual void OnStart()
        {
        }
        
        private void Start()
        {
            if (Instance == this)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogWarning($"{name} ({nameof(T)}) is duplicated!");
            }
            
            OnStart();
        }
    }
}