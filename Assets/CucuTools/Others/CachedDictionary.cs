using System;
using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.Others
{
    public class CachedDictionary<TKey, TValue>
    {
        private readonly Func<TKey, TValue> _factory;
        private readonly Func<TValue, bool> _validation;
        private readonly Dictionary<TKey, TValue> _cache;

        public int Count => _cache.Count;
        public ICollection<TKey> Keys => _cache.Keys;
        public ICollection<TValue> Values => _cache.Values;

        public CachedDictionary(Func<TKey, TValue> factory, Func<TValue, bool> validation)
        {
            _factory = factory;
            _validation = validation;
            _cache = new Dictionary<TKey, TValue>();
        }

        public CachedDictionary(Func<TKey, TValue> factory) : this(factory, value => true)
        {
        }

        protected CachedDictionary()
        {
        }
        
        public TValue this[TKey key]
        {
            get => _cache[key];
            set => _cache[key] = value;
        }

        public void Add(TKey key, TValue value)
        {
            _cache.Add(key, value);
        }
        
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (key == null)
            {
                value = default;
                return false;
            }
            
            if (!_cache.TryGetValue(key, out value))
            {
                value = _factory.Invoke(key);
                
                Add(key, value);
            }

            return _validation.Invoke(value);
        }
        
        public bool ContainsKey(TKey key)
        {
            return _cache.ContainsKey(key);
        }
        
        public bool Remove(TKey key)
        {
            return _cache.Remove(key);
        }
        
        public void Clear()
        {
            _cache.Clear();
        }
    }
    
    public class CachedComponent<TKey, TValue> : CachedDictionary<TKey, TValue>
        where TKey : Component
        where TValue : Component
    {
        public CachedComponent() : this(key => key.GetComponent<TValue>(), value => value != null)
        {
        }
        
        private CachedComponent(Func<TKey, TValue> factory, Func<TValue, bool> validation) : base(factory, validation)
        {
        }

        private CachedComponent(Func<TKey, TValue> factory) : base(factory)
        {
        }
    }
}