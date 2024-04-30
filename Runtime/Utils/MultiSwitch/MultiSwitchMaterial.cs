using UnityEngine;

namespace Main.Scripts.Utils.MultiSwitch
{
    public class MultiSwitchMaterial : MultiSwitchWithParams<Renderer, Material>
    {
        protected override void SetStateInternalAction(Renderer rend, Material material)
        {
            rend.material = material;
        }
    }
}