using UnityEngine;

namespace SkalluUtils.Utils.MultiSwitch
{
    public class MultiSwitchCompositeBehaviourEnable : BasicMultiSwitch
    {
        [SerializeField] private Behaviour[] _behaviours;
        
        protected override void SetstateInternal(int oldValue, int newValue)
        {
            if (oldValue == newValue)
            {
                return;
            }

            var len = _behaviours.Length;
            
            if (newValue < 0 || len < newValue)
            {
                return;
            }

            for (var i = 0; i < len; i++)
            {
                _behaviours[i].enabled = newValue == 1;
            }
        }
    }
}