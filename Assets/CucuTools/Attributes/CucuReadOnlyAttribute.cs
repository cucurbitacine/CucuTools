using System;
using UnityEngine;

namespace CucuTools.Attributes
{
    /// <summary>
    /// Make field readonly
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class CucuReadOnlyAttribute : PropertyAttribute
    {
    }
}