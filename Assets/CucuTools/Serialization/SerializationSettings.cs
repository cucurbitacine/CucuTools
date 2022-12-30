using CucuTools.Serialization.Impl;
using UnityEngine;

namespace CucuTools.Serialization
{
    [CreateAssetMenu(menuName = "Create SerializationSettings", fileName = "SerializationSettings", order = 0)]
    public class SerializationSettings : ScriptableObject
    {
        public const string ObjectName = nameof(SerializationSettings);
        
        private static SerializationSettings _instance = null;
        
        public static SerializationSettings Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = Resources.Load<SerializationSettings>(ObjectName);
                
                if (_instance != null) return _instance;
                
                _instance = CreateInstance<SerializationSettings>();
                
                return _instance;
            }
        }

        [SerializeField] private Serializator serializator = null;

        public Serializator Serializator
        {
            get
            {
                if (serializator != null) return serializator;

                serializator = CreateInstance<SerializatorByJsonUtility>();
                
                return serializator;
            }
        }
    }
}
