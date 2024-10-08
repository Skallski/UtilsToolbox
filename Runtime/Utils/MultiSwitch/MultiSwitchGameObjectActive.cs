using UnityEngine;

namespace UtilsToolbox.Utils.MultiSwitch
{
    public class MultiSwitchGameObjectActive : MultiSwitchWithSelection<GameObject>
    {
        protected override void SetSingleElement(GameObject obj, int value)
        {
            obj.SetActive(value == 1);
        }
    }
}