using System;
using UnityEngine;

namespace SkalluUtils.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ProgressBarAttribute : PropertyAttribute
    {
        public string Name { get; } = string.Empty;
        public float MaxValue { get; }
        public Color Color { get; }
        
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