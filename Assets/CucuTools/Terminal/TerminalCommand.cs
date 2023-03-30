using System;

namespace CucuTools.Terminal
{
    public abstract class TerminalCommand
    {
        public abstract string name { get; }

        public abstract void Execute(params string[] args);
    }

    public class ActionCommand : TerminalCommand
    {
        public override string name { get; }
        public Action<string[]> action { get; }

        public ActionCommand(string commandName, Action<string[]> commandAction)
        {
            name = commandName;
            action = commandAction;
        }
        
        public override void Execute(params string[] args)
        {
            action.Invoke(args);
        }
    }

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