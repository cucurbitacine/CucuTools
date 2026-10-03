using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.LevelSystem
{
    [DisallowMultipleComponent]
    public abstract class LevelController : MonoBehaviour
    {
        private LevelParams m_levelParams;
        private LevelContext m_levelContext;
        private readonly List<LevelComponent> m_components = new List<LevelComponent>();
        
        #region Public API

        public LevelParams GetParams()
        {
            return m_levelParams;
        }
        
        public void SetParams(LevelParams levelParams)
        {
            m_levelParams = levelParams;
        }

        public LevelContext GetContext()
        {
            if (m_levelContext == null) m_levelContext = new LevelContext();
            return m_levelContext;
        }

        public abstract void BindAll(LevelContext context);
        
        #endregion

        #region Virtual & Abstract API

        protected abstract IEnumerator StartLevel();
        protected abstract void DestroyLevel();
        protected virtual void OnAwake() { }

        #endregion

        private void FindAll()
        {
            m_components.Clear();
            m_components.AddRange(FindObjectsByType<LevelComponent>(FindObjectsInactive.Exclude));
        }

        private void BindAll()
        {
            var levelContext = GetContext();
            BindAll(levelContext);
        }
        
        private void ResolveAll()
        {
            var levelContext = GetContext();
            foreach (var levelComponent in m_components)
            {
                levelComponent.ResolveAll(levelContext);
            }
        }
        
        protected virtual void Awake()
        {
            OnAwake();
            
            FindAll();
            
            BindAll();

            ResolveAll();
        }
        
        private IEnumerator Start()
        {
            yield return StartLevel();
        }

        private void OnDestroy()
        {
            DestroyLevel();
        }
    }
}
