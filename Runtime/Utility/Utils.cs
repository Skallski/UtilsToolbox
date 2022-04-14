#if UNITY_EDITOR
using UnityEditor;
#endif
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

    namespace Utils
    {
        public static class Utils
        {
            
        }
    }
    
    namespace Extensions
    {
        public static class GameObjectExtensionUtils
        {
            // destroys all components of game object except the one provided as method parameter
            public static void DestroyComponentsExceptProvided(this GameObject gameObject, Component componentToKeep)
            {
                var allComponents = gameObject.GetComponents(typeof(Component));

                foreach (var component in allComponents)
                {
                    if (component != null)
                    {
                        if (component.GetType() != componentToKeep.GetType() && component.GetType() != typeof(Transform)) // Transform component cannot be destroyed
                            Object.Destroy(component);
                    }
                }
            }
            
            // destroys all components of game object except the list of those provided as method parameter
            public static void DestroyComponentsExceptProvided(this GameObject gameObject, List<Component> componentsToKeep)
            {
                var allComponents = gameObject.GetComponents(typeof(Component));

                foreach (var component in allComponents)
                {
                    if (component != null)
                    {
                        if (!componentsToKeep.Contains(component))
                        {
                            if (component.GetType() != typeof(Transform)) // Transform component cannot be destroyed
                                Object.Destroy(component);
                        }
                    }
                }
            }

            // destroys all components of game object
            public static void DestroyAllComponents(this GameObject gameObject)
            {
                var allComponents = gameObject.GetComponents(typeof(Component));
        
                foreach (var component in allComponents)
                {
                    if (component != null)
                    {
                        if (component.GetType() != typeof(Transform)) // Transform component cannot be destroyed
                            Object.Destroy(component);
                    }
                }
            }
        }

        public static class StringExtensionUtils
        {
            // changes color of log message to one provided as method parameter
            //
            // call example: Debug.Log("sample text".Color(Color.blue));
            public static string Color(this string text, Color color)
            {
                return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";
            }
        }
        
    }
    
}