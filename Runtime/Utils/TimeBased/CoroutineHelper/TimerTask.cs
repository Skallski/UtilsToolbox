using System;
using System.Collections;
using UnityEngine;

namespace SkalluUtils.Utils.TimeBased.CoroutineHelper
{
    public static class TimerTask
    {
        private static readonly WaitForSeconds OneSecondDelay = new WaitForSeconds(1);
        private static readonly WaitForEndOfFrame FrameDelay = new WaitForEndOfFrame();
        
        #region PUBLIC METHODS
        public static void EachFrame(MonoBehaviour caller, float duration, Action onEachFrame, Action onFinish = null)
        {
            Create(caller, duration, FrameDelay, onEachFrame, onFinish);
        }

        public static void EachSecond(MonoBehaviour caller, float duration, Action onEachSecond, Action onFinish = null)
        {
            Create(caller, duration, OneSecondDelay, onEachSecond, onFinish);
        }
        
        public static void EachTimestamp(MonoBehaviour caller, float duration, float timestamp, Action onEachTimestamp,
            Action onFinish = null)
        {
            Create(caller, duration, new WaitForSeconds(timestamp), onEachTimestamp, onFinish);
        }
        #endregion

        private static void Create(MonoBehaviour caller, float duration, YieldInstruction yieldInstruction,
            Action onTick, Action onFinish = null)
        {
            caller.StartCoroutine(Timer_Coroutine(duration, yieldInstruction, onTick, onFinish));
        }

        private static IEnumerator Timer_Coroutine(float duration, YieldInstruction yieldInstruction, Action onTick,
            Action onFinish = null)
        {
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                onTick?.Invoke();
                
                elapsedTime += Time.unscaledDeltaTime;
                yield return yieldInstruction;
            }
            
            onFinish?.Invoke();
        }
    }
}