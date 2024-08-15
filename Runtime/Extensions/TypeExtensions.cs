using System;
using System.Collections.Generic;
using System.Reflection;

namespace UtilsToolbox.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<FieldInfo> GetFieldsRecursive(this Type type, BindingFlags bindingFlags)
        {
            FieldInfo[] fields = type.GetFields(bindingFlags);
            for (int i = 0; i < fields.Length; i++)
            {
                yield return fields[i];
            }

            if (type.BaseType != typeof(object) && type.BaseType != null)
            {
                foreach (FieldInfo field in GetFieldsRecursive(type.BaseType, bindingFlags))
                {
                    yield return field;
                }
            }
        }
    }
}