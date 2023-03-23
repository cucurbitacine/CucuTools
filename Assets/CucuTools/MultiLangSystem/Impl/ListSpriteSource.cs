using UnityEngine;

namespace CucuTools.MultiLangSystem.Impl
{
    [CreateAssetMenu(menuName = CreateContentSource + AssetName, fileName = AssetName, order = 0)]
    public class ListSpriteSource : ListContentSource<Sprite>
    {
        public const string AssetName = nameof(ListSpriteSource);
    }
}