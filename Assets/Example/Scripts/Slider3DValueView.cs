using UnityEngine;

namespace Example.Scripts
{
    public class Slider3DValueView : MonoBehaviour
    {
        [Range(0, 3)]
        public int fixedPoint = 2;
        [Space]
        public CucuSlider Slider3D;
        public TextMesh TextMesh;

        public void SetValue(float value)
        {
            TextMesh.text = Mathf.Clamp(value, Slider3D.MinValue, Slider3D.MaxValue).ToString("F" + fixedPoint);
        }
        
        private void Start()
        {
            SetValue(Slider3D.Value);
            
            Slider3D.OnValueChanged.AddListener(SetValue);
        }
        
        
    }
}
