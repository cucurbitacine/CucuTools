using System;
using UnityEngine;

namespace CucuTools.PlayerSystem
{
    [Serializable]
    public class PlayerSettings
    {
        [Header("Move")]
        public bool canMove = true;
        [Min(0f)]
        public float speedMax = 2f;
        [Min(0f)]
        public float speedModificator = 1f;
        [Space]
        public bool useWalkDamping = true;
        [Min(0f)]
        public float dampWalk = 24f;

        [Header("View")]
        public bool canView = true;
        [Range(0f, 90f)]
        public float fovUpper = 89;
        [Range(0f, 90f)]
        public float fovBottom = 89;
        [Space]
        public bool useViewDamping = true;
        [Min(0f)]
        public float dampView = 24f;
        
        [Header("Jump")]
        public bool canJump = true;
        [Min(0f)]
        public float jumpHeight = 1f;
        [Tooltip(@"It is time when able jump after lost ground.")]
        [Min(0f)]
        public float jumpDelay = 0.2f;

        public float speed
        {
            get => speedMax * speedModificator;
            set => speedModificator = value / speedMax;
        }
    }

    [Serializable]
    public class PlayerInfo
    {
        public bool isMoving = false;
        public bool isViewing = false;
        public bool isJumping = false;
        public bool isFalling = false;
    }
}