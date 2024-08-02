using TMPro;
using UnityEngine;

namespace Samples.Demo.DamageSystem.Scripts.UI
{
    public class PopupText : MonoBehaviour
    {
        [SerializeField] private float floatingSpeed = 4f;
        [SerializeField] private TMP_Text displayText;

        public void SetColor(Color color)
        {
            if (displayText)
            {
                displayText.color = color;
            }
        }
        
        public void SetText(string text)
        {
            if (displayText)
            {
                displayText.text = text;
            }
        }

        private void Update()
        {
            transform.position += Vector3.up * (floatingSpeed * Time.deltaTime);
        }
    }
}