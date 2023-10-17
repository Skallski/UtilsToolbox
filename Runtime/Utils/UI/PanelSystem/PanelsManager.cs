using System.Collections.Generic;
using JetBrains.Annotations;
using SkalluUtils.PropertyAttributes;
using UnityEngine;

namespace SkalluUtils.Utils.UI
{
    public class PanelsManager : MonoBehaviour
    {
        [SerializeField, ReadOnly, CanBeNull] protected Panel _activePanel;
        [Space]
        [SerializeField, CanBeNull] protected Panel _homePanel;
        [SerializeField] protected List<Panel> _panels;

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

        private void OpenPanel(Panel panel)
        {
            if (panel == null)
            {
                Debug.LogError("Panel to open is null!");
                return;
            }
            
            panel.Open();
        }
        
        private void ClosePanel(Panel panel)
        {
            if (panel == null)
            {
                Debug.LogError($"Panel to close is null!");
                return;
            }

            panel.Close();
        }
        
        [UsedImplicitly]
        public void SwitchToPanel(Panel panel)
        {
            if (panel == null)
            {
                Debug.LogError("Panel to open is null!");
                return;
            }
            
            ClosePanel(_activePanel);
            OpenPanel(panel);
        }

        [UsedImplicitly]
        public void SwitchToPanel(int panelIndex)
        {
            if (_panels.Count >= panelIndex)
            {
                Debug.LogError($"Panel stack does not contain index of {panelIndex}!");
                return;
            }
            
            SwitchToPanel(panel: _panels[panelIndex]);
        }
    }
}