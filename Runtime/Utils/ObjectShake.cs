using UnityEngine;

namespace SkalluUtils.Utils
{
    [ExecuteAlways]
    public class ObjectShake : MonoBehaviour
    {
        private const float SHAKE_DECREASE_RATE = 0.002f;
        private const float MULTIPLIER = 0.2f;
        
        [SerializeField, Range(0.05f, 0.3f)] private float _shakeIntensity = 0.1f;
        
        private float _currentShakeIntensity = 0;
        private Vector3 _startPosition;
        private Quaternion _startRotation;

        private void Update()
        {
            if (!(_currentShakeIntensity > 0))
            {
                return;
            }

            transform.position = _startPosition + Random.insideUnitSphere * _currentShakeIntensity;
        
            transform.rotation = new Quaternion(
                _startRotation.x + Random.Range(-_currentShakeIntensity, _currentShakeIntensity) * MULTIPLIER, 
                _startRotation.y + Random.Range(-_currentShakeIntensity, _currentShakeIntensity) * MULTIPLIER,
                _startRotation.z + Random.Range(-_currentShakeIntensity, _currentShakeIntensity) * MULTIPLIER, 
                _startRotation.w + Random.Range(-_currentShakeIntensity, _currentShakeIntensity) * MULTIPLIER
            );
        
            _currentShakeIntensity -= SHAKE_DECREASE_RATE;
        }

        public void Shake()
        {
            var t = transform;
            
            _startPosition = t.position;
            _startRotation = t.rotation;
            _currentShakeIntensity = _shakeIntensity;
        }
    }
}