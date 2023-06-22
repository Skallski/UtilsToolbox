using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Utils.EditorGuiUtils
{
    public static class PropertySliderField
    {
        /// <summary>
        /// Creates slider field for serialized property
        /// </summary>
        /// <param name="property"> serialized property </param>
        /// <param name="leftValue"> min value </param>
        /// <param name="rightValue"> max value </param>
        /// <param name="label"> label (name, tooltip) </param>
        public static void Create(SerializedProperty property, float leftValue, float rightValue, GUIContent label)
        {
            var position = EditorGUILayout.GetControlRect();
            
            label = EditorGUI.BeginProperty(position, label, property);
            
            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUI.Slider(position, label, property.floatValue, leftValue, rightValue);
            
            if (EditorGUI.EndChangeCheck())
                property.floatValue = newValue;
            
            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Creates slider field for serialized property
        /// </summary>
        /// <param name="property"> serialized property </param>
        /// <param name="leftValue"> min value </param>
        /// <param name="rightValue"> max value </param>
        /// <param name="label"> label (name, tooltip) </param>
        public static void Create(SerializedProperty property, int leftValue, int rightValue, GUIContent label)
        {
            var position = EditorGUILayout.GetControlRect();
            
            label = EditorGUI.BeginProperty(position, label, property);
            
            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUI.IntSlider(position, label, property.intValue, leftValue, rightValue);
            
            if (EditorGUI.EndChangeCheck())
                property.intValue = newValue;
            
            EditorGUI.EndProperty();
        }
    }
}