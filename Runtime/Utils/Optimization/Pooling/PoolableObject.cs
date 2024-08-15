using System;
using UnityEngine;

namespace UtilsToolbox.Utils.Optimization.Pooling
{
    public abstract class PoolableObject : MonoBehaviour
    {
        internal Action<PoolableObject> OnReleased { get; set; }
        
        internal void Get()
        {
           OnGet();
        }
        
        public void Release()
        {
            OnRelease();
            OnReleased?.Invoke(this);
        }
        
        protected virtual void OnGet() {}
        
        protected virtual void OnRelease() {}
    }
}