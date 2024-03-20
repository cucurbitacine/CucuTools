using UnityEngine;

namespace CucuTools
{
    public sealed class Singleton : Singleton<Singleton>
    {
    }
    
    public abstract class Singleton<T> : CucuBehaviour where T : Singleton<T>
    {
        [SerializeField] private bool _isSingleton = false;

        public bool isSingleton
        {
            get => _isSingleton;
            private set => _isSingleton = value;
        }

        protected virtual void Awake()
        {
            if (isSingleton && singleton != this)
            {
                isSingleton = false;

                Debug.LogWarning($"{name} cannot be singleton because singleton already exists.");

                gameObject.SetActive(false);
            }
        }
        
        #region Static

        private static T _singleton;

        public static T singleton
        {
            get
            {
                if (_singleton == null)
                {
                    _singleton = FindOrCreateSingleton();

                    _singleton.isSingleton = true;
                    DontDestroyOnLoad(_singleton.gameObject);
                }

                return _singleton;
            }
        }

        private static T FindOrCreateSingleton()
        {
            return TryFindSingleton(out var found) ? found : CreateSingleton();
        }

        private static bool TryFindSingleton(out T t)
        {
            var allSingleton = FindObjectsOfType<T>();

            if (allSingleton.Length > 0)
            {
                for (var i = 0; i < allSingleton.Length; i++)
                {
                    if (allSingleton[i].isSingleton)
                    {
                        t = allSingleton[i];
                        return true;
                    }
                }
            }

            t = default;
            return false;
        }

        private static T CreateSingleton()
        {
            return new GameObject(typeof(T).Name).AddComponent<T>();
        }

        #endregion
    }
}