using System;
using UnityEngine;

namespace UtilsToolbox.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public class PreviewAssetAttribute : PropertyAttribute
    {
        public const float DEFAULT_WIDTH = 64;
        public const float DEFAULT_HEIGHT = 64;

        public readonly float Width;
        public readonly float Height;
        public readonly bool UseLabel;
        
        public PreviewAssetAttribute(float width = DEFAULT_WIDTH, float height = DEFAULT_HEIGHT, bool useLabel = true)
        {
            Width = width;
            Height = height;
            UseLabel = useLabel;
        }
    }
}