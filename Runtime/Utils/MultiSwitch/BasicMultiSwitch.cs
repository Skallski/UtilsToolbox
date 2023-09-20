using SkalluUtils.PropertyAttributes;
using UnityEngine;

namespace SkalluUtils.Utils.MultiSwitch
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BasicMultiSwitch : MultiSwitch
    {
        [SerializeField] private int _defaultValueOnAwake = -1;
        [SerializeField, ReadOnly] private int _state = -1;

        public override int State => _state;

        private void Awake()
        {
            if (_defaultValueOnAwake != -1 && _state == -1)
            {
                SetStateSafe(_defaultValueOnAwake);
            }
        }

        public sealed override void SetState(int value)
        {
            var oldValue = _state;
            _state = value;
            SetStateInternal(oldValue, value);
        }

        protected abstract void SetStateInternal(int oldValue, int newValue);
    }
} 
