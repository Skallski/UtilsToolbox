using TMPro;
using UnityEngine;

namespace SkalluUtils.Utils.MultiSwitch
{
    public class MultiSwitchTmpGradient : BasicMultiSwitch
    {
        [SerializeField] private TextMeshProUGUI _tmp;
        [SerializeField] private VertexGradient[] _gradients;

        protected override void SetStateInternal(int oldValue, int newValue)
        {
            if (oldValue == newValue || _tmp == null)
            {
                return;
            }

            if (newValue < 0 || _gradients.Length <= newValue)
            {
                return;
            }

            _tmp.colorGradient = _gradients[newValue];
        }
    }
}