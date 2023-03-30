using CucuTools.Attributes;
using UnityEngine;

namespace CucuTools.Terminal
{
    public class TerminalTest : TerminalCommandRegistrator
    {
        [Multiline]
        public string spam = "spam";
        
        [Button()]
        [TerminalCommand("test.spam")]
        public void Spam()
        {
            Debug.Log(spam);
        }

        [TerminalCommand("test.strings")]
        private void TestString(params string[] args)
        {
            Debug.Log(string.Join(CucuTerminal.CommandSeparator, args));
        }
    }
}