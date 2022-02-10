using System;
using System.Linq;
using UnityEngine;

namespace CucuTools.Terrains
{
    public class MapGenerator : MonoBehaviour
    {
        public bool autoUpdate = true;
    
        [Space]
        public Vector2Int Resolution = Vector2Int.one * 32;
        [Min(0f)]
        public Vector2 Size = Vector2.one;

        [Space]
        public bool UpdateAsset;
        public NoiseSeedAsset Noise;
        public NoiseSeed Seed;

        [Space]
        public DisplayType DisplayType;
        public TerrainType[] TerrainTypes;
    
        [Space] public MapDisplay MapDisplay;

        public void Build()
        {
            Resolution.x = Mathf.Max(2, Resolution.x);
            Resolution.y = Mathf.Max(2, Resolution.y);

            var map = (UpdateAsset ? Seed : Noise.Seed).GetMap(Resolution, Size);
        
            if (DisplayType == DisplayType.ColorMap)
            {
                var colorMap = new Color[Resolution.x,Resolution.y];
                for (var i = 0; i < Resolution.x; i++)
                {
                    for (var j = 0; j < Resolution.y; j++)
                    {
                        colorMap[i, j] = Color.black;
                        var height = map[i, j];
                        for (var k = 0; k < TerrainTypes.Length; k++)
                        {
                            var array = Array.FindAll(TerrainTypes, tt => tt.height <= height);
                            if (array.Length > 0)
                            {
                                var max = array.Select(a => a.height).Max();
                                var terrain = Array.Find(array, tt => Math.Abs(tt.height - max) < float.Epsilon);
                                colorMap[i, j] = terrain.color;
                            }
                        }
                    }
                }
            
                MapDisplay.DisplayFromColorMap(colorMap, Resolution);
            }

            if (DisplayType == DisplayType.NoiseMap)
            {
                MapDisplay.DisplayFromNoiseMap(map, Resolution);
            }
        }

        private void OnValidate()
        {
            if (autoUpdate) Build();

            if (UpdateAsset) Noise.Seed = Seed;
            else Seed = Noise.Seed;
        }
    }

    public enum DisplayType
    {
        NoiseMap,
        ColorMap,
    }

    [Serializable]
    public class TerrainType
    {
        public string name;
        public float height;
        public Color color;
    }
}