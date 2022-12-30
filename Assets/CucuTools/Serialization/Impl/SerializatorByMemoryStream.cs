using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace CucuTools.Serialization.Impl
{
    [CreateAssetMenu(menuName = "Create SerializatorByMemoryStream", fileName = "SerializatorByMemoryStream", order = 0)]
    public class SerializatorByMemoryStream : Serializator
    {
        public override byte[] Serialize<T>(T t)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, t);
                return ms.ToArray();
            }
        }

        public override T Deserialize<T>(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                return (T) new BinaryFormatter().Deserialize(ms);
            }
        }
    }
}