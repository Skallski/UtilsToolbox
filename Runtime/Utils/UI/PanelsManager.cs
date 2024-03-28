using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using SkalluUtils.PropertyAttributes;
using UnityEngine;

namespace SkalluUtils.Utils.UI
{
    public class PanelsManager : MonoBehaviour
    {
        [SerializeField, ReadOnly, CanBeNull] protected UiPanel _activePanel;
        [SerializeField, CanBeNull] protected UiPanel _homePanel;
        [SerializeField] protected List<UiPanel> _panels;

        public UiPanel ActivePanel => _activePanel;
        public UiPanel HomePanel => _homePanel;
        public List<UiPanel> Panels => _panels;

        protected virtual void OnEnable()
        {
            UiPanel.OnPanelOpened += OnPanelOpened;
            UiPanel.OnPanelClosed += OnPanelClosed;
        }

        protected virtual void OnDisable()
        {
            UiPanel.OnPanelOpened -= OnPanelOpened;
            UiPanel.OnPanelClosed -= OnPanelClosed;
        }

        private void OnPanelOpened(UiPanel panel)
        {
            _activePanel = panel;
        }
        
        private void OnPanelClosed(UiPanel panel)
        {
            if (panel == _activePanel)
            {
                _activePanel = null;
            }
        }

        private static void OpenPanel(UiPanel panel)
        {
            if (panel == null)
            {
                Debug.LogError("Panel to open is null!");
                return;
            }
            
            panel.Open();
        }
        
        private static void ClosePanel(UiPanel panel)
        {
            if (panel == null)
            {
                Debug.LogError($"Panel to close is null!");
                return;
            }

            panel.Close();
        }
        
        protected TPanel GetPanel<TPanel, TType>(TType panelType, Predicate<int> predicate) 
            where TPanel : UiPanel
            where TType : Enum
        {
            if (EqualityComparer<TType>.Default.Equals(panelType, default))
            {
                return null;
            }
            
            for (int i = 0, c = _panels.Count; i < c; i++)
            {
                if (predicate != null && predicate.Invoke(i) && _panels[i] is TPanel panel)
                {
                    return panel;
                }
            }

            return null;
        }
        
        public void SwitchToPanel(UiPanel panel)
        {
            if (panel == null)
            {
                Debug.LogError("Panel to open is null!");
                return;
            }

            StartCoroutine(SwitchToPanel_Coroutine(panel));
        }

        private IEnumerator SwitchToPanel_Coroutine(UiPanel panel)
        {
            if (_activePanel != null)
            {
                UiPanel bottomPanel = _activePanel;
                
                ClosePanel(_activePanel);
                
                yield return new WaitUntil(() => bottomPanel.IsOpened == false);
            }

            OpenPanel(panel);
        }
        
        public void SwitchToPanel(int panelIndex)
        {
            if (_panels.Count >= panelIndex)
            {
                Debug.LogError($"Panel stack does not contain index of {panelIndex}!");
                return;
            }
            
            SwitchToPanel(_panels[panelIndex]);
        }
    }
}