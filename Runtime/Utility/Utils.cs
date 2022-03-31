using UnityEditor;
using UnityEngine;

namespace Skallu.Utils
{
    namespace PropertyAttributes
    {
        namespace ReadOnlyInspector
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
    
    namespace Tools
    {
        public class Utils : MonoBehaviour
        {
            // destroys all components of game object except the one provided as method parameter
            public static void DestroyComponentsExceptProvided(GameObject gameObject, Component componentToKeep)
            {
                var allComponents = gameObject.GetComponents(typeof(Component));

                foreach (var component in allComponents)
                {
                    if (component != null)
                    {
                        if (component.GetType() != componentToKeep.GetType() && component.GetType() != typeof(Transform)) // Transform component cannot be destroyed
                            Destroy(component);
                    }
                }
            }
    
            // destroys all components of game object
            public static void DestroyComponents(GameObject gameObject)
            {
                var allComponents = gameObject.GetComponents(typeof(Component));
        
                foreach (var component in allComponents)
                {
                    if (component != null)
                    {
                        if (component.GetType() != typeof(Transform)) // Transform component cannot be destroyed
                            Destroy(component);
                    }
                }
            }

            // returns vector2 from angle provided as method parameter
            public static Vector2 ReturnVectorFromAngle(float angle)
            {
                angle = (angle + 90f) * 0.0055555f * Mathf.PI;
        
                return new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
            }
        
        }
    }
    
    
}

