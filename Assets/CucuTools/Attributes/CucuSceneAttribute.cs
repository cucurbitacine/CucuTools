using UnityEngine;
using System;

namespace CucuTools.Attributes
{
    /// <summary>
    /// Drawing selector scenes in Inspector.
    /// All scenes must be added to build settings!
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class CucuSceneAttribute : PropertyAttribute
    {
    }
}