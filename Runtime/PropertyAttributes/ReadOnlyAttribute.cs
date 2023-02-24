using System;
using UnityEngine;

namespace SkalluUtils.PropertyAttributes
{
    /// <summary>
    /// This attribute makes field readonly in inspector
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyAttribute : PropertyAttribute {}
}