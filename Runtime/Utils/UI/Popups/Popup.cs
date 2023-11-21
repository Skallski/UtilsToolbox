using System;
using JetBrains.Annotations;
using UnityEngine;

namespace SkalluUtils.Utils.UI.Popups
{
    public class Popup : MonoBehaviour
    {
        [SerializeField] private GameObject _content;

        private Action _onAccept;
        private Action _onDecline;

        public void Setup(Action onAccept, Action onDecline = null)
        {
            Open();
            
            _onAccept = onAccept;
            _onDecline = onDecline;
        }

        [UsedImplicitly]
        public void Accept()
        {
            _onAccept?.Invoke();
            
            Close();
        }
        
        [UsedImplicitly]
        public void Decline()
        {
            _onDecline?.Invoke();
            
            Close();
        }

        protected virtual void Open()
        {
            _content.SetActive(true);
        }
        
        protected virtual void Close()
        {
            _content.SetActive(false);
        }
    }
}