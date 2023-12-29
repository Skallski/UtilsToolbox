using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SkalluUtils.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkalluUtils.Tools
{
    public class ComponentsFinder : ResizableWindowBase
    {
        private string _searchQuery = "";
        private List<Type> _searchResults;
        private Type _selectedComponent;
        
        private Vector2 _scrollPositionComponents;
        private Vector2 _scrollPositionRelatedObjects;
        
        private int _selectedLeftButtonIndex = -1;
        private int _selectedRightButtonIndex = -1;
        
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
            EditorGUI.BeginChangeCheck();
            _searchQuery = EditorGUILayout.TextField(_searchQuery, GUILayout.ExpandWidth(true));
            if (EditorGUI.EndChangeCheck())
            {
                UpdateSearchField();
            }

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

        private void DisplaySearchResults()
        {
            if (_searchResults == null)
            {
                return;
            }
            
            if (_searchQuery.Length == 0)
            {
                return;
            }

            for (var i = 0; i < _searchResults.Count; i++)
            {
                var type = _searchResults[i];
                
                if (_selectedLeftButtonIndex == i)
                {
                    GUI.backgroundColor = Color.yellow;
                }

                if (GUILayout.Button($"{type.Name}"))
                {
                    _selectedComponent = type;

                    _selectedLeftButtonIndex = i;
                    _selectedRightButtonIndex = -1;
                }
                
                GUI.backgroundColor = Color.white;
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

            GameObject[] sceneObjects = SceneManager.GetActiveScene().GetAllObjects();
            for (var i = 0; i < sceneObjects.Length; i++)
            {
                var sceneObject = sceneObjects[i];
                
                // Check if the GameObject has the selected component
                if (sceneObject.GetComponent(_selectedComponent) == null)
                {
                    continue;
                }

                found = true;
                if (_selectedRightButtonIndex == i)
                {
                    GUI.backgroundColor = Color.yellow;
                }

                if (GUILayout.Button($"{sceneObject.name}: {_selectedComponent.Name}"))
                {
                    EditorGUIUtility.PingObject(sceneObject);
                    Selection.activeObject = sceneObject;

                    _selectedRightButtonIndex = i;
                }
                
                GUI.backgroundColor = Color.white;
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
            // reset searched objects and selection
            _searchResults = new List<Type>();
            _selectedComponent = null;
            _selectedLeftButtonIndex = -1;
            _selectedRightButtonIndex = -1;

            // get search results
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