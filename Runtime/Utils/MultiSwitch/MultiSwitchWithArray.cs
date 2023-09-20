using JetBrains.Annotations;
using UnityEngine;

namespace SkalluUtils.Utils.MultiSwitch
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MultiSwitchWithArray<T> : BasicMultiSwitch
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

            var len = _objects.Length;

            // deactivate
            for (int i = 0; i < len; i++)
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

                if (newValue != i)
                {
                    SetObject(obj, false);
                }
            }

            // activate
            for (int i = 0; i < len; i++)
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

                if (newValue == i)
                {
                    SetObject(obj, true);
                }
            }
        }

        protected abstract void SetObject([NotNull] T obj, bool value);
    }
}