using UnityEngine;

namespace CucuTools.PlayerSystem
{
    public abstract class PersonController : CucuBehaviour
    {
        public Vector3 position => transform.position;
        public Quaternion rotation => transform.rotation;
        
        public abstract PersonInfo info { get; }
        public abstract PersonSettings settings { get; }
        public abstract GroundController ground { get; }
        
        public abstract Transform head { get; }

        public abstract void Move(Vector3 move);
        public abstract void View(Vector2 view);
        public abstract void Jump();
        
        public abstract void MoveInDirection(Vector3 direction);
        public abstract void MoveAtPoint(Vector3 point);
        public abstract void LookInDirection(Vector3 direction);
        public abstract void LookAtPoint(Vector3 point);
        
        public abstract void Stop();
    }
}