using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SkalluUtils.Utils.Pooling
{
    /// <summary>
    /// Generic object pooling main class
    /// </summary>
    /// <typeparam name="T"> Generic type of poolable object </typeparam>
    public class ObjectPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        private readonly T _prefabInstance;
        private readonly Stack<T> _pooledObjects = new Stack<T>();

        /// <summary>
        /// Creates object pool of selected poolable object
        /// </summary>
        /// <param name="prefab"> object on which the pool will be created</param>
        /// <param name="quantityToSpawn"> pool size </param>
        /// <param name="parent"></param>
        public ObjectPool(T prefab, int quantityToSpawn = 0, Transform parent = null)
        {
            if (prefab == null)
            {
                Debug.LogError("Prefab cannot be null!");
                return;
            }

            _prefabInstance = prefab;
            
            for (int i = 0; i < quantityToSpawn; i++)
            {
                T tComponent = SpawnSingleObject(parent);
                _pooledObjects.Push(tComponent);
                tComponent.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Creates object pool from list of existing poolable objects
        /// </summary>
        /// <param name="objects"> list of objects from which the pool will be created </param>
        /// <exception cref="Exception"></exception>
        public ObjectPool(IReadOnlyList<T> objects)
        {
            if (objects == null || objects.Count < 1)
            {
                Debug.LogError("Object pool list is empty or null!");
                return;
            }

            _prefabInstance = objects[0];

            foreach (T obj in objects)
            {
                _pooledObjects.Push(obj);
                obj.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Spawns single object
        /// </summary>
        /// <param name="parent"> parent of the object that will be spawned </param>
        /// <returns> prefab clone </returns>
        private T SpawnSingleObject(Transform parent = null)
        {
            if (_prefabInstance == null)
            {
                Debug.LogError("Prefab inside object pool is null!");
                return null;
            }

            return Object.Instantiate(_prefabInstance, parent);
        }

        /// <summary>
        /// Pulls object from pool
        /// </summary>
        /// <param name="parent"> parent </param>
        /// <param name="position"> spawn position </param>
        /// <param name="rotation"> spawn rotation </param>
        /// <returns></returns>
        public T Pull(Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            T obj = _pooledObjects.Count > 0 ? _pooledObjects.Pop() : SpawnSingleObject(parent);

            if (obj == null)
            {
                Debug.LogError("Object pulled from the pool is null!");
                return null;
            }

            Transform objTransform = obj.transform;
            objTransform.SetParent(parent);
            objTransform.position = position;
            objTransform.rotation = rotation;
            obj.gameObject.SetActive(true);

            obj.OnPulledFromPool(Push);

            return obj;
        }

        /// <summary>
        /// Pushes object back to pool
        /// </summary>
        /// <param name="obj"> object to push </param>
        public void Push(T obj)
        {
            if (obj == null)
            {
                Debug.LogError("Object to push is null!");
                return;
            }

            obj.gameObject.SetActive(false);
            _pooledObjects.Push(obj);
        }
    }
}