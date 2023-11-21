using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Tools
{
    /// <summary>
    /// 1. Cache instance as global variable inside custom editor
    /// 2. Add items inside OnEnable() method
    /// 3. Call Show() inside OnInspectorGUI() method
    /// </summary>
    public sealed class DynamicSearchBar
    {
        private sealed class SearchBarItem
        {
            public readonly GUIContent Content;
            public readonly Action Function;

            internal SearchBarItem(GUIContent content, Action function)
            {
                Content = content;
                Function = function;
            }
        }
        
        private readonly List<SearchBarItem> _items = new List<SearchBarItem>();
        private readonly GUIContent _header;
        
        private string _searchQuery = string.Empty;
        private Vector2 _scrollPosition;

        public DynamicSearchBar(GUIContent header = null)
        {
            _header = header;
        }

        public void AddItem(GUIContent content, Action onSelected = null)
        {
            _items.Add(new SearchBarItem(content, onSelected));
        }

        public void Show()
        {
            if (_header != null)
            {
                EditorGUILayout.LabelField(_header, new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Bold
                });
            }
            
            _searchQuery = EditorGUILayout.TextField(_searchQuery, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            ShowSearchResults();
            EditorGUILayout.EndScrollView();
        }

        private void ShowSearchResults()
        {
            if (_searchQuery.Length == 0)
            {
                return;
            }

            foreach (var item in _items)
            {
                if (item.Content.text.StartsWith(_searchQuery, StringComparison.OrdinalIgnoreCase))
                {
                    if (GUILayout.Button(item.Content))
                    {
                        item.Function?.Invoke();
                    }
                }
            }
        }
    }
}