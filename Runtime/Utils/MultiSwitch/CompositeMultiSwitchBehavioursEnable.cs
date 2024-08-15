namespace UtilsToolbox.Utils.MultiSwitch
{
    public class CompositeMultiSwitchBehavioursEnable : MultiSwitchWithArray<UnityEngine.Behaviour>
    {
        protected override void SetSingleElement(UnityEngine.Behaviour obj, int value)
        {
            obj.enabled = value switch
            {
                1 => true,
                _ => false
            };
        }
    }
}