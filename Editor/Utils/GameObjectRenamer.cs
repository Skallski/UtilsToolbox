using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Utils
{
    public class GameObjectRenamer : EditorWindow
    {
        private enum NumerationOrder
        {
            Ascending = 0,
            Descending = 1
        }
        
        private string _prefix = "";
        private string _baseName = "";
        private string _suffix = "";
        
        private bool _useAutonumeration;
        private NumerationOrder _numerationOrder;
        private int _startIndex = 1;

        [MenuItem("SkalluUtils/Tools/GameObject Renamer")]
        private static void OpenWindow()
        {
            var window = GetWindow<GameObjectRenamer>();
            window.titleContent = new GUIContent("GameObject Renamer");
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.TextField("Rename GameObjects", EditorStyles.boldLabel);
            
            EditorGUILayout.Space(5);
            _prefix = EditorGUILayout.TextField("Prefix", _prefix);
            _baseName = EditorGUILayout.TextField("Base Name", _baseName);
            _suffix = EditorGUILayout.TextField("Suffix", _suffix);

            EditorGUILayout.Space(10);
            _useAutonumeration = EditorGUILayout.Toggle("Autonumeration", _useAutonumeration);

            if (_useAutonumeration)
            {
                _numerationOrder = (NumerationOrder) EditorGUILayout.EnumPopup("Numeration Order", _numerationOrder);
                _startIndex = EditorGUILayout.IntField("Start Index", _startIndex);
            }

            EditorGUILayout.Space(10);
            
            if (GUILayout.Button("Rename GameObjects"))
            {
                RenameGameObjects();
            }
        }

        private GameObject[] GetSelectedObjectsSorted()
        {
            GameObject[] selectedObjects = Selection.gameObjects;

            return _numerationOrder switch
            {
                NumerationOrder.Ascending => selectedObjects.OrderBy(go => go.transform.GetSiblingIndex()).ToArray(),
                NumerationOrder.Descending => selectedObjects.OrderByDescending(go => go.transform.GetSiblingIndex()).ToArray(),
                _ => selectedObjects.OrderBy(go => go.transform.GetSiblingIndex()).ToArray()
            };
        }

        private void RenameGameObjects()
        {
            GameObject[] objects = GetSelectedObjectsSorted();
            
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
    }
}