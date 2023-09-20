using UnityEngine;

namespace SkalluUtils.Utils.MultiSwitch
{
    public class MultiSwitchCompositeColors : MultiSwitchComposite<UnityEngine.UI.Graphic>
    {
        [SerializeField] private Color[] _colors;

        protected override void SetObject(UnityEngine.UI.Graphic obj, int value)
        {
            obj.color = _colors[value];
        }
    }
}