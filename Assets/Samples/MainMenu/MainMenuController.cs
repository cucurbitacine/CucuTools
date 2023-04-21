using System;
using System.Collections.Generic;
using CucuTools.Scenes;
using UnityEngine;

namespace Examples.MainMenu
{
    [SceneController("Menu")]
    public class MainMenuController : SceneController
    {
        public SceneReference[] scenes;

        private List<CucuArg> GetArgs()
        {
            var exampleArg = new ExampleArgs()
            {
                // fill
            };
            
            var args = new List<CucuArg>();
            args.Add(exampleArg);
            return args;
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
                    var args = GetArgs().ToArray();

                    scene.loader.args.Clear();
                    scene.loader.args.AddRange(args);

                    scene.loader.LoadSingleScene();
                }
            }
        }
    }

    [Serializable]
    public class ExampleArgs : CucuArg
    {
        // setup
    }
}