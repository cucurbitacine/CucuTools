using UnityEngine;

namespace Examples.Terrains.Scripts
{
    [CreateAssetMenu(menuName = "Examples/Terrain/" + nameof(MapSeedAsset), fileName = nameof(MapSeedAsset), order = 0)]
    public class MapSeedAsset : ScriptableObject
    {
        public MapSeed Seed = MapSeed.Default;
    }
}