using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace SkalluUtils.Utils.UI.PanelSwitcher
{
    /// <summary>
    /// Base panel class
    /// </summary>
    public abstract class PanelBase : MonoBehaviour
    {
        [SerializeField] protected GameObject _content;
        [CanBeNull, SerializeField] protected GameObject _background;
        [SerializeField] protected UnityEvent _opened;
        [SerializeField] protected UnityEvent _closed;
        
        public bool IsOpened => _content != null && _content.activeSelf;
        
        /// <summary>
        /// Safe open
        /// </summary>
        internal void Open()
        {
            if (!IsOpened)
            {
                OpenSelf();
            }
        }
        
        /// <summary>
        /// Safe close
        /// </summary>
        internal void Close()
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
        }

        /// <summary>
        /// Closes panel
        /// </summary>
        protected virtual void CloseSelf()
        {
            ForceClose();
            
            _closed?.Invoke();
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

#if UNITY_EDITOR
        
        [ContextMenu("Open_Editor")]
        public void Open_Editor()
        {
            ForceOpen();
        }
        
        [ContextMenu("Close_Editor")]
        public void Close_Editor()
        {
            ForceClose();
        }
#endif
    }
}