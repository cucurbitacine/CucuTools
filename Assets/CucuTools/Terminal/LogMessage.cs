using System;
using UnityEngine;

namespace CucuTools.Terminal
{
    [Serializable]
    public class LogMessage
    {
        public DateTime time = default;
        public string message = string.Empty;
        [Multiline] public string stackTrace = string.Empty;
        public LogType type = LogType.Log;
    }
}