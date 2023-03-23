using UnityEngine;

namespace Examples.Terrains.Scripts
{
    [CreateAssetMenu(fileName = nameof(MapSeedAsset), menuName = "CucuTools/Terrain/Map Seed")]
    public class MapSeedAsset : ScriptableObject
    {
        public MapSeed Seed = MapSeed.Default;
    }
}