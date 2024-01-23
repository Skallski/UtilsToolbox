﻿using System;
using UnityEngine;

namespace SkalluUtils.Utils.Pooling
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