using System;
using UnityEngine;

namespace CucuTools.LevelSystem
{
    [Serializable]
    public class LevelParameters
    {
        [field: SerializeField] public int PreviousSceneId { get; set; }
    }
}