using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SkalluUtils.Utils.VFX
{
    public class ObjectShake : MonoBehaviour
    {
        private const float SHAKE_DECREASE_RATE = 0.002f;
        private const float MULTIPLIER = 0.2f;
        
        [SerializeField, Range(0.05f, 0.3f)] private float _shakeIntensity = 0.1f;
        
        private float _currentShakeIntensity = 0;
        private Coroutine _shakeCoroutine;
        
        private Transform _transform;
        private Vector3 _startPosition;
        private Quaternion _startRotation;

        private void Awake()
        {
            _transform = transform;
        }
        
        public void Shake()
        {
            _startPosition = _transform.position;
            _startRotation = _transform.rotation;
            _currentShakeIntensity = _shakeIntensity;

            if (_shakeCoroutine != null)
            {
                StopCoroutine(_shakeCoroutine);
            }

            _shakeCoroutine = StartCoroutine(Shake_Coroutine());
        }

        private IEnumerator Shake_Coroutine()
        {
            while (_currentShakeIntensity > 0)
            {
                _transform.position = _startPosition + Random.insideUnitSphere * _currentShakeIntensity;
        
                _transform.rotation = new Quaternion(
                    _startRotation.x + Random.Range(-_currentShakeIntensity, _currentShakeIntensity) * MULTIPLIER, 
                    _startRotation.y + Random.Range(-_currentShakeIntensity, _currentShakeIntensity) * MULTIPLIER,
                    _startRotation.z + Random.Range(-_currentShakeIntensity, _currentShakeIntensity) * MULTIPLIER, 
                    _startRotation.w + Random.Range(-_currentShakeIntensity, _currentShakeIntensity) * MULTIPLIER
                );
        
                _currentShakeIntensity -= SHAKE_DECREASE_RATE;
                
                yield return null;
            }
        }

        private void OnDestroy()
        {
            if (_shakeCoroutine != null)
            {
                StopCoroutine(_shakeCoroutine);
            }
        }
    }
}