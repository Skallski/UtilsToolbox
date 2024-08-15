using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UtilsToolbox.Editor.EditorWindows
{
    public class ComponentsFinder : ResizableWindowBase
    {
        private int _currentToolbarTab;
        private string[] _toolbarHeaders = new string[] { "Search by component", "Find all missing" };
        
        private string _searchQuery = "";
        private List<Type> _searchResults;
        private Type _selectedComponent;
        
        private Vector2 _scrollPositionComponents;
        private Vector2 _scrollPositionRelatedObjects;
        
        private int _selectedLeftButtonIndex = -1;
        private int _selectedRightButtonIndex = -1;
        
        private GUIStyle _labelStyle;

        protected override float LeftPanelWidthMin => 275;
        protected override float RightPanelWidthMin => 450;

        protected override void DrawGUI()
        {
            _labelStyle = new GUIStyle(GUI.skin.label) 
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            };
            
            base.DrawGUI();
        }

        protected override void OnGUILeftSide()
        {
            EditorGUILayout.BeginVertical();

            EditorGUI.BeginChangeCheck();
            _currentToolbarTab = GUILayout.Toolbar(_currentToolbarTab, _toolbarHeaders);
            if (EditorGUI.EndChangeCheck())
            {
                OnToolbarTabChanged();
            }
            
            EditorGUILayout.Space();

            if (_currentToolbarTab == 0)
            {
                EditorGUILayout.LabelField("Component:", _labelStyle, GUILayout.ExpandWidth(true));
                EditorGUILayout.Space();
            
                // Search field
                EditorGUI.BeginChangeCheck();
                _searchQuery = EditorGUILayout.TextField(_searchQuery, GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck())
                {
                    OnSearchTextUpdated();
                }

                // Display search results as buttons
                _scrollPositionComponents = EditorGUILayout.BeginScrollView(_scrollPositionComponents);
                DisplaySearchResults();
                EditorGUILayout.EndScrollView();
            }
            else
            {
                EditorGUILayout.HelpBox("All GameObjects in the active scene that have missing components will be displayed in the right panel",
                    MessageType.Info);
            }

            EditorGUILayout.EndVertical();
        }

        protected override void OnGuiRightSide()
        {
            EditorGUILayout.BeginVertical();
            
            EditorGUILayout.LabelField("Related objects in scene:", _labelStyle, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            
            // Display related objects in scene that has selected component
            _scrollPositionRelatedObjects = EditorGUILayout.BeginScrollView(_scrollPositionRelatedObjects);
            DisplaySceneObjectsWithComponent();
            EditorGUILayout.EndScrollView();
            
            EditorGUILayout.EndVertical();
        }

        private void DisplaySearchResults()
        {
            if (_searchResults == null || _searchQuery.Length == 0)
            {
                return;
            }

            for (var i = 0; i < _searchResults.Count; i++)
            {
                Type type = _searchResults[i];
                int index = i;
                ShowSelectionButton(ref _selectedLeftButtonIndex, index, type.Name, () =>
                {
                    _selectedComponent = type;
                    _selectedRightButtonIndex = -1; // de-highlight right button when left is clicked
                });
            }
        }

        /// <summary>
        /// Displays related objects in scene that has selected component
        /// </summary>
        private void DisplaySceneObjectsWithComponent()
        {
            if (_currentToolbarTab == 0)
            {
                if (_searchQuery == string.Empty || _selectedComponent == null)
                {
                    return;
                }

                bool found = false;

                GameObject[] sceneObjects = FindObjectsOfType<GameObject>(true);
                for (var i = 0; i < sceneObjects.Length; i++)
                {
                    GameObject sceneObject = sceneObjects[i];
                
                    // Check if the GameObject has the selected component
                    if (sceneObject.GetComponent(_selectedComponent) == null)
                    {
                        continue;
                    }

                    found = true;
                    ShowSelectionButton(ref _selectedRightButtonIndex, i, 
                        $"{sceneObject.name}: {_selectedComponent.Name}", () =>
                    {
                        EditorGUIUtility.PingObject(sceneObject);
                        Selection.activeObject = sceneObject;
                    });
                }
                
                if (found == false)
                {
                    EditorGUILayout.LabelField($"No GameObjects with {_selectedComponent.Name} has been found in active scene.", 
                        _labelStyle,GUILayout.ExpandWidth(true));
                }
            }
            else
            {
                GameObject[] objectsWithMissingScripts = GetSceneObjectsWithMissingScripts();
                int length = objectsWithMissingScripts.Length;
                
                if (length > 0)
                {
                    for (int i = 0; i < length; i++)
                    {
                        GameObject obj = objectsWithMissingScripts[i];
                        ShowSelectionButton(ref _selectedRightButtonIndex,i, obj.name, () =>
                        {
                            EditorGUIUtility.PingObject(obj);
                            Selection.activeObject = obj;
                        });
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("No GameObjects with missing components has been found in active scene.",
                        _labelStyle, GUILayout.ExpandWidth(true));
                }
            }
        }

        private void ShowSelectionButton(ref int buttonIndex, int selectedIndex, string buttonName, Action onSelected)
        {
            if (buttonIndex == selectedIndex)
            {
                GUI.backgroundColor = Color.yellow;
            }

            if (GUILayout.Button(buttonName))
            {
                onSelected?.Invoke();
                buttonIndex = selectedIndex;
            }
                
            GUI.backgroundColor = Color.white;
        }

        private void OnToolbarTabChanged()
        {
            ResetSelection();
            
            _searchQuery = string.Empty;
            GUIUtility.keyboardControl = 0;
        }

        private void OnSearchTextUpdated()
        {
            ResetSelection();

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

        private void ResetSelection()
        {
            // reset searched objects and selection
            _searchResults = new List<Type>();
            _selectedComponent = null;
            _selectedLeftButtonIndex = -1;
            _selectedRightButtonIndex = -1;
        }
        
        private GameObject[] GetSceneObjectsWithMissingScripts()
        {
            List<GameObject> sceneObjects = new List<GameObject>();
            Queue<Transform> objectsToProcess = new Queue<Transform>();
            
            GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject rootObject in rootObjects)
            {
                objectsToProcess.Enqueue(rootObject.transform);
            }

            while (objectsToProcess.Count > 0)
            {
                Transform currentObj = objectsToProcess.Dequeue();
                if (currentObj != null && currentObj.gameObject != null)
                {
                    // Check for missing components (scripts)
                    Component[] components = currentObj.GetComponents<Component>();
                    foreach (Component component in components)
                    {
                        if (component == null)
                        {
                            sceneObjects.Add(currentObj.gameObject);
                            break; // Add the object once if any missing component is found
                        }
                    }

                    int childCount = currentObj.childCount;
                    for (int i = 0; i < childCount; i++)
                    {
                        Transform child = currentObj.GetChild(i);
                        if (child != null)
                        {
                            objectsToProcess.Enqueue(child);
                        }
                    }
                }
            }

            return sceneObjects.ToArray();
        }
    }
}