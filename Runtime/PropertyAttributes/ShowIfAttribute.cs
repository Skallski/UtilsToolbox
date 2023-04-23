using System;
using UnityEngine;

namespace SkalluUtils.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ShowIfAttribute : PropertyAttribute
    {
        public string ComparedPropertyName { get; }
        public object ComparedValue { get; }

        public ShowIfAttribute(string comparedPropertyName, object comparedValue)
        {
            ComparedPropertyName = comparedPropertyName;
            ComparedValue = comparedValue;
        }
    }
}