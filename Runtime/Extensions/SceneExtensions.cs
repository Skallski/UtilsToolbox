using System.Collections.Generic;
using UnityEngine;

namespace SkalluUtils.Extensions
{
    public static class SceneExtensions
    {
        public static GameObject[] GetAllObjects(this UnityEngine.SceneManagement.Scene scene)
        {
            List<GameObject> sceneObjects = new List<GameObject>();
            Queue<Transform> objectsToProcess = new Queue<Transform>();

            GameObject[] rootObjects = scene.GetRootGameObjects();
            foreach (GameObject rootObject in rootObjects)
            {
                objectsToProcess.Enqueue(rootObject.transform);
            }

            while (objectsToProcess.Count > 0)
            {
                Transform currentObj = objectsToProcess.Dequeue();
                if (currentObj != null && currentObj.gameObject != null)
                {
                    sceneObjects.Add(currentObj.gameObject);

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

        public static T[] GetAllObjectsOfType<T>(this UnityEngine.SceneManagement.Scene scene) where T : Component
        {
            List<T> sceneObjectsWithType = new List<T>();
            Queue<Transform> objectsToProcess = new Queue<Transform>();
            
            GameObject[] rootObjects = scene.GetRootGameObjects();
            foreach (GameObject rootObject in rootObjects)
            {
                objectsToProcess.Enqueue(rootObject.transform);
            }

            while (objectsToProcess.Count > 0)
            {
                Transform currentObj = objectsToProcess.Dequeue();
                if (currentObj != null && currentObj.gameObject != null)
                {
                    // Check if the current object has the specified component
                    T component = currentObj.GetComponent<T>();
                    if (component != null)
                    {
                        sceneObjectsWithType.Add(component);
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

            return sceneObjectsWithType.ToArray();
        }
    }
}