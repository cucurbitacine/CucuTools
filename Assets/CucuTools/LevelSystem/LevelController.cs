using System;
using System.Collections;
using UnityEngine;

namespace CucuTools.LevelSystem
{
    [DisallowMultipleComponent]
    public class LevelController : MonoBehaviour, IContextable
    {
        [SerializeField] private LevelParameters levelParameters;

        private ContextContainer levelContext;
        
        #region Public API

        public LevelParameters GetLevelParameters()
        {
            return levelParameters;
        }
        
        public void SetLevelParameters(LevelParameters parameters)
        {
            levelParameters = parameters;
        }

        public ContextContainer GetLevelContext()
        {
            if (levelContext == null) levelContext = new ContextContainer();
            return levelContext;
        }
        
        public virtual void Init(ContextContainer context)
        {
            context.Bind<LevelController>(this);
        }
        
        #endregion

        #region Virtual API

        protected virtual IEnumerator StartLevel()
        {
            yield break;
        }

        protected virtual void DestroyLevel()
        {
        }

        #endregion
        

        protected virtual void Awake()
        {
            Init(GetLevelContext());
        }
        
        protected virtual IEnumerator Start()
        {
            yield return StartLevel();
        }

        protected virtual void OnDestroy()
        {
            DestroyLevel();
        }
    }

    [Serializable]
    public class LevelParameters
    {
        [field: SerializeField] public int PreviousSceneId { get; set; }
    }
}
