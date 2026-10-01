using System;
using System.Collections;
using UnityEngine;

namespace CucuTools.LevelSystem
{
    [DisallowMultipleComponent]
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private CameraController camera;
        
        [Header("Parameters")]
        [SerializeField] private LevelParameters levelParameters;

        private ContextContainer _levelContext;
        
        #region Public API

        public PlayerController GetPlayer() => player;
        public CameraController GetCamera() => camera;

        public LevelParameters GetParameters()
        {
            return levelParameters;
        }
        
        public void SetParameters(LevelParameters parameters)
        {
            levelParameters = parameters;
        }

        public ContextContainer GetLevelContext()
        {
            if (_levelContext == null) _levelContext = new ContextContainer();
            return _levelContext;
        }
        
        #endregion

        #region Virtual API

        protected virtual IEnumerator EnableLevel()
        {
            InitializeContext(GetLevelContext());
            
            FindAll();
            
            yield return EnablePlayer();
            yield return EnableCamera();
        }

        protected virtual void DisableLevel()
        {
            DisableCamera();
            DisablePlayer();
        }

        protected virtual void InitializeContext(ContextContainer levelContext)
        {
            levelContext.Bind<LevelController>(this);
        }
        
        protected virtual IEnumerator EnablePlayer()
        {
            yield return player.EnablePlayer(GetLevelContext());
        }
        
        protected virtual IEnumerator EnableCamera()
        {
            yield return camera.EnableCamera(GetLevelContext());
        }
        
        protected virtual void DisablePlayer()
        {
            player?.DisablePlayer();
        }
        
        protected virtual void DisableCamera()
        {
            camera?.DisableCamera();
        }

        #endregion
        
        private void FindAll()
        {
            if (player == null) player = Find<PlayerController>();
            if (camera == null) camera = Find<CameraController>();
        }
        
        private static T Find<T>() where T : MonoBehaviour
        {
            return FindAnyObjectByType<T>();
        }
        
        private IEnumerator Start()
        {
            yield return EnableLevel();
        }

        private void OnDestroy()
        {
            DisableLevel();
        }
    }

    [Serializable]
    public class LevelParameters
    {
        [field: SerializeField] public int PreviousSceneId { get; set; }
    }
    
    public static class LevelControllerExt
    {
        public static T GetPlayer<T>(this LevelController level) where T : PlayerController => level.GetPlayer() is T t ? t : null;
        public static T GetCamera<T>(this LevelController level) where T : CameraController => level.GetCamera() is T t ? t : null;
    }
}
