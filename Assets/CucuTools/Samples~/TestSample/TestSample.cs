using UnityEngine;

namespace CucuTools.Samples.TestSample
{
    public class TestSample : MonoBehaviour
    {
        public float scale = 1f;
        
        private void Update()
        {
            transform.localScale = Vector3.one * Mathf.Abs(Mathf.Sin(Time.time * scale * 2 * Mathf.PI));
        }
    }
}
