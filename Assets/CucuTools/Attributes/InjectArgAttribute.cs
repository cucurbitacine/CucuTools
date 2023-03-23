using System;
using UnityEngine;

namespace CucuTools.Attributes
{
    /// <summary>
    /// Attribute for injection field
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class InjectArgAttribute : PropertyAttribute
    {
    }
}