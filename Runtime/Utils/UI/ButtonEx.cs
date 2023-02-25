using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SkalluUtils.Utils.UI
{
    public class ButtonEx : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
    {
        [SerializeField] private UnityEvent _onPointerEnter;
        [SerializeField] private UnityEvent _onPointerDown;
        [SerializeField] private UnityEvent _onPointerExit;

        public UnityEvent OnPointerEnterEvent => _onPointerEnter;
        public UnityEvent OnPointerDownEvent => _onPointerDown;
        public UnityEvent OnPointerExitEvent => _onPointerExit;

        public void OnPointerEnter(PointerEventData eventData) => _onPointerEnter?.Invoke();

        public void OnPointerDown(PointerEventData eventData) => _onPointerDown?.Invoke();

        public void OnPointerExit(PointerEventData eventData) => _onPointerExit?.Invoke();
    }
} 
