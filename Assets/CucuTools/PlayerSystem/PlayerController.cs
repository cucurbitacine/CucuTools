using UnityEngine;

namespace CucuTools.PlayerSystem
{
    public abstract class PlayerController : CucuBehaviour
    {
        public Vector3 position => transform.position;
        public Quaternion rotation => transform.rotation;
        
        public abstract PlayerInfo info { get; }
        public abstract PlayerSettings settings { get; }
        public abstract GroundController ground { get; }
        
        public abstract Transform eyes { get; }

        public abstract void Move(Vector2 moveInput);
        public abstract void View(Vector2 viewInput);
        
        public abstract void MoveIn(Vector3 direction);
        public abstract void MoveAt(Vector3 point);
        public abstract void LookIn(Vector3 direction);
        public abstract void LookAt(Vector3 point);
        
        public abstract void Jump();
        public abstract void Stop();

        public virtual int GetCollidersNonAlloc(Collider[] colliders)
        {
            return 0;
        }
    }
}