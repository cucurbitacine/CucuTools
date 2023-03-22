using CucuTools.PlayerSystem;
using UnityEngine;

namespace Examples.Playground.Scripts
{
    public class DebugInfo : MonoBehaviour
    {
        public PlayerInput input;

        public const string SmoothFPS = nameof(SmoothFPS);
        public const string FPS = nameof(FPS);

        public const string MouseX = nameof(MouseX);
        public const string MouseY = nameof(MouseY);

        public const string AngularVelocity = nameof(AngularVelocity);

        private float _maxAngularVelocity;

        private void GraphFPS()
        {
            if (Time.smoothDeltaTime != 0)
            {
                var smoothFPS = 1 / Time.smoothDeltaTime;
                DebugGUI.SetGraphProperties(SmoothFPS, "Smooth FPS: " + smoothFPS.ToString("F3"), 0, 200, 0, new Color(0, 1, 1), false);
                DebugGUI.Graph(SmoothFPS, smoothFPS);
            }

            if (Time.deltaTime != 0)
            {
                var fps = 1 / Time.deltaTime;
                DebugGUI.SetGraphProperties(FPS, "FPS: " + fps.ToString("F3"), 0, 200, 0, new Color(1, 0.5f, 1), false);
                DebugGUI.Graph(FPS, fps);
            }
        }
        
        private void GraphMouse()
        {
            var x = input.view.x;
            var y = input.view.y;

            DebugGUI.SetGraphProperties(MouseX, "x : " + x.ToString("F3"), -4, 4, 1, Color.red, false);
            DebugGUI.SetGraphProperties(MouseY, "y : " + y.ToString("F3"), -4, 4, 1, Color.green, false);
            
            DebugGUI.Graph(MouseX, x);
            DebugGUI.Graph(MouseY, y);
        }

        private void GraphAngularVelocity()
        {
            var angularVelocity = input.player.rigid.angularVelocity.y;
            _maxAngularVelocity = Mathf.Max(_maxAngularVelocity, Mathf.Abs(angularVelocity));

            DebugGUI.SetGraphProperties(AngularVelocity, "AV: " + angularVelocity.ToString("F3"), -_maxAngularVelocity,
                _maxAngularVelocity, 2, Color.yellow, false);
            DebugGUI.Graph(AngularVelocity, angularVelocity);
        }

        private void Update()
        {
            GraphFPS();

            GraphMouse();

            GraphAngularVelocity();
        }
    }
}