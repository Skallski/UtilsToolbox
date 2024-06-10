using System;
using System.Collections.Generic;
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
            None = -1,
            PointerEnter = 0,
            PointerDown = 1,
            PointerUp = 2,
            PointerExit = 3
        }

        public enum ButtonEventType
        {
            PointerEnter = 0,
            PointerDown = 1,
            PointerUp = 2,
            PointerExit = 3
        }
        
        [Serializable]
        private class Entry
        {
            public ButtonEventType buttonEventType = ButtonEventType.PointerEnter;
            public UnityEvent callback = new UnityEvent();
        }

        [SerializeField] private ButtonState _state = ButtonState.None;
        [SerializeField] private bool _isInteractible = true;
        [SerializeField] private List<Entry> _events = new List<Entry>();

        public bool IsInteractible
        {
            get => _isInteractible;
            set => _isInteractible = value;
        }

        #region POINTER EVENTS
        public void OnPointerEnter(PointerEventData eventData)
        {
            HandlePointerEvent(ButtonState.PointerEnter);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            HandlePointerEvent(ButtonState.PointerDown);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_state == ButtonState.PointerExit)
            {
                return;
            }

            HandlePointerEvent(ButtonState.PointerUp);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HandlePointerEvent(ButtonState.PointerExit);
        }
        #endregion
        
        private void HandlePointerEvent(ButtonState buttonState)
        {
            if (_state == buttonState || _isInteractible == false)
            {
                return;
            }

            foreach (Entry entry in _events)
            {
                if ((int) entry.buttonEventType == (int) buttonState)
                {
                    entry.callback?.Invoke();
                }
            }
            
            _state = buttonState;
        }
    }
}