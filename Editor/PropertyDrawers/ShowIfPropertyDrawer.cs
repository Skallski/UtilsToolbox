using SkalluUtils.PropertyAttributes;
using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfPropertyDrawer : PropertyDrawer
    {
        private ShowIfAttribute _showIf;
        private SerializedProperty _comparedField;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return Show(property)
                ? base.GetPropertyHeight(property, label)
                : 0f;
        }

        private bool Show(SerializedProperty property)
        {
            _showIf = attribute as ShowIfAttribute;

            if (_showIf == null)
            {
                return false;
            }
            
            string path = property.propertyPath.Contains(".")
                ? System.IO.Path.ChangeExtension(property.propertyPath, _showIf.PropertyName)
                : _showIf.PropertyName;

            _comparedField = property.serializedObject.FindProperty(path);

            if (_comparedField == null)
            {
                Debug.LogError($"Cannot find property with name: {path}");
                return true;
            }

            object comparedValue = _showIf.Value;

            switch (_comparedField.type)
            {
                case "int":
                    return _comparedField.intValue.Equals(comparedValue);
                case "float":
                    return _comparedField.floatValue.Equals(comparedValue);
                case "bool":
                    return _comparedField.boolValue.Equals(comparedValue);
                case "Enum":
                    return _comparedField.enumValueIndex.Equals((int) comparedValue);
                default:
                    Debug.LogError($"{_comparedField.type} is not supported of {path}");
                    return true;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (Show(property))
            {
                EditorGUI.PropertyField(position, property, new GUIContent(property.displayName), true);
            }
        }
    }
}