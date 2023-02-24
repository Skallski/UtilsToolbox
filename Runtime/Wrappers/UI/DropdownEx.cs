using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SkalluUtils.Utils.UI.Wrappers
{
    [PublicAPI]
    public class DropdownEx : TMP_Dropdown
    {
        #region EVENTS
        [SerializeField] private UnityEvent onCreateDropdownList;
        [SerializeField] private UnityEvent onDestroyDropdownList;
        #endregion

        protected override GameObject CreateDropdownList(GameObject template)
        {
            onCreateDropdownList?.Invoke();
            return base.CreateDropdownList(template);
        }

        protected override void DestroyDropdownList(GameObject dropdownList)
        {
            onDestroyDropdownList?.Invoke();
            base.DestroyDropdownList(dropdownList);
        }
    }
}