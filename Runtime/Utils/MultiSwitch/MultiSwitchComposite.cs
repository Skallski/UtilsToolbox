using JetBrains.Annotations;
using UnityEngine;

namespace SkalluUtils.Utils.MultiSwitch
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MultiSwitchComposite<T> : BasicMultiSwitch
    {
        [SerializeField] 
        [Tooltip("Array members can be null")]
        private T[] _objects;

        protected sealed override void SetStateInternal(int oldValue, int newValue)
        {
            if (oldValue == newValue || _objects == null)
            {
                return;
            }
            
            for (int i = 0, c = _objects.Length; i < c; i++)
            {
                T obj = _objects[i];

                if (obj == null)
                {
                    continue;
                }

                if (obj is Object unityObject && unityObject == null)
                {
                    continue;
                }
                
                SetObject(obj, newValue);
            }
        }

        protected abstract void SetObject([NotNull] T obj, int value);
    }
}