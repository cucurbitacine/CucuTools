using System.Linq;
using CucuTools.Attributes;
using CucuTools.Scenes;
using Newtonsoft.Json;
using UnityEngine;

namespace CucuTools.Serialization
{
    public class SerializationSceneController : CucuBehaviour
    {
        public string playerPrefsKey = "";

        public SerializedScene serializedScene;
        public string jsonScene;


        [CucuButton()]
        public void Serialize()
        {
            var gameObjects = FindObjectsOfType<SerializableGameObject>().Select(go => go.Serialize()).ToArray();

            serializedScene = new SerializedScene(CucuSceneManager.GetActiveScene().name, gameObjects);
            
            jsonScene = JsonConvert.SerializeObject(serializedScene);
            PlayerPrefs.SetString(playerPrefsKey, jsonScene);
        }

        [CucuButton()]
        public void Deserialize()
        {
            if (PlayerPrefs.HasKey(playerPrefsKey))
            {
                jsonScene = PlayerPrefs.GetString(playerPrefsKey);
                serializedScene = JsonConvert.DeserializeObject<SerializedScene>(jsonScene);

                var gameObjects = FindObjectsOfType<SerializableGameObject>();
                foreach (var sgo in serializedScene.gameObjects)
                {
                    var go = gameObjects.FirstOrDefault(t => t.Cuid == sgo.guid);
                    if (go != null)
                    {
                        go.Deserialize(sgo);
                    }
                }
            }
        }
        
        [CucuButton()]
        public void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteKey(playerPrefsKey);
        }
    }
}