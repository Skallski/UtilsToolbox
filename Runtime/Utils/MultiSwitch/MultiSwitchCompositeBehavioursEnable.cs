namespace SkalluUtils.Utils.MultiSwitch
{
    public class MultiSwitchCompositeBehavioursEnable : MultiSwitchComposite<UnityEngine.Behaviour>
    {
        protected override void SetObject(UnityEngine.Behaviour obj, int value)
        {
            obj.enabled = value switch
            {
                0 => false,
                1 => true,
                _ => false
            };
        }
    }
}