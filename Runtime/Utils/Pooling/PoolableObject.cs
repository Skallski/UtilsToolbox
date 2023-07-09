using System;
using UnityEngine;

namespace SkalluUtils.Utils.Pooling
{
    /// <summary>
    /// Class to inherit from if you wish to use simple pooling on the object.
    /// </summary>
    public class PoolableObject : MonoBehaviour, IPoolable<PoolableObject>
    {
        private Action<PoolableObject> _onPushInternal;

        /// <summary>
        /// initialize push callback
        /// </summary>
        /// <param name="onPush"></param>
        public virtual void Pull(Action<PoolableObject> onPush) => _onPushInternal = onPush; 
        
        /// <summary>
        /// push back to pool (should be called last!)
        /// </summary>
        public virtual void Push() => _onPushInternal?.Invoke(this);
    }
}