using System;
using UnityEngine;

namespace CucuTools.PlayerSystem2D
{
    [Serializable]
    public class Player2DSettings
    {
        [Header("Movement")]
        public bool canMove = true;
        [Min(0f)] public float moveSpeedMax = 2f;
        [Min(0f)] public float moveSpeedModificator = 1f;

        [Header("Jump")]
        public bool canJump = true;
        [Min(0f)] public float jumpHeight = 1.2f;
        [Min(0)] public int jumpMaxAmount = 2;
        
        [Header("Gravity")]
        public Vector2 gravityMax = Vector2.up * -9.81f; 
        public float gravityModificator = 1f;
        
        public float moveSpeed
        {
            get => moveSpeedMax * moveSpeedModificator;
            set => moveSpeedModificator = value / moveSpeedMax;
        }

        public Vector2 gravity
        {
            get => gravityMax * gravityModificator;
            set => gravityMax = value / gravityModificator;
        }
    }
    
    [Serializable]
    public class Player2DInfo
    {
        public bool moving = false;
        public bool jumping = false;
        public bool falling = false;

        public bool inAir => jumping || falling;
    }
}