using System.Collections.Generic;
using SkalluUtils.Utils.UI.PanelSwitcher;
using UnityEditor;
using UnityEngine;

namespace SkalluUtils.CustomEditors.UI.PanelSwitcher
{
    [CustomEditor(typeof(Panel), true)]
    public class PanelEditor : Editor
    {
        private Panel _panel;
        
        private SerializedProperty _content;
        private SerializedProperty _background;
        private SerializedProperty _opened;
        private SerializedProperty _closed;

        private bool _eventsUnfolded;
        
        private Color _oldGuiBackgroundColor;
        
        private readonly HashSet<string> _propertyNamesToExclude = 
            new HashSet<string>() { "m_Script", "_content", "_background", "_opened", "_closed"};

        private void OnEnable()
        {
            _panel = target as Panel;

            _content = serializedObject.FindProperty("_content");
            _background = serializedObject.FindProperty("_background");
            _opened = serializedObject.FindProperty("_opened");
            _closed = serializedObject.FindProperty("_closed");
        }

        public override void OnInspectorGUI()
        {
            if (_panel == null)
            {
                return;
            }

            _oldGuiBackgroundColor = GUI.backgroundColor;
            
            serializedObject.Update();
            EditorGUILayout.BeginVertical();
            
            // validate content field
            if (_content.objectReferenceValue == null)
            {
                GUI.backgroundColor = Color.red;
                EditorGUILayout.PropertyField(_content);
                GUI.backgroundColor = _oldGuiBackgroundColor;
                
                EditorGUILayout.LabelField(
                    new GUIContent("Null reference!", EditorGUIUtility.IconContent("Error@2x").image),
                    new GUIStyle(EditorStyles.helpBox)
                    {
                        fixedHeight = 30,
                        fontSize = 10
                    });
                
                EditorGUILayout.Space();
            }
            else
            {
                EditorGUILayout.PropertyField(_content);
            }
            
            EditorGUILayout.PropertyField(_background); // show background field
            EditorGUILayout.Space();
            
            // show events
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1), new Color(0f, 0f, 0f, 0.3f));
            EditorGUILayout.Space();
            
            if (_eventsUnfolded)
            {
                if (GUILayout.Button("Hide Events"))
                {
                    _eventsUnfolded = !_eventsUnfolded;
                }
            
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(_opened);
                EditorGUILayout.PropertyField(_closed);
            }
            else
            {
                if (GUILayout.Button("Show Events"))
                {
                    _eventsUnfolded = !_eventsUnfolded;
                }
            }
            
            EditorGUILayout.Space();
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1), new Color(0f, 0f, 0f, 0.3f));
            EditorGUILayout.Space();

            // workaround to display fields of the inheritors
            SerializedProperty iterator = serializedObject.GetIterator();
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
            {
                if (_propertyNamesToExclude.Contains(iterator.name))
                {
                    continue;
                }

                EditorGUILayout.PropertyField(iterator, true);
            }

            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
    }
}