using System;
using JetBrains.Annotations;
using UnityEngine;

namespace SkalluUtils.Utils.UI.Popups
{
    public class Popup : MonoBehaviour
    {
        [CanBeNull, SerializeField] protected GameObject _background;
        [SerializeField] protected GameObject _content;
        
        private Action _onAccept;
        private Action _onDecline;
        
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
            if (_background != null)
            {
                _background.SetActive(true);
            }
            
            _content.SetActive(true);
        }
        
        protected virtual void Close()
        {
            if (_background != null)
            {
                _background.SetActive(false);
            }

            _content.SetActive(false);
        }
    }
}