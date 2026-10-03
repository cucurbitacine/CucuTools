using System.Collections;
using UnityEngine;

namespace CucuTools.LevelSystem
{
    public abstract class LevelComponent : MonoBehaviour
    {
        public virtual int Order { get; } = 0;
        
        public abstract void ResolveAll(LevelContext context);
        public abstract IEnumerator OnStartLevel();
        public abstract void OnDestroyLevel();
    }
}