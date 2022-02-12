using CucuTools;
using CucuTools.Interactables;
using UnityEngine;

namespace Example.Shaders
{
    public class OutlineEffect : MonoBehaviour
    {
        private static readonly int outlineColor = Shader.PropertyToID("_OutlineColor");
        private static readonly int outlineThickness = Shader.PropertyToID("_OutlineThickness");
        
        public Color IdleColor = new Color(1.000f, 1.000f, 1.000f);
        public Color HoverColor = new Color(0.435f, 0.972f, 0.892f);
        public Color PressColor = new Color(0.973f, 0.962f, 0.310f);

        [Space]
        public float IdleThickness = 0.02f;
        public float HoverThickness = 0.06f;
        public float PressThickness = 0.01f;
        
        [Space]
        public InteractableBehaviour Interactable;
        public Renderer[] Renderers;

        private Shader outlineShader;
        
        private void Awake()
        {
            if (Renderers == null || Renderers.Length == 0) Renderers = GetComponentsInChildren<Renderer>();
            if (Interactable == null) Interactable = GetComponentInChildren<InteractableBehaviour>();

            Interactable.InteractEvents.OnIdleStart.AddListener(Idle);
            Interactable.InteractEvents.OnHoverStart.AddListener(Hover);
            Interactable.InteractEvents.OnPressStart.AddListener(Press);

            outlineShader = Shader.Find("CucuTools/Outlines/Surface");
            foreach (var renderer in Renderers)
            {
                renderer.material.shader = outlineShader;
            }
        }

        public void SetColor(Color color)
        {
            foreach (var renderer in Renderers)
            {
                renderer.material.SetColor(outlineColor, color);
            }
        }

        public void SetThickness(float thickness)
        {
            foreach (var renderer in Renderers)
            {
                renderer.material.SetFloat(outlineThickness, thickness);
            }
        }
        
        public void Idle()
        {
            SetColor(IdleColor);
            SetThickness(IdleThickness);
        }

        public void Hover(ICucuContext context)
        {
            SetColor(HoverColor);
            SetThickness(HoverThickness);
        }

        public void Press(ICucuContext context)
        {
            SetColor(PressColor);
            SetThickness(PressThickness);
        }
    }
}
