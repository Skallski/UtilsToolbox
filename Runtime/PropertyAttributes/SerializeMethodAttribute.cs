using System;

namespace SkalluUtils.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
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