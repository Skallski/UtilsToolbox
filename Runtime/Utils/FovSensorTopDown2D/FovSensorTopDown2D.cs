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
        
        // basic zone parameters
        [SerializeField, Range(0f, 20f)] private float _radius;
        [SerializeField, Range(0, 360)] private int _angle;
        [SerializeField] private Color _color = Color.white;
        [SerializeField, Range(0.5f, 2f)] private float _thickness = 0.5f;
        [SerializeField, ReadOnly] private bool _targetInRange;
        [SerializeField, ReadOnly] private bool _targetSpotted;

        // additional zones
        [SerializeField] private bool _useAdditionalZones = false;
        [SerializeField] protected List<FovZone2D> _additionalZones = new List<FovZone2D>();

        public Transform AgentTransform { get; private set; }
        public Transform TargetTransform { get; private set; }

        public bool UseAdditionalZones => _useAdditionalZones;

        public bool TargetSpotted
        {
            get => _targetSpotted;
            protected set => _targetSpotted = value;
        }

        public virtual void Setup(GameObject targetObject)
        {
            if (targetObject == null)
            {
                return;
            }

            _targetObject = targetObject;
            TargetTransform = targetObject.transform;
            AgentTransform = transform;
            
            if (UseAdditionalZones)
            {
                foreach (var zone in _additionalZones)
                {
                    zone.Setup(this);
                }
            }
            
            InvokeRepeating(nameof(Scan), 0, _scanRate);
        }

        /// <summary>
        /// Main scanning function called every scan rate
        /// </summary>
        private void Scan()
        {
            ScanForTarget();
            
            if (UseAdditionalZones)
            {
                foreach (var zone in _additionalZones)
                {
                    zone.ScanForTarget();
                }
            }
        }
        
        /// <summary>
        /// Basic fov scan
        /// </summary>
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

        /// <summary>
        /// Get additional zone by it's index
        /// </summary>
        /// <param name="zoneIndex"> index inside list of zones </param>
        /// <returns> found zone </returns>
        public FovZone2D GetZone(int zoneIndex)
        {
            if (zoneIndex >= _additionalZones.Count)
            {
                return null;
            }
            
            return _additionalZones[zoneIndex];
        }

        /// <summary>
        /// Get additional zone by it's name
        /// </summary>
        /// <param name="zoneName"> name of the zone </param>
        /// <returns> found zone </returns>
        public FovZone2D GetZone(string zoneName)
        {
            if (zoneName == string.Empty)
            {
                return null;
            }
            
            for (int i = 0, c = _additionalZones.Count; i < c; i++)
            {
                var zone = _additionalZones[i];
                if (zone.Name.Equals(zoneName))
                {
                    return zone;
                }
            }
            
            return null;
        }

        /// <summary>
        /// Get copy of the zones list
        /// </summary>
        /// <returns></returns>
        public List<FovZone2D> GetZones()
        {
            var zonesCopy = _additionalZones;
            return zonesCopy;
        }
    }
} 
