using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace UtilsToolbox.Utils.UI.UiPanel
{
    public class UiPanelsManager : MonoBehaviour
    {
        [SerializeField, CanBeNull] protected UiPanel _activePanel;
        [SerializeField, CanBeNull] protected UiPanel _homePanel;
        [SerializeField] protected List<UiPanel> _panels;

        private bool _isSwitchingPanels;

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

        protected TPanel GetPanel<TPanel>(Predicate<int> predicate) where TPanel : UiPanel
        {
            for (int i = 0, c = _panels.Count; i < c; i++)
            {
                if (predicate != null && predicate.Invoke(i) && _panels[i] is TPanel panel)
                {
                    return panel;
                }
            }

            return null;
        }
        
        public void SwitchToPanel(UiPanel panel, UiPanelOpeningParameters openingParameters = null) 
        {
            if (panel == null)
            {
                Debug.LogError("Panel to open is null!");
                return;
            }

            if (_isSwitchingPanels)
            {
                return;
            }
            
            StartCoroutine(SwitchToPanel_Coroutine());
            IEnumerator SwitchToPanel_Coroutine()
            {
                if (_activePanel == null)
                {
                    Debug.LogError("Panel to close is null!");
                    yield break;
                }

                _isSwitchingPanels = true;
                UiPanel bottomPanel = _activePanel;
                _activePanel.Close();

                yield return new WaitUntil(() => bottomPanel.IsOpened == false);
                panel.Open(openingParameters);
                _isSwitchingPanels = false;
            }
        }

        public void SwitchToPanel(int panelIndex, UiPanelOpeningParameters openingParameters = null)
        {
            if (_panels.Count >= panelIndex)
            {
                Debug.LogError($"Panel stack does not contain index of {panelIndex}!");
                return;
            }
            
            SwitchToPanel(_panels[panelIndex], openingParameters);
        }
    }
}