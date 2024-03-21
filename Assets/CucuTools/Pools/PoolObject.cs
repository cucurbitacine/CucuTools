using UnityEngine;

namespace CucuTools.Pools
{
    [DisallowMultipleComponent]
    public class PoolObject : CucuBehaviour
    {
        public bool releaseOnDisable = true;
        
        public PoolObjectProfile profile { get; set; }
        
        public void Release()
        {
            if (profile != null && !profile.released)
            {
                profile.released = true;
            }
        }

        public void Dispose()
        {
            if (profile != null && !profile.disposed)
            {
                profile.disposed = true;
            }
        }
        
        protected virtual void Awake()
        {
            if (profile != null)
            {
                profile.disposed = false;
            }
        }
        
        protected virtual void OnDisable()
        {
            if (releaseOnDisable)
            {
                Release();
            }
        }
        
        protected virtual void OnDestroy()
        {
            Dispose();
        }
    }
}