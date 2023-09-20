using JetBrains.Annotations;
using SkalluUtils.PropertyAttributes;
using UnityEngine;

namespace SkalluUtils.Utils.MultiSwitch
{
    /// <summary>
    /// Allows to set state of multiple multi switches
    /// </summary>
    public class MultiSwitchBroadcast : MultiSwitch
    {
        [SerializeField] private int _defaultValueOnAwake = -1;
        [SerializeField, ReadOnly] private int _state = -1;
    
        [SerializeField] private MultiSwitch[] _switches;
    
        public override int State => _state;

        private void Awake()
        {
            if (_defaultValueOnAwake != -1 && _state == -1)
            {
                SetStateSafe(_defaultValueOnAwake);
            }
        }

        [UsedImplicitly]
        public override void SetState(int value)
        {
            var len = _switches.Length;
            for (int i = 0; i < len; i++)
            {
                var currentSwitch = _switches[i];

                if (currentSwitch == null)
                {
                    continue;
                }

                currentSwitch.SetStateSafe(value);
            }

            _state = value;
        }
    }
} 
