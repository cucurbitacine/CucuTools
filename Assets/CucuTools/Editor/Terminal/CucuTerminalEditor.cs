using System;
using CucuTools.Terminal;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CucuTools.Editor.Terminal
{
    public abstract class CucuTerminalEditor
    {
        private const string TerminalResourceName = "CucuTerminal";

        [MenuItem(Cucu.CreateGameObject + nameof(CucuTerminal))]
        private static void CreateTerminal(MenuCommand command)
        {
            if (Object.FindObjectOfType<CucuTerminal>())
            {
                Debug.LogWarning("Terminal already exist on the scene");
            }
            else
            {
                try
                {
                    var terminalPrefab = Resources.Load<CucuTerminal>(TerminalResourceName);
                    var terminal = Object.Instantiate(terminalPrefab);
                    terminal.name = TerminalResourceName;

                    Debug.Log("Terminal created");
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }
    }
}