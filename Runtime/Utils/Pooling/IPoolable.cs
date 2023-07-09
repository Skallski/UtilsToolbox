using System;

namespace SkalluUtils.Utils.Pooling
{
    /// <summary>
    /// Interface to implement if you wish to use more generic pooling on the object.
    /// </summary>
    /// <example>
    /// <code>
    /// public class PoolableObject : MonoBehaviour, IPoolable<PoolableObject>
    /// {
    ///     private Action<PoolableObject> _onPushInternal;
    ///    
    ///     public void Pull(Action<PoolableObject> onPush)
    ///     { 
    ///        _onPushInternal = onPush; // initialize push callback
    ///        // some code...
    ///     }
    ///    
    ///     public void Push()
    ///     {
    ///        // some code...
    ///        _onPushInternal?.Invoke(this); // push to pool (should be called last!)
    ///     }
    /// }
    /// </code>>
    /// </example>>
    /// <typeparam name="T"> Generic type of poolable object </typeparam>
    public interface IPoolable<T>
    {
        /// <summary>
        /// This method is called everytime object is pulled from the pool
        /// </summary>
        void Pull(Action<T> onPush);
        
        /// <summary>
        /// This method is called everytime object is pushed back to the pool
        /// </summary>
        void Push();
    }
}