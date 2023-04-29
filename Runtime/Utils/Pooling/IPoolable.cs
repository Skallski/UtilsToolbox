using System.Collections;

namespace SkalluUtils.Utils.Pooling
{
    public interface IPoolable<T>
    {
        /// <summary>
        /// This method will be called everytime object is pulled from the pool (like OnEnable)
        /// </summary>
        /// <param name="returnAction"> push action that will be globally cached and initialized here </param>
        void Initialize(System.Action<T> returnAction);
        
        /// <summary>
        /// This method will be called everytime object is pushed to the pool (like OnDisable)
        /// </summary>
        void ReturnToPool();

        // basic implementation sample:
        // public class PoolableObject : MonoBehaviour, IPoolable<PoolableObject>
        // {
        //     private Action<PoolableObject> _returnToPoolAction;
        //
        //     public void Initialize(Action<PoolableObject> returnAction)
        //     {
        //         _returnToPoolAction = returnAction;
        //     }
        //
        //     public void ReturnToPool()
        //     {
        //         _returnToPoolAction?.Invoke(this);
        //     }
        // }
    }
}