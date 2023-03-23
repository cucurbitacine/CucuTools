using System;
using CucuTools;
using CucuTools.Attributes;
using UnityEngine;

namespace Examples.Terrains.Scripts
{
    public class MapGenerator : CucuBehaviour
    {
        [Space]
        public Vector2Int Resolution = Vector2Int.one * 32;
        [Min(0f)]
        public Vector2 Size = Vector2.one;

        [Space]
        public bool UpdateAsset;
        public MapSeedAsset Map;
        public MapSeed Seed;
        public ColorPalette Palette;
        
        [Space]
        public bool AutoBuild = false;
        public MapConverter Converter;
        public MapDisplayer Displayer;

        public float[,] Generate()
        {
            return Map.Seed.GetMap(Resolution, Size);
        }

        [CucuButton()]
        public void Build()
        {
            var noiseMap = Generate();
            var colorMap = Converter.Convert(noiseMap, Resolution);
            Displayer.DisplayFromColorMap(colorMap, Resolution);
        }

        private void OnValidate()
        {
            if (UpdateAsset)
            {
                Map.Seed = Seed;
                Converter.TerrainColors.Palette = Palette;

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(Converter.TerrainColors);
#endif
            }
            else
            {
                Seed = Map.Seed;
                Palette = Converter.TerrainColors.Palette;
            }

            
            if (AutoBuild) Build();
        }

        private void OnGUI()
        {
            GUILayout.Space(512);
            
            GUILayout.BeginHorizontal();
            GUILayout.Box("Frequency");
            Seed.frequency = GUILayout.HorizontalSlider(Seed.frequency, 0, 2, GUILayout.Width(128));
            if (Seed.frequency != Map.Seed.frequency)
            {
                Map.Seed = Seed;
                Build();
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Box("Amplitude");
            Seed.amplitude = GUILayout.HorizontalSlider(Seed.amplitude, 0, 2, GUILayout.Width(128));
            if (Seed.amplitude != Map.Seed.amplitude)
            {
                Map.Seed = Seed;
                Build();
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Box("Lacunarity");
            Seed.lacunarity = GUILayout.HorizontalSlider(Seed.lacunarity, 0, 3, GUILayout.Width(128));
            if (Seed.lacunarity != Map.Seed.lacunarity)
            {
                Map.Seed = Seed;
                Build();
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Box("Persistance");
            Seed.persistance = GUILayout.HorizontalSlider(Seed.persistance, 0, 1, GUILayout.Width(128));
            if (Seed.persistance != Map.Seed.persistance)
            {
                Map.Seed = Seed;
                Build();
            }
            GUILayout.EndHorizontal();
        }
    }
}