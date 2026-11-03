using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools.LevelSystem
{
    [DisallowMultipleComponent]
    public abstract class LevelController : MonoBehaviour
    {
        private LevelParameters _levelParameters;
        private LevelContext _levelContext;
        private readonly Dictionary<int, List<LevelComponent>> _components = new Dictionary<int, List<LevelComponent>>();
        
        #region Public API

        public LevelParameters GetLevelParameters()
        {
            return _levelParameters;
        }
        
        public void SetLevelParameters(LevelParameters levelParameters)
        {
            _levelParameters = levelParameters;
        }

        public LevelContext GetOrCreateContext()
        {
            if (_levelContext == null) _levelContext = new LevelContext();
            return _levelContext;
        }

        public abstract void BindAll(LevelContext context);
        
        #endregion

        #region Virtual & Abstract API
        
        protected abstract IEnumerator StartLevel();
        protected abstract void OnDestroyLevel();
        protected virtual void OnAwake() { }

        protected virtual IEnumerator StartComponents()
        {
            var coroutines = new List<Coroutine>();
            foreach (var key in _components.Keys.OrderBy(k => k))
            {
                foreach (var component in _components[key])
                {
                    var coroutine = StartCoroutine(component.OnStartLevel());
                    coroutines.Add(coroutine);
                }

                foreach (var coroutine in coroutines)
                {
                    yield return coroutine;
                }
                
                coroutines.Clear();
            }
        }

        protected virtual void OnDestroyComponents()
        {
            foreach (var key in _components.Keys.OrderBy(k => k))
            {
                foreach (var component in _components[key])
                {
                    component.OnDestroyLevel();
                }
            }
        }
        
        #endregion

        private void FindAll()
        {
            var components = FindObjectsByType<LevelComponent>(FindObjectsInactive.Exclude);
            foreach (var component in components)
            {
                var order = component.Order;
                if (!_components.TryGetValue(order, out var list))
                {
                    list = new List<LevelComponent>();
                    _components[order] = list;
                }
                list.Add(component);
            }
        }

        private void BindAll()
        {
            var levelContext = GetOrCreateContext();
            
            levelContext.Bind(GetType(), this);
            foreach (var component in _components.SelectMany(c=>c.Value))
            {
                levelContext.Bind(component.GetType(), component);
            }
            
            BindAll(levelContext);
        }
        
        private void ResolveAll()
        {
            var levelContext = GetOrCreateContext();
            
            foreach (var key in _components.Keys.OrderBy(k => k))
            {
                foreach (var component in _components[key])
                {
                    component.ResolveAll(levelContext);
                }
            }
        }
        
        private void Awake()
        {
            FindAll();
            OnAwake();
        }
        
        private IEnumerator Start()
        {
            BindAll();
            ResolveAll();
            
            yield return StartComponents();
            yield return StartLevel();
        }

        private void OnDestroy()
        {
            OnDestroyComponents();
            OnDestroyLevel();
        }
    }
}
