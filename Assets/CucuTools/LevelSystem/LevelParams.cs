using System;
using UnityEngine;

namespace CucuTools.LevelSystem
{
    [Serializable]
    public class LevelParams
    {
        [field: SerializeField] public int PreviousSceneId { get; set; }
    }
}