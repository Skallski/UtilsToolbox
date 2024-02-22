using System;
using SkalluUtils.Extensions;
using SkalluUtils.PropertyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace SkalluUtils.Utils.Optimization.Culling
{
    public class ObjectCuller : MonoBehaviour
    {
        internal static Func<ObjectCuller, Camera> OnCullerAdded;
        internal static Action<ObjectCuller> OnCullerRemoved;
        
        [SerializeField] private CullingBoundingBox _cullingBoundingBox;
        [SerializeField] private bool _isObjectMovable;
        [SerializeField, ReadOnly] private bool _isObjectVisible;

        [Space]
        [SerializeField] private UnityEvent OnBecameVisible;
        [SerializeField] private UnityEvent OnBecameInvisible;
        
        private Camera _camera;

        private void Start()
        {
            AddCuller();
        }

        private void OnDestroy()
        {
            RemoveCuller();
        }

        private void AddCuller()
        {
            _camera = OnCullerAdded?.Invoke(this);

            if (_camera == null)
            {
                return;
            }
            
            _cullingBoundingBox.Setup(transform.position);
            _isObjectVisible = _camera.IsObjectVisible(_cullingBoundingBox.Bounds);
        }

        private void RemoveCuller()
        {
            OnCullerRemoved?.Invoke(this);
        }

        internal void CheckForVisibility()
        {
            if (_isObjectMovable)
            {
                _cullingBoundingBox.Setup(transform.position);
            }
        
            if (_camera.IsObjectVisible(_cullingBoundingBox.Bounds))
            {
                if (_isObjectVisible == false)
                {
                    OnVisible();
                    _isObjectVisible = true;
                }
            }
            else
            {
                if (_isObjectVisible)
                {
                    OnInvisible();
                    _isObjectVisible = false;
                }
            }
        }

        public void OnVisible()
        {
            OnBecameVisible?.Invoke();
        }

        public void OnInvisible()
        {
            OnBecameInvisible?.Invoke();
        }

        private void OnDrawGizmosSelected()
        {
            _cullingBoundingBox.DrawBoundingBox(transform.position);
        }
    }
} 
