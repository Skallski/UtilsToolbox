using UnityEngine;

namespace SkalluUtils.Utils.FovSensorTopDown2D
{
    [System.Serializable]
    public class FovZone2D
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField, Range(0f, 20f)] public float Radius { get; private set; }
        [field: SerializeField] public Color Color { get; private set; } = Color.white;
        [field: SerializeField, Range(0.5f, 2f)] public float Thickness { get; private set; } = 0.5f;
        
        [Space]
        [SerializeField] private bool _usedByFovCone = false;
        
        public bool TargetSpotted { get; private set; }

        private FovSensorTopDown2D _sensor;
        private Transform _agentTransform;
        private Transform _targetTransform;
        
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
            
            if ((_targetTransform.position - _agentTransform.position).sqrMagnitude <= Radius * Radius)
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