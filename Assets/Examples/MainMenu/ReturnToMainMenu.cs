using CucuTools.Scenes;
using UnityEngine;

namespace Examples.MainMenu
{
    public class ReturnToMainMenu : MonoBehaviour
    {
        public KeyCode keyCode = KeyCode.Escape;

        [Min(0f)]
        public float holdDuration = 3f;
        
        [Space]
        public SceneReference reference;

        private static ReturnToMainMenu _instance = null;
        
        private float _timer = 0f;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (Input.GetKey(keyCode))
            {
                _timer += Time.deltaTime;
                if (_timer > holdDuration)
                {
                    Debug.Log("Start LoadSingleScene");
                    reference.loader.LoadSingleScene();
                }
            }
            else
            {
                _timer = 0f;
            }
        }

        private void OnGUI()
        {
            if (!reference.loader.sceneName.Equals(CucuSceneManager.GetActiveScene().name))
            {
                GUILayout.Box($"Hold [{keyCode}] over {holdDuration} sec to load \"{reference.displayName}\"");
            }
        }
    }
}
