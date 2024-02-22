using SkalluUtils.Utils.Audio;
using UnityEditor;
using UnityEngine;

namespace SkalluUtils.CustomEditors.MultiSoundPlayer
{
    [CustomPropertyDrawer(typeof(SoundClip))]
    public class SoundClipDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded)
            {
                return (EditorGUIUtility.singleLineHeight + 2 * EditorGUIUtility.standardVerticalSpacing) * 4;
            }
            
            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;
            float halfWidth = position.width * 0.5f;
            
            EditorGUI.BeginProperty(position, label, property);
            
            property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, lineHeight),
                property.isExpanded, GetFoldoutLabelContent(property, label));

            if (property.isExpanded)
            {
                EditorGUIUtility.labelWidth = 50;
                
                position.y += lineHeight + spacing; // space

                // audio clip field
                EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, lineHeight),
                    property.FindPropertyRelative("_audioClip"), new GUIContent("SFX"));

                position.y += lineHeight + spacing; // space
            
                // volume and volume range fields
                EditorGUI.PropertyField(new Rect(position.x, position.y, halfWidth, lineHeight),
                    property.FindPropertyRelative("_volume"), new GUIContent("Volume"));

                EditorGUI.PropertyField(new Rect(position.x + halfWidth, position.y, halfWidth, lineHeight),
                    property.FindPropertyRelative("_volumeRandomRange"), 
                    new GUIContent("Range", "Volume random range (0 - 1)"));

                position.y += lineHeight + spacing; // space
            
                // pitch and pitch range fields
                EditorGUI.PropertyField(new Rect(position.x, position.y, halfWidth, lineHeight),
                    property.FindPropertyRelative("_pitch"), new GUIContent("Pitch"));

                EditorGUI.PropertyField(new Rect(position.x + halfWidth, position.y, halfWidth, lineHeight),
                    property.FindPropertyRelative("_pitchRandomRange"),
                    new GUIContent("Range", "Pitch random range (0 - 1)"));
                
                EditorGUIUtility.labelWidth = 0;
            }

            EditorGUI.EndProperty();
        }

        private GUIContent GetFoldoutLabelContent(SerializedProperty property, GUIContent label)
        {
            var index = label.text.Substring(7);
            var clipValue = property.FindPropertyRelative("_audioClip").objectReferenceValue;
            
            return clipValue == null 
                ? new GUIContent("none!", EditorGUIUtility.IconContent("Warning@2x").image)
                : new GUIContent($"{index} : {clipValue.name}");
        }
    }
}