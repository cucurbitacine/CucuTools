using UnityEngine;

namespace CucuTools.MultiLangSystem.Impl
{
    [CreateAssetMenu(menuName = CreateContentSource + AssetName, fileName = AssetName, order = 0)]
    public class ListStringSource : ListContentSource<string>
    {
        public const string AssetName = nameof(ListStringSource);
    }
}