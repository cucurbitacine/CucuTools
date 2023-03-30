using System;

namespace CucuTools.Terminal
{
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