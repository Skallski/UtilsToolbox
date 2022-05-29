#if UNITY_EDITOR
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

namespace SkalluUtils
{
    namespace PropertyAttributes
    {
        namespace ReadOnlyInspectorPropertyAttribute
        {
            [CustomPropertyDrawer(typeof(ReadOnlyInspectorAttribute))]
            public class ReadOnlyInspectorDrawer : PropertyDrawer
            {
                public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
                {
                    GUI.enabled = false;
                    EditorGUI.PropertyField(position, property, label);
                    GUI.enabled = true;
                }
            }
            
            // [ReadOnlyInspector] property attribute
            public class ReadOnlyInspectorAttribute : PropertyAttribute {}
        }
    }
}

#endif