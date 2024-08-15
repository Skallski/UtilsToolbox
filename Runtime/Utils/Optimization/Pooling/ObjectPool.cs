using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UtilsToolbox.Utils.Optimization.Pooling
{
    public class ObjectPool<T> where T : PoolableObject
    {
        private readonly Stack<T> _pooledObjects = new Stack<T>();

        private readonly Func<T> _objectInstantiator;
        private readonly Action<T> _onGet;
        private readonly Action<T> _onRelease;

        public ObjectPool(Func<T> objectInstantiator, int size = 0, Transform parent = null, 
            Action<T> onGet = null, Action<T> onRelease = null)
        {
            if (objectInstantiator == null)
            {
                Debug.LogError("Object instantiator function cannot be null!");
                return;
            }

            _objectInstantiator = objectInstantiator;
            _onGet = onGet;
            _onRelease = onRelease;

            for (int i = 0; i < size; i++)
            {
                InitializeObject(InstantiateObject(parent));
            }
        }
        
        public ObjectPool(IReadOnlyList<T> objects, Action<T> onGet = null, Action<T> onRelease = null)
        {
            if (objects == null || objects.Count == 0)
            {
                Debug.LogError("Object pool cannot be empty or null!");
                return;
            }

            _objectInstantiator = () => objects[0];
            _onGet = onGet;
            _onRelease = onRelease;

            foreach (T obj in objects)
            {
                InitializeObject(obj);
            }
        }

        /// <summary>
        /// Initializes object
        /// </summary>
        /// <param name="obj"></param>
        private void InitializeObject(T obj)
        {
            if (obj == null)
            {
                Debug.LogError("Object to initialize cannot be null!");
                return;
            }
            
            obj.OnReleased = ReleaseObject;
            _pooledObjects.Push(obj);
            obj.gameObject.SetActive(false);
        }

        /// <summary>
        /// Instantiates single object
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        private T InstantiateObject(Transform parent = null)
        {
            T obj = _objectInstantiator?.Invoke();
            
            if (obj == null)
            {
                Debug.LogError("Instantiated object cannot be null!");
                return null;
            }

            return Object.Instantiate(obj, parent);
        }

        /// <summary>
        /// Gets object from pool
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public T GetObject(Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            T obj = _pooledObjects.Count > 0 
                ? _pooledObjects.Pop() 
                : InstantiateObject(parent);

            if (obj == null)
            {
                Debug.LogError("Object that has been get from pool cannot be null!");
                return null;
            }

            Transform objTransform = obj.transform;
            objTransform.SetParent(parent);
            objTransform.position = position;
            objTransform.rotation = rotation;
            
            obj.gameObject.SetActive(true);
            
            obj.Get();
            _onGet?.Invoke(obj);

            return obj;
        }

        /// <summary>
        /// Returns object to pool
        /// </summary>
        /// <param name="obj"></param>
        private void ReleaseObject(PoolableObject obj)
        {
            if (obj == null)
            {
                Debug.LogError("Object to release cannot be null!");
                return;
            }

            obj.gameObject.SetActive(false);

            T t = obj as T;
            
            _onRelease?.Invoke(t);
            _pooledObjects.Push(t);
        }
    }
}