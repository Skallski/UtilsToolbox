using UnityEngine;

namespace SkalluUtils.Utils.MultiSwitch
{
    public class MultiSwitchColor : BasicMultiSwitch
    {
        [SerializeField] private UnityEngine.UI.Graphic _graphic;
        [SerializeField] private Color[] _colors;

        protected override void SetStateInternal(int oldValue, int newValue)
        {
            if (oldValue == newValue || _graphic == null)
            {
                return;
            }

            if (newValue < 0 || _colors.Length <= newValue)
            {
                return;
            }

            _graphic.color = _colors[newValue];
        }
    }
} 
