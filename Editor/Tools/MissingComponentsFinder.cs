using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Tools
{
    public class MissingComponentsFinder : EditorWindow
    {
        private List<GameObject> _foundObjects = new List<GameObject>();
        private int _selectedButtonIndex = -1;

        private bool _found;

        [MenuItem("SkalluUtils/Tools/Missing Components Finder")]
        public static void ShowWindow()
        {
            GetWindow(typeof(MissingComponentsFinder));
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            if (GUILayout.Button("Find"))
            {
                _foundObjects.Clear(); // Clear the list before searching again
                _found = FindMissing();
            }
            
            EditorGUILayout.Space(10);

            if (_found)
            {
                for (int i = 0; i < _foundObjects.Count; i++)
                {
                    var obj = _foundObjects[i];

                    if (_selectedButtonIndex == i)
                    {
                        GUI.backgroundColor = Color.yellow;
                    }

                    if (GUILayout.Button(obj.name))
                    {
                        EditorGUIUtility.PingObject(obj);
                        Selection.activeGameObject = obj;

                        _selectedButtonIndex = i;
                    }

                    GUI.backgroundColor = Color.white;
                }
            }
            else
            {
                EditorGUILayout.LabelField("No GameObjects with missing components found!",
                    new GUIStyle(GUI.skin.label)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        fontStyle = FontStyle.Bold
                    }, GUILayout.ExpandWidth(true));
            }

            EditorGUILayout.EndVertical();
        }

        private bool FindMissing()
        {
            bool found = false;
            
            _foundObjects.Clear();
            _foundObjects = GetSceneObjectsWithMissingScripts();

            if (_foundObjects.Count > 0)
            {
                found = true;
            }

            return found;
        }
        
        private List<GameObject> GetSceneObjectsWithMissingScripts()
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

            return sceneObjects;
        }
    }
}