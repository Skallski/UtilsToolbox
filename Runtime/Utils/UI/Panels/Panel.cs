using System;
using UnityEngine;

namespace SkalluUtils.Utils.UI.Panels
{
    /// <summary>
    /// More concrete panel class that can be used by PanelsManager
    /// </summary>
    public class Panel : PanelBase
    {
        internal static event Action<Panel> OnPanelOpened;
        internal static event Action<Panel> OnPanelClosed;
        
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

        protected override void OpenSelf()
        {
            base.OpenSelf();
            
            OnPanelOpened?.Invoke(this);
        }

        protected override void CloseSelf()
        {
            base.CloseSelf();
            
            OnPanelClosed?.Invoke(this);
        }
    }
}