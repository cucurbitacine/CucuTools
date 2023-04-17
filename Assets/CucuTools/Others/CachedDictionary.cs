using System;
using System.Collections.Generic;

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
        
        public TValue this[TKey key]
        {
            get => _cache[key];
            set => _cache[key] = value;
        }
        
        public bool TryGetValue(TKey key, out TValue value)
        {
            return _cache.TryGetValue(key, out value);
        }

        public void Add(TKey key, TValue value)
        {
            _cache.Add(key, value);
        }
        
        public bool TryGetValidValue(TKey key, out TValue value)
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
}