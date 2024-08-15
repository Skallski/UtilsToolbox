using TMPro;

namespace UtilsToolbox.Utils.MultiSwitch
{
    public class MultiSwitchText : MultiSwitchWithParams<TextMeshProUGUI, string>
    {
        protected override void SetStateInternalAction(TextMeshProUGUI label, string text)
        {
            label.SetText(text);
        }
    }
}