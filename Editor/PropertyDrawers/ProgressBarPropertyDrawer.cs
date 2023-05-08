using SkalluUtils.PropertyAttributes;
using UnityEditor;
using UnityEngine;

namespace SkalluUtils.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(ProgressBarAttribute))]
    public class ProgressBarPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!(attribute is ProgressBarAttribute progressBarAttribute))
            {
                return;
            }
            
            var maxValue = progressBarAttribute.MaxValue;
                
            var name = progressBarAttribute.Name == string.Empty
                ? property.displayName
                : progressBarAttribute.Name;
            
            GUI.color = progressBarAttribute.Color;

            switch (property.propertyType)
            {
                case SerializedPropertyType.Float:
                {
                    var value = property.floatValue;
                    EditorGUI.ProgressBar(position, value / maxValue, $"{name} = {value} / {maxValue}");
                  
                    break;
                }
                case SerializedPropertyType.Integer:
                {
                    var value = property.intValue;
                    EditorGUI.ProgressBar(position, value / maxValue, $"{name} = {value} / {maxValue}");

                    break;
                }
            }
            
            GUI.color = Color.white;
        }
    }
}