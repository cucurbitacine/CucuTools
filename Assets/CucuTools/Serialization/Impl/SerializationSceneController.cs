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


        [Button()]
        public void Serialize()
        {
            var gameObjects = FindObjectsOfType<SerializableGameObject>().Select(go => go.Serialize()).ToArray();

            serializedScene = new SerializedScene(CucuSceneManager.GetActiveScene().name, gameObjects);
            
            jsonScene = JsonUtility.ToJson(serializedScene);
            PlayerPrefs.SetString(playerPrefsKey, jsonScene);
        }

        [Button()]
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
        
        [Button()]
        public void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteKey(playerPrefsKey);
        }
    }
}