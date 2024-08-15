using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace UtilsToolbox.Utils.UI.UiPanel
{
    /// <summary>
    /// Base panel class
    /// </summary>
    public abstract class UiPanel : MonoBehaviour
    {
        internal static event Action<UiPanel> OnPanelOpened;
        internal static event Action<UiPanel> OnPanelClosed;
        
        [CanBeNull, SerializeField] protected GameObject _background;
        [SerializeField] protected GameObject _content;
        
        [Tooltip("Optional event called when panel is opened")]
        [SerializeField] protected UnityEvent _opened;
        
        [Tooltip("Optional event called when panel is closed")]
        [SerializeField] protected UnityEvent _closed;
        
        public bool IsOpened => _content != null && _content.activeSelf;

        protected UiPanelOpeningParameters OpeningParameters { get; private set; }

#if UNITY_EDITOR
        protected virtual void Reset()
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
        /// Opens panel
        /// </summary>
        internal void Open(UiPanelOpeningParameters openingParameters)
        {
            if (!IsOpened)
            {
                OpeningParameters = openingParameters;
                OpenSelf();
            }
        }
        
        /// <summary>
        /// Closes panel
        /// </summary>
        internal void Close()
        {
            if (IsOpened)
            {
                CloseSelf();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        protected virtual void OpenSelf()
        {
            ForceOpen();
            OnOpened();
        }
        
        /// <summary>
        /// 
        /// </summary>
        protected virtual void CloseSelf()
        {
            OnClosed();
            ForceClose();
        }
        
        
        /// <summary>
        /// Called when panel is successfully opened
        /// </summary>
        protected virtual void OnOpened()
        {
            OnPanelOpened?.Invoke(this);
            _opened?.Invoke();
        }
        
        /// <summary>
        /// Called when panel is successfully closed
        /// </summary>
        protected virtual void OnClosed()
        {
            OnPanelClosed?.Invoke(this);
            
            _closed?.Invoke();
        }
        
        /// <summary>
        /// Opens panel without executing additional logic
        /// <remarks>
        /// Good for debugging inside editor
        /// </remarks>>
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
        /// Closes panel without executing additional logic
        /// <remarks>
        /// Good for debugging inside editor
        /// </remarks>>
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