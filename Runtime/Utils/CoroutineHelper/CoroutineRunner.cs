using System;
using System.Collections;
using UnityEngine;

namespace SkalluUtils.Utils.CoroutineHelper
{
    public static class CoroutineRunner
    {
        /// <summary>
        /// Runs coroutine by passing IEnumerator
        /// </summary>
        /// <param name="caller"> MonoBehaviour, on which coroutine start will be called </param>
        /// <param name="coroutine"> coroutine reference </param>
        /// <param name="iEnumerator"> coroutine instruction </param>
        public static void Run(MonoBehaviour caller, ref Coroutine coroutine, IEnumerator iEnumerator)
        {
            if (caller == null)
            {
                return;
            }

            if (coroutine != null)
            {
                caller.StopCoroutine(coroutine); // stop current coroutine, when new one is called
            }
            
            coroutine = caller.StartCoroutine(iEnumerator);
        }
        
        /// <summary>
        /// Runs coroutine by passing it's name
        /// </summary>
        /// <param name="caller"> MonoBehaviour, on which coroutine start will be called </param>
        /// <param name="coroutine"> coroutine reference </param>
        /// <param name="methodName"> method to call as coroutine </param>
        public static void Run(MonoBehaviour caller, ref Coroutine coroutine, string methodName)
        {
            if (caller == null)
            {
                return;
            }

            if (coroutine != null)
            {
                caller.StopCoroutine(coroutine); // stop current coroutine, when new one is called
            }
            
            coroutine = caller.StartCoroutine(methodName);
        }

        /// <summary>
        /// Stops coroutine
        /// </summary>
        /// <param name="caller"> MonoBehaviour, on which coroutine stop will be called </param>
        /// <param name="coroutine"> coroutine to stop </param>
        /// <param name="onStop"> callback that will be invoked after stopping the coroutine </param>
        public static void Stop(MonoBehaviour caller, Coroutine coroutine)
        {
            if (caller == null)
            {
                return;
            }

            if (coroutine != null)
            {
                caller.StopCoroutine(coroutine);
            }
        }
        
        /// <summary>
        /// Stops coroutine with invoking a callback afterwards
        /// </summary>
        /// <param name="caller"> MonoBehaviour, on which coroutine stop will be called </param>
        /// <param name="coroutine"> coroutine to stop </param>
        /// <param name="onStop"> callback that will be invoked after stopping the coroutine </param>
        public static void Stop(MonoBehaviour caller, Coroutine coroutine, Action onStop)
        {
            if (caller == null)
            {
                return;
            }

            if (coroutine != null)
            {
                caller.StopCoroutine(coroutine);
                onStop?.Invoke();
            }
        }
    }
}