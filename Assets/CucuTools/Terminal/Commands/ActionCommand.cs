using System;

namespace CucuTools.Terminal.Commands
{
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
}