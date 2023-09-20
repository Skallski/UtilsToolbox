namespace SkalluUtils.Utils.MultiSwitch
{
    public class MultiSwitchCompositeGameObjectsActive : MultiSwitchComposite<UnityEngine.GameObject>
    {
        protected override void SetObject(UnityEngine.GameObject obj, int value)
        {
            obj.SetActive(value switch
            {
                0 => false,
                1 => true,
                _ => false
            });
        }
    }
}