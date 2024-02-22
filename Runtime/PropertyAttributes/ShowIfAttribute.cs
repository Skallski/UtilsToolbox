using System;
using UnityEngine;

namespace SkalluUtils.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
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