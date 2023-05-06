using UnityEngine;

namespace CucuTools.PlayerSystem2D
{
    public class SpeedController2D : CucuBehaviour
    {
        public bool running = false;
        public bool sneaking = false;
        
        [Space]
        [Min(0)] public float moveSpeed = 5;
        [Min(0)] public float runSpeed = 10;
        [Min(0)] public float sneakSpeed = 2;

        [Space] public PlayerController2D player2d;

        public void Move(float move)
        {
            player2d.Move(move, GetSpeed());
        }

        private float GetSpeed()
        {
            if (running) return runSpeed;

            if (sneaking) return sneakSpeed;

            return moveSpeed;
        }
    }
}