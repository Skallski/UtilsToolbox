using System.Collections.Generic;
using SkalluUtils.PropertyAttributes;
using UnityEngine;

namespace SkalluUtils.Utils.FovSensorTopDown2D
{
    public class FovSensorTopDown2D : MonoBehaviour
    {
        [SerializeField] private LayerMask _obstacleLayer;
        [SerializeField] private GameObject _targetObject;
        [SerializeField, Range(0f, 0.5f)] private float _scanRate = 0.02f;
        
        [SerializeField, Range(0f, 20f)] private float _radius;
        [SerializeField, Range(0, 360)] private int _angle;
        [SerializeField] private Color _color = Color.white;
        [SerializeField, Range(0.5f, 2f)] private float _thickness = 0.5f;

        [SerializeField] private bool _useAdditionalZones = false;
        [SerializeField] private List<FovZone2D> _additionalZones = new List<FovZone2D>();

        [SerializeField, ReadOnly] private bool _targetInRange;
        [SerializeField, ReadOnly] private bool _targetSpotted;

        public Transform AgentTransform { get; private set; }
        public Transform TargetTransform { get; private set; }

        public bool TargetSpotted
        {
            get => _targetSpotted;
            protected set => _targetSpotted = value;
        }

        public bool UseAdditionalZones => _useAdditionalZones;
        public List<FovZone2D> AdditionalZones => _additionalZones;

        public virtual void Setup(GameObject targetObject)
        {
            if (targetObject == null)
            {
                return;
            }

            _targetObject = targetObject;
            TargetTransform = targetObject.transform;
            AgentTransform = transform;
            
            if (_useAdditionalZones)
            {
                foreach (var zone in _additionalZones)
                {
                    zone.Setup(this);
                }
            }
            
            InvokeRepeating(nameof(Scan), 0, _scanRate);
        }

        private void Scan()
        {
            ScanForTarget();
            
            if (_useAdditionalZones)
            {
                foreach (var zone in _additionalZones)
                {
                    zone.ScanForTarget();
                }
            }
        }

        private void ScanForTarget()
        {
            var offset = TargetTransform.position - AgentTransform.position;
            var sqrDistanceToTarget = offset.sqrMagnitude;

            if (sqrDistanceToTarget <= _radius * _radius)
            {
                _targetInRange = true;
                
                var directionToTarget = offset.normalized;

                if (Vector3.Angle(AgentTransform.right, directionToTarget) < _angle * 0.5f)
                {
                    _targetSpotted = !Physics2D.Raycast(AgentTransform.position, directionToTarget, 
                        sqrDistanceToTarget * sqrDistanceToTarget, _obstacleLayer);
                }
                else
                {
                    _targetSpotted = false;
                }
            }
            else
            {
                _targetInRange = false;
                _targetSpotted = false;
            }
        }
    }
} 
