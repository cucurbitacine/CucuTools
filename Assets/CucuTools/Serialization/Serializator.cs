using UnityEngine;

namespace CucuTools.Serialization
{
    /// <summary>
    /// Serialization Core
    /// </summary>
    public abstract class Serializator : ScriptableObject
    {
        public abstract byte[] Serialize<T>(T t);
        public abstract T Deserialize<T>(byte[] bytes);
    }
}