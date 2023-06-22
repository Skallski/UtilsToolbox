using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Utils.EditorGuiUtils
{
    public static class EditorGUILayoutUtils
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
        
        #region PROPERTY SLIDER FIELD
        public static void PropertySliderField(SerializedProperty property, float leftValue, float rightValue, GUIContent label)
        {
            EditorGuiUtils.PropertySliderField.Create(property, leftValue, rightValue, label);
        }
        
        public static void PropertyIntSliderField(SerializedProperty property, int leftValue, int rightValue, GUIContent label)
        {
            EditorGuiUtils.PropertySliderField.Create(property, leftValue, rightValue, label);
        }
        #endregion

        #region HORIZONTAL LINE
        public static void HorizontalLine(Color color, float height, Vector2 margin)
        {
            EditorGuiUtils.HorizontalLine.Create(color, height, margin);
        }

        public static void HorizontalLine(Color color, float height) => HorizontalLine(color, height, default);
        public static void HorizontalLine(Color color, Vector2 margin) => HorizontalLine(color, default, margin);
        public static void HorizontalLine(float height, Vector2 margin) => HorizontalLine(default, height, margin);
        public static void HorizontalLine(Color color) => HorizontalLine(color, default, default);
        public static void HorizontalLine(float height) => HorizontalLine(default, height, default);
        public static void HorizontalLine(Vector2 margin) => HorizontalLine(default, default, margin);
        public static void HorizontalLine() => HorizontalLine(default, default, default);
        #endregion
    }
}