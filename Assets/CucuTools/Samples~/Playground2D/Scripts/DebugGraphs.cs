using CucuTools.PlayerSystem2D;
using UnityEngine;

namespace Samples.Playground2D.Scripts
{
    public class DebugGraphs : MonoBehaviour
    {
        public const string X = "X";
        public const string Y = "Y";
        public const string V = "V";
        public const string R = "R";

        public PlayerController2D player;
    
        private readonly Color _colorRed = new Color(1, 0.5f, 0.5f);
        private readonly Color _colorGreen = new Color(0.5f, 1, 0.5f);
        private readonly Color _colorBlue = new Color(0.5f, 0.5f, 1f);
        private readonly Color _colorYellow = new Color(1.0f, 1.0f, 0.5f);

        private float maxSpeed = 0f;
        
        private void FixedUpdate()
        {
            var x = player.rigidbody2d.velocity.x;
            var y = player.rigidbody2d.velocity.y;
            var v = player.velocity.magnitude;
            var r = player.rigidbody2d.velocity.magnitude;

            maxSpeed = Mathf.Max(maxSpeed, Mathf.Max(v, r));
            
            DebugGUI.SetGraphProperties(X, "X: " + x.ToString("F1"), -player.speedMax * 1.5f, player.speedMax * 1.5f, 0, _colorRed, false);
            DebugGUI.Graph(X, x);
        
            DebugGUI.SetGraphProperties(Y, "Y: " + y.ToString("F1"), -player.speedMax * 1.5f, player.speedMax * 1.5f, 0, _colorGreen, false);
            DebugGUI.Graph(Y, y);
            
            DebugGUI.SetGraphProperties(V, "V: " + v.ToString("F1"), 0, maxSpeed, 1, _colorBlue, false);
            DebugGUI.Graph(V, v);
            
            DebugGUI.SetGraphProperties(R, "R: " + r.ToString("F1"), 0, maxSpeed, 1, _colorYellow, false);
            DebugGUI.Graph(R, r);
        }
    }
}
