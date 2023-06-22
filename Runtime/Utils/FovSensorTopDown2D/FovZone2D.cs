using UnityEngine;

namespace SkalluUtils.Utils.FovSensorTopDown2D
{
    [System.Serializable]
    public class FovZone2D
    {
        [SerializeField, Range(0f, 20f)] private float _radius;
        [SerializeField] private Color _color = Color.white;
        [SerializeField, Range(0.5f, 2f)] private float _thickness = 0.5f;
        
        [Space]
        [SerializeField] private bool _usedByFovCone = false;
        
        public bool TargetSpotted { get; private set; }

        private FovSensorTopDown2D _sensor;
        private Transform _agentTransform;
        private Transform _targetTransform;

        public float Radius => _radius;
        public Color Color => _color;
        public float Thickness => _thickness;

        public void Setup(FovSensorTopDown2D sensor)
        {
            if (sensor == null)
            {
                return;
            }
            
            _sensor = sensor;
            _agentTransform = sensor.AgentTransform;
            _targetTransform = sensor.TargetTransform;
        }

        public void ScanForTarget()
        {
            if (_sensor == null)
            {
                return;
            }
            
            if ((_targetTransform.position - _agentTransform.position).sqrMagnitude <= _radius * _radius)
            {
                TargetSpotted = !_usedByFovCone || _sensor.TargetSpotted;
            }
            else
            {
                TargetSpotted = false;
            }
        }
    }
}