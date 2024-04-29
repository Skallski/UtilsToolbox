using System.Collections.Generic;
using System.Reflection;
using SkalluUtils.Utils.UI.UiPanel;
using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Editor.CustomEditors.UI.UiPanel
{
    [CustomEditor(typeof(UiPanelsManager), true)]
    public class UiPanelsManagerEditor : UnityEditor.Editor
    {
        private UiPanelsManager _uiPanelsManager;
        
        private SerializedProperty _activePanel;
        private SerializedProperty _homePanel;
        private SerializedProperty _panels;
        
        private readonly HashSet<string> _propertyNamesToExclude = 
            new HashSet<string>() { "m_Script", "_activePanel", "_homePanel", "_panels"};

        private void OnEnable()
        {
            _uiPanelsManager = target as UiPanelsManager;
            
            _activePanel = serializedObject.FindProperty("_activePanel");
            _homePanel = serializedObject.FindProperty("_homePanel");
            _panels = serializedObject.FindProperty("_panels");
        }

        public override void OnInspectorGUI()
        {
            if (_uiPanelsManager == null)
            {
                return;
            }
            
            serializedObject.Update();
            EditorGUILayout.BeginVertical();

            // show active panel
            GUI.enabled = false;
            EditorGUILayout.PropertyField(_activePanel);
            GUI.enabled = true;

            // show home panel
            EditorGUILayout.PropertyField(_homePanel);
            
            EditorGUILayout.Space();
            if (Application.isPlaying == false)
            {
                // show panels list
                EditorGUILayout.PropertyField(_panels);
            }
            else
            {
                // show button that switches to selected panel
                if (GUILayout.Button("Switch To Panel"))
                {
                    ShowGenericMenu();
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
        
        private void ShowGenericMenu()
        {
            if (_uiPanelsManager.GetType()
                    ?.GetField("_panels", BindingFlags.Instance | BindingFlags.NonPublic)
                    ?.GetValue(_uiPanelsManager) is not List<SkalluUtils.Utils.UI.UiPanel.UiPanel> panels)
            {
                return;
            }
            
            GenericMenu menu = new GenericMenu();
            
            for (int i = 0, c = panels.Count; i < c; i++)
            {
                int index = i;
                SkalluUtils.Utils.UI.UiPanel.UiPanel panel = panels[i];

                menu.AddItem(panel == null
                        ? new GUIContent($"{index}: _")
                        : new GUIContent($"{index}: {panel.gameObject.name}"),
                    false,
                    () => _uiPanelsManager.SwitchToPanel(panel));
            }
            
            menu.ShowAsContext();
        }
    }
}