using System;

namespace SkalluUtils.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SerializeMethod : Attribute
    {
        public readonly string ButtonName;

        public SerializeMethod()
        {
            ButtonName = string.Empty;
        }
        
        public SerializeMethod(string buttonName)
        {
            ButtonName = buttonName;
        }
    }
}