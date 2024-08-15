namespace UtilsToolbox.Utils.MultiSwitch
{
    public class CompositeMultiSwitchGameObjectsActive : MultiSwitchWithArray<UnityEngine.GameObject>
    {
        protected override void SetSingleElement(UnityEngine.GameObject obj, int value)
        {
            obj.SetActive(value switch
            {
                1 => true,
                _ => false
            });
        }
    }
}