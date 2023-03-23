using System;
using CucuTools.Injects;
using CucuTools.Scenes;
using UnityEngine;

namespace Examples.MainMenu
{
    [SceneController("Menu")]
    public class MainMenuController : SceneController
    {
        public SceneReference[] scenes;

        private ExampleSettings GetSettings()
        {
            return new ExampleSettings()
            {
                // fill
            };
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Quit"))
            {
                Application.Quit();
            }
            
            GUILayout.Space(32);
            GUILayout.Box("Scenes");
            
            foreach (var scene in scenes)
            {
                if (scene == null) continue;

                if (GUILayout.Button(scene.displayName))
                {
                    var settings = GetSettings();

                    scene.loader.RemoveArg(settings);
                    scene.loader.AddArg(settings);

                    scene.loader.LoadSingleSceneAsync();
                }
            }
        }
    }

    [Serializable]
    public class ExampleSettings : CucuArg
    {
        // setup
    }
}