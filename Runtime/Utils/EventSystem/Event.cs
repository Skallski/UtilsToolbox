using System;

namespace SkalluUtils.Utils.EventSystem
{
    /// <summary>
    /// Event without parameters
    /// <example>
    /// public static readonly EventEx sampleEvent = new EventEx();
    /// </example>>
    /// </summary>
    public class Event
    {
        private event Action _action = delegate { };

        internal void Invoke() => _action?.Invoke();
        
        internal void AddListener(Action listener) => _action += listener;
        
        internal void RemoveListener(Action listener) => _action -= listener;
    }

    /// <summary>
    /// Event with single parameter
    /// </summary>
    /// <typeparam name="T"> generic param </typeparam>
    public class Event<T>
    {
        private event Action<T> _action = delegate { };

        internal void Invoke(T param) => _action?.Invoke(param);

        internal void AddListener(Action<T> listener) => _action += listener;
        
        internal void RemoveListener(Action<T> listener) => _action -= listener;
    }
    
    /// <summary>
    /// Event with two parameters
    /// </summary>
    /// <typeparam name="T1"> first generic param </typeparam>
    /// <typeparam name="T2"> second generic param </typeparam>
    public class Event<T1, T2>
    {
        private event Action<T1, T2> _action = delegate { };
        
        internal void Invoke(T1 param1, T2 param2) => _action?.Invoke(param1, param2);

        internal void AddListener(Action<T1, T2> listener) => _action += listener;
        
        internal void RemoveListener(Action<T1, T2> listener) => _action -= listener;
    }
}