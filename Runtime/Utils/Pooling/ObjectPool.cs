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
        private readonly T _prefab;
        private readonly Stack<T> _pooledObjects = new Stack<T>();

        /// <summary>
        /// Creates object pool of selected poolable object
        /// </summary>
        /// <param name="pooledObject"> object on which the pool will be created</param>
        /// <param name="quantityToSpawn"> pool size </param>
        /// <param name="parent"></param>
        public ObjectPool(T pooledObject, int quantityToSpawn = 0, Transform parent = null)
        {
            _prefab = pooledObject;
            SpawnObjects(quantityToSpawn, parent);
        }

        /// <summary>
        /// Creates object pool from list of existing poolable objects
        /// </summary>
        /// <param name="objects"> list of objects from which the pool will be created </param>
        /// <exception cref="Exception"></exception>
        public ObjectPool(IReadOnlyList<T> objects)
        {
            int len = objects.Count;
            if (len < 1)
            {
                throw new Exception("Object pool list is empty!");
            }
            
            _prefab = objects[0]; // get first object in pool as prefab
                
            for (int i = 0; i < len; i++)
            {
                T obj = objects[i];
                _pooledObjects.Push(obj);
                obj.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Spawns objects
        /// </summary>
        /// <param name="quantity"> number of objects to spawn </param>
        /// <param name="parent"> parent of the object that will be spawned </param>
        private void SpawnObjects(int quantity, Transform parent = null)
        {
            for (int i = 0; i < quantity; i++)
            {
                T tComponent = SpawnSingleObject(parent);
                _pooledObjects.Push(tComponent);
                tComponent.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Spawns single object
        /// </summary>
        /// <param name="parent"> parent of the object that will be spawned </param>
        /// <returns> prefab clone </returns>
        private T SpawnSingleObject(Transform parent = null)
        {
            if (_prefab == null)
            {
                throw new Exception("Prefab inside object pool is null!");
            }
            
            return Object.Instantiate(_prefab, parent);
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
            T tComponent = _pooledObjects.Count > 0 
                ? _pooledObjects.Pop() 
                : SpawnSingleObject(parent);
            
            // set transform and set active
            Transform objTransform = tComponent.transform;
            objTransform.SetParent(parent);
            objTransform.position = position;
            objTransform.rotation = rotation;
            tComponent.gameObject.SetActive(true);
            
            tComponent.Pull(Push);

            return tComponent;
        }

        /// <summary>
        /// Pushes object back to pool
        /// </summary>
        /// <param name="poolableObject"> poolable object to push </param>
        public void Push(T poolableObject)
        {
            poolableObject.gameObject.SetActive(false);
            _pooledObjects.Push(poolableObject);
        }
    }
}