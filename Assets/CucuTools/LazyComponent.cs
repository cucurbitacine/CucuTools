using System;
using UnityEngine;

namespace CucuTools
{
    [Serializable]
    public class LazyComponent<T> : LazyValue<T>
    {
        public LazyComponent(Func<T> builder) : base(builder)
        {
        }

        public LazyComponent(GameObject gameObject) : this(gameObject.GetComponent<T>)
        {
        }
    }

    [Serializable]
    public class LazyValue<T>
    {
        [SerializeField] private T value;

        private readonly Func<T> builder;
        
        public T Value
        {
            get
            {
                if (value == null)
                {
                    value = builder.Invoke();
                }

                return value;
            }
        }
        
        public LazyValue(Func<T> builder)
        {
            this.builder = builder;
        }
    }
}