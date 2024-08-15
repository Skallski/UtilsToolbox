using System;
using System.Collections;
using UnityEngine;

namespace UtilsToolbox.Utils.TimeBased.CoroutineHelper
{
    public class TimerTask
    {
        public static void Create(MonoBehaviour caller, float duration, float timestamp, Action<float> onEachTimestamp,
            Action onFinish = null)
        {
            caller.StartCoroutine(EachTimestamp(duration, timestamp, onEachTimestamp, onFinish));
        }
        
        public static void Create(MonoBehaviour caller, Func<bool> stopPredicate, float timestamp,
            Action<float> onEachTimestamp, Action onFinish = null)
        {
            caller.StartCoroutine(EachTimestamp(stopPredicate, timestamp, onEachTimestamp, onFinish));
        }

        #region EACH FRAME
        /// <summary>
        /// Invokes callback each frame in n seconds duration
        /// </summary>
        /// <param name="duration"> duration of whole task </param>
        /// <param name="onEachFrame"> callback that will be invoked every frame </param>
        /// <param name="onFinish"> callback that will be invoked after task is completed </param>
        /// <returns></returns>
        public static IEnumerator EachFrame(float duration, Action<float> onEachFrame, Action onFinish = null)
        {
            return EachTimestamp(duration, Time.unscaledDeltaTime, onEachFrame, onFinish);
        }
        
        /// <summary>
        /// Invokes callback each frame until stopPredicate condition is met
        /// </summary>
        /// <param name="stopPredicate"> a condition that must be met in order to stop the callback execution </param>
        /// <param name="onEachFrame"> callback that will be invoked every frame </param>
        /// <param name="onFinish"> callback that will be invoked after task is completed </param>
        /// <returns></returns>
        public static IEnumerator EachFrame(Func<bool> stopPredicate, Action<float> onEachFrame, Action onFinish = null)
        {
            return EachTimestamp(stopPredicate, Time.unscaledDeltaTime, onEachFrame, onFinish);
        }
        #endregion

        #region EACH SECOND
        /// <summary>
        /// Invokes callback each second in n seconds duration
        /// </summary>
        /// <param name="duration"> duration of whole task </param>
        /// <param name="onEachSecond"> callback that will be invoked every second </param>
        /// <param name="onFinish"> callback that will be invoked after task is completed </param>
        /// <returns></returns>
        public static IEnumerator EachSecond(float duration, Action<float> onEachSecond, Action onFinish = null)
        {
            return EachTimestamp(duration, 1, onEachSecond, onFinish);
        }
        
        /// <summary>
        /// Invokes callback each second until stopPredicate condition is met
        /// </summary>
        /// <param name="stopPredicate"> a condition that must be met in order to stop the callback execution </param>
        /// <param name="onEachSecond"> callback that will be invoked every second </param>
        /// <param name="onFinish"> callback that will be invoked after task is completed </param>
        /// <returns></returns>
        public static IEnumerator EachSecond(Func<bool> stopPredicate, Action<float> onEachSecond, Action onFinish = null)
        {
            return EachTimestamp(stopPredicate, 1, onEachSecond, onFinish);
        }
        #endregion

        #region EACH TIMESTAMP 
        /// <summary>
        /// Invokes callback each timestamp in n seconds duration
        /// </summary>
        /// <param name="duration"> duration of whole task </param>
        /// <param name="timestamp"> time interval between which the callback will be invoked </param>
        /// <param name="onEachTimestamp"> callback that will be invoked each timestamp </param>
        /// <param name="onFinish"> callback that will be invoked after task is completed </param>
        /// <returns></returns>
        public static IEnumerator EachTimestamp(float duration, float timestamp, Action<float> onEachTimestamp,
            Action onFinish = null)
        {
            float timer = 0;
            
            bool StopPredicate()
            {
                return timer >= duration;
            }
            
            void OnEachTimestamp(float t)
            {
                onEachTimestamp(t);
                timer = t;
            }

            return EachTimestamp(StopPredicate, timestamp, OnEachTimestamp, onFinish);
        }

        /// <summary>
        /// Invokes callback each timestamp until stopPredicate condition is met
        /// </summary>
        /// <param name="stopPredicate"> a condition that must be met in order to stop the callback execution </param>
        /// <param name="timestamp"> time interval between which the callback will be invoked </param>
        /// <param name="onEachTimestamp"> callback that will be invoked each timestamp </param>
        /// <param name="onFinish"> callback that will be invoked after task is completed </param>
        /// <returns></returns>
        public static IEnumerator EachTimestamp(Func<bool> stopPredicate, float timestamp, Action<float> onEachTimestamp, 
            Action onFinish = null)
        {
            WaitForSeconds delay = new WaitForSeconds(timestamp);
            float elapsedTime = 0f;

            while (stopPredicate() == false)
            {
                onEachTimestamp?.Invoke(elapsedTime);
                elapsedTime += timestamp; 
                yield return delay;
            }
            
            onFinish?.Invoke();
        }
        #endregion
    }
}