using UnityEngine;

namespace Examples
{
    public class DebugFPS : MonoBehaviour
    {
        public int fps = 0;
        public int smoothFPS = 0;
        public int averageFPS = 0;
        
        [Space]
        [Min(1)]
        public int fpsHistoryLength = 120;
        
        [Space]
        public int group = 0;
        
        [Space]
        [Min(0)]
        public int min = 0;
        [Min(0)]
        public int max = 120;
        public float damp = 2;
        
        [Space]
        public Color colorFPS = new Color(1, 0.5f, 0.5f);
        public Color colorSmoothFPS = new Color(1, 1, 0.5f);
        public Color colorAverageFPS = new Color(0.5f, 1, 0.5f);
        
        public const string FPS = nameof(FPS);
        public const string SmoothFPS = nameof(SmoothFPS);
        public const string AverageFPS = nameof(AverageFPS);

        private int _fpsIndex = 0;
        private int _fpsSum = 0;
        private int[] _fpsHistory = null;

        private void UpdateMinMax()
        {
            min = 0;

            max = (int)Mathf.Lerp(max, Mathf.Max(2 * averageFPS, 120), damp * Time.deltaTime);
        }
        
        private void GraphFPS()
        {
            if (Time.deltaTime != 0)
            {
                fps = (int)(1 / Time.deltaTime);
            }
            
            if (Time.smoothDeltaTime != 0)
            {
                smoothFPS = (int)(1 / Time.smoothDeltaTime);
            }

            if (_fpsHistory == null || _fpsHistory.Length < fpsHistoryLength)
            {
                _fpsHistory = new int[fpsHistoryLength];
            }

            _fpsHistory[_fpsIndex] = fps;
            _fpsIndex = (_fpsIndex + 1) % fpsHistoryLength;

            _fpsSum = 0;
            for (var i = 0; i < fpsHistoryLength; i++)
            {
                _fpsSum += _fpsHistory[i];
            }
            
            averageFPS = _fpsSum / fpsHistoryLength;
            
            DebugGUI.SetGraphProperties(FPS, "FPS: " + fps, min, max, group, colorFPS, false);
            DebugGUI.Graph(FPS, fps);
            
            DebugGUI.SetGraphProperties(SmoothFPS, "Smooth FPS: " + smoothFPS, min, max, group, colorSmoothFPS, false);
            DebugGUI.Graph(SmoothFPS, smoothFPS);
            
            DebugGUI.SetGraphProperties(AverageFPS, "Average FPS: " + averageFPS, min, max, group, colorAverageFPS, false);
            DebugGUI.Graph(AverageFPS, averageFPS);
        }
        
        private void Update()
        {
            UpdateMinMax();
            
            GraphFPS();
        }
    }
}
