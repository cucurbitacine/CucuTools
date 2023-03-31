using System;
using UnityEngine;

namespace CucuTools.PlayerSystem
{
    [Serializable]
    public class PersonSettings
    {
        [Header("Movement")]
        public bool canMove = true;
        [Min(0f)] public float moveSpeedMax = 2f;
        [Min(0f)] public float moveSpeedModificator = 1f;
        
        [Header("Rotate")]
        public bool canRotate = true;

        [Min(0f)] public float rotateSpeedMax = 1f;
        [Min(0f)] public float rotateSpeedModificator = 1f;

        [Header("Jump")]
        public bool canJump = true;
        [Min(0f)] public float jumpHeight = 1.2f;

        [Header("Gravity")]
        public Vector3 gravityMax = Physics.gravity;
        public float gravityModificator = 1f;
        
        public float moveSpeed
        {
            get => moveSpeedMax * moveSpeedModificator;
            set => moveSpeedModificator = value / moveSpeedMax;
        }
        
        public float rotateSpeed
        {
            get => rotateSpeedMax * rotateSpeedModificator;
            set => rotateSpeedModificator = value / rotateSpeedMax;
        }
        
        public Vector3 gravity
        {
            get => gravityMax * gravityModificator;
            set => gravityMax = value / gravityModificator;
        }
    }

    [Serializable]
    public class PersonInfo
    {
        public bool moving = false;
        public bool rotating = false;
        public bool jumping = false;
        public bool falling = false;
    }
}