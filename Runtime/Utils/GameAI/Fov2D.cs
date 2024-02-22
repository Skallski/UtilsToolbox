using UnityEngine;

namespace SkalluUtils.Utils.GameAI
{
    public class Fov2D : MonoBehaviour
    {
        [SerializeField] private GameObject _targetObject;
        [SerializeField, Range(0f, 0.5f)] private float _scanRate = 0.02f;

        [SerializeField] private bool _seeThroughObstacles;
        [SerializeField] private LayerMask _obstacleLayer;

        [SerializeField, Range(0f, 20f)] private float _radius;
        [SerializeField, Range(0, 360)] private int _angle;

#if UNITY_EDITOR
        public enum HandlesType
        {
            Solid,
            Wire
        }

        [SerializeField] private HandlesType _fovHandlesType = HandlesType.Wire;
        [SerializeField] private bool _useCustomColor;
        [SerializeField] private Color _customColor = Color.white;
#endif

        private Transform _transform;

        public bool TargetInRange { get; internal set; }
        public bool TargetSpotted { get; internal set; }
        
        private void Awake()
        {
            _transform = transform;
        }

        public void Setup(GameObject targetObject, float scanRate)
        {
            if (targetObject == null)
            {
                return;
            }

            _targetObject = targetObject;
            _scanRate = scanRate;

            CancelInvoke(nameof(Scan));
            InvokeRepeating(nameof(Scan), 0, _scanRate);
        }
        
        public void Setup(GameObject targetObject)
        {
            Setup(targetObject, _scanRate);
        }

        public void Scan()
        {
            var offset = _targetObject.transform.position - _transform.position;
            var sqrDistanceToTarget = offset.sqrMagnitude;

            if (sqrDistanceToTarget <= _radius * _radius)
            {
                TargetInRange = true;
                
                var directionToTarget = offset.normalized;

                if (Vector3.Angle(_transform.right, directionToTarget) < _angle * 0.5f)
                {
                    if (_seeThroughObstacles)
                    {
                        TargetSpotted = true;
                    }
                    else
                    {
                        TargetSpotted = !Physics2D.Raycast(_transform.position, directionToTarget, 
                            sqrDistanceToTarget * sqrDistanceToTarget, _obstacleLayer);
                    }
                }
                else
                {
                    TargetSpotted = false;
                }
            }
            else
            {
                TargetInRange = false;
                TargetSpotted = false;
            }
        }
    }
}