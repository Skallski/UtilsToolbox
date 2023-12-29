using System.Collections.Generic;
using JetBrains.Annotations;
using SkalluUtils.PropertyAttributes;
using UnityEngine;

namespace SkalluUtils.Utils.UI.Panels
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

        private void OpenPanel(UiPanel panel)
        {
            if (panel == null)
            {
                Debug.LogError("Panel to open is null!");
                return;
            }
            
            panel.Open();
        }
        
        private void ClosePanel(UiPanel panel)
        {
            if (panel == null)
            {
                Debug.LogError($"Panel to close is null!");
                return;
            }

            panel.Close();
        }
        
        public void SwitchToPanel(UiPanel panel)
        {
            if (panel == null)
            {
                Debug.LogError("Panel to open is null!");
                return;
            }

            if (_activePanel != null)
            {
                ClosePanel(_activePanel);
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