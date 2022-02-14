using UnityEngine;

namespace Example.Scripts.DamageSystem.Balloon
{
    public class BalloonHealthViewer : MonoBehaviour
    {
        public Gradient Gradient;
        public Renderer Renderer;
        
        [Space]
        public HealthBehaviour Health;

        public void UpdateView()
        {
            var t = (float) Health.Amount / Health.Maximum;
            var target = Gradient.Evaluate(t);
            Renderer.material.color = target;
        }
    }
}