using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SkalluUtils.Utils.Pooling
{
    public class ObjectPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        private readonly GameObject _prefab;
        private readonly Stack<T> _pooledObjects = new Stack<T>();

        /// <summary>
        /// Creates object pool of selected poolable object
        /// </summary>
        /// <param name="pooledObject"> object on which the pool will be created</param>
        /// <param name="quantityToSpawn"> pool size </param>
        /// <param name="parent"></param>
        public ObjectPool(GameObject pooledObject, int quantityToSpawn = 0, Transform parent = null)
        {
            _prefab = pooledObject;
            SpawnObjects(quantityToSpawn, parent);
        }

        /// <summary>
        /// Creates object pool of selected poolable object
        /// </summary>
        /// <param name="pooledComponentObject"> object on which the pool will be created</param>
        /// <param name="quantityToSpawn"> pool size </param>
        /// <param name="parent"></param>
        public ObjectPool(Component pooledComponentObject, int quantityToSpawn = 0, Transform parent = null)
        {
            _prefab = pooledComponentObject.gameObject;
            SpawnObjects(quantityToSpawn, parent);
        }
        
        /// <summary>
        /// Creates object pool from list of existing poolable objects
        /// </summary>
        /// <param name="objects"> list of objects from which the pool will be created </param>
        /// <exception cref="Exception"></exception>
        public ObjectPool(IReadOnlyList<T> objects)
        {
            var len = objects.Count;

            if (len < 1)
            {
                throw new Exception("Object pool list is empty!");
            }
            
            _prefab = objects[0].gameObject;
                
            for (int i = 0; i < len; i++)
            {
                var obj = objects[i];

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
                T tComponent = Object.Instantiate(_prefab, parent).GetComponent<T>();
                _pooledObjects.Push(tComponent);
                tComponent.gameObject.SetActive(false);
            }
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
                : Object.Instantiate(_prefab).GetComponent<T>();

            var obj = tComponent.gameObject;
            obj.SetActive(true);
            obj.transform.SetParent(parent);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            
            tComponent.Initialize(Push);

            return tComponent;
        }

        /// <summary>
        /// Pushes object to pool
        /// </summary>
        /// <param name="t"> poolable object to push </param>
        private void Push(T t)
        {
            _pooledObjects.Push(t);
            t.gameObject.SetActive(false);
        }
    }
}