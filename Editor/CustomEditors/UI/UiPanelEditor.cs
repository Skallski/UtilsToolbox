using System.Collections.Generic;
using SkalluUtils.Utils.UI;
using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Editor.CustomEditors.UI
{
    [CustomEditor(typeof(UiPanel), true)]
    public class UiPanelEditor : UnityEditor.Editor
    {
        private UiPanel _panel;

        private SerializedProperty _content;
        private SerializedProperty _background;
        private SerializedProperty _opened;
        private SerializedProperty _closed;

        private bool _eventsUnfolded;
        private Color _oldGuiBackgroundColor;
        
        private readonly HashSet<string> _propertyNamesToExclude = 
            new HashSet<string>() { "m_Script", "_content", "_background", "_opened", "_closed"};

        protected virtual void OnEnable()
        {
            _panel = target as UiPanel;

            SetupProperties();
        }

        protected virtual void SetupProperties()
        {
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
            
            serializedObject.Update();
            EditorGUILayout.BeginVertical();
            
            DrawBase();
            EditorGUILayout.Space();
            
            DrawEvents();
            EditorGUILayout.Space();
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1), new Color(0f, 0f, 0f, 0.3f));
            EditorGUILayout.Space();

            DrawInheritorsFields(_propertyNamesToExclude);

            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawBase()
        {
            _oldGuiBackgroundColor = GUI.backgroundColor;
            
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
        }

        protected void DrawEvents()
        {
            _eventsUnfolded = EditorGUILayout.BeginFoldoutHeaderGroup(_eventsUnfolded,
                "Panel Events");
            if (_eventsUnfolded)
            {
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(_opened);
                EditorGUILayout.PropertyField(_closed);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        protected void DrawInheritorsFields(HashSet<string> excludedPropertiesNames)
        {
            // workaround to display fields of the inheritors
            SerializedProperty iterator = serializedObject.GetIterator();
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
            {
                if (excludedPropertiesNames.Contains(iterator.name))
                {
                    continue;
                }

                EditorGUILayout.PropertyField(iterator, true);
            }
        }
    }
}