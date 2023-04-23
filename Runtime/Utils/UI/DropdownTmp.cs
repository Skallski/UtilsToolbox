using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SkalluUtils.Utils.UI
{
    public class DropdownTmp : TMP_Dropdown
    {
        [SerializeField] private UnityEvent _onDropdownListCreated;
        [SerializeField] private UnityEvent _onDropdownListDestroyed;

        protected override GameObject CreateDropdownList(GameObject temp)
        {
            _onDropdownListCreated?.Invoke();
            
            return base.CreateDropdownList(temp);
        }

        protected override void DestroyDropdownList(GameObject dropdownList)
        {
            _onDropdownListDestroyed?.Invoke();
            
            base.DestroyDropdownList(dropdownList);
        }
    }
}