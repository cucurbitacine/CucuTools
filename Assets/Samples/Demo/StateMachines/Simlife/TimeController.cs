using CucuTools;
using UnityEngine;

namespace Samples.Demo.StateMachines.Simlife
{
    public class TimeController : CucuBehaviour
    {
        [Range(0.1f, 5f)] [SerializeField] private float _timeScale = 1f;

        public float timeScale
        {
            get => _timeScale;
            set
            {
                _timeScale = value;
                Time.timeScale = Mathf.Clamp(timeScale, 0.1f, 5f);
            }
        }

        private void OnEnable()
        {
            Time.timeScale = 1f;
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                Time.timeScale = timeScale;
            }
        }
    }
}