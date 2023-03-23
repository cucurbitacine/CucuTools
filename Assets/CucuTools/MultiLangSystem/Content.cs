using System;

namespace CucuTools.MultiLangSystem
{
    [Serializable]
    public class Content<TContent>
    {
        public TContent value;
        public LangType lang;
    }
}