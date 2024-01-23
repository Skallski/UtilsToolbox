using SkalluUtils.Extensions;
using SkalluUtils.PropertyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace SkalluUtils.Utils.ObjectCulling
{
    public class ObjectCuller : MonoBehaviour
    {
        [SerializeField] private CullingBoundingBox _cullingBoundingBox;
        [SerializeField] private bool _isObjectMovable;
        [SerializeField, ReadOnly] private bool _isObjectVisible;

        [Space]
        [SerializeField] protected UnityEvent OnBecameVisible;
        [SerializeField] protected UnityEvent OnBecameInvisible;
        
        private Camera _camera;
        
        public void AddCuller(Camera targetCamera)
        {
            _camera = targetCamera == null ? Camera.main : targetCamera;
            _cullingBoundingBox.Setup(transform.position);
            _isObjectVisible = _camera.IsObjectVisible(_cullingBoundingBox.Bounds);

            ObjectCullingManager.AddCuller(this);
        }

        public void RemoveCuller()
        {
            ObjectCullingManager.RemoveCuller(this);
        }

        internal void CheckForVisibility()
        {
            if (_isObjectMovable)
            {
                _cullingBoundingBox.Setup(transform.position);
            }
        
            if (_camera.IsObjectVisible(_cullingBoundingBox.Bounds))
            {
                if (_isObjectVisible)
                {
                    return;
                }

                OnBecameVisible?.Invoke();
                _isObjectVisible = true;
            }
            else
            {
                if (!_isObjectVisible)
                {
                    return;
                }

                OnBecameInvisible?.Invoke();
                _isObjectVisible = false;
            }
        }

        private void OnDrawGizmosSelected()
        {
            _cullingBoundingBox.DrawBoundingBox(transform.position);
        }
    }
} 
