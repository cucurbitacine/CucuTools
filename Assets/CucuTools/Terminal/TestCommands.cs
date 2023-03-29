using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CucuTools.Attributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CucuTools.Terminal
{
    public class TestCommands : CucuBehaviour
    {
        private readonly List<string> _commands = new List<string>();

        [Button()]
        public void Spam()
        {
            Debug.Log(Random.Range(0, 1000));
        }
        
        private void Invoke(MethodBase method, params string[] args)
        {
            var parameters = method.GetParameters();

            if (args.Length < parameters.Length) return;
            
            var methodArgs = new object[args.Length];

            for (var i = 0; i < args.Length; i++)
            {
                if (parameters[i].ParameterType == typeof(bool))
                {
                    if (bool.TryParse(args[i], out var boolVal))
                    {
                        methodArgs[i] = boolVal;
                    }
                }
                else if (parameters[i].ParameterType == typeof(int))
                {
                    if (int.TryParse(args[i], out var intVal))
                    {
                        methodArgs[i] = intVal;
                    }
                }
                else if (parameters[i].ParameterType == typeof(float))
                {
                    if (float.TryParse(args[i], out var floatVal))
                    {
                        methodArgs[i] = floatVal;
                    }
                }
                else if (parameters[i].ParameterType == typeof(string))
                {
                    methodArgs[i] = args[i];
                }
            }

            method.Invoke(this, methodArgs);
        }

        private void RegisterCommands()
        {
            var methods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            var commands = methods.Where(m => m.GetCustomAttribute<TerminalCommandAttribute>() != null);

            foreach (var command in commands)
            {
                var commandName = command.GetCustomAttribute<TerminalCommandAttribute>().commandName;

                if (commandName == null) commandName = command.Name;

                var success = CucuTerminal.Singleton.RegisterCommand(commandName, args => Invoke(command, args));
                
                if (success)
                {
                    _commands.Add(commandName);
                    Debug.Log($"Command [{commandName}] successfully registered");
                }
                else
                {
                    Debug.LogWarning($"Command [{commandName}] registering failed");
                }
            }
        }

        protected void UnregisterCommands()
        {
            foreach (var command in _commands)
            {
                var success = CucuTerminal.Singleton.UnregisterCommand(command);
                
                if (success)
                {
                    Debug.Log($"Command [{command}] successfully unregistered");
                }
                else
                {
                    Debug.LogWarning($"Command [{command}] unregistering failed");
                }
            }
        }
        
        protected virtual void OnEnable()
        {
            RegisterCommands();
        }

        protected virtual void OnDisable()
        {
            UnregisterCommands();
        }

        private void Move(string objectName, float x, float y, float z, string spaceName)
        {
            var go = GameObject.Find(objectName);
            if (go)
            {
                var move = new Vector3(x, y, z);
                var space = spaceName.Contains("self") ? Space.Self : Space.World;
                go.transform.Translate(move, space);
                    
                Debug.Log($"Move \"{objectName}\" to {move} - {space}");
            }
        }
        
        private void Start()
        {
            var trm = CucuTerminal.Singleton;

            trm.RegisterCommand("warn", args =>
            {
                Debug.LogWarning(string.Join(CucuTerminal.CommandSeparator, args));
            });

            trm.RegisterCommand("move", args =>
            {
                CucuTerminal.TryGetString(out var objectName, 0, args);
                CucuTerminal.TryGetFloat(out var x, 1, args);
                CucuTerminal.TryGetFloat(out var y, 2, args);
                CucuTerminal.TryGetFloat(out var z, 3, args);
                CucuTerminal.TryGetString(out var spaceName, 4, args);
                
                var go = GameObject.Find(objectName);
                if (go)
                {
                    var move = new Vector3(x, y, z);
                    var space = spaceName.Contains("self") ? Space.Self : Space.World;
                    go.transform.Translate(move, space);
                    
                    Debug.Log($"Move \"{objectName}\" to {move} - {space}");
                }
            });
        }
    }


    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TerminalCommandAttribute : Attribute
    {
        public string commandName { get; }
        
        public TerminalCommandAttribute()
        {
        }
        
        public TerminalCommandAttribute(string commandName)
        {
            this.commandName = commandName;
        }
    }
}