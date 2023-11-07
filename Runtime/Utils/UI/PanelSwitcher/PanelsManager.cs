using System.Collections.Generic;
using JetBrains.Annotations;
using SkalluUtils.PropertyAttributes;
using UnityEngine;

namespace SkalluUtils.Utils.UI.PanelSwitcher
{
    public class PanelsManager : MonoBehaviour
    {
        [SerializeField, ReadOnly, CanBeNull] protected Panel _activePanel;
        [Space]
        [SerializeField, CanBeNull] protected Panel _homePanel;
        [SerializeField] protected List<Panel> _panels;

        public Panel ActivePanel => _activePanel;
        public Panel HomePanel => _homePanel;
        public List<Panel> Panels => _panels;

        protected virtual void OnEnable()
        {
            Panel.OnPanelOpened += OnPanelOpened;
            Panel.OnPanelClosed += OnPanelClosed;
        }

        protected virtual void OnDisable()
        {
            Panel.OnPanelOpened -= OnPanelOpened;
            Panel.OnPanelClosed -= OnPanelClosed;
        }

        private void OnPanelOpened(Panel panel)
        {
            _activePanel = panel;
        }
        
        private void OnPanelClosed(Panel panel)
        {
            if (panel == _activePanel)
            {
                _activePanel = null;
            }
        }

        public void OpenPanel(Panel panel)
        {
            if (panel == null)
            {
                Debug.LogError("Panel to open is null!");
                return;
            }
            
            panel.Open();
        }
        
        public void ClosePanel(Panel panel)
        {
            if (panel == null)
            {
                Debug.LogError($"Panel to close is null!");
                return;
            }

            panel.Close();
        }
        
        public void SwitchToPanel(Panel panel)
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