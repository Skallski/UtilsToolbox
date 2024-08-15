using UnityEngine;
using UnityEngine.UI;

namespace UtilsToolbox.Utils.MultiSwitch
{
    public class MultiSwitchColor : MultiSwitchWithParams<Graphic, Color>
    {
        protected override void SetStateInternalAction(Graphic graphic, Color color)
        {
            graphic.color = color;
        }
    }
} 
