using System;
using System.Diagnostics;
using UnityEngine;

namespace SkalluUtils.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    [Conditional("UNITY_EDITOR")]
    public class PreviewAssetAttribute : PropertyAttribute
    {
        public const float DEFAULT_WIDTH = 64;
        public const float DEFAULT_HEIGHT = 64;

        public float Width { get; private set; }
        public float Height { get; private set; }
        public bool UseLabel { get; private set; }
        
        public PreviewAssetAttribute(float width = DEFAULT_WIDTH, float height = DEFAULT_HEIGHT, bool useLabel = true)
        {
            Width = width;
            Height = height;
            UseLabel = useLabel;
        }
    }
}