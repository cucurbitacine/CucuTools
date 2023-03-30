using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CucuTools.Terminal
{
    public abstract class TerminalCommandRegistrator : CucuBehaviour
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

                if (CucuTerminal.Singleton.RegisterCommand(cmd))
                {
                    _commands.Add(cmd);
                }
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

        protected virtual void OnDisable()
        {
            UnregisterCommands();
        }
    }
}