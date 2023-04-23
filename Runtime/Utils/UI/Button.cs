using SkalluUtils.PropertyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SkalluUtils.Wrappers.UI
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
            PointerEnter,
            PointerDown,
            PointerUp,
            PointerExit
        }

        [SerializeField, ReadOnly] private ButtonState _state = ButtonState.PointerEnter;

        [Space]
        [SerializeField] private UnityEvent _onPointerEnter;
        [SerializeField] private UnityEvent _onPointerDown;
        [SerializeField] private UnityEvent _onPointerUp;
        [SerializeField] private UnityEvent _onPointerExit;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_state == ButtonState.PointerDown)
            {
                return;
            }

            _onPointerEnter?.Invoke();
            _state = ButtonState.PointerEnter;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _onPointerDown?.Invoke();
            _state = ButtonState.PointerDown;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _onPointerUp?.Invoke();
            _state = ButtonState.PointerUp;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_state == ButtonState.PointerDown)
            {
                return;
            }

            _onPointerExit?.Invoke();
            _state = ButtonState.PointerExit;
        }
    }
} 
