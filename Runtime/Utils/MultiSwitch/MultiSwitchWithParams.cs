using UnityEngine;

namespace Main.Scripts.Utils.MultiSwitch
{
    /// <summary>
    /// Simple multi switch that can modify the source .
    /// </summary>
    /// <typeparam name="TSource"> Generic type of source component. </typeparam>
    /// <typeparam name="TValue"> Generic type of values. </typeparam>
    public abstract class MultiSwitchWithParams<TSource, TValue> : MultiSwitch
    {
        [SerializeField] private TSource _source;
        [SerializeField] private TValue[] _values;
        
        public TSource Source => _source;
        public TValue[] Values => _values;

#if UNITY_EDITOR
        private void Reset()
        {
            SetSource();
        }
        
        [ContextMenu(nameof(SetSource))]
        private void SetSource()
        {
            if (_source == null)
            {
                _source = GetComponent<TSource>();
            }
        }
#endif

        protected sealed override bool SetStateInternal(int value)
        {
            if (_source == null)
            {
                Debug.LogError("Source object cannot be null!");
                return false;
            }

            if (_values.Length <= value)
            {
                Debug.LogError("Value cannot exceed parameters quantity!");
                return false;
            }
            
            SetStateInternalAction(_source, _values[value]);

            return true;
        }

        /// <summary>
        /// Performs action on source component with value as parameter.
        /// </summary>
        /// <param name="source"> Source component on which the action will be performed </param>
        /// <param name="value"> New value to set </param>
        protected abstract void SetStateInternalAction(TSource source, TValue value);

        /// <summary>
        /// Sets the values for the internal array by copying the provided values.
        /// </summary>
        /// <param name="values"> An array of values of type. </param>
        public void SetValues(TValue[] values)
        {
            _values = new TValue[values.Length];
            System.Array.Copy(values, _values, values.Length);
        }
    }
}