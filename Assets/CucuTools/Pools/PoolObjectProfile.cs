using System;
using UnityEngine;

namespace CucuTools.Pools
{
    [Serializable]
    public class PoolObjectProfile
    {
        public PoolObject poolObject = null;
        
        [Space]
        public int typeId = 0;
        public int groupId = 0;
        
        [Space]
        public bool disposed = false;
        public bool released = false;
        
        public bool available
        {
            get => !disposed;
            set => disposed = !value;
        }

        public GameObject gameObject => poolObject.gameObject;
    }
}