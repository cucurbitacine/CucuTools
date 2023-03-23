using System;
using System.Collections.Generic;

namespace CucuTools.Scenes
{
    /// <summary>
    /// <see cref="CucuArg"/> list manager
    /// </summary>
    [Serializable]
    public class CucuArgumentManager
    {
        /// <summary>
        /// Arguments list
        /// </summary>
        public readonly List<CucuArg> args = new List<CucuArg>();
        
        /// <summary>
        /// Singleton
        /// </summary>
        public static CucuArgumentManager Instance { get; }

        /// <summary>
        /// Name of manager
        /// </summary>
        public string Name { get; }

        static CucuArgumentManager()
        {
            Instance = new CucuArgumentManager("Singleton");
        }

        public CucuArgumentManager(string name)
        {
            Name = name;
        }
    }
}