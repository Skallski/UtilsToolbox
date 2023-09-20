using UnityEngine;

namespace SkalluUtils.Utils.MultiSwitch
{
    public class MultiSwitchGameObjectActive : MultiSwitchWithArray<GameObject>
    {
        protected override void SetObject(GameObject obj, bool value)
        {
            obj.SetActive(value);
        }
    }
}