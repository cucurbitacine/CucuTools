using UnityEngine;

namespace CucuTools.MultiLangSystem
{
    public abstract class ContentSource : ScriptableObject
    {
        public const string ContentGroup = "Content Sources/";
        public const string CreateContentSource = Cucu.CreateAsset + Cucu.MultiLangGroup + ContentGroup;
    }
    
    public abstract class ContentSource<TContent> : ContentSource
    {
        public abstract TContent GetContent();
    }
    
    public abstract class ListContentSource<TContent> : ContentSource<TContent>
    {
        public TContent defaultContent;
        
        [Space]
        public Content<TContent>[] contents;
        
        public override TContent GetContent()
        {
            for (var i = 0; i < contents.Length; i++)
            {
                if (MultiLangManager.CurrentLang == contents[i].lang) return contents[i].value;
            }
            
            return defaultContent;
        }
    }
}