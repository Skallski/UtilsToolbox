namespace UtilsToolbox.Utils.MultiSwitch
{
    /// <summary>
    /// Multi switch that can set multiple stored multi switches at once
    /// <remarks>
    /// Every multi switch in the array will be affected during state change!
    /// Linking multiple broadcasts with each other could crash the application!
    /// </remarks>
    /// </summary>
    public sealed class MultiSwitchBroadcast : MultiSwitchWithArray<MultiSwitch>
    {
        protected override void SetSingleElement(MultiSwitch multiSwitch, int value)
        {
            multiSwitch.SetState(value);
        }
    }
} 
