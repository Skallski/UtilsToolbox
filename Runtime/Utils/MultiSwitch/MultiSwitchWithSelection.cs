using UnityEngine;

namespace Main.Scripts.Utils.MultiSwitch
{
    /// <summary>
    /// Multi switch that can select one from stored elements and deselect others ones
    /// </summary>
    /// <typeparam name="T"> Generic type of stored elements </typeparam>
    public abstract class MultiSwitchWithSelection<T> : MultiSwitchWithArray<T>
    {
        protected sealed override bool SetStateInternal(int value)
        {
            if (_elements == null)
            {
                Debug.LogError("Elements array cannot be null!");
                return false;
            }
            
            for (int i = 0; i < _elements.Length; i++)
            {
                T obj = _elements[i];
                if (obj != null && (obj is not Object unityObject || unityObject != null))
                {
                    SetSingleElement(obj, value == i ? 1 : 0);
                }
            }

            return true;
        }
    }
}