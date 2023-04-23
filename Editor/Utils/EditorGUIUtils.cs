using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Utils
{
    public static class EditorGUIUtils
    {
        /// <summary>
        /// Default "script" object field
        /// </summary>
        /// <param name="target"> target object </param>
        public static void DefaultScriptField(Object target)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script", target, target.GetType(), false);
            EditorGUILayout.Space();
            EditorGUI.EndDisabledGroup();
        }
        
        /// <summary>
        /// Creates slider field for serialized property
        /// </summary>
        /// <param name="property"> serialized property </param>
        /// <param name="leftValue"> min value </param>
        /// <param name="rightValue"> max value </param>
        /// <param name="label"> label (name, tooltip) </param>
        public static void PropertySliderField(SerializedProperty property, float leftValue, float rightValue, GUIContent label)
        {
            var position = EditorGUILayout.GetControlRect();
            
            label = EditorGUI.BeginProperty(position, label, property);
            
            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUI.Slider(position, label, property.floatValue, leftValue, rightValue);
            
            if (EditorGUI.EndChangeCheck())
                property.floatValue = newValue;
            
            EditorGUI.EndProperty();
        }
    }
}