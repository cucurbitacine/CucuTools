using System.Text;
using UnityEngine;

namespace CucuTools.Serialization.Impl
{
    /// <summary>
    /// Serialization: Object -> Json (by JsonUtility) -> byte[] (by Encoding)
    /// </summary>
    [CreateAssetMenu(menuName = "Create SerializatorByJsonUtility", fileName = "SerializatorByJsonUtility", order = 0)]
    public class SerializatorByJsonUtility : Serializator
    {
        public Encoding Encoding { get; set; } = Encoding.UTF8; 
        
        public override byte[] Serialize<T>(T t)
        {
            return Encoding.GetBytes(JsonUtility.ToJson(t));
        }

        public override T Deserialize<T>(byte[] bytes)
        {
            return JsonUtility.FromJson<T>(Encoding.GetString(bytes));
        }
    }
}