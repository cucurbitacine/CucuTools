using UnityEngine;

namespace CucuTools.LevelSystem
{
    public abstract class LevelComponent : MonoBehaviour
    {
        public abstract void ResolveAll(LevelContext context);
    }
}