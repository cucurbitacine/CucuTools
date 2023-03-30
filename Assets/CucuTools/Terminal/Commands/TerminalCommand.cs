namespace CucuTools.Terminal
{
    public abstract class TerminalCommand
    {
        public abstract string name { get; }

        public abstract void Execute(params string[] args);
    }
}