using System;
using UnityEngine;

namespace UtilsToolbox.PropertyAttributes
{
    /// <summary>
    /// This attribute makes field readonly in inspector
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public class ReadOnlyAttribute : PropertyAttribute
    {
        // Should stay empty
    }
}