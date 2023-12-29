using SkalluUtils.PropertyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SkalluUtils.Utils.UI.Elements
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
        [SerializeField] private bool _interactible = true;
        
        [SerializeField] private UnityEvent _onPointerEnter;
        [SerializeField] private UnityEvent _onPointerDown;
        [SerializeField] private UnityEvent _onPointerUp;
        [SerializeField] private UnityEvent _onPointerExit;

        public bool Interactible
        {
            get => _interactible;
            set => _interactible = value;
        }

        #region POINTER EVENTS
        public void OnPointerEnter(PointerEventData eventData) => OnPointerEnterInternal();
        public void OnPointerDown(PointerEventData eventData) => OnPointerDownInternal();
        public void OnPointerUp(PointerEventData eventData) => OnPointerUpInternal();
        public void OnPointerExit(PointerEventData eventData) => OnPointerExitInternal();
        #endregion

        private void OnPointerEnterInternal()
        {
            if (_interactible == false)
            {
                return;
            }
            
            _onPointerEnter?.Invoke();
            _state = ButtonState.PointerEnter;
        }

        private void OnPointerDownInternal()
        {
            if (_interactible == false)
            {
                return;
            }
            
            _onPointerDown?.Invoke();
            _state = ButtonState.PointerDown;
        }

        private void OnPointerUpInternal()
        {
            if (_interactible == false)
            {
                return;
            }
            
            if (_state == ButtonState.PointerExit)
            {
                return;
            }
            
            _onPointerUp?.Invoke();
            _state = ButtonState.PointerUp;
        }

        private void OnPointerExitInternal()
        {
            if (_interactible == false)
            {
                return;
            }
            
            _onPointerExit?.Invoke();
            _state = ButtonState.PointerExit;
        }
    }
}