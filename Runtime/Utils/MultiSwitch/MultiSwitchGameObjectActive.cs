using UnityEngine;

namespace SkalluUtils.Utils.MultiSwitch
{
    public class MultiSwitchGameObjectActive : BasicMultiSwitch
    {
        [SerializeField] private GameObject[] _gameObjects;
        
        protected override void SetstateInternal(int oldValue, int newValue)
        {
            if (oldValue == newValue)
            {
                return;
            }

            var len = _gameObjects.Length;
            
            if (newValue < 0 || len <= newValue)
            {
                return;
            }

            for (var i = 0; i < len; i++)
            {
                _gameObjects[i].SetActive(i == newValue);
            }
        }
    }
}