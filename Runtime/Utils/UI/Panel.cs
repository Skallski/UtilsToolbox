using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace SkalluUtils.Utils.UI
{
    public abstract class Panel : MonoBehaviour
    {
        [SerializeField] private GameObject _content;
        [CanBeNull, SerializeField] private GameObject _background;

        [Space]
        [SerializeField] private UnityEvent _opened;
        [SerializeField] private UnityEvent _closed;

        public void Open()
        {
            if (!IsOpened())
            {
                OpenSelf();
            }
        }

        public void Close()
        {
            if (IsOpened())
            {
                CloseSelf();
            }
        }

        private bool IsOpened() => _content.activeSelf;

        /// <summary>
        /// Display background and open context menu
        /// </summary>
        protected virtual void OpenSelf()
        {
            if (_background != null) _background.SetActive(true);
            _content.SetActive(true);
            
            _opened?.Invoke();
        }

        /// <summary>
        /// Hides background and closes context menu
        /// </summary>
        protected virtual void CloseSelf()
        {
            if (_background != null) _background.SetActive(false);
            _content.SetActive(false);
            
            _closed?.Invoke();
        }
    }
}