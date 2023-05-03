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
        [SerializeField] private Vector3 _boundingBoxSize;
        [SerializeField] private bool _isObjectMovable;
        [SerializeField, ReadOnly] private bool _isObjectVisible = true;

        private Camera _camera;
        private readonly UnityEvent _onVisible = new UnityEvent();
        private readonly UnityEvent _onInvisible = new UnityEvent();
        private Bounds _bounds;
        
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
                _bounds = new Bounds(transform.position, _boundingBoxSize);
            }

            InvokeRepeating(nameof(CheckForVisibility), 0, _visibilityCheckRate);
        }
        
        private void CheckForVisibility()
        {
            if (_isObjectMovable)
            {
                _bounds = new Bounds(transform.position, _boundingBoxSize);
            }
        
            if (_camera.IsObjectVisible(_bounds))
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
            Gizmos.DrawWireCube(transform.position, _boundingBoxSize);
        }
    }
} 
