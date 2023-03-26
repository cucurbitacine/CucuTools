using UnityEngine;

namespace CucuTools.PlayerSystem
{
    public abstract class PlayerController : CucuBehaviour
    {
        public abstract PlayerInfo info { get; }
        public abstract PlayerSettings settings { get; }
        public abstract GroundSettings ground { get; }
        
        public abstract Transform eyes { get; }
        public abstract Rigidbody rigid { get; }
        public abstract CapsuleCollider capsule { get; }

        public abstract void Move(Vector3 direction);
        public abstract void Rotate(Vector2 viewInput);
        public abstract void LookIn(Vector3 direction);
        public abstract void LookAt(Vector3 point);
        public abstract void Jump();
        public abstract void Stop();
        public abstract void MoveLocal(Vector3 localDirection);
    }

    public abstract class PlayerInput : CucuBehaviour
    {
        public abstract PlayerController player { get; }
    }
    
    public abstract class PlayerInput<TPlayer> : PlayerInput
        where TPlayer : PlayerController
    {
        public TPlayer playerCurrent;

        public sealed override PlayerController player => playerCurrent;
    }
}