using UnityEngine;

namespace SkalluUtils.Utils.MultiSwitch
{
    public class MultiSwitchMaterial : BasicMultiSwitch
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material[] _materials;

        protected override void SetStateInternal(int oldValue, int newValue)
        {
            if (oldValue == newValue || _renderer == null)
            {
                return;
            }

            if (newValue < 0 || _materials.Length <= newValue)
            {
                return;
            }

            _renderer.material = _materials[newValue];
        }
    }
}