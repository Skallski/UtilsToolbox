using System;

namespace SkalluUtils.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public class SerializeMethodAttribute : Attribute
    {
        public readonly string ButtonName;

        public SerializeMethodAttribute()
        {
            ButtonName = string.Empty;
        }
        
        public SerializeMethodAttribute(string buttonName)
        {
            ButtonName = buttonName;
        }
    }
}