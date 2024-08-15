using UnityEngine;

namespace UtilsToolbox.Utils.MultiSwitch
{
    public class CompositeMultiSwitchColors : MultiSwitchWithArray<UnityEngine.UI.Graphic>
    {
        [SerializeField] private Color[] _colors;

        protected override void SetSingleElement(UnityEngine.UI.Graphic obj, int value)
        {
            obj.color = _colors[value];
        }
    }
}