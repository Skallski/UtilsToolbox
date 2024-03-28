using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Editor.EditorWindows
{
    public class TmpFinder : EditorWindow
    {
        private string _searchText = "";
        private List<TextMeshProUGUI> _searchResults = new List<TextMeshProUGUI>();
        private int _selectedButtonIndex = -1;

        [MenuItem("SkalluUtils/Tools/Tmp Finder")]
        public static void ShowWindow()
        {
            var window = GetWindow<TmpFinder>();
            window.titleContent = new GUIContent("Tmp Finder");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Enter Text to Search for in TextMeshProUGUI Component:");
            
            EditorGUI.BeginChangeCheck();
            _searchText = EditorGUILayout.TextArea(_searchText, GUILayout.Height(60));
            if (EditorGUI.EndChangeCheck())
            {
                UpdateSearchResults();
            }

            EditorGUILayout.Space(15);
            ShowSearchResults();
        }
        
        private void UpdateSearchResults()
        {
            _searchResults.Clear();
            _selectedButtonIndex = -1;
        
            if (_searchText.Length < 3)
            {
                return;
            }

            TextMeshProUGUI[] sceneObjectsWithTMP = FindObjectsOfType<TextMeshProUGUI>(true);
            
            _searchResults = sceneObjectsWithTMP
                .Where(textMeshPro => textMeshPro.text.Contains(_searchText))
                .ToList();
        }

        private void ShowSearchResults()
        {
            for (int i = 0; i < _searchResults.Count; i++)
            {
                TextMeshProUGUI tmp = _searchResults[i];

                if (_selectedButtonIndex == i)
                {
                    GUI.backgroundColor = Color.yellow;
                }

                if (GUILayout.Button($"{tmp.text}"))
                {
                    GameObject tmpObject = tmp.gameObject;

                    EditorGUIUtility.PingObject(tmpObject);
                    Selection.activeGameObject = tmpObject;

                    _selectedButtonIndex = i;
                }

                GUI.backgroundColor = Color.white;
            }
        }
    }
}