using System;
using UnityEngine;

namespace SkalluUtils.Utils.MultiSwitch
{
    public abstract class MultiSwitch : MonoBehaviour
    {
        private int _lastUpdatedFrame;
        private int _updateCount;
        private const int MAX_UPDATE_PER_FRAME = 10;
    
        public abstract int State { get; }

        internal void SetStateSafe(int value)
        {
            var currentFrameCount = Time.frameCount;

            if (_lastUpdatedFrame != currentFrameCount)
            {
                _updateCount = 0;
                _lastUpdatedFrame = currentFrameCount;
            }

            ++_updateCount;

            if (_updateCount > MAX_UPDATE_PER_FRAME)
            {
                return;
            }
        
            SetState(value);
        }
    
        public abstract void SetState(int value);
    
        public void SetState<TEnum>(TEnum enumValue) where TEnum : Enum
        {
            SetStateSafe(Convert.ToInt32(enumValue));
        }
    
        public void SetState(bool boolValue)
        {
            SetStateSafe(boolValue ? 1 : 0);
        }
    }
}