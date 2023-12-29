using System;

namespace SkalluUtils.Utils.Pooling
{
    /// <summary>
    /// Interface to implement if you wish to use more generic pooling on the object.
    /// </summary>
    public interface IPoolable<T>
    {
        /// <summary>
        /// This method is called everytime object is pulled from the pool
        /// </summary>
        void OnPulledFromPool(Action<T> onPush);
        
        /// <summary>
        /// This method is called everytime object is pushed back to the pool
        /// </summary>
        void PushToPool();
    }
}