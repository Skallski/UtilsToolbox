using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SkalluUtils.Utils.UI
{
    public class DropdownEx : TMP_Dropdown
    {
        [SerializeField] private UnityEvent _onDropdownListCreated;
        [SerializeField] private UnityEvent _onDropdownListDestroyed;

        protected override GameObject CreateDropdownList(GameObject template)
        {
            _onDropdownListCreated?.Invoke();
            
            return base.CreateDropdownList(template);
        }

        protected override void DestroyDropdownList(GameObject dropdownList)
        {
            _onDropdownListDestroyed?.Invoke();
            
            base.DestroyDropdownList(dropdownList);
        }
    }
}