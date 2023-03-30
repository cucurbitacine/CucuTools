using CucuTools.Attributes;
using UnityEngine;

namespace CucuTools.Terminal
{
    public class TerminalTest : CommandsRegistrator
    {
        [Button()]
        [TerminalCommand]
        public void Spam()
        {
            Debug.Log(Random.Range(0, 1000));
        }

        [TerminalCommand]
        private void TestString(params string[] args)
        {
            Debug.Log(string.Join(CucuTerminal.CommandSeparator, args));
        }
        
        [TerminalCommand("move")]
        private void Move(string objectName, float x, float y, float z, string spaceName)
        {
            var go = GameObject.Find(objectName);
            if (go)
            {
                var move = new Vector3(x, y, z);
                var space = spaceName != null && spaceName.Contains("self") ? Space.Self : Space.World;
                go.transform.Translate(move, space);
                    
                Debug.Log($"\"{objectName}\" moved to {move} as {space}");
            }
        }
        
        private void Start()
        {
            var trm = CucuTerminal.Singleton;

            trm.RegisterCommand("warn", args =>
            {
                Debug.LogWarning(string.Join(CucuTerminal.CommandSeparator, args));
            });

            trm.RegisterCommand("err", args =>
            {
                Debug.LogError(string.Join(CucuTerminal.CommandSeparator, args));
            });
            
            trm.RegisterCommand("tp", args =>
            {
                if (!CucuTerminal.TryGetString(out var objectName, 0, args))
                {
                    Debug.LogError("Parameter \"Object Name\" was not found");
                    return;
                }
                CucuTerminal.TryGetFloat(out var x, 1, args);
                CucuTerminal.TryGetFloat(out var y, 2, args);
                CucuTerminal.TryGetFloat(out var z, 3, args);
                
                var go = GameObject.Find(objectName);
                if (go)
                {
                    var pos = new Vector3(x, y, z);
                    go.transform.position = pos;
                    
                    Debug.Log($"\"{objectName}\" teleported to {pos}");
                }
            });
        }
    }
}