using System;
using CucuTools.Terminal.Commands;

namespace CucuTools.Terminal
{
    public static class CucuTerminalExtensions
    {
        public static bool RegisterCommand(this CucuTerminal trm, string commandName, Action<string[]> action)
        {
            return trm.RegisterCommand(new ActionCommand(commandName, action));
        }
        
        public static bool UnregisterCommand(this CucuTerminal trm, string commandName)
        {
            return trm.UnregisterCommand(commandName);
        }
    }
}