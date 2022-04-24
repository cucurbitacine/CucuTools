using System;
using UnityEngine;

namespace CucuTools.Attributes
{
    /// <summary>
    /// Presenting INTEGER field as Layer selector in Inspector 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class CucuLayerAttribute : PropertyAttribute
    {
    }
}