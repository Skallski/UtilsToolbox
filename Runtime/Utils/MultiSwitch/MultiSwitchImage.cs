using UnityEngine;
using UnityEngine.UI;

namespace SkalluUtils.Utils.MultiSwitch
{
    public class MultiSwitchImage : BasicMultiSwitch
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite[] _sprites;

        protected override void SetstateInternal(int oldValue, int newValue)
        {
            if (oldValue == newValue || _image == null)
            {
                return;
            }

            if (newValue < 0 || _sprites.Length <= newValue)
            {
                return;
            }

            _image.sprite = _sprites[newValue];
        }
    }
}