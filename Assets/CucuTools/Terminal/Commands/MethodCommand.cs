using System.Reflection;

namespace CucuTools.Terminal
{
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