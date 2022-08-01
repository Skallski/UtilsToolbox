using UnityEngine;

namespace SkalluUtils.Utils
{
    [ExecuteAlways]
    public class ObjectShake : MonoBehaviour
    {
        private Vector3 startPosition;
        private Quaternion startRotation;

        [SerializeField, Range(0.05f, 0.3f)] private float shakeIntensity = 0.1f; 
        private readonly float shakeDecreaseRate = 0.002f;
        private readonly float multiplier = 0.2f;

        private float currentShakeIntensity = 0;

        private void Update()
        {
            if (currentShakeIntensity > 0)
            {
                transform.position = startPosition + Random.insideUnitSphere * currentShakeIntensity;
        
                transform.rotation = new Quaternion(
                    startRotation.x + Random.Range(-currentShakeIntensity, currentShakeIntensity) * multiplier, 
                    startRotation.y + Random.Range(-currentShakeIntensity, currentShakeIntensity) * multiplier,
                    startRotation.z + Random.Range(-currentShakeIntensity, currentShakeIntensity) * multiplier, 
                    startRotation.w + Random.Range(-currentShakeIntensity, currentShakeIntensity) * multiplier
                );
        
                currentShakeIntensity -= shakeDecreaseRate;
            }
        }

        public void Shake()
        {
            startPosition = transform.position;
            startRotation = transform.rotation;
            currentShakeIntensity = shakeIntensity;
        }
    }
}