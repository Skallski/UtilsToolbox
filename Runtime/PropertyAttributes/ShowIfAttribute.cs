using System;
using UnityEngine;

namespace UtilsToolbox.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public class ShowIfAttribute : PropertyAttribute
    {
        public readonly string PropertyName;
        public readonly object Value;

        public ShowIfAttribute(string propertyName, object value)
        {
            PropertyName = propertyName;
            Value = value;
        }
    }
}