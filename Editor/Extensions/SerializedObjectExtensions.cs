using System.Collections.Generic;
using UnityEditor;

namespace SkalluUtils.Editor.Extensions
{
    public static class SerializedObjectExtensions
    {
        public static void DrawInspectorFieldsExcept(this SerializedObject obj, params string[] excludedPropertiesNames)
        {
            DrawInspectorFieldsExcept(obj, new HashSet<string>(excludedPropertiesNames));
        }
        
        public static void DrawInspectorFieldsExcept(this SerializedObject obj, HashSet<string> excludedPropertiesNames)
        {
            SerializedProperty iterator = obj.GetIterator();
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
            {
                if (excludedPropertiesNames.Contains(iterator.name))
                {
                    continue;
                }

                EditorGUILayout.PropertyField(iterator, true);
            }
        }
    }
}