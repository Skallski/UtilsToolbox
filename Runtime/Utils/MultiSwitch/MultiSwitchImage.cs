using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.Utils.MultiSwitch
{
    public class MultiSwitchImage : MultiSwitchWithParams<Image, Sprite>
    {
        protected override void SetStateInternalAction(Image image, Sprite sprite)
        {
            image.sprite = sprite;
        }
    }
}