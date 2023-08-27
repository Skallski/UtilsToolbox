using UnityEditor;

namespace SkalluUtils.Extensions
{
    public static class SerializedPropertyExtensions
    {
        public static object GetValue(this SerializedProperty property)
        {
            object obj = property.serializedObject.targetObject;
 
            System.Reflection.FieldInfo field = null;
            foreach (var path in property.propertyPath.Split('.'))
            {
                var type = obj.GetType();
                field = type.GetField(path);
                obj = field.GetValue(obj);
            }
            
            return obj;
        }
    }
}