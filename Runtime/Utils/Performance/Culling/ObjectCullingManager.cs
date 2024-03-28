using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkalluUtils.Utils.Performance.Culling
{
    internal class ObjectCullingManager : MonoBehaviour
    {
        [SerializeField, Range(0.01f, 1)] private float _visibilityCheckRate;
        [SerializeField] private Camera _cullingCamera;
        
        private float _timer;
        private HashSet<ObjectCuller> _cullers = new HashSet<ObjectCuller>();

#if UNITY_EDITOR
        private void Reset()
        {
            if (_cullingCamera == null)
            {
                _cullingCamera = Camera.main;
            }
        }
#endif

        private void OnEnable()
        {
            ObjectCuller.OnCullerAdded += AddCuller;
            ObjectCuller.OnCullerRemoved += RemoveCuller;
        }

        private void OnDisable()
        {
            ObjectCuller.OnCullerAdded -= AddCuller;
            ObjectCuller.OnCullerRemoved -= RemoveCuller;
        }

        private Camera AddCuller(ObjectCuller objectCuller)
        {
            if (objectCuller != null && _cullers.Contains(objectCuller) == false)
            {
                _cullers.Add(objectCuller);
            }

            return _cullingCamera;
        }

        private void RemoveCuller(ObjectCuller objectCuller)
        {
            if (objectCuller != null && _cullers.Contains(objectCuller))
            {
                _cullers.Remove(objectCuller);
            }
        }

        private IEnumerator Start()
        {
            while (true)
            {
                _timer += Time.deltaTime;
                if (_timer >= _visibilityCheckRate)
                {
                    foreach (var culler in _cullers)
                    {
                        culler.CheckForVisibility();
                    }
                    
                    _timer = 0;
                    yield return null;
                }
            }
        }
    }
}