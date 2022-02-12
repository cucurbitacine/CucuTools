using UnityEngine;

namespace Example.Scripts
{
    public class RotatorBySlider : MonoBehaviour
    {
        public float phase = 180f;
        public CucuSlider Slider;

        public void Rotate(float value)
        {
            transform.localRotation = Quaternion.Euler(0f, value + phase, 0f);
        }
        
        private void Awake()
        {
            Rotate(Slider.Value);
            Slider.OnValueChanged.AddListener(Rotate);
        }
    }
}
