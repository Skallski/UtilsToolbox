using UnityEngine;

namespace SkalluUtils.Utils.MultiSwitch
{
    public class MultiSwitchScale : BasicMultiSwitch
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private Vector3[] _scales;
        
        protected override void SetstateInternal(int oldValue, int newValue)
        {
            if (oldValue == newValue || _transform == null)
            {
                return;
            }

            if (newValue < 0 || _scales.Length <= newValue)
            {
                return;
            }

            _transform.localScale = _scales[newValue];
        }
    }
}