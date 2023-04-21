using UnityEngine;

namespace Examples.Playground.Scripts
{
    public class MaterialOffsetShift : MonoBehaviour
    {
        public Vector2 offset = Vector2.zero;
        
        public Material material;
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");

        private void UpdateOffset()
        {
            if (material != null) material.SetTextureOffset(MainTex, offset);
        }
        
        private void Awake()
        {
            material = GetComponent<Renderer>().material;
        }

        private void Update()
        {
            UpdateOffset();
        }
    }
}
