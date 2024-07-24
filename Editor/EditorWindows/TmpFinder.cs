using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Editor.EditorWindows
{
    public class TmpFinder : EditorWindowBase
    {
        private string _searchText = "";
        private List<TextMeshProUGUI> _searchResults = new List<TextMeshProUGUI>();
        private int _selectedButtonIndex = -1;

        protected override void SetSize()
        {
            minSize = new Vector2(460, 460);
        }

        private void OnGUI()
        {
            GUILayout.Label("Search for a TextMeshProUGUI component in active scene by entering the text");
            
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
            if (_searchResults.Count > 0)
            {
                EditorGUILayout.LabelField("Results:");
                EditorGUILayout.Space();
                
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
            else
            {
                if (_searchText.Length >= 3)
                {
                    EditorGUILayout.LabelField("No results found");
                }
            }
        }
    }
}