using System.Linq;
using CucuTools.Attributes;
using CucuTools.Scenes;
using UnityEngine;

namespace CucuTools.Serialization.Impl
{
    public class SerializationSceneController : CucuBehaviour
    {
        public string playerPrefsKey = "";

        public SerializedScene serializedScene;
        public string jsonScene;


        [DrawButton()]
        public void Serialize()
        {
            var gameObjects = FindObjectsOfType<SerializableGameObject>().Select(go => go.Serialize()).ToArray();

            serializedScene = new SerializedScene(CucuSceneManager.GetActiveScene().name, gameObjects);
            
            jsonScene = JsonUtility.ToJson(serializedScene);
            PlayerPrefs.SetString(playerPrefsKey, jsonScene);
        }

        [DrawButton()]
        public void Deserialize()
        {
            if (PlayerPrefs.HasKey(playerPrefsKey))
            {
                jsonScene = PlayerPrefs.GetString(playerPrefsKey);
                serializedScene = JsonUtility.FromJson<SerializedScene>(jsonScene);

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
        
        [DrawButton()]
        public void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteKey(playerPrefsKey);
        }
    }
}