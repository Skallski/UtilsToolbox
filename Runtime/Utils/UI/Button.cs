using SkalluUtils.PropertyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SkalluUtils.Utils.UI
{
    public class Button :
        MonoBehaviour,
        IPointerEnterHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerExitHandler
    {
        private enum ButtonState
        {
            None,
            PointerEnter,
            PointerDown,
            PointerUp,
            PointerExit
        }

        [SerializeField, ReadOnly] private ButtonState _state = ButtonState.None;
        [SerializeField] private UnityEvent _onPointerEnter;
        [SerializeField] private UnityEvent _onPointerDown;
        [SerializeField] private UnityEvent _onPointerUp;
        [SerializeField] private UnityEvent _onPointerExit;

        #region POINTER EVENTS
        public void OnPointerEnter(PointerEventData eventData) => OnPointerEnterInternal();
        public void OnPointerDown(PointerEventData eventData) => OnPointerDownInternal();
        public void OnPointerUp(PointerEventData eventData) => OnPointerUpInternal();
        public void OnPointerExit(PointerEventData eventData) => OnPointerExitInternal();
        #endregion

        private void OnPointerEnterInternal()
        {
            _onPointerEnter?.Invoke();
            _state = ButtonState.PointerEnter;
        }

        private void OnPointerDownInternal()
        {
            _onPointerDown?.Invoke();
            _state = ButtonState.PointerDown;
        }

        private void OnPointerUpInternal()
        {
            if (_state == ButtonState.PointerExit)
            {
                return;
            }
            
            _onPointerUp?.Invoke();
            _state = ButtonState.PointerUp;
        }

        private void OnPointerExitInternal()
        {
            _onPointerExit?.Invoke();
            _state = ButtonState.PointerExit;
        }
    }
}