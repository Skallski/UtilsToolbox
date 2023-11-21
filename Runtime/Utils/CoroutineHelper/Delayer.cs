using System;
using System.Collections;
using UnityEngine;

namespace SkalluUtils.Utils.CoroutineHelper
{
    public static class Delayer
    {
        public static readonly WaitForEndOfFrame WaitForFrameEnd = new WaitForEndOfFrame();
        public static readonly WaitForSeconds WaitTwoSec = new WaitForSeconds(2f);
        public static readonly WaitForSeconds WaitOneSec = new WaitForSeconds(1f);
        public static readonly WaitForSeconds WaitHalfSec = new WaitForSeconds(0.5f);

        /// <summary>
        /// Invokes callback after coroutine is completed
        /// </summary>
        /// <param name="callback"> callback, that will be called after coroutine completes </param>
        /// <param name="coroutine"> coroutine, after which callback will be called </param>
        /// <returns></returns>
        public static IEnumerator InvokeDelayed(Action callback, IEnumerator coroutine = null)
        {
            yield return coroutine;
            callback?.Invoke();
        }

        /// <summary>
        /// Invokes action after yield instruction is completed
        /// </summary>
        /// <param name="callback"> callback, that will be called after yield instruction completes </param>
        /// <param name="yieldInstruction"> custom yield instruction </param>
        /// <returns></returns>
        public static IEnumerator InvokeDelayed(Action callback, CustomYieldInstruction yieldInstruction)
        {
            yield return yieldInstruction;
            callback?.Invoke();
        }
        
        /// <summary>
        /// Invokes action after yield instruction is completed
        /// </summary>
        /// <param name="callback"> callback, that will be called after yield instruction completes </param>
        /// <param name="yieldInstruction"> basic yield instruction </param>
        /// <returns></returns>
        public static IEnumerator InvokeDelayed(Action callback, YieldInstruction yieldInstruction)
        {
            yield return yieldInstruction;
            callback?.Invoke();
        }
    }
}