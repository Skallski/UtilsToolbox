using UnityEngine;

namespace SkalluUtils.Utils
{
    [System.Serializable]
    public struct Range
    {
        [SerializeField] public float min;
        [SerializeField] public float max;
        [SerializeField] private float _valueLeft;
        [SerializeField] private float _valueRight;

        public Range(float min, float max)
        {
            this.min = min;
            this.max = max;
            
            _valueLeft = min;
            _valueRight = max;
        }

        public float GetRandomValue()
        {
            return Random.Range(_valueLeft, _valueRight);
        }
    }
}