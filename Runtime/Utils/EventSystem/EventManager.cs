using System;

namespace SkalluUtils.Utils.EventSystem
{
    public static class EventManager
    {
        #region EVENT WITHOUT PARAMETERS
        /// <summary>
        /// Adds listener to the event by adding callback
        /// </summary>
        /// <param name="myEvent"> event to add listener to </param>
        /// <param name="callback"> callback that will be added to the event </param>
        public static void AddListener(Event myEvent, Action callback)
        {
            myEvent.AddListener(callback);
        }
        
        /// <summary>
        /// Removes listener from the event by removing callback
        /// </summary>
        /// <param name="myEvent"> event to remove listener from </param>
        /// <param name="callback"> callback that will be removed from event  </param>
        public static void RemoveListener(Event myEvent, Action callback)
        {
            myEvent.RemoveListener(callback);
        }
        
        /// <summary>
        /// Invokes event
        /// </summary>
        /// <param name="myEvent"> event to invoke </param>
        public static void InvokeEvent(Event myEvent)
        {
            myEvent?.Invoke();
        }
        #endregion
        
        #region EVENT WITH ONE PARAMETER
        /// <summary>
        /// Adds listener to event by adding callback
        /// </summary>
        /// <param name="myEvent"> event to add listener to </param>
        /// <param name="callback"> callback that will be added to the event </param>
        /// <typeparam name="T"> generic parameter type </typeparam>
        public static void AddListener<T>(Event<T> myEvent, Action<T> callback)
        {
            myEvent.AddListener(callback);
        }
        
        /// <summary>
        /// Removes listener from the event by removing callback
        /// </summary>
        /// <param name="myEvent"> event to remove listener from </param>
        /// <param name="callback"> callback that will be removed from event  </param>
        /// <typeparam name="T"> generic parameter type </typeparam>
        public static void RemoveListener<T>(Event<T> myEvent, Action<T> callback)
        {
            myEvent.RemoveListener(callback);
        }

        /// <summary>
        /// Invokes event with parameter
        /// </summary>
        /// <param name="myEvent"> event to invoke </param>
        /// <param name="param"> parameter with which the event will be invoked </param>
        public static void InvokeEvent<T>(Event<T> myEvent, T param)
        {
            myEvent?.Invoke(param);
        }
        #endregion

        #region EVENT WITH TWO PARAMETERS
        /// <summary>
        /// Adds listener to event by adding callback
        /// </summary>
        /// <param name="myEvent"> event to add listener to </param>
        /// <param name="callback"> callback that will be added to the event </param>
        /// <typeparam name="T1"> first generic parameter type </typeparam>
        /// <typeparam name="T2"> second generic parameter type </typeparam>
        public static void AddListener<T1, T2>(Event<T1, T2> myEvent, Action<T1, T2> callback)
        {
            myEvent.AddListener(callback);
        }
        
        /// <summary>
        /// Removes listener from the event by removing callback
        /// </summary>
        /// <param name="myEvent"> event to remove listener from </param>
        /// <param name="callback"> callback that will be removed from event </param>
        /// <typeparam name="T1"> first generic parameter type </typeparam>
        /// <typeparam name="T2"> second generic parameter type </typeparam>
        public static void RemoveListener<T1, T2>(Event<T1, T2> myEvent, Action<T1, T2> callback)
        {
            myEvent.RemoveListener(callback);
        }

        /// <summary>
        /// Invokes event with two parameters
        /// </summary>
        /// <param name="myEvent"> event to invoke </param>
        /// <param name="paramOne"> first parameter with which the event will be invoked </param>
        /// <param name="paramTwo"> second parameter with which the event will be invoked </param>
        public static void InvokeEvent<T1, T2>(Event<T1, T2> myEvent, T1 paramOne, T2 paramTwo)
        {
            myEvent?.Invoke(paramOne, paramTwo);
        }
        #endregion
    }
}