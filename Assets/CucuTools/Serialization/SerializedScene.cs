using System;

namespace CucuTools.Serialization
{
    [Serializable]
    public class SerializedScene
    {
        public string sceneName = string.Empty;
        public SerializedGameObject[] gameObjects = null;

        public SerializedScene()
        {
        }

        public SerializedScene(string sceneName, params SerializedGameObject[] gameObjects)
        {
            this.sceneName = sceneName;
            this.gameObjects = gameObjects;
        }
    }

    [Serializable]
    public class SerializedGameObject
    {
        public string id = string.Empty;
        public SerializedComponent[] components = null;

        public Guid guid
        {
            get => Guid.TryParse(id, out var g) ? g : Guid.Empty;
            set => id = value.ToString();
        }

        public SerializedGameObject()
        {
        }

        public SerializedGameObject(Guid guid, params SerializedComponent[] components)
        {
            this.guid = guid;
            this.components = components;
        }
    }


    [Serializable]
    public class SerializedComponent
    {
        public string typeName = string.Empty;
        public byte[] bytes = null;

        public SerializedComponent()
        {
        }

        public SerializedComponent(string typeName, byte[] bytes)
        {
            this.typeName = typeName;
            this.bytes = bytes;
        }

        public SerializedComponent(Type type, byte[] bytes) : this(type.FullName, bytes)
        {
        }
    }
}