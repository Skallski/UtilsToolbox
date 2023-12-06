using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace SkalluUtils.Utils.UI.Panels
{
    /// <summary>
    /// Base panel class
    /// </summary>
    public abstract class Panel : MonoBehaviour
    {
        internal static event Action<Panel> OnPanelOpened;
        internal static event Action<Panel> OnPanelClosed;
        
        [CanBeNull, SerializeField] protected GameObject _background;
        [SerializeField] protected GameObject _content;
        
        [Tooltip("Optional event called when panel is opened")]
        [SerializeField] protected UnityEvent _opened;
        
        [Tooltip("Optional event called when panel is closed")]
        [SerializeField] protected UnityEvent _closed;
        
        public bool IsOpened => _content != null && _content.activeSelf;

#if UNITY_EDITOR
        private void Reset()
        {
            if (_background == null) _background = FindInChildren("Background");
            if (_content == null) _content = FindInChildren("Content");

            GameObject FindInChildren(string nameToFind)
            {
                for (int i = 0, c = transform.childCount; i < c; i++)
                {
                    var child = transform.GetChild(i);
                    if (child.name.Equals(nameToFind))
                    {
                        return child.gameObject;
                    }
                }

                return null;
            }
        }
#endif
        
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
        /// Opens panel with animation
        /// </summary>
        protected virtual void OpenSelf()
        {
            ForceOpen();
            OnOpened();
        }
        
        /// <summary>
        /// Closes panel with animation
        /// </summary>
        protected virtual void CloseSelf()
        {
            OnClosed();
            ForceClose();
        }
        
        protected virtual void OnOpened()
        {
            OnPanelOpened?.Invoke(this);
            
            _opened?.Invoke();
        }
        
        protected virtual void OnClosed()
        {
            OnPanelClosed?.Invoke(this);
            
            _closed?.Invoke();
        }
        
        /// <summary>
        /// Opens panel without invoking an event
        /// </summary>
        [ContextMenu("DEBUG_Open")]
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
        [ContextMenu("DEBUG_Close")]
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