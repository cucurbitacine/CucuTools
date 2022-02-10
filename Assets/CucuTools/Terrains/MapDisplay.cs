using UnityEngine;

namespace CucuTools.Terrains
{
    public class MapDisplay : MonoBehaviour
    {
        public Texture2D texture = default;
        public Renderer Renderer = default;

        public void DisplayFromColors(Color[] colors, Vector2Int resolution)
        {
            if (Renderer == null) return;
            var material = Renderer.sharedMaterial;

            if (texture == null) texture = new Texture2D(resolution.x, resolution.y);
        
            texture.Resize(resolution.x, resolution.y);
            texture.SetPixels(colors);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.Apply();

            material.mainTexture = texture;
        }
    
        public void DisplayFromColorMap(Color[,] map, Vector2Int resolution)
        {
            var colors = new Color[resolution.x * resolution.y];
            for (var j = 0; j < resolution.y; j++)
            {
                var i0 = j * resolution.x;
                for (var i = 0; i < resolution.x; i++)
                {
                    colors[i0 + i] = map[i, j];
                }
            }
        
            DisplayFromColors(colors, resolution);
        }
    
        public void DisplayFromNoiseMap(float[,] map, Vector2Int resolution)
        {
            var colorMap = new Color[resolution.x, resolution.y];
            for (var i = 0; i < resolution.x; i++)
            {
                for (var j = 0; j < resolution.y; j++)
                {
                    colorMap[i, j] = Color.Lerp(Color.black, Color.white, map[i, j]);
                }
            }

            DisplayFromColorMap(colorMap, resolution);
        }
    }
}