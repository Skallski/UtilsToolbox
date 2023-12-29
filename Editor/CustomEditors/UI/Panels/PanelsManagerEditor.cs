using System.Collections.Generic;
using System.Reflection;
using SkalluUtils.Utils.UI.Panels;
using UnityEditor;
using UnityEngine;

namespace SkalluUtils.CustomEditors.UI.Panels
{
    [CustomEditor(typeof(PanelsManager), true)]
    public class PanelsManagerEditor : Editor
    {
        private PanelsManager _panelsManager;
        
        private SerializedProperty _activePanel;
        private SerializedProperty _homePanel;
        private SerializedProperty _panels;
        
        private readonly HashSet<string> _propertyNamesToExclude = 
            new HashSet<string>() { "m_Script", "_activePanel", "_homePanel", "_panels"};

        private void OnEnable()
        {
            _panelsManager = target as PanelsManager;
            
            _activePanel = serializedObject.FindProperty("_activePanel");
            _homePanel = serializedObject.FindProperty("_homePanel");
            _panels = serializedObject.FindProperty("_panels");
        }

        public override void OnInspectorGUI()
        {
            if (_panelsManager == null)
            {
                return;
            }
            
            serializedObject.Update();
            EditorGUILayout.BeginVertical();

            // show active panel
            GUI.enabled = false;
            EditorGUILayout.PropertyField(_activePanel);
            GUI.enabled = true;
            
            EditorGUILayout.Space();
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1), new Color(0f, 0f, 0f, 0.3f));
            EditorGUILayout.Space();
            
            // show home panel and panels stack
            GUI.enabled = !Application.isPlaying;
            EditorGUILayout.PropertyField(_homePanel);
            EditorGUILayout.PropertyField(_panels);
            GUI.enabled = true;
            
            EditorGUILayout.Space();
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1), new Color(0f, 0f, 0f, 0.3f));
            EditorGUILayout.Space();

            // show button that switches to selected panel
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("Switch To Panel"))
            {
                ShowGenericMenu();
            }
            GUI.enabled = true;

            // show button that closes active panel
            if (_panelsManager.ActivePanel != null)
            {
                if (GUILayout.Button("Close Active Panel"))
                {
                    _panelsManager.GetType()
                        ?.GetMethod("ClosePanel", BindingFlags.Instance | BindingFlags.NonPublic)
                        ?.Invoke(_panelsManager, new object[] {_panelsManager.ActivePanel});
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
            var menu = new GenericMenu();
            var panels = _panelsManager.Panels;
            
            for (int i = 0, c = panels.Count; i < c; i++)
            {
                var index = i;
                var panel = panels[i];
                
                menu.AddItem(panel == null 
                        ? new GUIContent($"{index}: _") 
                        : new GUIContent($"{index}: {panel.gameObject.name}"),
                    false,
                    () => OnOptionSelected(index));
            }
            
            menu.ShowAsContext();
        }

        private void OnOptionSelected(int index)
        {
            _panelsManager.SwitchToPanel(_panelsManager.Panels[index]);
        }
    }
}