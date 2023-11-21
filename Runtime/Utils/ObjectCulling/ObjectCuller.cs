using System;
using SkalluUtils.Extensions;
using SkalluUtils.PropertyAttributes;
using UnityEngine;

namespace SkalluUtils.Utils.ObjectCulling
{
    public class ObjectCuller : MonoBehaviour
    {
        [SerializeField] private CullingBoundingBox _cullingBoundingBox;
        [SerializeField] private bool _isObjectMovable;
        [SerializeField, ReadOnly] private bool _isObjectVisible;

        private Camera _camera;
        
        private Action _onVisible;
        private Action _onInvisible;

        public void AddCuller(ICullable cullable, Camera targetCamera = null)
        {
            if (cullable == null)
            {
                return;
            }
            
            _camera = targetCamera == null ? Camera.main : targetCamera;
            _cullingBoundingBox.Setup(transform.position);
            _isObjectVisible = _camera.IsObjectVisible(_cullingBoundingBox.Bounds);

            _onVisible = cullable.OnVisible;
            _onInvisible = cullable.OnInvisible;
            
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
                    return;
                
                _onVisible?.Invoke();
                _isObjectVisible = true;
            }
            else
            {
                if (!_isObjectVisible)
                    return;
                
                _onInvisible?.Invoke();
                _isObjectVisible = false;
            }
        }

        private void OnDrawGizmosSelected()
        {
            _cullingBoundingBox.DrawBoundingBox(transform.position);
        }
    }
} 
