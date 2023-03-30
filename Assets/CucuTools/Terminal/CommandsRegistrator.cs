using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CucuTools.Terminal
{
    public abstract class CommandsRegistrator : CucuBehaviour
    {
        private readonly List<TerminalCommand> _commands = new List<TerminalCommand>();
        
        private void RegisterCommands()
        {
            var methods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            var commandMethods = methods.Where(m => m.GetCustomAttribute<TerminalCommandAttribute>() != null);

            foreach (var commandMethod in commandMethods)
            {
                var commandName = commandMethod.GetCustomAttribute<TerminalCommandAttribute>().commandName;

                if (commandName == null) commandName = commandMethod.Name;

                var cmd = new MethodCommand(this, commandName, commandMethod);
                
                CucuTerminal.Singleton.RegisterCommand(cmd);
            }
        }

        private void UnregisterCommands()
        {
            foreach (var command in _commands)
            {
                CucuTerminal.Singleton.UnregisterCommand(command);
            }
        }

        protected virtual void OnEnable()
        {
            RegisterCommands();
        }

        protected virtual  void OnDisable()
        {
            UnregisterCommands();
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
    
    public class MethodCommand : TerminalCommand
    {
        public object target { get; }
        public MethodBase method { get; }
        
        public MethodCommand(object commandTarget, string commandName, MethodBase commandMethod)
        {
            target = commandTarget;
            name = commandName;
            method = commandMethod;
        }
        
        public MethodCommand(object commandTarget, MethodBase commandMethod) : this(commandTarget, commandMethod.Name, commandMethod)
        {
        }
        
        public override string name { get; }
        
        public override void Execute(params string[] args)
        {
            var parameters = method.GetParameters();

            //if (args.Length < parameters.Length) return;
            
            var methodArgs = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                methodArgs[i] = default;

                if (args.Length <= i) continue;
                
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
                else if (parameters[i].ParameterType == typeof(string[]))
                {
                    methodArgs[i] = args;
                }
            }

            method.Invoke(target, methodArgs);
        }
    }
}