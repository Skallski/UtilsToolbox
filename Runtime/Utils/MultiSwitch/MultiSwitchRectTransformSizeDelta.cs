using UnityEngine;

namespace UtilsToolbox.Utils.MultiSwitch
{
    public class MultiSwitchRectTransformSizeDelta : MultiSwitchWithParams<RectTransform, Vector2>
    {
        protected override void SetStateInternalAction(RectTransform rectTransform, Vector2 sizeDelta)
        {
            rectTransform.sizeDelta = sizeDelta;
        }
    }
}