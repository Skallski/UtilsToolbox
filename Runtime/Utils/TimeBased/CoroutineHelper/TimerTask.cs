using System;
using System.Collections;
using UnityEngine;

namespace SkalluUtils.Utils.TimeBased.CoroutineHelper
{
    public class TimerTask
    {
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
            WaitForSeconds delay = new WaitForSeconds(timestamp);
            for (float elapsedTime = 0f; elapsedTime < duration; elapsedTime += timestamp)
            {
                onEachTimestamp?.Invoke(elapsedTime);
                yield return delay;
            }
            
            onFinish?.Invoke();
        }
    }
}