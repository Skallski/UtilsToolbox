using UnityEngine;

namespace UtilsToolbox.Utils.MultiSwitch
{
    public class MultiSwitchCanvasGroupAlpha : MultiSwitchWithParams<CanvasGroup, float>
    {
        protected override void SetStateInternalAction(CanvasGroup canvasGroup, float alpha)
        {
            canvasGroup.alpha = alpha;
        }
    }
}