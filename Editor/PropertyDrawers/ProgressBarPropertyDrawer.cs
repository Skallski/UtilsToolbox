using SkalluUtils.PropertyAttributes;
using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(ProgressBarAttribute))]
    public class ProgressBarPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (attribute is not ProgressBarAttribute progressBarAttribute)
            {
                return;
            }

            Color oldBackgroundColor = GUI.backgroundColor;
            
            float maxValue = progressBarAttribute.MaxValue;
                
            string name = string.IsNullOrEmpty(progressBarAttribute.Name)
                ? property.displayName
                : progressBarAttribute.Name;
            
            GUI.backgroundColor = progressBarAttribute.Color;

            switch (property.propertyType)
            {
                case SerializedPropertyType.Float:
                {
                    float value = property.floatValue;
                    EditorGUI.ProgressBar(position, value / maxValue, $"{name} = {value} / {maxValue}");
                  
                    break;
                }
                case SerializedPropertyType.Integer:
                {
                    int value = property.intValue;
                    EditorGUI.ProgressBar(position, value / maxValue, $"{name} = {value} / {maxValue}");

                    break;
                }
            }
            
            GUI.backgroundColor = oldBackgroundColor;
        }
    }
}