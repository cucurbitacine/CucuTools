using CucuTools.PlayerSystem2D;
using UnityEngine;

namespace Samples.Playground2D.Scripts.Tools
{
    public class SpeedController2D : ToolController2D
    {
        [Header("Speed")]
        public bool running = false;
        
        [Space]
        [Min(0)] public float moveSpeed = 5;
        [Min(0)] public float runSpeed = 10;

        public void Move(float move)
        {
            player2d.Move(move, GetSpeed());
        }

        private float GetSpeed()
        {
            if (running) return runSpeed;

            return moveSpeed;
        }
    }
}