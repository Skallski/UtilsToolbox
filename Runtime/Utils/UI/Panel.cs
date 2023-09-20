using JetBrains.Annotations;
using SkalluUtils.PropertyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace SkalluUtils.Utils.UI
{
    public abstract class Panel : MonoBehaviour
    {
        [SerializeField] private GameObject _content;
        [CanBeNull, SerializeField] private GameObject _background;

        [Header("Events")]
        [SerializeField] private UnityEvent _opened;
        [SerializeField] private UnityEvent _closed;
        
        private bool IsOpened => _content.activeSelf;

        [SerializeMethod]
        public void Open()
        {
            if (!IsOpened)
            {
                OpenSelf();
            }
        }

        [SerializeMethod]
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
        /// Opens panel without invoking event
        /// </summary>
        public void ForceOpen()
        {
            if (_background != null)
            {
                _background.SetActive(true);
            }
            
            _content.SetActive(true);
        }

        /// <summary>
        /// Closes panel without invoking event
        /// </summary>
        public void ForceClose()
        {
            if (_background != null)
            {
                _background.SetActive(false);
            }

            _content.SetActive(false);
        }
    }
}