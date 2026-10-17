using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.LevelSystem
{
    [DisallowMultipleComponent]
    public abstract class LevelController : MonoBehaviour
    {
        private LevelParameters _levelParameters;
        private LevelContext _levelContext;
        private readonly List<LevelComponent> _levelComponents = new List<LevelComponent>();
        
        #region Public API

        public LevelParameters GetLevelParameters()
        {
            return _levelParameters;
        }
        
        public void SetLevelParameters(LevelParameters levelParameters)
        {
            _levelParameters = levelParameters;
        }

        public LevelContext GetContext()
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
            foreach (var levelComponent in _levelComponents)
            {
                yield return levelComponent.OnStartLevel();
            }
        }

        protected virtual void OnDestroyComponents()
        {
            foreach (var levelComponent in _levelComponents)
            {
                levelComponent.OnDestroyLevel();
            }
        }
        
        #endregion

        private void FindAll()
        {
            _levelComponents.Clear();
            _levelComponents.AddRange(FindObjectsByType<LevelComponent>(FindObjectsInactive.Exclude));
            _levelComponents.Sort((a, b) => a.Order.CompareTo(b.Order));
        }

        private void BindAll()
        {
            var levelContext = GetContext();
            
            levelContext.Bind(GetType(), this);
            
            foreach (var levelComponent in _levelComponents)
            {
                levelContext.Bind(levelComponent.GetType(), levelComponent);
            }
            
            BindAll(levelContext);
        }
        
        private void ResolveAll()
        {
            var levelContext = GetContext();
            
            foreach (var levelComponent in _levelComponents)
            {
                levelComponent.ResolveAll(levelContext);
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
