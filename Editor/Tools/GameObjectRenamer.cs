using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Tools
{
    public class GameObjectRenamer : EditorWindow
    {
        private enum NumerationOrder
        {
            Ascending = 0,
            Descending = 1
        }

        private int _currentToolbarTab;
        private string[] _toolbarHeaders = new string[] { "Rename", "Replace name" };
        
        private string _prefix = "";
        private string _baseName = "";
        private string _suffix = "";
        
        private bool _useAutonumeration;
        private NumerationOrder _numerationOrder;
        private int _startIndex = 1;
        
        private string _strToReplace = "";
        private string _strReplacement = "";

        [MenuItem("SkalluUtils/Tools/GameObject Renamer")]
        private static void OpenWindow()
        {
            var window = GetWindow<GameObjectRenamer>();
            window.titleContent = new GUIContent("GameObject Renamer");
            window.Show();
        }

        private void OnGUI()
        {
            var labelGuiStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            };

            _currentToolbarTab = GUILayout.Toolbar(_currentToolbarTab, _toolbarHeaders);

            if (_currentToolbarTab == 0)
            {
                EditorGUILayout.LabelField("Rename selected GameObjects", 
                    labelGuiStyle, GUILayout.ExpandWidth(true));
                
                EditorGUILayout.Space(5);
                _prefix = EditorGUILayout.TextField("Prefix", _prefix);
                _baseName = EditorGUILayout.TextField("Base Name", _baseName);
                _suffix = EditorGUILayout.TextField("Suffix", _suffix);

                EditorGUILayout.Space(10);
                _useAutonumeration = EditorGUILayout.Toggle("Autonumeration", _useAutonumeration);

                if (_useAutonumeration)
                {
                    _numerationOrder = (NumerationOrder)EditorGUILayout.EnumPopup("Numeration Order", _numerationOrder);
                    _startIndex = EditorGUILayout.IntField("Start Index", _startIndex);
                }

                EditorGUILayout.Space(10);
                GUI.enabled = _baseName != string.Empty || _prefix != string.Empty || _suffix != string.Empty;
                if (GUILayout.Button("Rename GameObjects"))
                {
                    RenameGameObjects();
                }
            
                if (GUI.enabled == false)
                {
                    GUI.enabled = true;
                }
            }
            else
            {
                EditorGUILayout.LabelField("Replace name substrings of selected GameObjects", 
                    labelGuiStyle, GUILayout.ExpandWidth(true));
            
                EditorGUILayout.Space(5);
                _strToReplace = EditorGUILayout.TextField("To Replace:", _strToReplace);
                _strReplacement = EditorGUILayout.TextField("Replacement", _strReplacement);

                EditorGUILayout.Space(10);
                GUI.enabled = _strToReplace != string.Empty && _strReplacement != string.Empty;
                if (GUILayout.Button("Replace Names"))
                {
                    ReplaceNames(_strToReplace, _strReplacement);
                }

                if (GUI.enabled == false)
                {
                    GUI.enabled = true;
                }
            }

            EditorGUILayout.Space(10f);
            EditorGUI.DrawRect(
                EditorGUILayout.GetControlRect(false, 1), new Color(0f, 0f, 0f, 0.3f));
            EditorGUILayout.Space(10f);
            
            EditorGUILayout.HelpBox("This will affect all selected GameObjects in the hierarchy", 
                MessageType.Warning);
        }

        private GameObject[] GetSelectedObjectsSorted(NumerationOrder numerationOrder)
        {
            GameObject[] selectedObjects = Selection.gameObjects;
            return numerationOrder switch
            {
                NumerationOrder.Ascending => selectedObjects.OrderBy(go => go.transform.GetSiblingIndex()).ToArray(),
                NumerationOrder.Descending => selectedObjects.OrderByDescending(go => go.transform.GetSiblingIndex()).ToArray(),
                _ => selectedObjects.OrderBy(go => go.transform.GetSiblingIndex()).ToArray()
            };
        }

        private void RenameGameObjects()
        {
            GameObject[] objects = GetSelectedObjectsSorted(_numerationOrder);
            for (int i = 0, c = objects.Length; i < c; i++)
            {
                GameObject obj = objects[i];
                Undo.RecordObject(obj, "Rename GameObject");
                
                string newName = _useAutonumeration
                    ? $"{_prefix}{_baseName}{_suffix}{_startIndex + i}"
                        : $"{_prefix}{_baseName}{_suffix}";

                obj.name = newName;
            }
        }
        
        private void ReplaceNames(string toReplace, string replacement)
        {
            GameObject[] selectedObjects = Selection.gameObjects;
            foreach (GameObject obj in selectedObjects)
            {
                string newName = obj.name.Replace(toReplace, replacement);
                Undo.RecordObject(obj, "Name Change");
                obj.name = newName;
            }
        }
    }
}