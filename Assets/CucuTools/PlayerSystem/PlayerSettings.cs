using System;
using UnityEngine;

namespace CucuTools.PlayerSystem
{
    [Serializable]
    public class PlayerSettings
    {
        [Header("Body")]
        [Min(0.001f)] public float mass = 70f;
        [Min(0.001f)] public float height = 1.8f;
        [Min(0.001f)] public float radius = 0.248f;
        
        [Header("Walk")]
        public bool canWalk = true;
        [Min(0f)] public float walkSpeedMax = 2f;
        public bool useWalkDamping = true;
        [Min(0f)] public float dampWalk = 24f;

        [Header("Run")]
        public bool canRun = true;
        [Min(0f)] public float runSpeedMax = 4f;
        
        [Header("View")]
        public bool canView = true;
        [Range(0f, 90f)] public float fovUpper = 89;
        [Range(0f, 90f)] public float fovBottom = 89;
        public bool useViewDamping = true;
        [Min(0f)] public float dampView = 24f;
        
        [Header("Jump")]
        public bool canJump = true;
        [Min(0f)] public float jumpHeight = 1f;
        [Tooltip(@"It is time when able jump after lost ground.")]
        [Min(0f)] public float jumpDelay = 0.2f;
        public Vector3 gravity = Physics.gravity;
    }

    [Serializable]
    public class PlayerInfo
    {
        public bool isMoving = false;
        public bool isRunning = false;
        public bool isJumping = false;
        public bool isFalling = false;
        public bool isGrounded = false;
    }
}