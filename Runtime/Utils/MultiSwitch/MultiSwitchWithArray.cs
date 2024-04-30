using UnityEngine;

namespace Main.Scripts.Utils.MultiSwitch
{
    /// <summary>
    /// Multi switch that can set multiple stored elements at once
    /// </summary>
    /// <typeparam name="T"> Generic type of stored elements </typeparam>
    public abstract class MultiSwitchWithArray<T> : MultiSwitch
    {
        [SerializeField] 
        [Tooltip("Array members can be null")]
        protected T[] _elements;
        
        protected override bool SetStateInternal(int value)
        {
            if (_elements == null)
            {
                Debug.LogError("Elements array cannot be null!");
                return false;
            }

            foreach (T obj in _elements)
            {
                if (obj != null && (obj is not Object unityObject || unityObject != null))
                {
                    SetSingleElement(obj, value);
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        protected abstract void SetSingleElement(T obj, int value);
    }
}