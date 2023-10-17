using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace SkalluUtils.Utils.UI
{
    public abstract class Panel : MonoBehaviour
    {
        internal static event Action<Panel> OnPanelOpened;
        internal static event Action<Panel> OnPanelClosed;
        
        [SerializeField] private GameObject _content;
        [CanBeNull, SerializeField] private GameObject _background;

        [Header("Events")]
        [SerializeField] private UnityEvent _opened;
        [SerializeField] private UnityEvent _closed;
        
        private bool IsOpened => _content.activeSelf;
        
        public void Open()
        {
            if (!IsOpened)
            {
                OpenSelf();
            }
        }
        
        public void Close()
        {
            if (IsOpened)
            {
                CloseSelf();
            }
        }
        
        /// <summary>
        /// Opens panel
        /// </summary>
        protected virtual void OpenSelf()
        {
            ForceOpen();
            
            _opened?.Invoke();
            OnPanelOpened?.Invoke(this);
        }

        /// <summary>
        /// Closes panel
        /// </summary>
        protected virtual void CloseSelf()
        {
            ForceClose();
            
            _closed?.Invoke();
            OnPanelClosed?.Invoke(this);
        }

        /// <summary>
        /// Opens panel without invoking an event
        /// </summary>
        protected void ForceOpen()
        {
            if (_background != null)
            {
                _background.SetActive(true);
            }
            
            _content.SetActive(true);
        }

        /// <summary>
        /// Closes panel without invoking an event
        /// </summary>
        protected void ForceClose()
        {
            if (_background != null)
            {
                _background.SetActive(false);
            }

            _content.SetActive(false);
        }
    }
}