using System;
using UnityEngine;

namespace SkalluUtils.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public class ProgressBarAttribute : PropertyAttribute
    {
        public readonly string Name = string.Empty;
        public readonly float MaxValue;
        public readonly Color Color;
        
        public ProgressBarAttribute(float maxValue, string colorHex = "")
        {
            MaxValue = maxValue;
            Color = ColorUtility.TryParseHtmlString(colorHex, out Color newColor) ? newColor : Color.white;
        }

        public ProgressBarAttribute(string name, float maxValue, string colorHex = "")
        {
            Name = name;
            MaxValue = maxValue;
            Color = ColorUtility.TryParseHtmlString(colorHex, out Color newColor) ? newColor : Color.white;
        }
    }
}