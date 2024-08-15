using TMPro;

namespace UtilsToolbox.Utils.MultiSwitch
{
    public class MultiSwitchTmpGradient : MultiSwitchWithParams<TextMeshProUGUI, VertexGradient>
    {
        protected override void SetStateInternalAction(TextMeshProUGUI tmp, VertexGradient gradient)
        {
            tmp.colorGradient = gradient;
        }
    }
}