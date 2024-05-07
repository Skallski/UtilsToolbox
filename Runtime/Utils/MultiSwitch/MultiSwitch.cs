using System;
using UnityEngine;

namespace Main.Scripts.Utils.MultiSwitch
{
    /// <summary>
    /// Base class that contains basic implementation of multi switch
    /// </summary>
    public abstract class MultiSwitch : MonoBehaviour
    {
        [SerializeField] private int _defaultStateOnAwake = -1;
        [SerializeField] private int _state = -1;
        [Space]
        
        private int _lastUpdatedFrame;
        private int _updateCount;
        private const int MAX_UPDATE_PER_FRAME = 10;
        
        public int State => _state;
        
        private void Awake()
        {
            if (_defaultStateOnAwake > -1)
            {
                SetState(_defaultStateOnAwake);
            }
        }

        private void SetStateSafe(int value)
        {
            int currentFrameCount = Time.frameCount;
            if (_lastUpdatedFrame != currentFrameCount)
            {
                _updateCount = 0;
                _lastUpdatedFrame = currentFrameCount;
            }
        
            ++_updateCount;
            if (_updateCount > MAX_UPDATE_PER_FRAME)
            {
                Debug.LogError("Max number of multi switch updates reached!", gameObject);
                return;
            }
            
            if (SetStateInternal(value))
            {
                _state = value;
            }
        }
        
        /// <summary>
        /// Sets state of multi switch
        /// </summary>
        /// <param name="value"> int value to set </param>
        public void SetState(int value)
        {
            if (value < 0)
            {
                Debug.LogWarning("MultiSwitch value cannot be below 0! Value has been changed to 0.");
                value = 0;
            }

            if (_state != value)
            {
                SetStateSafe(value);
            }
        }

        /// <summary>
        /// Sets state of multi switch
        /// </summary>
        /// <param name="value"> Enum value to set </param>
        /// <typeparam name="TEnum"></typeparam>
        public void SetState<TEnum>(TEnum value) where TEnum : Enum
        {
            SetStateSafe(Convert.ToInt32(value));
        }
    
        /// <summary>
        /// Sets state of multi switch
        /// </summary>
        /// <param name="value"> Bool value to set </param>
        public void SetState(bool value)
        {
            SetStateSafe(value ? 1 : 0);
        }
        
        protected abstract bool SetStateInternal(int value);
    }
}