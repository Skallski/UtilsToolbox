using JetBrains.Annotations;
using SkalluUtils.Extensions;
using SkalluUtils.PropertyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace SkalluUtils.Utils.ObjectCulling
{
    public class ObjectCuller : MonoBehaviour
    {
        [SerializeField, Range(0.001f, 1f)] private float _visibilityCheckRate = 0.1f;
        [SerializeField] private CullingBoundingBox _cullingBoundingBox;
        [SerializeField] private bool _isObjectMovable;
        [SerializeField, ReadOnly] private bool _isObjectVisible;
        [Space]
        [SerializeField] private bool _showGizmos = true;

        private Camera _camera;
        private readonly UnityEvent _onVisible = new UnityEvent();
        private readonly UnityEvent _onInvisible = new UnityEvent();

        public void AddCuller([NotNull] ICullable iCullable)
        {
            _onVisible.AddListener(iCullable.OnVisible);
            _onInvisible.AddListener(iCullable.OnInvisible);
        }
        
        public void RemoveCuller([NotNull] ICullable iCullable)
        {
            _onVisible.RemoveListener(iCullable.OnVisible);
            _onInvisible.RemoveListener(iCullable.OnInvisible);
        }
        
        public void Setup(Camera cam)
        {
            if (cam == null)
            {
                Debug.LogError($"{cam} cannot be found!");
                return;
            }

            _camera = cam;
        
            if (!_isObjectMovable)
            {
                _cullingBoundingBox.Setup(transform.position);
            }

            _isObjectVisible = _camera.IsObjectVisible(_cullingBoundingBox.Bounds);

            InvokeRepeating(nameof(CheckForVisibility), 0, _visibilityCheckRate);
        }
        
        private void CheckForVisibility()
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
            if (!_showGizmos)
            {
                return;
            }
            
            _cullingBoundingBox.DrawBoundingBox(transform.position);
        }
    }
} 
