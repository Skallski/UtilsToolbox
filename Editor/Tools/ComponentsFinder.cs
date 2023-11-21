using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Tools
{
    public class ComponentsFinder : ResizableWindowBase
    {
        private string _searchQuery = "";
        private List<Type> _searchResults;
        private Type _selectedComponent;
        
        private Vector2 _scrollPositionComponents;
        private Vector2 _scrollPositionRelatedObjects;

        [MenuItem("SkalluUtils/Tools/Components Finder")]
        private static void OpenWindow()
        {
            var window = GetWindow<ComponentsFinder>();
            window.titleContent = new GUIContent("Components Finder");
            window.Show();
        }
        
        private void OnEnable()
        {
            minSize = new Vector2(600, 400);
        }

        protected override void OnGUILeftSide()
        {
            EditorGUILayout.BeginVertical();
            
            EditorGUILayout.LabelField("Component:", 
                new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            }, GUILayout.ExpandWidth(true));
            
            EditorGUILayout.Space();
            
            // Search field
            _searchQuery = EditorGUILayout.TextField(_searchQuery, GUILayout.ExpandWidth(true));

            // Display search results as buttons
            _scrollPositionComponents = EditorGUILayout.BeginScrollView(_scrollPositionComponents);
            DisplaySearchResults();
            EditorGUILayout.EndScrollView();
            
            EditorGUILayout.EndVertical();
        }

        protected override void OnGuiRightSide()
        {
            EditorGUILayout.BeginVertical();
            
            EditorGUILayout.LabelField("Related objects in scene:",
                new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            }, GUILayout.ExpandWidth(true));
            
            EditorGUILayout.Space();
            
            // Display related objects in scene that has selected component
            _scrollPositionRelatedObjects = EditorGUILayout.BeginScrollView(_scrollPositionRelatedObjects);
            DisplaySceneObjectsWithComponent();
            EditorGUILayout.EndScrollView();
            
            EditorGUILayout.EndVertical();
        }

        private void OnGUI()
        {
            if (Event.current.type == EventType.KeyDown)
            {
                UpdateSearchField();
            }
            
            DrawGUI();
        }

        private void DisplaySearchResults()
        {
            if (_searchResults == null)
            {
                return;
            }

            foreach (var type in _searchResults)
            {
                if (GUILayout.Button($"{type.Name}"))
                {
                    _selectedComponent = type;
                }
            }
        }

        /// <summary>
        /// Displays related objects in scene that has selected component
        /// </summary>
        private void DisplaySceneObjectsWithComponent()
        {
            if (_searchQuery == string.Empty || _selectedComponent == null)
            {
                return;
            }

            bool found = false;

            // Get all GameObjects in the scene
            GameObject[] sceneObjects = FindObjectsOfType<GameObject>();

            foreach (var sceneObject in sceneObjects)
            {
                // Check if the GameObject has the selected component
                if (sceneObject.GetComponent(_selectedComponent) == null)
                {
                    continue;
                }

                found = true;
                if (GUILayout.Button($"{sceneObject.name}: {_selectedComponent.Name}"))
                {
                    Selection.activeObject = sceneObject;
                }
            }

            if (found == false)
            {
                EditorGUILayout.LabelField($"No GameObjects with {_selectedComponent.Name} found!", 
                    new GUIStyle(GUI.skin.label) 
                    { 
                        alignment = TextAnchor.MiddleCenter, 
                        fontStyle = FontStyle.Bold
                    },GUILayout.ExpandWidth(true));
            }
        }

        private void UpdateSearchField()
        {
            if (_searchQuery.Length == 0)
            {
                _searchResults = new List<Type>();
                _selectedComponent = null;
                
                return;
            }

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            _searchResults = assemblies
                .SelectMany(assembly =>
                    assembly.GetTypes()
                        .Where(type =>
                            typeof(MonoBehaviour).IsAssignableFrom(type) && !type.IsAbstract &&
                            type.Name.StartsWith(_searchQuery, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }
    }
}